using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.WorkShiftManagement.Core.DTOs.WorkShift;
using MISA.WorkShiftManagement.Core.Entities;
using MISA.WorkShiftManagement.Core.Interfaces.Repositories;
using MISA.WorkShiftManagement.Core.Interfaces.Services;

namespace MISA.WorkShiftManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkShiftsController : ControllerBase
    {
        private readonly IWorkShiftRepository _workShiftRepo;
        private readonly IWorkShiftService _workShiftService;

        public WorkShiftsController(IWorkShiftRepository workShiftRepo, IWorkShiftService workShiftService)
        {
            _workShiftRepo = workShiftRepo;
            _workShiftService = workShiftService;
        }

        /// <summary>
        /// Lấy ca làm việc theo Id
        /// </summary>
        /// <param name="id">Id của ca cần lấy thông tin</param>
        /// <returns>Ca làm việc (status code: 200)</returns>
        /// CreatedBy: THPHU (10/01/2026)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var data = await _workShiftRepo.GetByIdAsync(id);
            return Ok(data);
        }

        /// <summary>
        /// Tạo mới ca làm việc
        /// </summary>
        /// <param name="workShift">Entity chứa thông tin đối tượng</param>
        /// <returns>Mã 201 cùng với số dòng được thêm trong database</returns>
        /// CreatedBy: THPHU (11/01/2026)
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] WorkShift workShift)
        {
            workShift.CreatedBy = "Trương Huy Phú";
            var res = await _workShiftService.CreateAsync(workShift);
            return StatusCode(201, res);
        }

        /// <summary>
        /// Cập nhật ca làm việc
        /// </summary>
        /// <param name="id">Id của ca cần cập nhật</param>
        /// <param name="workShift">Entity chứa thông tin cần cập nhật</param>
        /// <returns>Mã 200 cùng với số dòng được thêm trong database</returns>
        /// CreatedBy: THPHU (11/01/2026)
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] WorkShift workShift)
        {
            // Gán mặc định người sửa
            workShift.ModifiedBy = "Trương Huy Phú";
            var res = await _workShiftService.UpdateAsync(id, workShift);
            return Ok(res);
        }

        /// <summary>
        /// Xóa ca làm việc
        /// </summary>
        /// <param name="id">Id ca cần xóa</param>
        /// <returns>Mã 204No Content</returns>
        /// CreatedBy: THPHU (11/01/2026)
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _workShiftService.DeleteAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Lấy tất cả ca làm việc có phân trang và lọc
        /// </summary>
        /// <param name="filter">Các tham số (pageSize, pageIndex, keyword, workingTime, isActive)</param>
        /// <returns>Danh sách đã paging và lọc (nếu có lọc)</returns>
        /// CreatedBy: THPHU (11/01/2026)
        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] WorkShiftFilter filter)
        {
            var data = await _workShiftRepo.GetAllPaging(filter);
            return Ok(data);
        }

        /// <summary>
        /// Xóa nhiều ca làm việc một lần
        /// </summary>
        /// <param name="ids">Danh sách id ca làm việc cần xóa</param>
        /// <returns>Số dòng đã xóa</returns>
        /// CreatedBy: THPHU (11/01/2026)
        [HttpPost("delete-batch")]
        public async Task<IActionResult> DeleteMany([FromBody] IEnumerable<Guid> ids)
        {
            await _workShiftRepo.DeleteManyAsync(ids);
            return NoContent();
        }

        /// <summary>
        /// Kiểm tra trùng mã ca làm việc
        /// </summary>
        /// <param name="payload">Thông tin mã ca và id (nếu có)</param>
        /// <returns>true nếu trùng, false nếu không trùng</returns>
        /// CreatedBy: THPHU (16/01/2026)
        [HttpPost("check-duplicate")]
        public async Task<IActionResult> CheckDuplicate([FromBody] CheckShiftCodeDuplicateDto payload)
        {
            if (string.IsNullOrWhiteSpace(payload.ShiftCode))
            {
                return BadRequest("Mã ca làm việc không được để trống.");
            }

            var isExists = await _workShiftRepo
                .CheckShiftCodeExistsAsync(payload.ShiftCode, payload.ShiftId);

            return Ok(isExists);
        }
    }
}