using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WorkShiftManagement.Core.DTOs.WorkShift
{
    public class WorkShiftFilter
    {
        /// <summary>
        /// Từ khóa lọc theo mã ca làm việc, tên ca làm việc, người tạo, người sửa
        /// </summary>
        public string? Keyword { get; set; }

        /// <summary>
        /// Trạng thái hoạt động:
        /// true: Đang hoạt động;
        /// false: Ngừng hoạt động;
        /// null: Không lọc
        /// </summary>
        public bool? IsActive { get; set; }

        /// <summary>
        /// Thời gian làm việc
        /// null: Không lọc
        /// </summary>
        public decimal? WorkingTime { get; set; }

        /// <summary>
        /// Trang hiện tại (bắt đầu từ 1)
        /// </summary>
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// Số bản ghi trên một trang
        /// </summary>
        public int PageSize { get; set; } = 20;
    }
}