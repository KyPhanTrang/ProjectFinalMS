<!-- Ms table -->
<template>
  <div class="ms-table">
    <table>
      <thead>
        <tr>
          <th
            v-for="field in props.fields"
            :key="field.key"
            :style="getColumnStyle(field)"
            :class="[getAlignClass(field), getCustomClass(field, 'th')]"
          >
            <template v-if="field.key === 'select'">
              <label class="checkbox">
                <!-- <input type="checkbox" v-model="checked" class="checkbox__input" /> -->
                <input type="checkbox" class="checkbox__input" />

                <span class="checkbox__box"></span>
              </label>
            </template>
            <template v-else>
              {{ field.label }}
            </template>
            <div class="ms-resize"></div>
          </th>
          <th class="th-action"></th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="(row, rowIndex) in props.rows" :key="rowIndex" class="table-row">
          <td
            v-for="field in props.fields"
            :key="field.key"
            :class="[getAlignClass(field), getCustomClass(field, 'td')]"
          >
            <!-- Custom type with slot -->
            <template v-if="field.type === 'custom'">
              <slot :name="field.key" :row="row" :field="field" :value="row[field.key]">
                {{ handleFormat(row[field.key], 'text') }}
              </slot>
            </template>
            <!-- Other type -->
            <template v-else>
              {{ handleFormat(row[field.key], field.type || 'text') }}
            </template>
          </td>
          <td class="td-action">
            <div class="action-buttons">
              <!-- Edit -->
              <div class="action-buttons__icon">
                <div @click="onClickEdit(row)" class="edit-btn"></div>
              </div>
              <!-- More -->
              <div class="action-buttons__icon js-action-trigger">
                <div @click.stop="toggleMenu(rowIndex)" class="more-btn"></div>
              </div>
              <!-- Action menu -->
              <teleport to="body">
                <div
                  v-if="openMenuRowIndex === rowIndex"
                  class="action-menu js-action-menu"
                  :style="menuStyle"
                >
                  <div class="wrapper-item" @click="onClickDuplicate(row)">
                    <div class="icon--item icon--mask icon--duplicate"></div>
                    <div class="action-menu__item">Nhân bản</div>
                  </div>
                  <template v-if="row.isActive">
                    <div class="wrapper-item" @click="onClickChangeStatus(row)">
                      <div class="icon--item icon--mask icon--stop-use"></div>
                      <div class="action-menu__item">Ngừng sử dụng</div>
                    </div>
                  </template>
                  <template v-else>
                    <div class="wrapper-item" @click="onClickChangeStatus(row)">
                      <div class="icon--item icon--mask icon--active"></div>
                      <div class="action-menu__item">Sử dụng</div>
                    </div>
                  </template>
                  <div class="wrapper-item" @click="onClickDelete(row)">
                    <div class="icon--item icon--mask icon--trash"></div>
                    <div class="action-menu__item">Xóa</div>
                  </div>
                </div>
              </teleport>
            </div>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>
<script setup>
import { formatDate, formatNumber, formatText, formatTime, formatStatus } from '@/utils/formatter';
import { ref, Teleport, onMounted, onBeforeUnmount } from 'vue';

/**
 * DEFINE PROPS
 *
 * fields:
 * - Cấu hình các cột của bảng
 * - Mỗi phần tử trong mảng fields phải có:
 *   + key   : key mapping với dữ liệu row
 *   + label : tiêu đề cột
 *   + type  : kiểu hiển thị (text | number | date | time | custom)
 *
 * rows:
 * - Danh sách dữ liệu hiển thị trong bảng
 */
const props = defineProps({
  fields: {
    type: Array,
    required: true,
    validator: (value) => {
      return value.every((field) => {
        const validTypes = ['text', 'date', 'number', 'custom', 'time'];

        // Mỗi field bắt buộc có key + label
        // type nếu không truyền thì mặc định là 'text'
        return field.key && field.label && validTypes.includes(field.type || 'text');
      });
    },
  },
  rows: {
    type: Array,
    required: true,
  },
});

/**
 * DEFINE EMITS
 *
 * edit:
 * - Emit khi người dùng click vào nút sửa trên 1 dòng
 * - Payload là object row tương ứng
 * delete:
 * - Emit khi người dùng click vào nút xóa trên 1 dòng
 * - Payload là id của row tương ứng
 * duplicate:
 * - Emit khi người dùng click vào nút nhân bản trên 1 dòng
 * - Pay load là object (trừ shiftId, shiftCode)
 * disable:
 * - Emit khi người dùng click vào nút ngừng sử dụng trên 1 dòng
 * - Payload là object row tương ứng (và có isActive là false)
 */
