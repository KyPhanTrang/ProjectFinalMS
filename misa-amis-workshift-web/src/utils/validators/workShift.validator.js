import { required, maxLength } from "./base.validator"
import { validateBreakTime } from "./time.validator"

/**
 * Validate dữ liệu ca làm việc (Work Shift)
 *
 * Thực hiện:
 * - Validate các trường bắt buộc
 * - Validate độ dài chuỗi
 * - Validate logic thời gian nghỉ trong ca
 *
 * @param {Object} model - Dữ liệu ca làm việc
 * @param {string} model.shiftCode - Mã ca
 * @param {string} model.shiftName - Tên ca
 * @param {string} model.startTime - Thời gian bắt đầu ca (HH:mm)
 * @param {string} model.endTime - Thời gian kết thúc ca (HH:mm)
 * @param {string|null} model.breakStartTime - Thời gian bắt đầu nghỉ
 * @param {string|null} model.breakEndTime - Thời gian kết thúc nghỉ
 *
 * @returns {Object} errors
 * - Object chứa danh sách lỗi theo từng field
 * - Object rỗng nếu dữ liệu hợp lệ
 */
export function validateWorkShift(model) {
    const errors = {}

    // Validate mã ca
    errors.shiftCode =
        required('Mã ca', model.shiftCode) ||
        maxLength('Mã ca', model.shiftCode, 20)

    // Validate tên ca
    errors.shiftName =
        required('Tên ca', model.shiftName) ||
        maxLength('Tên ca', model.shiftName, 50)

    // Validate thời gian bắt đầu & kết thúc ca
    errors.startTime = required('Giờ vào ca', model.startTime)
    errors.endTime = required('Giờ hết ca', model.endTime)

    // Validate logic thời gian nghỉ trong ca
    const breakError = validateBreakTime({
        startTime: model.startTime,
        endTime: model.endTime,
        breakStart: model.breakStartTime,
        breakEnd: model.breakEndTime
    })

    // Nếu có lỗi thời gian nghỉ → gán lỗi cho cả 2 field
    if (breakError) {
        errors.breakStartTime = breakError
        errors.breakEndTime = breakError
    }

    // Loại bỏ các field không có lỗi (null / undefined / empty)
    Object.keys(errors).forEach(key => {
        if (!errors[key]) delete errors[key]
    })

    return errors
}
