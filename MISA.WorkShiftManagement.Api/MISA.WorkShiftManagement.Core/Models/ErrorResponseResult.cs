using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WorkShiftManagement.Core.Models
{
    /// <summary>
    /// Đối tượng chứa thông tin lỗi trả về cho client (lỗi validate, lỗi db)
    /// </summary>
    /// CreateBy: THPHU(12/01/2026)
    public class ErrorResponseResult
    {
        public ErrorResponseResult()
        {
            Timestamp = DateTime.Now;
        }

        // Mã trả về (400 phía client, 500 server (backend))
        public string? Code { get; set; }

        // Thông điệp trả về
        public string? Message { get; set; }

        // Dữ liệu lỗi (trường, thông tin lỗi)
        public object? Data { get; set; }

        // Ngày phát sinh lỗi
        public DateTime? Timestamp { get; set; }
    }
}