const emit = defineEmits(['edit', 'delete', 'duplicate', 'change-status']);

// Xử lý tắt
const handleClickOutside = (e) => {
  // click bên trong menu → bỏ qua
  if (e.target.closest('.js-action-menu')) return;

  // click vào nút mở menu → bỏ qua
  if (e.target.closest('.js-action-trigger')) return;

  openMenuRowIndex.value = null;
};

onMounted(() => {
  document.addEventListener('click', handleClickOutside);
});

onBeforeUnmount(() => {
  document.removeEventListener('click', handleClickOutside);
});

/**
 * Lưu index của dòng đang mở menu
 *
 * Giá trị:
 * - null        : Không có menu nào đang mở
 * - number (0,1,2,...) : Index của row đang mở menu
 */
var openMenuRowIndex = ref(null);

const menuStyle = ref({});

/**
 * Toggle menu của một dòng trong bảng
 *
 * Hành vi:
 * - Nếu menu của dòng đang click đã mở → đóng menu (set null)
 * - Nếu menu của dòng khác đang mở → đóng menu cũ, mở menu mới
 *
 * @param {number} rowIndex - Vị trí (index) của dòng trong danh sách
 */
function toggleMenu(rowIndex) {
  const rect = event.currentTarget.getBoundingClientRect();

  menuStyle.value = {
    position: 'fixed',
    top: rect.bottom + 'px',
    left: rect.right - 140 + 'px',
  };

  openMenuRowIndex.value = openMenuRowIndex.value === rowIndex ? null : rowIndex;
}

/**
 * Đóng toàn bộ menu (thường dùng khi click ra ngoài bảng)
 */
function closeMenu() {
  openMenuRowIndex.value = null;
}

/**
 * FORMAT CELL VALUE
 *
 * Chuẩn hóa dữ liệu hiển thị theo type của field
 * @param {*} value - giá trị cần format
 * @param {string} type - kiểu dữ liệu của cột
 * @returns {*} giá trị đã được format
 */
const handleFormat = (value, type) => {
  switch (type) {
    case 'text':
      return formatText(value);
    case 'number':
      return formatNumber(value);
    case 'date':
      return formatDate(value);
    case 'time':
      return formatTime(value);
    default:
      return formatText(value);
  }
};

/**
 * Emit sự kiện edit khi click nút sửa
 *
 * @param {Object} row - Dữ liệu của dòng được chọn
 */
function onClickEdit(row) {
  emit('edit', { ...row });
}

function onClickDuplicate(row) {
  emit('duplicate', { ...row });
  closeMenu();
}
function onClickChangeStatus(row) {
  emit('change-status', { ...row });
  closeMenu();
}
function onClickDelete(row) {
  emit('delete', { ...row });
  closeMenu();
}

/**
 * Trả về class căn chỉnh nội dung của cột
 *
 * @param {Object} field - Cấu hình cột
 * @returns {string|Object} class CSS tương ứng
 */
function getAlignClass(field) {
  // Không cấu hình align → không áp dụng class
  if (!field.align) return {};

  // Căn phải (thường dùng cho number)
  if (field.align === 'right') return 'text-right';
}

/**
 * Trả về class custom cho cột đặc biệt (ví dụ checkbox/select)
 *
 * @param {Object} field - Cấu hình cột
 * @param {'td'|'th'} tdOrth - Phân biệt cell body hay header
 * @returns {string} class CSS
 */
function getCustomClass(field, tdOrth) {
  // Chỉ áp dụng cho cột select
  if (field.key !== 'select') return '';

  // Phân biệt class cho td và th
  if (tdOrth === 'td') return 'select-box-td';
  return 'select-box-th';
}

/**
 * Tính toán style độ rộng cho cột
 *
 * @param {Object} field - Object cấu hình cột
 * @returns {Object} style object dùng cho binding :style
 *
 * Quy ước:
 * - width có thể là number (px) hoặc string
 * - Fix cứng width, minWidth và maxWidth để tránh co giãn
 */
function getColumnStyle(field) {
  // Không cấu hình width → không set style
  if (!field.width) return {};

  const width = typeof field.width === 'number' ? `${field.width}px` : field.width + 'px';

  return {
    width,
    minWidth: width,
    maxWidth: width,
  };
}
</script>

<style scoped>
/* ------------------ */
.ms-table {
  flex: 1;
  overflow: auto;
  position: relative;
  scrollbar-width: thin;
  background-color: #fff;
}

table {
  table-layout: fixed;
}

/* td sticky */
.select-box-td {
  padding: 0 12px;
  position: sticky;
  left: 0;
  z-index: 10;
  background-color: #fff;
}

