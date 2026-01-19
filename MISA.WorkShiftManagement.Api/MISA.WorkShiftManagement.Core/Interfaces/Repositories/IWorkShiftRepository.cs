using MISA.WorkShiftManagement.Core.DTOs;
using MISA.WorkShiftManagement.Core.DTOs.WorkShift;
using MISA.WorkShiftManagement.Core.Entities;

namespace MISA.WorkShiftManagement.Core.Interfaces.Repositories
{
    /// <summary>
    /// Các phương thức riêng cho Repository của ca làm việc
    /// </summary>
    /// CreatedBy: THPHU (09/01/2026)
    public interface IWorkShiftRepository : IBaseRepository<WorkShift>
    {
        /// <summary>
        /// Lấy danh sách ca làm việc có phân trang và điều kiện lọc
        /// </summary>
        /// <param name="filter">Điều kiện lọc</param>
        /// <returns> Danh sách ca làm việc theo trang và tổng số bản ghi</returns>
        /// CreatedBy: THPHU (09/01/2026)
        Task<PagingResult<WorkShift>> GetAllPaging(WorkShiftFilter filter);

        /// <summary>
        /// Xóa nhiều ca làm việc
        /// </summary>
        /// <param name="ids">Danh sách id ca làm việc cần xóa</param>
        /// <returns>Số bản ghi đã được xóa</returns>
        /// CreatedBy: THPHU (09/01/2026)
        Task<int> DeleteManyAsync(IEnumerable<Guid> ids);

        /// <summary>
        /// Check mã ca làm việc đã tồn tại hay chưa
        /// </summary>
        /// <param name="shiftCode">Mã ca làm việc</param>
        /// <returns>True - mã đã tồn tại, false - mã hợp lệ</returns>
        /// CreatedBy: THPHU (09/01/2026)
        Task<bool> CheckShiftCodeExistsAsync(string shiftCode, Guid? shiftId);
    }
}