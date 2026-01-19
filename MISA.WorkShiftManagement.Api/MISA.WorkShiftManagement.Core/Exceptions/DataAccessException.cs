namespace MISA.WorkShiftManagement.Core.Exceptions
{
    /// <summary>
    /// Ngoại lệ phát sinh tại tầng truy cập dữ liệu (Repository / Infrastructure).
    /// Được sử dụng để bao bọc (wrap) các ngoại lệ kỹ thuật liên quan đến
    /// cơ sở dữ liệu hoặc nguồn dữ liệu bên ngoài.
    /// </summary>
    /// <remarks>
    /// Ngoại lệ này không chứa logic nghiệp vụ và không nên được
    /// xử lý trực tiếp tại Controller.
    /// Service layer có trách nhiệm chuyển đổi ngoại lệ này
    /// sang ngoại lệ nghiệp vụ phù hợp.
    /// </remarks>
    /// CreatedBy: THPHU (11/01/2026)
    public class DataAccessException : Exception
    {
        public DataAccessException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}