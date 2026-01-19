using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MISA.WorkShiftManagement.Core.Interfaces.Repositories;
using MySqlConnector;
using Dapper;
using MISA.WorkShiftManagement.Core.MISAAttribute;
using System.Reflection;
using MISA.WorkShiftManagement.Core.Exceptions;

namespace MISA.WorkShiftManagement.Infrastructure.Repositories
{
    /// <summary>
    /// Thực hiện những phương thức chung cho các repository
    /// </summary>
    /// <typeparam name="T">Entity chứa thông tin của đối tượng cần thao tác</typeparam>
    /// CreatedBy: THPHU (11/01/2026)
    public class BaseRepository<T> : IBaseRepository<T>
    {
        private readonly string _connectionString;
        private readonly string _tableName;

        protected BaseRepository(IConfiguration config, IHostEnvironment ev)
        {
            if (ev.IsDevelopment())
            {
                _connectionString = config.GetConnectionString("Developement");
            }
            else
            {
                _connectionString = config.GetConnectionString("Production");
            }
            // Xác định tên bảng
            _tableName = GetTableName();
        }

        /// <summary>
        /// Tạo kết nối đến database dùng chung cho các repository con
        /// </summary>
        /// CreatedBy: THPHU (11/01/2026)
        protected MySqlConnection CreateConnection()
        {
            return new MySqlConnection(_connectionString);
        }

        #region DeleteAsync

