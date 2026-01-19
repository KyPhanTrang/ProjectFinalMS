using MISA.WorkShiftManagement.Core.Entities;
using MISA.WorkShiftManagement.Core.Exceptions;
using MISA.WorkShiftManagement.Core.Interfaces.Repositories;
using MISA.WorkShiftManagement.Core.Interfaces.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MISA.WorkShiftManagement.Core.Services
{
    /// <summary>
    /// Xử lý nghiệp vụ riêng cho WorkShift
    /// </summary>
    /// CreatedBy: THPHU(12/01/2026)
    public class WorkShiftService : BaseService<WorkShift>, IWorkShiftService
    {
        private readonly IWorkShiftRepository _workShiftRepository;

        public WorkShiftService(IWorkShiftRepository workShiftRepository) : base(workShiftRepository)
        {
            _workShiftRepository = workShiftRepository;
        }

        #region CustomerValidateAsync

        /// <summary>
        /// Thêm validate riêng (nghiệp vụ riêng) cho WorkShift
        /// </summary>
        /// <param name="entity">Entity chứa thông tin cần xử lý</param>
        /// <exception cref="ValidateException">Validate thất bại cùng thông tin</exception>
        protected override async Task CustomerValidateAsync(WorkShift entity)
        {
            var errorsValidate = new Dictionary<string, string>();

            // Bắt buộc phải có thời gian bắt đầu vào ca
            if (!entity.StartTime.HasValue)
                errorsValidate.Add("Start time", "Giờ vào ca là bắt buộc.");

            // Bắt buộc phải có thời gian kết thúc ca
            if (!entity.EndTime.HasValue)
                errorsValidate.Add("End time", "Giờ kết thúc ca là bắt buộc.");

            // Validate mã ca làm việc không được trùng
            var isExistShiftCode = await _workShiftRepository.CheckShiftCodeExistsAsync(entity.ShiftCode, entity.ShiftId);
            if (isExistShiftCode)
            {
                errorsValidate.Add("ShiftCode", "Mã ca làm việc đã tồn tại trong hệ thống.");
            }

            // Giới hạn ký tự (ShiftCode.length <= 20, ShiftName.length <= 50)
            if (entity.ShiftCode.Length > 20)
            {
                errorsValidate.Add("ShiftCode", "Mã ca vượt quá giới hạn ký tự cho phép (>20 ký tự).");
            }

            if (entity.ShiftName?.Length > 50)
            {
                errorsValidate.Add("ShiftName", "Tên ca vượt quá giới hạn ký tự cho phép (>50 ký tự).");
            }

            // Ném ra dữ liệu nếu có bất kỳ validate nào thất bại
            if (errorsValidate.Any())
                throw new ValidateException(errorsValidate);
        }

        #endregion CustomerValidateAsync

        #region BeforeCreate

        /// <summary>
        /// Trước khi tạo cần tính toán và xử lý các thời gian (tổng thời gian làm việc, thời gian nghỉ (nếu có))
        /// Đồng thời validate thời gian nghỉ (nếu có)
        /// </summary>
        /// <param name="entity"></param>
        protected override void BeforeCreate(WorkShift entity)
        {
            ValidateBeforeCreateOrUpdate(entity);
        }

        #endregion BeforeCreate

        #region ValidateBeforeCreateOrUpdate

        /// <summary>
        /// Validate chung cho trước khi thêm và cập nhật, thêm vào đó tính toán lại các trường không được nhập (backend tự động tính và lưu)
        /// </summary>
        /// <param name="entity"></param>
        /// <exception cref="ValidateException">Lỗi nếu các trường BreakStartTime, BreakEndTime không nằm trong điều kiện</exception>
        private void ValidateBeforeCreateOrUpdate(WorkShift entity)
        {
            //Giả định: Thời gian làm việc của một ca không vượt quá 24 giờ.

            // Nếu EndTime <= StartTime
            // → Ca làm việc được hiểu là kéo dài sang ngày hôm sau.

            // Nếu EndTime > StartTime
            // → Ca làm việc trong cùng một ngày.

            // ======= Validate list
            var errorsValidate = new Dictionary<string, string>();

            // ======== Chuẩn hóa thời gian (breakStartTime, breakEndTime có thể null)
            var startTime = entity.StartTime;
            var endTime = NormalizeTime(entity.EndTime.Value, startTime.Value);

            var breakStartTime = entity.BreakStartTime.HasValue
                ? NormalizeTime(entity.BreakStartTime.Value, startTime.Value)
                : (TimeSpan?)null;
            var breakEndTime = entity.BreakEndTime.HasValue
                ? NormalizeTime(entity.BreakEndTime.Value, startTime.Value)
                : (TimeSpan?)null;

            // ======== Validate Break Time (breakStart, breakEnd)
            if ((breakStartTime.HasValue && !breakEndTime.HasValue) || (!breakStartTime.HasValue && breakEndTime.HasValue))
            {
                errorsValidate.Add("BreakTime", "Phải nhập đầy đủ thời gian bắt đầu và kết thúc nghỉ.");
            }

            if (breakStartTime.HasValue && breakEndTime.HasValue)
            {
                // Rule (startTime <= breakStartTime <= breakEndTime <= endTime)
                if (breakStartTime < startTime)
                    errorsValidate.Add(nameof(entity.BreakStartTime), "Giờ bắt đầu nghỉ phải nằm trong ca làm.");

                if (breakEndTime > endTime)
                    errorsValidate.Add(nameof(entity.BreakEndTime), "Giờ kết thúc nghỉ phải nằm trong ca làm.");

                if (breakStartTime > breakEndTime)
                    errorsValidate.Add("BreakTime", "Giờ nghỉ không hợp lệ.");
            }

            if (errorsValidate.Any())
                throw new ValidateException(errorsValidate);

            // ===== Tính toán nếu pass hết ở trên
            // Tổng thời gian làm việc
            var totalShiftTime = endTime - startTime;

            // Tính thời gian nghỉ (nếu không có thì là 00:00)
            var breakingTime = TimeSpan.Zero;
            if (breakStartTime.HasValue && breakEndTime.HasValue)
            {
                breakingTime = breakEndTime.Value - breakStartTime.Value;
            }

            // Thời gian làm việc (tổng thời gian làm - tổng thời gian nghỉ)
            var workingTime = totalShiftTime.Value - breakingTime;

            // Format lại thành decimal(4,2)
            entity.BreakingTime = Math.Round((decimal)breakingTime.TotalHours, 2);
            entity.WorkingTime = Math.Round((decimal)workingTime.TotalHours, 2);
        }

        #endregion ValidateBeforeCreateOrUpdate

        #region BeforeUpdate

        /// <summary>
        /// Trước khi tạo cần tính toán và xử lý các thời gian (tổng thời gian làm việc, thời gian nghỉ (nếu có))
        /// Đồng thời validate thời gian nghỉ (nếu có)
        /// </summary>
        /// <param name="entity"></param>
        protected override void BeforeUpdate(WorkShift entity)
        {
            ValidateBeforeCreateOrUpdate(entity);
        }

        #endregion BeforeUpdate

        #region NormalizeTime

        /// <summary>
        /// Hàm chuẩn hóa thời gian
        /// Nếu là ca ngày thì thời gian sẽ bình thường (24 giờ)
        /// Còn nếu là ca đêm thì timeline sẽ là (48 giờ)
        /// </summary>
        /// <param name="time">Biến cần chuyển đổi</param>
        /// <param name="startTime">Thời gian bắt đầu ca làm việc</param>
        /// <returns></returns>
        private TimeSpan NormalizeTime(TimeSpan time, TimeSpan startTime)
        {
            var normalizedTime = TimeSpan.Zero;
            // Nếu time > startTime thì giữ nguyên, ngược lại cộng thêm 24 giờ, để đúng với logic ca làm việc (startTime < breakStart < breakEnd < endTime)
            normalizedTime = (time < startTime) ? time.Add(new TimeSpan(24, 0, 0)) : time;
            return normalizedTime;
        }

        #endregion NormalizeTime
    }
}