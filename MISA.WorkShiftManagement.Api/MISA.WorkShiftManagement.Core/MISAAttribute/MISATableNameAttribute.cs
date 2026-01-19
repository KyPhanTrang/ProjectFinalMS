namespace MISA.WorkShiftManagement.Core.MISAAttribute
{
    /// <summary>
    /// Class định nghĩa tên bảng trong database
    /// </summary>
    /// CreatedBy: THPHU (09/01/2026)
    [AttributeUsage(AttributeTargets.Class)]
    public class MISATableNameAttribute : Attribute
    {
        public string TableName { get; set; }

        public MISATableNameAttribute(string tableName)
        {
            TableName = tableName;
        }
    }
}