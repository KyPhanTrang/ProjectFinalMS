namespace MISA.WorkShiftManagement.Core.MISAAttribute
{
    /// <summary>
    /// Những thông tin bắt buộc nhập
    /// </summary>
    /// CreatedBy: THPHU (11/01/2026)
    // Chỉ dùng cho thuộc tính
    [AttributeUsage(AttributeTargets.Property)]
    public class MISARequiredAttribute : Attribute
    {
    }
}