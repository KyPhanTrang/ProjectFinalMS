using MISA.WorkShiftManagement.Core.Exceptions;
using MISA.WorkShiftManagement.Core.Interfaces.Repositories;
using MISA.WorkShiftManagement.Core.Interfaces.Services;
using MISA.WorkShiftManagement.Core.MISAAttribute;
using System.Reflection;

namespace MISA.WorkShiftManagement.Core.Services
{
    /// <summary>
    /// Thực hiện các phương thức chung cho Service
    /// </summary>
    /// <typeparam name="T">Entity bất kỳ</typeparam>
    /// CreatedBy: THPHU (11/01/2026)
    public class BaseService<T> : IBaseService<T>
    {
        private readonly IBaseRepository<T> _baseRepository;

        public BaseService(IBaseRepository<T> baseRepository)
        {
            _baseRepository = baseRepository;
        }

        #region ValidateEntity

        /// <summary>
        /// Hàm validate dữ liệu chung cho tất cả các entity (chỉ validate các trường bắt buộc)
        /// </summary>
        /// <param name="entity">Entity chứa thông tin cần xử lý</param>
        /// <exception cref="ValidateException">Nếu không tồn tại giá trị, hoặc giá trị rỗng thì ném exception</exception>
        private void ValidateEntity(T entity)
        {
            // Thực hiện validate chung cho tất cả các entity
            // Ví dụ: Kiểm tra các trường bắt buộc, định dạng dữ liệu, v.v.
            if (entity == null)
            {
                throw new ValidateException(new Dictionary<string, string>
                {
                    { "Entity", "Dữ liệu không được phép rỗng." }
                });
            }

            // Kiểm tra các thuộc tính của entity
            var properties = typeof(T).GetProperties();
            var validationErrors = new Dictionary<string, string>();
            foreach (var property in properties)
            {
                // Kiểm tra nếu thuộc tính có MISARequiredAttribute
                var requiredAttribute = property.GetCustomAttribute<MISARequiredAttribute>();
                // Nếu có, kiểm tra giá trị của thuộc tính
                if (requiredAttribute != null)
                {
                    var value = property.GetValue(entity);
                    // Nếu giá trị là null hoặc chuỗi rỗng, thêm lỗi vào danh sách
                    if (value == null || (value != null && value.ToString() == ""))
                    {
                        validationErrors.Add(property.Name, $"{property.Name} là bắt buộc.");
                    }
                }
            }
            if (validationErrors.Any())
            {
                throw new ValidateException(validationErrors);
            }
        }

        #endregion ValidateEntity

        #region CreateAsync

        public async Task<T> CreateAsync(T entity)
        {
            // Thực hiện validate dữ liệu
            ValidateEntity(entity);

            // Tự động sinh giá trị cho khóa chính
            AssignPrimaryKeyForCreate(entity);

            // Thêm validate riêng cho từng entity nếu cần
            await CustomerValidateAsync(entity);

            // Các nghiệp vụ khác trước khi thêm mới của service con (nếu có)
            BeforeCreate(entity);

            // Thực hiện thêm mới dữ liệu thông qua Repository
            var result = await _baseRepository.InsertAsync(entity);
            if (result == 0)
            {
                throw new Exception("Thêm mới dữ liệu thất bại.");
            }
            return entity;
        }

        #endregion CreateAsync

        #region CustomerValidateAsync

        /// <summary>
        /// Nghiệp vụ validate riêng cho từng entity (nếu có)
        /// </summary>
        /// <param name="entity"></param>

        protected virtual Task CustomerValidateAsync(T entity)
        {
            // Các validate riêng cho từng entity sẽ được ghi đè trong các Service con
            return Task.CompletedTask;
        }

        #endregion CustomerValidateAsync

        #region BeforeCreate

