/**
 * Format lại dữ liệu để hiển thị
 * 
 * @param {*} value (string) - Giá trị cần format
 * @returns {string}
 * - Value Nếu tồn tại giá trị
 * - "-" nếu không tồn tại giá trị
 */
export function formatText(value) {
    if (value === null || value === undefined || value === '')
        return '-';
    return value
}

/**
 * Format lại những trường có kiểu dateTime
 * 
 * @param {*} value (date) - Giá trị date muốn format
 * @returns {string}
 * - DateTime định dạng Việt Nam (ngày/tháng/năm) nếu tồn tại giá trị
 * - '-' nếu không tồn tại
 */
export function formatDate(value) {
    if (!value) return '-'
    return new Date(value).toLocaleDateString('vi-VN')
}

/**
 * Format lại trường có dữ liệu kiểu số
 * 
 * @param {*} value (int, float) - Giá trị muốn format
 * @returns {string}
 * - "-" nếu không tồn tại giá trị
 * - Giá trị sau format nếu tồn tại giá trị
 */
export function formatNumber(value) {
    if (value === null || value === undefined || value === '')
        return '-'

    const number = Number(value)
    if (Number.isNaN(number)) return '-'

    return new Intl.NumberFormat('vi-VN').format(number)
}

/**
 * Format Time
 * 
 * @param {*} value (time (giờ:phút:giây)) - Giá trị muốn format
 * @returns 
 * - '-' nếu rỗng
 * - Giờ:phút nếu có giá trị
 */
export function formatTime(value) {
    if (!value) return '-'

    // 08:30:00 (HH:MM:SS)
    const parts = value.split(':')
    return `${parts[0]}:${parts[1]}`
}

/**
 * 
 * @param {*} value (Status) - Trạng thái của ca (true: đang sử dụng, false: ngừng sử dụng)
 * @returns 
 * - '-' Nếu là null, rỗng, hoặc undefined
 * - Đang sử dụng - true
 * - Ngừng sử dụng - false
 */
export function formatStatus(value) {
    if (value === null || value === undefined || value === '')
        return '-'

    if (value) return 'Đang sử dụng'
    return "Ngừng sử dụng"
}