import { ref, reactive, watch, computed } from 'vue'
import WorkShiftsAPI from '@/apis/components/work-shifts/WorkShiftsAPI'
import { validateWorkShift } from '@/utils/validators/workShift.validator'
import { calcBreakingTime, calcWorkingTime, toTimeSpan, round2 } from '@/utils/time.util'

/**
 * Composition function quản lý nghiệp vụ Ca làm việc (Work Shift)
 *
 * Bao gồm:
 * - Danh sách ca làm việc (paging)
 * - Form thêm / sửa ca
 * - Validate dữ liệu
 * - Tự động tính thời gian làm & nghỉ
 * - CRUD (create, update, delete)
 *
 * @returns {Object} State + methods dùng cho component
 */
export function useWorkShift() {

    /**
     *  DANH SÁCH CA LÀM VIỆC 
     */

    /**
     * Danh sách ca làm việc hiển thị trên bảng
     */
    const workShifts = ref([])

    /**
     * Tổng số bản ghi (phục vụ phân trang)
     */
    const total = ref(0)

    /**
     * Trạng thái loading khi gọi API
     */
    const loading = ref(false)

    /**
     * Bộ lọc phân trang
     */
    const filter = reactive({
        pageSize: 20,
        pageIndex: 1,
        keyword: null,
        workingTime: null,
        isActive: null
    })

    /**
     * Lấy danh sách ca làm việc theo phân trang
     */
    async function getPaging() {
        loading.value = true

        const res = await WorkShiftsAPI.paging(filter)

        workShifts.value = res.data.data
        total.value = res.data.totalRecord

        loading.value = false
    }

    /**
     *  FORM CA LÀM VIỆC 
     */

    /**
     * Dữ liệu form thêm / sửa ca làm việc
     */
    const form = reactive({
        shiftCode: '',          // Mã ca làm việc
        shiftName: '',          // Tên ca làm việc
        startTime: '',          // Giờ bắt đầu ca (HH:mm)
        endTime: '',            // Giờ kết thúc ca (HH:mm)
        breakStartTime: '',     // Giờ bắt đầu nghỉ (HH:mm)
        breakEndTime: '',       // Giờ kết thúc nghỉ (HH:mm)
        workingTime: null,      // Tổng thời gian làm việc
        breakingTime: null,     // Tổng thời gian nghỉ
        isActive: true,         // Trạng thái của ca (true: đang sử dụng DEFAULT, false: Ngừng sử dụng)
        description: ''         // Mô tả ca làm việc
    })

    /**
     * Danh sách lỗi validate theo từng field
     * key: tên field
     * value: message lỗi
     */
    const errors = reactive({})

    /**
     *  RESET FORM 
     */

    /**
     * Reset dữ liệu form về trạng thái ban đầu
     * Đồng thời xóa toàn bộ lỗi validate
     */
    const resetForm = () => {
        Object.assign(form, {
            shiftCode: '',
            shiftName: '',
            startTime: '',
            endTime: '',
            breakStartTime: '',
            breakEndTime: '',
            workingTime: null,
            breakingTime: null,
            isActive: true,
            description: ''
        })

        // Xóa toàn bộ lỗi
        Object.keys(errors).forEach(key => delete errors[key])
    }

    /**
     *  TỰ ĐỘNG TÍNH THỜI GIAN 
     */

    // Tính thời gian nghỉ của ca làm việc dựa trên giờ bắt đầu/kết thúc và giờ nghỉ
    const computedBreakingTime = computed(() => {
        // Chưa đủ dữ liệu (chưa chọn giờ bắt đầu hoặc kết thúc ca)
        // → chưa thể tính thời gian nghỉ
        if (!form.startTime || !form.endTime) return null

        // Tính tổng thời gian nghỉ trong ca
        return calcBreakingTime(
            form.breakStartTime,
            form.breakEndTime,
            form.startTime
        )
    })



    // Tính thời gian làm việc thực tế của ca (tổng ca - thời gian nghỉ)
    const computedWorkingTime = computed(() => {
        // Chưa đủ dữ liệu để tính thời gian làm việc
        if (!form.startTime || !form.endTime) return null

        // Tổng thời gian ca làm việc
        const total = calcWorkingTime(form.startTime, form.endTime)

        // Thời gian làm việc thực tế = tổng ca - thời gian nghỉ
        // Nếu chưa có thời gian nghỉ thì coi như 0
        return round2(total - (computedBreakingTime.value ?? 0))
    })

    // Theo dõi thay đổi, nếu có (computedWorkingTime, computedBreakingTime) => gán lại cho form
    watch(
        [computedWorkingTime, computedBreakingTime],
        ([working, breaking]) => {
            form.workingTime = working
            form.breakingTime = breaking
        },
        { immediate: true }
    )

    /**
     *  CRUD 
     */


    /**
         * Build payload gửi lên backend từ dữ liệu form
         *
         * Mục đích:
         * - Clone toàn bộ dữ liệu form hiện tại
         * - Chuẩn hóa các trường thời gian (startTime, endTime, breakTime)
         *   từ định dạng UI (HH:mm) sang định dạng backend yêu cầu (HH:mm:ss)
         *
         * @returns {Object} payload đã được chuẩn hóa để submit API
         */
    function buildPayload() {
        return {
            // Copy toàn bộ field trong form
            ...form,

            // Chuẩn hóa các trường thời gian về TimeSpan
            startTime: toTimeSpan(form.startTime),
            endTime: toTimeSpan(form.endTime),
            breakStartTime: toTimeSpan(form.breakStartTime),
            breakEndTime: toTimeSpan(form.breakEndTime)
        }
    }

    /**
     * Thực hiện validate dữ liệu form ca làm việc
     *
     * Quy trình:
     * 1. Validate form bằng validateWorkShift để lấy danh sách lỗi
     * 2. Xóa toàn bộ lỗi cũ trong object errors (giữ nguyên reference reactive)
     * 3. Gán lại lỗi mới vào errors để UI hiển thị
     * 4. Trả về kết quả validate
     *
     * @returns {boolean}
     * - true  : Form hợp lệ (không có lỗi)
     * - false : Form không hợp lệ (tồn tại lỗi)
     */
    function validateForm() {
        // 1. Thực hiện validate dữ liệu form
        const validateErrors = validateWorkShift(form)

        // 2. Clear toàn bộ lỗi cũ (bắt buộc để Vue reactivity hoạt động đúng)
        Object.keys(errors).forEach(k => delete errors[k])

        // 3. Gán lỗi mới vào object errors
        Object.assign(errors, validateErrors)

        // 4. Không có lỗi → hợp lệ
        return Object.keys(errors).length === 0
    }

    /**
     * Thêm mới ca làm việc
     *
     * @returns {boolean} true nếu thành công, false nếu validate lỗi
     */
    async function create() {
        /**
         * 1. Validate dữ liệu form
         */
        if (!validateForm()) return false;

        /**
         * 2. Kiểm tra trùng mã ca làm việc
         * (thêm mới → shiftId = null)
         */
        const duplicate = await WorkShiftsAPI.checkDuplicate({
            shiftCode: form.shiftCode,
            shiftId: null
        })

        if (duplicate.data === true) {
            errors.shiftCode = 'Mã ca làm việc đã tồn tại'
            return false
        }

        /**
         * 3. Build payload + gọi API thêm mới
         */
        const payload = buildPayload()
        await WorkShiftsAPI.create(payload)

        // Reload danh sách
        await getPaging()

        // Reset form
        resetForm()

        return true
    }

    /**
     * Cập nhật ca làm việc
     *
     * @param {string} id - Id ca làm việc cần cập nhật
     * @returns {boolean} true nếu thành công, false nếu validate lỗi
     */
    async function update(id) {
        /**
         * 1. Validate dữ liệu form
         */
        if (!validateForm()) return false;

        /**
         * 2. Kiểm tra trùng mã ca làm việc
         * (thêm mới → shiftId = null)
         */
        const duplicate = await WorkShiftsAPI.checkDuplicate({
            shiftCode: form.shiftCode,
            shiftId: id.value
        })

        if (duplicate.data === true) {
            errors.shiftCode = 'Mã ca làm việc đã tồn tại'
            return false
        }

        /**
         * 3. Build payload + gọi API cập nhật
         */
        const payload = buildPayload()
        await WorkShiftsAPI.update(id.value, payload)

        // Reload danh sách
        await getPaging()

        // Reset form
        resetForm()

        return true
    }

    /**
     * Xóa ca làm việc
     *
     * @param {string} id - Id ca làm việc cần xóa
     */
    async function remove(id) {
        await WorkShiftsAPI.delete(id)
        await getPaging()
    }


    /**
     * Paging (phân trang)
     */
    watch(
        () => [filter.keyword, filter.workingTime, filter.isActive],
        () => {
            filter.pageIndex = 1
            getPaging()
        }
    )
    watch(
        () => filter.pageSize,
        () => {
            filter.pageIndex = 1
            getPaging()
        }
    )
    function changePage(pageIndex) {
        filter.pageIndex = pageIndex
        getPaging()
    }

    /**
     *  EXPORT 
     */

    return {
        // list
        workShifts,
        total,
        filter,
        loading,

        // paging
        changePage,

        // form
        form,
        errors,

        // methods
        getPaging,
        create,
        update,
        remove,
        resetForm
    }
}