        public async Task<int> DeleteAsync(Guid id)
        {
            try
            {
                // Lấy tên của khóa chính (trong db)
                var primaryKeyColumnName = GetPrimaryKeyColumnName();
                // Câu lệnh truy vấn
                var sqlDelete = $"DELETE FROM {_tableName} WHERE {primaryKeyColumnName} = @id";
                // Khai báo parameter
                var parameter = new { id = id };
                // Kết nối đến database và thực hiện truy vấn
                using (var mySqlConnection = new MySqlConnection(_connectionString))
                {
                    var rowAffected = await mySqlConnection.ExecuteAsync(sqlDelete, param: parameter);
                    // Trả về số bản ghi bị ảnh hưởng
                    return rowAffected;
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Lỗi truy cập dữ liệu trong quá trình xóa bản ghi.", ex);
            }
        }

        #endregion DeleteAsync

        #region GetByIdAsync

        public async Task<T?> GetByIdAsync(Guid id)
        {
            try
            {
                // Lấy tên của khóa chính
                var primaryKeyColumnName = GetPrimaryKeyColumnName();
                // Câu lệnh truy vấn
                var sql = @$"SELECT * FROM {_tableName} WHERE {primaryKeyColumnName} = @id";
                // Khai báo parameter
                var parameter = new { id = id };
                // Kết nối đến database và thực hiện truy vấn
                using (var mySqlConnection = new MySqlConnection(_connectionString))
                {
                    var result = await mySqlConnection.QueryFirstOrDefaultAsync<T>(sql, param: parameter);
                    // Trả về kết quả
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Lỗi truy cập dữ liệu trong quá trình lấy bản ghi.", ex);
            }
        }

        #endregion GetByIdAsync

        #region GetPrimaryKeyColumnName

        /// <summary>
        /// Xây dựng phương thức lấy tên của khóa chính (tên cột trong db)
        /// </summary>
        /// <returns>Tên cột của property trong db</returns>
        /// <exception cref="InvalidOperationException"></exception>
        private string GetPrimaryKeyColumnName()
        {
            // Lấy ra trường khóa chính
            var primaryKeyColumnProperty = typeof(T).GetProperties()
                .FirstOrDefault(prop => Attribute.IsDefined(prop, typeof(MISAPrimaryKeyAttribute)));
            // Nếu không tìm thấy khóa chính thì ném ra ngoại lệ
            if (primaryKeyColumnProperty == null)
            {
                throw new InvalidOperationException($"Không tìm thấy khóa chính cho entity '{typeof(T).Name}'. Vui lòng khai báo thuộc tính có gắn [MISAPrimaryKeyAttribute].");
            }
            // Lấy tên của khóa chính
            var primaryKeyColumnName = primaryKeyColumnProperty.Name;
            // Nếu có attribute MISAColumnName thì lấy tên cột từ attribute
            var primaryKeyColumnAttribute = primaryKeyColumnProperty?.GetCustomAttribute<MISAColumnNameAttribute>();
            if (primaryKeyColumnAttribute != null)
            {
                primaryKeyColumnName = primaryKeyColumnAttribute.ColumnName ?? primaryKeyColumnName;
            }
            return primaryKeyColumnName;
        }

        #endregion GetPrimaryKeyColumnName

        #region InsertAsync

        public async Task<int> InsertAsync(T entity)
        {
            try
            {
                // Lấy câu lệnh sql và parameter
                var (sql, parameters) = BuildInsertSql(entity);

                // Kết nối đến database và thực hiện truy vấn
                using (var mySqlConnection = new MySqlConnection(_connectionString))
                {
                    var rowAffected = await mySqlConnection.ExecuteAsync(sql, param: parameters);
                    return rowAffected;
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Lỗi truy cập dữ liệu trong quá trình thêm bản ghi.", ex);
            }
        }

        #endregion InsertAsync

        #region BuildInsertSql

        /// <summary>
        /// Xây dựng câu lệnh SQL thêm mới dữ liệu
        /// </summary>
        /// <param name="entity">Entity chứa thông tin đối tượng thêm</param>
        /// <returns>Câu lệnh sql và danh sách parameter</returns>
        private (string Sql, DynamicParameters Params) BuildInsertSql(T entity)
        {
            // Lấy danh sách cột của bảng (ShiftID, ShiftCode, ShiftName, ...) từ entity
            var columns = new List<string>();
            // Tên của các Prameter (@ShiftID, @ShiftCode, @ShiftName, ...)
            var paramNames = new List<string>();
            // Các giá trị của Parameter (@ShiftID = entity.ShiftID, @ShiftCode = entity.ShiftCode, ...)
            var parameters = new DynamicParameters();

            // Lấy tất cả properties của entity
            var properties = typeof(T).GetProperties();

            // Duyệt từng property để xây dựng câu lệnh SQL
            foreach (var property in properties)
            {
                // Kiểm tra property có attribute MISAIgnoreInsert không, nếu có thì bỏ qua
                if (Attribute.IsDefined(property, typeof(MISAIgnoreInsertAttribute)))
                {
                    continue;
                }
                ;
                // Lấy tên của property
                var columnName = property.Name;
                // Lấy thông tin column (tên trong db) từ attribute
                var columnAttribute = property.GetCustomAttribute<MISAColumnNameAttribute>();
                if (columnAttribute != null)
                {
                    columnName = columnAttribute.ColumnName ?? columnName;
                }
                columns.Add(columnName);
                paramNames.Add($"@{columnName}");
                // Kiểm tra trường nào sinh tự động và gán giá trị (Ngày tạo)
                if (Attribute.IsDefined(property, typeof(MISACreatedDateAttribute)))
                {
                    property.SetValue(entity, DateTime.Now);
                }
                // Lấy giá trị của property từ entity
                parameters.Add($"@{columnName}", property.GetValue(entity));
            }

            // conver list sang chuỗi
            var sqlInsert = @$"INSERT INTO {_tableName} ({string.Join(",", columns)}) VALUES ({string.Join(",", paramNames)});";

            return (sqlInsert, parameters);
        }

        #endregion BuildInsertSql

        #region UpdateAsync

        public async Task<int> UpdateAsync(Guid id, T entity)
        {
            try
            {
                // Lấy câu lệnh sql và parameter
                var (sqlUpdate, parameters) = BuildUpdateSql(id, entity);
                // Kết nối đến database và thực hiện truy vấn
                using (var mySqlConnection = new MySqlConnection(_connectionString))
                {
                    var rowAffected = await mySqlConnection.ExecuteAsync(sqlUpdate, param: parameters);

                    // Trả về số bản ghi bị ảnh hưởng
                    return rowAffected;
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Lỗi truy cập dữ liệu trong quá trình cập nhật bản ghi.", ex);
            }
        }

        #endregion UpdateAsync

        #region BuildUpdateSql

        /// <summary>
        /// Xây dựng câu lệnh SQL cập nhật dữ liệu
        /// </summary>
        /// <param name="id">Id của đối tượng sửa</param>
        /// <param name="entity">Entity chứa thông tin đối tượng sửa</param>
        /// <returns>Câu lệnh và danh sách parameter</returns>
        private (string Sql, DynamicParameters Param) BuildUpdateSql(Guid id, T entity)
        {
            // Lấy danh sách cột của bảng (ShiftID, ShiftCode, ShiftName, ...) từ entity
            var columns = new List<string>();
            // Tên của các Prameter (@ShiftID, @ShiftCode, @ShiftName, ...)
            var columnsParamName = new List<string>();
            // Các giá trị của Parameter (@ShiftID = entity.ShiftID, @ShiftCode = entity.ShiftCode, ...)
            var parameters = new DynamicParameters();

            // Lấy tất cả properties của entity
            var properties = typeof(T).GetProperties();
            // Duyệt từng property để xây dựng câu lệnh SQL
            foreach (var property in properties)
            {
                // Kiểm tra property có attribute MISAIgnoreUpdate không, nếu có thì bỏ qua
                if (Attribute.IsDefined(property, typeof(MISAIgnoreUpdateAttribute)))
                {
                    continue;
                }
                ;
                // Kiểm tra property có phải là khóa chính không, nếu có thì bỏ qua
                if (Attribute.IsDefined(property, typeof(MISAPrimaryKeyAttribute)))
                {
                    continue;
                }
                // Lấy tên của property
                var propertyName = property.Name;
                // Lấy thông tin column từ attribute
                var columnAttribute = property.GetCustomAttribute<MISAColumnNameAttribute>();
                if (columnAttribute != null)
                {
                    propertyName = columnAttribute.ColumnName ?? propertyName;
                }
                // Thêm vào danh sách cột
                columns.Add(propertyName);
                // Tên parameter
                columnsParamName.Add($"@{propertyName}");
                // Kiểm tra trường nào sinh tự động và gán giá trị
                if (Attribute.IsDefined(property, typeof(MISAModifiedDateAttribute)))
                {
                    property.SetValue(entity, DateTime.Now);
                }
                // Lấy giá trị của property từ entity
                parameters.Add($"@{propertyName}", property.GetValue(entity));
            }
            // Xử lý danh sách cột để tạo câu lệnh SET
            for (var i = 0; i < columns.Count; i++)
            {
                columns[i] = $"{columns[i]} = {columnsParamName[i]}";
            }
            // Xử lý trường khóa chính
            var primaryKeyColumnName = GetPrimaryKeyColumnName();

            // Thêm giá trị của khóa chính vào parameter
            parameters.Add($"@{primaryKeyColumnName}", id);

            // Câu lệnh sql
            var sqlUpdate = $"UPDATE {_tableName} SET {string.Join(",", columns)} WHERE {primaryKeyColumnName} = @{primaryKeyColumnName};"; // Set cột = giá trị, cột N = giá trị N WHERE Id = @Id

            // Trả về câu lệnh sql và parameter
            return (sqlUpdate, parameters);
        }

        #endregion BuildUpdateSql

        #region GetTableName

        /// <summary>
        /// Lấy tên của bảng từ attribute hoặc tên class
        /// </summary>
        /// <returns>Tên bảng hoặc tên của class</returns>
        /// CreatedBy: THPHU (09/01/2026)
        private string GetTableName()
        {
            var tableName = typeof(T).Name.ToLower();
            var tableNameAttribute = typeof(T).GetCustomAttribute<MISATableNameAttribute>();
            if (tableNameAttribute != null)
            {
                tableName = tableNameAttribute.TableName;
            }
            return tableName;
        }

        #endregion GetTableName
    }
}