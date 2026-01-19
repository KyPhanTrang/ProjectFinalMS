// useToast.js
import { ref } from 'vue';

/**
 * Trạng thái hiển thị toast
 * - true  : hiển thị toast
 * - false : ẩn toast
 */
const isToast = ref(false);

/**
 * Nội dung thông báo của toast
 */
const msgToast = ref('');

/**
 * Lưu timeout hiện tại để:
 * - Tránh việc show toast mới nhưng timeout cũ vẫn chạy
 * - Reset thời gian hiển thị khi show liên tiếp
 */
let timeoutId = null;

/**
 * Composable quản lý Toast thông báo
 *
 * Cung cấp:
 * - Trạng thái hiển thị toast
 * - Nội dung toast
 * - Hàm show / close toast
 */
export function useToast() {

    /**
     * Đóng toast thủ công
     * (dùng khi click nút close hoặc khi cần ẩn ngay)
     */
    function closeToast() {
        isToast.value = false;
    }

    /**
     * Hiển thị toast với nội dung và thời gian tự động ẩn
     *
     * @param {string} message - Nội dung hiển thị
     * @param {number} duration - Thời gian hiển thị (ms), mặc định 3000ms
     *
     * Cơ chế:
     * - Nếu đang có toast → clear timeout cũ
     * - Show toast mới
     * - Set timeout để tự động ẩn toast
     */
    function show(message, duration = 3000) {
        // Hủy timeout cũ nếu toast đang hiển thị
        if (timeoutId) clearTimeout(timeoutId);

        // Gán nội dung và hiển thị toast
        msgToast.value = message;
        isToast.value = true;

        // Tự động ẩn toast sau duration
        timeoutId = setTimeout(() => {
            isToast.value = false;
            timeoutId = null;
        }, duration);
    }

    return {
        isToast,
        msgToast,
        show,
        closeToast
    };
}
