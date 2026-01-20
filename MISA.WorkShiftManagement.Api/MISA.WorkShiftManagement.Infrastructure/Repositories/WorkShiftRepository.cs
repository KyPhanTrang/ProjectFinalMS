using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MISA.WorkShiftManagement.Core.DTOs;
using MISA.WorkShiftManagement.Core.DTOs.WorkShift;
using MISA.WorkShiftManagement.Core.Entities;
using MISA.WorkShiftManagement.Core.Exceptions;
using MISA.WorkShiftManagement.Core.Interfaces.Repositories;

namespace MISA.WorkShiftManagement.Infrastructure.Repositories
{
    /// <summary>
    /// Xử lý riêng cho repo của ca làm việc
    /// CreatedBy: THPHU (11/01/2026)
    /// </summary>
    public class WorkShiftRepository : BaseRepository<WorkShift>, IWorkShiftRepository
    {
        public WorkShiftRepository(IConfiguration config, IHostEnvironment ev) : base(config, ev)
        {
        }

        public async Task<bool> CheckShiftCodeExistsAsync(string shiftCode, Guid? shiftId)
        {
            // Câu lệnh kiểm tra trùng mã ca
            var sql = @"
                SELECT COUNT(*)
                FROM work_shift
                WHERE shift_code = @ShiftCode
                  AND (@ShiftId IS NULL OR shift_id <> @ShiftId);
            ";

            // Khai báo tham số
            var parameter = new { ShiftCode = shiftCode, ShiftId = shiftId };

            try
            {
                // Kết nối đến database và thực hiện truy vấn
                using (var mySqlConnection = CreateConnection())
                {
                    var result = await mySqlConnection.QueryFirstOrDefaultAsync<int>(sql, parameter);
                    return result > 0;
                }
            }
            catch (DataAccessException ex)
            {
                throw new DataAccessException("Lỗi truy cập dữ liệu khi xóa danh sách id ca làm việc.", ex);
            }
        }

        public async Task<int> DeleteManyAsync(IEnumerable<Guid> ids)
        {
            // Nếu danh sách id rỗng thì trả về 0
            if (ids == null || !ids.Any())
            {
                return 0;
            }

            // Câu lệnh sql xóa nhiều ca làm việc
            var sqlDeletes = "DELETE FROM work_shift WHERE shift_id IN @Ids";
            // Khai báo tham số
            var parameters = new { Ids = ids };
            try
            {
                // Kết nối đến database và thực hiện truy vấn
                using (var mySqlConnection = CreateConnection())
                {
                    var result = await mySqlConnection.ExecuteAsync(sqlDeletes, parameters);
                    return result;
                }
            }
            catch (DataAccessException ex)
            {
                throw new DataAccessException("Lỗi truy cập dữ liệu khi xóa danh sách id ca làm việc.", ex);
            }
        }

        public async Task<int> DisableWorkShiftsAsync(IEnumerable<Guid> ids)
        {
            // Nếu danh sách id rỗng thì trả về 0
            if (ids == null || !ids.Any())
            {
                return 0;
            }

            // Câu lệnh SQL ngừng sử dụng nhiều ca làm việc
            var sqlDisable = "UPDATE work_shift SET is_active = 0 WHERE shift_id IN @Ids";

            // Khai báo tham số
            var parameters = new { Ids = ids };

            try
            {
                // Kết nối đến database và thực hiện truy vấn
                using (var mySqlConnection = CreateConnection())
                {
                    var result = await mySqlConnection.ExecuteAsync(sqlDisable, parameters);
                    return result;
                }
            }
            catch (DataAccessException ex)
            {
                throw new DataAccessException("Lỗi truy cập dữ liệu khi ngừng sử dụng danh sách ca làm việc.", ex);
            }
        }

        public async Task<int> EnableWorkShiftsAsync(IEnumerable<Guid> ids)
        {
            // Nếu danh sách id rỗng thì trả về 0
            if (ids == null || !ids.Any())
            {
                return 0;
            }

            // Câu lệnh SQL kích hoạt nhiều ca làm việc
            var sqlEnable = "UPDATE work_shift SET is_active = 1 WHERE shift_id IN @Ids";

            // Khai báo tham số
            var parameters = new { Ids = ids };

            try
            {
                // Kết nối đến database và thực hiện truy vấn
                using (var mySqlConnection = CreateConnection())
                {
                    var result = await mySqlConnection.ExecuteAsync(sqlEnable, parameters);
                    return result;
                }
            }
            catch (DataAccessException ex)
            {
                throw new DataAccessException("Lỗi truy cập dữ liệu khi kích hoạt danh sách ca làm việc.", ex);
            }
        }

        public async Task<PagingResult<WorkShift>> GetAllPaging(WorkShiftFilter filter)
        {
            try
            {
                // Tạo điều kiện dùng chung
                var whereSql = " WHERE 1 = 1 ";

                // Khai báo tham số
                var parameters = new DynamicParameters();

                // Lọc theo từ khóa
                if (!string.IsNullOrEmpty(filter.Keyword))
                {
                    whereSql += " AND (shift_code LIKE @Keyword OR shift_name LIKE @Keyword OR created_by LIKE @Keyword OR modified_by LIKE @Keyword)";
                    parameters.Add("@Keyword", $"%{filter.Keyword}%");
                }

                // Lọc theo trạng thái hoạt động
                if (filter.IsActive != null)
                {
                    // chuyển đổi sang kiểu giống db (1: đang hoạt động, 0: là ngừng hoạt động)
                    var isActive = (filter.IsActive.Value) ? 1 : 0;
                    whereSql += $" AND (is_active = @IsActive)";
                    parameters.Add("@IsActive", isActive);
                }

                // Lọc theo thời gian làm việc
                if (filter.WorkingTime.HasValue)
                {
                    whereSql += " AND (working_time = @WorkingTime)";
                    parameters.Add("@WorkingTime", filter.WorkingTime.Value);
                }

                // Câu lệnh lấy tổng số bản ghi
                var countSql = "SELECT COUNT(*) FROM work_shift" + whereSql;

                // Câu lệnh lấy dữ liệu
                var dataSql = $@"SELECT * FROM work_shift {whereSql}";

                // Phân trang
                dataSql += " ORDER BY created_date DESC ";
                dataSql += " LIMIT @Limit OFFSET @Offset ";
                parameters.Add("@Limit", filter.PageSize);
                parameters.Add("@Offset", (filter.PageIndex - 1) * filter.PageSize);

                // Kết nối đến database và thực hiện truy vấn
                using (var mySqlConnection = CreateConnection())
                {
                    var workShifts = await mySqlConnection.QueryAsync<WorkShift>(dataSql, parameters);
                    var totalRecord = await mySqlConnection.ExecuteScalarAsync<int>(countSql, parameters);
                    return new PagingResult<WorkShift>()
                    {
                        Data = workShifts,
                        TotalRecord = totalRecord
                    };
                }
            }
            catch (DataAccessException ex)
            {
                // Xử lý lỗi truy cập dữ liệu
                throw new DataAccessException("Lỗi truy cập dữ liệu khi lấy danh sách ca làm việc phân trang.", ex);
            }
        }
    }
}