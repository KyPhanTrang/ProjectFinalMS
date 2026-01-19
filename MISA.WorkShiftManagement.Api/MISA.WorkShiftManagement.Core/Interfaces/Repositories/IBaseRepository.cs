namespace MISA.WorkShiftManagement.Core.Interfaces.Repositories
{
    /// <summary>
    /// Định nghĩa các phương thức thao tác dữ liệu cơ bản cho Repository
    /// </summary>
    /// <typeparam name="T">Kiểu dữ liệu được truyền vào</typeparam>
    /// CreatedBy: THPHU (09/01/2026)
    public interface IBaseRepository<T>
    {
        /// <summary>
        /// Thêm mới một bản ghi vào database
        /// </summary>
        /// <param name="entity">Entity chứa dữ liệu cần thêm</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// CreatedBy: THPHU (09/01/2026)
        Task<int> InsertAsync(T entity);

        /// <summary>
        /// Cập nhật thông tin một bản ghi trong database
        /// </summary>
        /// <param name="id">Khóa chính của bản ghi</param>
        /// <param name="entity">Entity chứa dữ liệu cần cập nhật</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// CreatedBy: THPHU (09/01/2026)
        Task<int> UpdateAsync(Guid id, T entity);

        /// <summary>
        /// Xóa một bản ghi theo Id
        /// </summary>
        /// <param name="id">Khóa chính của bản ghi cần xóa</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// CreatedBy: THPHU (09/01/2026)
        Task<int> DeleteAsync(Guid id);

        /// <summary>
        /// Lấy một bản ghi theo Id
        /// </summary>
        /// <param name="id">Khóa chính của bản ghi</param>
        /// <returns>Entity tương ứng hoặc null nếu không tồn tại</returns>
        /// CreatedBy: THPHU (09/01/2026)
        Task<T?> GetByIdAsync(Guid id);
    }
}