        /// <summary>
        /// Các nghiệp vụ khác trước khi thêm mới của service con (nếu có)
        /// </summary>
        /// <param name="entity">Entity chứa thông tin cần xử lý</param>
        protected virtual void BeforeCreate(T entity)
        {
            // Các nghiệp vụ khác trước khi thêm mới của service con (nếu có)
        }

        #endregion BeforeCreate

        #region AssignPrimaryKeyForCreate

        /// <summary>
        /// Gán giá trị cho khóa chính trước khi thêm mới
        /// </summary>
        /// <param name="entity">Entity chứa thông tin đối tượng</param>
        /// <exception cref="Exception">Khóa chính không tồn tại (không được đánh dấu trong entity)</exception>
        private void AssignPrimaryKeyForCreate(T entity)
        {
            var primaryKeyProperty = typeof(T).GetProperties()
                .FirstOrDefault(prop => Attribute.IsDefined(prop, typeof(MISAPrimaryKeyAttribute)));
            if (primaryKeyProperty == null)
            {
                throw new Exception("Không tìm thấy khóa chính để tự động sinh giá trị.");
            }
            primaryKeyProperty.SetValue(entity, Guid.NewGuid());
        }

        #endregion AssignPrimaryKeyForCreate

        #region DeleteAsync

        public async Task DeleteAsync(Guid id)
        {
            // Validate khóa chính
            ValidatePrimaryKey(id);
            var result = await _baseRepository.DeleteAsync(id);
            if (result == 0) { throw new Exception("Xóa dữ liệu thất bại."); }
        }

        #endregion DeleteAsync

        #region UpdateAsync

        public async Task<T> UpdateAsync(Guid id, T entity)
        {
            // Thực hiện validate dữ liệu
            ValidateEntity(entity);

            // Gán quá chính cho entity
            AssignPrimaryKeyForUpdate(id, entity);

            // Thêm validate riêng cho từng entity nếu cần
            await CustomerValidateAsync(entity);

            // Giống create
            BeforeUpdate(entity);

            // Thực hiện thêm mới dữ liệu thông qua Repository
            var result = await _baseRepository.UpdateAsync(id, entity);
            if (result == 0)
            {
                throw new Exception("Cập nhật dữ liệu thất bại.");
            }
            return entity;
        }

        #endregion UpdateAsync

        #region AssignPrimaryKeyForUpdate

        /// <summary>
        /// Gán id cho entity với hàm update
        /// </summary>
        /// <param name="id">id của đối tượng truyền vào</param>
        /// <param name="entity">Entity chứa thông tin đối tượng</param>
        private void AssignPrimaryKeyForUpdate(Guid id, T entity)
        {
            // Lấy ra trường id
            var primaryKeyProperty = typeof(T).GetProperties().FirstOrDefault(p => Attribute.IsDefined(p, typeof(MISAPrimaryKeyAttribute)));

            if (primaryKeyProperty == null)
                throw new Exception("Không tìm thấy khóa chính để gán giá trị.");

            primaryKeyProperty.SetValue(entity, id);
        }

        #endregion AssignPrimaryKeyForUpdate

        #region BeforeUpdate

        /// <summary>
        /// Các nghiệp vụ khác trước khi sửa bản ghi của service con (nếu có)
        /// </summary>
        /// <param name="entity">Entity chứa thông tin cần xử lý</param>
        protected virtual void BeforeUpdate(T entity)
        {
            // Các nghiệp vụ khác nếu cần
        }

        #endregion BeforeUpdate

        #region ValidatePrimaryKey

        /// <summary>
        /// Validate khóa chính
        /// </summary>
        /// <param name="id">Khóa chính</param>
        /// <exception cref="ValidateException">Nếu không tồn tại thì throw</exception>
        private void ValidatePrimaryKey(Guid id)
        {
            if (id == Guid.Empty)
                throw new ValidateException(new Dictionary<string, string>
                {
                    { "PrimaryKey", "Khóa chính không hợp lệ, vui lòng kiểm tra lại thông tin." }
                });
        }

        #endregion ValidatePrimaryKey
    }
}