/* th sticky */
.select-box-th {
  padding: 0 12px;
  position: sticky;
  left: 0;
  top: 0;
  z-index: 15;
}
/* Ngăn cách giữa các header */
.ms-resize {
  margin-top: 7px;
  width: 5px;
  height: calc(100% - 16px);
  position: absolute;
  top: 0;
  right: 0;
  cursor: col-resize;
  border-right: 1px solid #d1d5db !important;
}

/* Cấu hình table, row */
table {
  width: 100%;
  border-collapse: collapse;
}

/* custom checkbox */
/* wrapper */
.checkbox {
  display: inline-flex;
  align-items: center;
  cursor: pointer;
}

/* ẩn input nhưng vẫn giữ logic */
.checkbox__input {
  position: absolute;
  opacity: 0;
  pointer-events: none;
}

/* box custom */
.checkbox__box {
  width: 16px;
  height: 16px;
  border: 1.5px solid #d1d5db;
  border-radius: 2px;
  background-color: #fff;
  display: flex;
  align-items: center;
  justify-content: center;
  box-sizing: border-box;
  transition: all 0.2s ease;
}

/* dấu tick */
.checkbox__box::after {
  content: '';
  width: 4px;
  height: 8px;
  border-right: 2px solid white;
  border-bottom: 2px solid white;
  transform: rotate(45deg);
  opacity: 0;
}

/* khi checked → đổi màu + hiện tick */
.checkbox__input:checked + .checkbox__box {
  background-color: #009b71;
  border-color: #009b71;
}

.checkbox__input:checked + .checkbox__box::after {
  opacity: 1;
}

tr {
  width: 100%;
  border-bottom: 1px solid #d1d5db;
}

tr:target {
  background-color: #a4f6d3;
}

/* Thead */
thead th {
  height: 33px;
  line-height: 33px;
  padding: 0 16px;
  font-weight: 600;
  font-size: 13px;
  position: sticky;
  top: 0;
  z-index: 2;
  background-color: #f3f4f6;
  color: rgb(30, 38, 51);
}

/* Body */
tbody td {
  height: 32px;
  line-height: 32px;
  padding: 0 12px;
}

th,
td {
  padding: 15px 12px;
  border-bottom: 1px solid #e0e0e0;
  text-align: left;
  white-space: nowrap;
  color: rgb(30, 38, 51);
}

/* Table row hover */
.table-row:hover {
  background-color: rgb(229 229 238);
  color: black;
  z-index: 30;
}

/* Table row hover */
.table-row:hover .select-box-td {
  background-color: rgb(229 229 238);
}

.table-row:hover .td-action {
  background-color: rgb(229 229 238);
}

.table-row:hover .action-buttons {
  opacity: 1;
  pointer-events: auto;
}

/* Action button */
.td-action {
  z-index: 0;
  position: sticky;
  right: 0;
  background-color: #fff;
}

.action-buttons {
  display: flex;
  justify-content: space-between;
  padding: 0px 11px;
}

/* Edit button, delete button */
.action-buttons__icon {
  width: 24px;
  height: 24px;
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
  transition: all ease 0.1s;
  border-radius: 50%;
  background-color: #fff;
}

.edit-btn,
.more-btn {
  cursor: pointer;
  z-index: 1;
}

.edit-btn {
  mask-image: url(@/assets/icons/icon.svg);
  mask-position: -271px 0px;
  background-color: #4b5563;
  width: 16px;
  height: 16px;
}

.more-btn {
  mask-image: url(@/assets/icons/icon.svg);
  mask-position: -288px 0px;
  background-color: #4b5563;
  width: 16px;
  height: 16px;
}

.edit-btn:hover,
.more-btn:hover {
  background-color: #009b71;
}

.text-right {
  text-align: right;
}

/* table header Action */
table thead th.th-action {
  width: 100px;
  padding: 0;
  font-weight: 400;
  background: #f3f4f6;
  border-bottom: 1px solid #d1d5db;
  position: sticky;
  top: 0;
  right: 0;
  z-index: 2;
  cursor: pointer;
}

/* Action menu */
/* ===== ACTION MENU ===== */
.action-menu {
  min-width: 140px;
  border-radius: 8px;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
  background-color: #fff;
  padding: 6px 0;
}

.wrapper-item {
  display: flex;
}

.wrapper-item {
  flex: 1;
  padding: 8px 12px;
  font-size: 13px;
  column-gap: 8px;
  cursor: pointer;
  white-space: nowrap;
}

.wrapper-item:hover {
  background-color: #f3f4f6;
}

.wrapper-item--danger:hover {
  background-color: #fdecea;
}

.icon--item {
  width: 16px;
  height: 16px;
}
</style>
