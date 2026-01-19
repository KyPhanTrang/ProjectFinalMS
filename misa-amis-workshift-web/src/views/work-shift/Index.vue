<template>
  <div class="work-shift__page">
    <!-- Header -->
    <work-shift-header @open="openFormCreate"></work-shift-header>
    <section class="work-shift__content">
      <!-- Toolbar -->
      <work-shift-toolbar @search="handleSearch" @reload="handleReload"></work-shift-toolbar>
      <!-- Table -->
      <work-shift-table
        :fields="fields"
        :data="workShifts"
        :page-index="filter.pageIndex"
        :page-size="filter.pageSize"
        :total="total"
        :loading="loading"
        @page-change="changePage"
        @page-size-change="changePageSize"
        @edit="openFormEdit"
        @delete="handleDelete"
        @duplicate="handleDuplicate"
        @change-status="handleChangeStatus"
      ></work-shift-table>
    </section>
    <!-- Form -->
    <work-shift-form
      v-if="isOpenForm"
      :form="form"
      :errors="errors"
      :mode="mode"
      @close="closeForm"
      @form-submit="save"
    ></work-shift-form>
  </div>
  <!-- Message Box -->
  <base-msg-box
    v-if="msgBox.visible"
    :title="msgBox.title"
    :message="msgBox.message"
    :type="msgBox.type"
    @close="close"
    @cancel="close"
    @confirm="confirm"
  ></base-msg-box>
  <!-- Toast -->
  <ms-toast v-if="isToast" :msg="msgToast" @close="closeToast"></ms-toast>
</template>

<script setup>
import WorkShiftHeader from './WorkShiftHeader.vue';
import WorkShiftToolbar from './WorkShiftToolbar.vue';
import WorkShiftTable from './WorkShiftTable.vue';
import WorkShiftForm from './WorkShiftForm.vue';
import BaseMsgBox from '@/components/ms-msg-box/BaseMsgBox.vue';
import MsToast from '@/components/ms-toast/MsToast.vue';

import { onMounted, ref } from 'vue';
import { workShiftFields } from './workShift.fields';
import { useWorkShift } from '@/composables/useWorkShift';
import { useMsgBox } from '@/composables/useMsgBox';
import { useToast } from '@/composables/useToast';

/**
 *  COMPOSABLE
 */

const {
  workShifts,
  total,
  filter,
  loading,

  changePage,

  form,
  errors,

  getPaging,
  create,
  update,
  remove,
  resetForm,
} = useWorkShift();

function changePageSize(value) {
  filter.pageSize = value;
}

const { msgBox, showAlert, showConfirm, close, confirm } = useMsgBox();

const { isToast, msgToast, show: showToast, closeToast } = useToast();
/**
 *  STATE
 */
const isOpenForm = ref(false);
const fields = workShiftFields;
const mode = ref('create');
const editingId = ref(null);

/**
 *  LIFECYCLE
 */
onMounted(() => {
  getPaging();
});

/**
 * ACTION (CREATE, EDIT, REMOVE) FORM
 */

/**
 * Mở form ở chế độ THÊM MỚI ca làm việc
 *
 * Thực hiện:
 * - Chuyển mode sang 'create'
 * - Reset id đang chỉnh sửa
 * - Reset dữ liệu form
 * - Hiển thị form
 */
function openFormCreate() {
  mode.value = 'create'; // Đánh dấu chế độ thêm mới
  editingId.value = null; // Không có id đang chỉnh sửa
  resetForm(); // Reset dữ liệu form
  isOpenForm.value = true; // Mở form
}

/**
 * Mở form ở chế độ CẬP NHẬT ca làm việc
 *
 * Thực hiện:
 * - Chuyển mode sang 'update'
 * - Lưu lại id của ca làm việc đang chỉnh sửa
 * - Đổ dữ liệu từ dòng được chọn lên form
 * - Hiển thị form
 *
 * @param {Object} row - Dữ liệu ca làm việc được chọn từ danh sách
 */
function openFormEdit(row) {
  mode.value = 'update'; // Đánh dấu chế độ cập nhật

  // Payload (form) không cần chứa trường id
  const { shiftId, ...payload } = row;
  editingId.value = shiftId; // Lưu id ca làm việc cần chỉnh sửa

  // Gán dữ liệu ca làm việc lên form
  Object.assign(form, payload);

  isOpenForm.value = true; // Mở form
}

