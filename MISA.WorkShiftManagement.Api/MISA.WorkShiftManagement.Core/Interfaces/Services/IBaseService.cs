namespace MISA.WorkShiftManagement.Core.Interfaces.Services
{
    /// <summary>
    /// Định nghĩa những phương thức chung cho Service
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// CreatedBy: THPHU (09/01/2026)
    public interface IBaseService<T>
    {
        /// <summary>
        /// Tạo một thực thể mới trong kho dữ liệu.
        /// </summary>
        /// <param name="entity">Entity chứa dữ liệu cần thêm (không được null)</param>
        /// <returns>Thực thể đã tạo</returns>
        /// CreatedBy: THPHU (09/01/2026)
        Task<T> CreateAsync(T entity);

        /// <summary>
        /// Cập nhật một thực thể hiện có trong kho dữ liệu.
        /// </summary>
        /// <param name="id">Khóa chính của bản ghi cần cập nhật</param>
        /// <param name="entity">Entity chứa dữ liệu cần cập nhật(không được null)</param>
        /// <returns>Thực thể đã tạo</returns>
        /// CreatedBy: THPHU (09/01/2026)
        Task<T> UpdateAsync(Guid id, T entity);

        /// <summary>
        /// Xóa một thực thể khỏi kho dữ liệu dựa trên Khóa chính.
        /// </summary>
        /// <param name="id">Khóa chính của bản ghi cần xóa</param>
        /// <exception cref="NotFoundException"> Ném ra khi không tìm thấy bản ghi</exception>
        /// CreatedBy: THPHU (09/01/2026)
        Task DeleteAsync(Guid id);
    }
}