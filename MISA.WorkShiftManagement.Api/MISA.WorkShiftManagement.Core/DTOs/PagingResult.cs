using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WorkShiftManagement.Core.DTOs
{
    /// <summary>
    /// Trả về dữ liệu paging và total record
    /// </summary>
    /// CreateBy: THPHU (12/01/2026)
    public class PagingResult<T>
    {
        // Data động kiểu gì cũng được
        public IEnumerable<T> Data { get; set; } = Enumerable.Empty<T>();

        // Tổng số bản ghi
        public int TotalRecord { get; set; }
    }
}