namespace MISA.WorkShiftManagement.Core.MISAAttribute
{
    /// <summary>
    /// Class định nghĩa tên cột trong database
    /// </summary>
    /// CreatedBy: THPHU (10/01/2026)
    [AttributeUsage(AttributeTargets.Property)]
    public class MISAColumnNameAttribute : Attribute
    {
        public string ColumnName { get; set; }

        public MISAColumnNameAttribute(string columnName)
        {
            ColumnName = columnName;
        }
    }
}