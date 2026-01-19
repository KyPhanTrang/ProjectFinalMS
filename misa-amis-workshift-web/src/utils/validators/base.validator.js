/**
 * Kiểm tra giá trị bắt buộc (required)
 * 
 * @param {*} value - Giá trị cần kiểm tra (string, number, object, ...)
 * @returns {string}
 * - Thông báo lỗi khi giá trị rỗng, null, undefine
 * - Chuỗi rỗng nếu hợp lệ
 */
export function required(fields, value) {
    if (value === null || value === undefined || value === '' || value === 'HH:MM')
        return fields + " không được để trống."
    return ""
}

/**
 * Kiểm tra số lượng ký tự
 * 
 * @param {*} value - Giá trị cần kiểm tra (string)
 * @param {*} max - Số ký tự tối đa (số lượng <= max)
 * @returns {string}
 *  - Thông báo lỗi nếu vượt quá số lượng cho phép
 *  - Chuỗi rỗng nếu hợp lệ
 */
export function maxLength(fields, value, max = 20) {
    if (value && value.length > max)
        return fields + ` không được vượt quá ${max} ký tự.`
    return ''
}

/**
 * Kiểm tra phải số hay không
 * 
 * @param {*} value - Giá trị cần kiểm tra
 * @returns {string}
 * - Thông báo lỗi nếu không phải là số
 * - Chuỗi rỗng nếu hợp lệ
 */
export function isNumber(value) {
    if (value && isNaN(value)) {
        return 'Phải là số.'
    }
    return ''
}