using MISA.WorkShiftManagement.Core.MISAAttribute;

namespace MISA.WorkShiftManagement.Core.Entities
{
    /// <summary>
    /// Thông tin ca làm việc
    /// </summary>
    /// CreatedBy: THPHU (09/01/2026)

    // Đặt tên cho bảng trong database (work_shift)
    [MISATableName("work_shift")]
    public class WorkShift
    {
        // Id ca làm việc
        [MISAPrimaryKey]
        [MISAColumnName("shift_id")]
        public Guid? ShiftId { get; set; }

        // Mã ca làm việc
        [MISARequired]
        [MISAColumnName("shift_code")]
        public string? ShiftCode { get; set; }

        // Tên ca làm việc
        [MISARequired]
        [MISAColumnName("shift_name")]
        public string? ShiftName { get; set; }

        // Thời gian bắt đầu ca làm việc
        [MISARequired]
        [MISAColumnName("start_time")]
        public TimeSpan? StartTime { get; set; }

        // Thời gian kết thúc ca làm việc
        [MISARequired]
        [MISAColumnName("end_time")]
        public TimeSpan? EndTime { get; set; }

        // Thời gian nghỉ giữa ca
        [MISAColumnName("break_start_time")]
        public TimeSpan? BreakStartTime { get; set; }

        // Thời gian kết thúc nghỉ giữa ca
        [MISAColumnName("break_end_time")]
        public TimeSpan? BreakEndTime { get; set; }

        // Tổng số giờ làm việc trong ca
        [MISAColumnName("working_time")]
        public decimal? WorkingTime { get; set; }

        // Tổng số giờ nghỉ giữa ca
        [MISAColumnName("breaking_time")]
        public decimal? BreakingTime { get; set; }

        // Trạng thái ca làm việc (true: Đang hoạt động, false: Ngừng hoạt động)
        [MISAColumnName("is_active")]
        public bool IsActive { get; set; } = true;

        // Mô tả ca làm việc
        [MISAColumnName("description")]
        public string? Description { get; set; }

        // Người tạo ca làm việc
        [MISAIgnoreUpdate]
        [MISAColumnName("created_by")]
        public string? CreatedBy { get; set; }

        // Ngày tạo ca làm việc'
        [MISAIgnoreUpdate]
        [MISACreatedDate]
        [MISAColumnName("created_date")]
        public DateTime? CreatedDate { get; set; }

        // Người sửa ca làm việc
        [MISAIgnoreInsert]
        [MISAColumnName("modified_by")]
        public string? ModifiedBy { get; set; }

        // Ngày sửa ca làm việc
        [MISAIgnoreInsert]
        [MISAModifiedDate]
        [MISAColumnName("modified_date")]
        public DateTime? ModifiedDate { get; set; }
    }
}