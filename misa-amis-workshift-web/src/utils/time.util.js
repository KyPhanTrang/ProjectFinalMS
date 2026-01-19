/**
 * Chuyển đổi thời gian dạng "HH:mm" sang tổng số phút
 *
 * @param {string} time - Thời gian cần chuẩn hóa (định dạng "HH:mm")
 * @returns {number} Tổng số phút (hour * 60 + minute)
 */
export function toMinutes(time) {
    const [hour, minute] = time.split(':').map(Number)
    return hour * 60 + minute
}

/**
 * Chuẩn hóa thời gian theo mốc bắt đầu ca làm việc
 * Dùng cho các mốc nằm trong ca (breakStart, breakEnd, endTime)
 *
 * Nếu thời gian nhỏ hơn thời điểm bắt đầu ca → coi là qua ngày,
 * sẽ cộng thêm 1440 phút (1 ngày)
 *
 * @param {string} time - Thời gian cần chuẩn hóa (định dạng "HH:mm")
 * @param {string} shiftStart - Thời điểm bắt đầu ca làm việc (startTime)
 * @returns {number} Thời gian đã chuẩn hóa theo phút
 */
export function normalizeInShift(time, shiftStart) {
    // Quy ước: startTime <= breakStart <= breakEnd <= endTime
    const t = toMinutes(time)
    const s = toMinutes(shiftStart)

    return t < s ? t + 1440 : t
}

/**
 * Tính tổng thời gian làm việc của ca
 * Hỗ trợ ca làm việc qua ngày (ví dụ 22:00 → 06:00)
 *
 * @param {string} startTime - Thời gian bắt đầu ca (HH:mm)
 * @param {string} endTime - Thời gian kết thúc ca (HH:mm)
 * @returns {number} Tổng số giờ làm việc (làm tròn 2 số sau dấu ,)
 */
export function calcWorkingTime(startTime, endTime) {
    const s = toMinutes(startTime)
    let e = toMinutes(endTime)

    // Nếu kết thúc nhỏ hơn bắt đầu → ca qua ngày
    if (e < s) e += 1440

    return round2((e - s) / 60)
}

/**
 * Tính tổng thời gian nghỉ trong ca làm việc
 *
 * @param {string|null} breakStart - Thời gian bắt đầu nghỉ (HH:mm)
 * @param {string|null} breakEnd - Thời gian kết thúc nghỉ (HH:mm)
 * @param {string} startTime - Thời gian bắt đầu ca (HH:mm)
 * @returns {number} Số giờ nghỉ (làm tròn 2 số sau dấu ,)
 */
export function calcBreakingTime(breakStart, breakEnd, startTime) {
    // Không có thời gian nghỉ
    if (!breakStart || !breakEnd) return 0

    const bs = normalizeInShift(breakStart, startTime)
    const be = normalizeInShift(breakEnd, startTime)

    return round2((be - bs) / 60)
}

// Làm tròn sau thập phân hai chữ số
function round2(value) {
    return Math.round((value + Number.EPSILON) * 100) / 100
}


/**
 * Chuyển đổi chuỗi thời gian từ UI sang định dạng TimeSpan cho backend
 *
 * Quy ước:
 * - UI sử dụng định dạng: HH:mm
 * - Backend yêu cầu:     HH:mm:ss
 *
 * Xử lý:
 * - Nếu time rỗng → trả về null (phù hợp khi không nhập giờ nghỉ)
 * - Nếu đúng định dạng HH:mm → tự động thêm ":00" giây
 * - Nếu đã có giây (HH:mm:ss) → giữ nguyên
 *
 * @param {string|null} time - Chuỗi thời gian từ input (HH:mm | HH:mm:ss)
 * @returns {string|null} Chuỗi thời gian chuẩn TimeSpan hoặc null
 */
export function toTimeSpan(time) {
    // Không có giá trị → không gửi backend
    if (!time) return null

    // Trường hợp chỉ có giờ và phút (HH:mm) → thêm giây
    if (/^\d{2}:\d{2}$/.test(time)) {
        return `${time}:00`
    }

    // Trường hợp đã đúng định dạng HH:mm:ss → giữ nguyên
    return time
}
