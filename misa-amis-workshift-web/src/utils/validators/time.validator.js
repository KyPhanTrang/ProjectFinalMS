import { toMinutes, normalizeInShift } from "@/utils/time.util"

/**
 * Kiểm tra tính hợp lệ của thời gian nghỉ trong ca làm việc
 *
 * Các quy tắc kiểm tra:
 * 1. Chỉ được nhập đồng thời cả thời gian bắt đầu và kết thúc nghỉ
 * 2. Thời gian nghỉ phải nằm trong khoảng thời gian làm việc
 * 3. Giờ bắt đầu nghỉ phải nhỏ hơn giờ kết thúc nghỉ
 * 4. Hỗ trợ ca làm việc qua ngày (ví dụ: 22:00 → 06:00)
 *
 * @param {Object} params - Thông tin ca làm việc
 * @param {string} params.startTime - Thời gian bắt đầu ca (HH:mm)
 * @param {string} params.endTime - Thời gian kết thúc ca (HH:mm)
 * @param {string|null} params.breakStart - Thời gian bắt đầu nghỉ (HH:mm)
 * @param {string|null} params.breakEnd - Thời gian kết thúc nghỉ (HH:mm)
 *
 * @returns {string}
 * - Chuỗi rỗng nếu hợp lệ
 * - Thông báo lỗi nếu dữ liệu không hợp lệ
 */
export function validateBreakTime({
    startTime,
    endTime,
    breakStart,
    breakEnd
}) {
    // Chỉ nhập một trong hai mốc nghỉ (thiếu đầu hoặc cuối)
    if ((breakStart && !breakEnd) || (!breakStart && breakEnd)) {
        return "Phải nhập đầy đủ thời gian bắt đầu và kết thúc nghỉ."
    }

    // Không nhập thời gian nghỉ → hợp lệ
    if (!breakStart && !breakEnd) return ""

    // Chuẩn hóa thời gian bắt đầu và kết thúc ca làm việc (tính theo phút)
    const s = toMinutes(startTime)
    const e = toMinutes(endTime) <= s
        ? toMinutes(endTime) + 1440 // Ca làm việc qua ngày
        : toMinutes(endTime)

    // Chuẩn hóa thời gian nghỉ theo mốc bắt đầu ca
    const bs = normalizeInShift(breakStart, startTime)
    const be = normalizeInShift(breakEnd, startTime)

    // Giờ bắt đầu nghỉ phải nhỏ hơn giờ kết thúc nghỉ
    if (bs >= be)
        return "Thời gian bắt đầu nghỉ phải nhỏ hơn giờ kết thúc nghỉ."

    // Thời gian nghỉ phải nằm hoàn toàn trong thời gian làm việc
    if (bs < s)
        return 'Thời gian bắt đầu nghỉ giữa ca phải nằm trong khoảng thời gian tính từ giờ vào ca đến giờ hết ca. Vui lòng kiểm tra lại.'
    if (be > e)
        return "Thời gian kết thúc nghỉ giữa ca phải nằm trong khoảng thời gian tính từ giờ vào ca đến giờ hết ca. Vui lòng kiểm tra lại."

    return ""
}
