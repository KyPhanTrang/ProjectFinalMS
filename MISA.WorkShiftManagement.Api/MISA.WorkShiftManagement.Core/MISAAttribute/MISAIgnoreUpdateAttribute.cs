namespace MISA.WorkShiftManagement.Core.MISAAttribute
{
    /// <summary>
    /// Class định nghĩa thuộc tính bỏ qua khi thao tác sửa với database
    /// </summary>
    /// CreatedBy: THPHU (11/01/2026)
    [AttributeUsage(AttributeTargets.Property)]
    public class MISAIgnoreUpdateAttribute : Attribute
    {
    }
}