/**
 * Đóng form thêm / sửa ca làm việc
 *
 * Được gọi khi:
 * - Người dùng bấm Cancel
 * - Lưu dữ liệu thành công
 */
function closeForm() {
  // Nếu có thời gian làm thêm check nếu có trường nào đang điền dở rồi thì hỏi confirm lại
  isOpenForm.value = false;
}

/**
 * Lưu dữ liệu ca làm việc
 *
 * Tự động xác định hành động:
 * - mode = 'create' → gọi API thêm mới
 * - mode = 'update' → gọi API cập nhật
 *
 * Nếu lưu thành công → đóng form → hiển thị thông báo
 */
async function save() {
  // Gọi hàm tạo (validate, check trùng đã làm ở đây, và tả về true nếu thêm thành công, không thì danh sách lỗi)
  const ok = mode.value === 'create' ? await create() : await update(editingId);

  // Kiểm tra lại lỗi mới (đã làm ở bước trên nếu false)
  if (!ok) {
    const errorKeys = Object.keys(errors);
    if (errorKeys.length > 0) {
      showAlert(errors[errorKeys[0]]);
    }
    return;
  }
  // Thành công (đóng form, thông báo thành công)
  closeForm();

  if (mode.value === 'create') {
    showToast('Thêm Ca làm việc thành công', 5000);
  }
  if (mode.value === 'update') {
    showToast('Sửa Ca làm việc thành công', 5000);
  }
}

/**
 * Xử lý hành động xóa ca làm việc
 *
 * Luồng xử lý:
 * 1. Hiển thị hộp thoại confirm xóa
 * 2. Nếu người dùng xác nhận:
 *    - Gọi API xóa ca làm việc theo shiftId
 *    - Hiển thị toast thông báo kết quả
 *
 * @param {Object} row - Dữ liệu của dòng được chọn xóa
 */
function handleDelete(row) {
  showConfirm({
    // Tiêu đề hộp thoại confirm
    title: 'Xóa ca làm việc',
    // Nội dung cảnh báo (cho phép HTML để highlight mã ca)
    message: `Ca làm việc <strong>${row.shiftCode}</strong> sau khi bị xóa sẽ không thể khôi phục. <br>Bạn có muốn tiếp tục xóa không?`,

    // Dữ liệu truyền vào callback confirm (ở đây là shiftId)
    payload: row.shiftId,

    /**
     * Callback được gọi khi người dùng bấm "Đồng ý"
     *
     * @param {string|number} id - shiftId được truyền từ payload
     */
    onConfirm: async function (id) {
      try {
        // Gọi hàm xóa ca làm việc
        await remove(id);

        // Thông báo xóa thành công
        showToast('Xóa ca làm việc thành công', 5000);
      } catch (error) {
        // Thông báo xóa thất bại
        showToast('Xóa ca làm việc thất bại', 5000);
      }
    },
  });
}

// Handle duplicate
function handleDuplicate(row) {
  mode.value = 'create';
  editingId.value = null;

  // clone + loại bỏ field không được copy
  const { shiftId, shiftCode, createdBy, createdDate, modifiedBy, modifiedDate, ...payload } = row;
  Object.assign(form, payload);

  isOpenForm.value = true;
}

// Handle change status
async function handleChangeStatus(row) {
  const oldStatus = row.isActive;
  row.isActive = !oldStatus;

  editingId.value = row.shiftId;
  try {
    Object.assign(form, { ...row });
    await update(editingId);
    resetForm();
  } catch {
    showToast('Cập nhật thất bại', 5000);
  }
}

// Handle Search
function handleSearch(keyword) {
  filter.keyword = keyword;
  filter.pageIndex = 1;
  getPaging();
}

// Handle Reload
function handleReload() {
  filter.keyword = '';
  filter.pageIndex = 1;
  getPaging();
}

// Handle Reload
</script>

<style scoped>
.work-shift__page {
  flex: 1;
  padding: 16px 20px 20px 20px;
  display: flex;
  flex-direction: column;
  overflow: hidden;
}
.work-shift__content {
  flex: 1;
  display: flex;
  flex-direction: column;
  overflow: hidden;
}
</style>
