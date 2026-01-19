using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WorkShiftManagement.Core.DTOs.WorkShift
{
    /// <summary>
    /// DTO chứa thông tin gửi đi khi cần check trùng mã ca (id, mã ca)
    /// </summary>
    /// CreatedBy: THPHU(16/01/2026)
    public class CheckShiftCodeDuplicateDto
    {
        /// <summary>
        /// Mã ca làm việc
        /// </summary>
        public string ShiftCode { get; set; }

        /// <summary>
        /// Id ca làm việc (null nếu thêm mới)
        /// </summary>
        public Guid? ShiftId { get; set; }
    }
}