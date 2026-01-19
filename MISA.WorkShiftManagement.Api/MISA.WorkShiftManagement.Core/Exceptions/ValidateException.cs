using System.Collections;

namespace MISA.WorkShiftManagement.Core.Exceptions
{
    /// <summary>
    /// Xử lý ngoại lệ validate dữ liệu
    /// </summary>
    /// CreatedBy: THPHU (11/01/2026)
    public class ValidateException : Exception
    {
        // data lỗi validate
        private IDictionary _data = new Dictionary<string, string>();

        // Thông báo chung
        private string _message = "Dữ liệu không hợp lệ. Vui lòng kiểm tra lại thông tin.";

        public ValidateException(IDictionary data)
        {
            _data = data;
        }

        public override string Message => _message;
        public override IDictionary Data => _data;
    }
}