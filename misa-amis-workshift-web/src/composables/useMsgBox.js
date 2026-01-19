import { ref } from 'vue';

/**
 * Composable quản lý MessageBox (Alert / Confirm)
 *
 * Mục đích:
 * - Dùng chung cho toàn bộ hệ thống
 * - Quản lý trạng thái hiển thị popup
 * - Hỗ trợ alert thông báo và confirm xác nhận hành động
 */
export function useMsgBox() {

    /**
     * State của MessageBox
     *
     * visible  : hiển thị / ẩn popup
     * type     : loại popup ('alert' | 'confirm')
     * title    : tiêu đề popup
     * message  : nội dung hiển thị
     * payload  : dữ liệu đi kèm (ví dụ: bản ghi cần xóa)
     * onConfirm: callback được gọi khi người dùng bấm nút Confirm
     */
    const msgBox = ref({
        visible: false,
        type: 'alert',
        title: '',
        message: '',
        payload: null,
        onConfirm: null,
    });

    /**
     * Hiển thị MessageBox dạng Alert (chỉ có nút OK)
     *
     * @param {string} message - Nội dung thông báo
     * @param {string} title   - Tiêu đề popup (mặc định: 'Thông báo')
     */
    function showAlert(message, title = 'Cảnh báo') {
        Object.assign(msgBox.value, {
            visible: true,
            type: 'alert',
            title,
            message,
            payload: null,
            onConfirm: null,
        });
    }

    /**
     * Hiển thị MessageBox dạng Confirm (OK / Cancel)
     *
     * @param {Object} options
     * @param {string} options.title       - Tiêu đề popup
     * @param {string} options.message     - Nội dung xác nhận
     * @param {*} options.payload     - Dữ liệu truyền vào callback khi confirm
     * @param {Function} options.onConfirm - Hàm được gọi khi người dùng bấm Confirm
     */
    function showConfirm({ title, message, payload, onConfirm }) {
        Object.assign(msgBox.value, {
            visible: true,
            type: 'confirm',
            title,
            message,
            payload,
            onConfirm,
        });
    }

    /**
     * Đóng MessageBox
     *
     * Được gọi khi:
     * - Người dùng bấm Cancel
     * - Sau khi confirm xong
     */
    function close() {
        msgBox.value.visible = false;
    }

    /**
     * Xử lý khi người dùng bấm nút Confirm
     *
     * - Gọi callback onConfirm (nếu có)
     * - Truyền payload đi kèm
     * - Đóng MessageBox sau khi xử lý xong
     */
    async function confirm() {
        if (typeof msgBox.value.onConfirm === 'function') {
            await msgBox.value.onConfirm(msgBox.value.payload);
        }
        close();
    }

    // Public API của composable
    return {
        msgBox,
        showAlert,
        showConfirm,
        close,
        confirm,
    };
}
