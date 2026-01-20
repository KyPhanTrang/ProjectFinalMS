<!-- ========== BaseTimeSelect.vue ========== -->
<template>
  <div class="base-field" :class="{ 'full-width': fullWidth }" ref="root">
    <label class="base-label">{{ label }} <span v-if="required" class="req">*</span></label>
    <div class="combobox" :class="{ error }">
      <input
        type="text"
        class="combobox__input"
        :value="displayValue"
        placeholder="HH:MM"
        ref="inputRef"
        tabindex="0"
        @input="onInput"
      />
      <button class="combobox__button js-combobox__data-trigger" tabindex="-1">
        <div class="icon--mask icon--clock" @click="open = !open"></div>
      </button>
      <div v-if="open" class="combobox__data js-combobox__data">
        <template v-for="t in times">
          <div class="combobox-item" @click="select(t)">{{ t }}</div>
        </template>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, onUnmounted, onBeforeUnmount, computed } from 'vue';

/**
 * DEFINE PROPS
 *
 * label       : Nhãn hiển thị của input
 * modelValue : Giá trị binding từ v-model
 * required   : Đánh dấu bắt buộc nhập
 * fullWidth  : Hiển thị full chiều ngang
 * error      : Thông báo lỗi validate
 */
const props = defineProps({
  label: String,
  modelValue: String,
  required: Boolean,
  fullWidth: Boolean,
  error: String,
});

/**
 * Emit để hỗ trợ v-model
 */
const emit = defineEmits(['update:modelValue']);

/**
 *
 * STATE
 */

// Trạng thái mở / đóng dropdown
const open = ref(false);

// Ref gốc của component (dùng để detect click outside)
const root = ref(null);

// Truyền lên (HH:mm:ss) => hiển thị (HH:mm) => Gửi đi (HH:mm)
const displayValue = computed(() => {
  if (!props.modelValue) return '';
  return props.modelValue.slice(0, 5);
});

/**
 * DATA SOURCE
 *
 * Danh sách thời gian dạng HH:mm
 * Mỗi 30 phút (00, 30)
 */
const times = [];
for (let h = 0; h < 24; h++) {
  for (let m of ['00', '30']) {
    times.push(`${String(h).padStart(2, '0')}:${m}`);
  }
}

/**
 * HANDLERS
 */

/**
 * Chọn 1 mốc thời gian từ dropdown
 *
 * @param {string} t - Thời gian được chọn (HH:mm)
 */
function select(t) {
  emit('update:modelValue', t);
  open.value = false;
}

/**
 * Xử lý nhập tay vào input time
 *
 * Luật nhập:
 * - Chỉ cho phép số
 * - Tự động format HH:mm
 * - Validate từng ký tự (giờ/phút hợp lệ)
 *
 * @param {Event} e - Input event
 */

function onInput(e) {
  // Lấy toàn bộ số người dùng nhập
  let raw = e.target.value.replace(/\D/g, '');
  let result = '';

  for (let i = 0; i < raw.length; i++) {
    const digit = raw[i];

    // H1: chữ số đầu của giờ (0–2)
    if (i === 0) {
      if (digit > '2') break;
      result += digit;
    }
    // H2: chữ số thứ 2 của giờ (20->23) range(0-3)
    else if (i === 1) {
      if (result[0] === '2' && digit > '3') break;
      result += digit;
    }
    // M1: chữ số đầu của phút (0–5)
    else if (i === 2) {
      if (digit > '5') break;
      result += digit;
    }
    // M2: chữ số thứ 2 của phút (0–9)
    else if (i === 3) {
      result += digit;
    }
  }

  // Tự động thêm dấu ':' khi đủ 3 ký tự
  if (result.length >= 3) {
    result = result.slice(0, 2) + ':' + result.slice(2);
  }

  // Gán lại giá trị hiển thị cho input
  e.target.value = result;

  // Emit ra ngoài để cập nhật v-model
  emit('update:modelValue', result);
}

/**
 * ===============================
 * XỬ LÝ ĐÓNG DROPDOWN KHI CLICK RA NGOÀI
 * ===============================
 *
 * Hiện tại component có 2 cách bắt sự kiện click outside.
 * Mục đích là đảm bảo dropdown (combobox) sẽ được đóng
 * khi người dùng click ra ngoài vùng component.
 *
 * Lưu ý:
 * - Cách này CHƯA tối ưu (trùng logic)
 * - Nhưng đang được giữ lại do giới hạn thời gian implement
 * - Vẫn đảm bảo hành vi UI đúng trong phạm vi hiện tại
 */

/**
 * Cách 1:
 * Đóng dropdown nếu click KHÔNG nằm trong root component
 * (toàn bộ BaseTimeSelect)
 */
function clickOutside(e) {
  // Nếu click không nằm trong component (root)
  // → đóng dropdown
  if (!root.value.contains(e.target)) {
    open.value = false;
  }
}

// Gắn listener khi component mount
onMounted(() => {
  document.addEventListener('click', clickOutside);
});

// Gỡ listener khi component unmount
onUnmounted(() => {
  document.removeEventListener('click', clickOutside);
});

/**
 * Cách 2:
 * Xử lý chi tiết hơn cho các case đặc biệt trong combobox
 *
 * Mục đích:
 * - Không đóng dropdown khi click vào:
 *   + Danh sách thời gian (menu)
 *   + Nút icon mở dropdown
 * - Chỉ đóng khi click ra ngoài hoàn toàn
 */
const handleClickOutside = (e) => {
  // Click bên trong danh sách dropdown → không đóng
  if (e.target.closest('.js-combobox__data')) return;

  // Click vào nút trigger mở dropdown → không đóng
  if (e.target.closest('.js-combobox__data-trigger')) return;

  // Các trường hợp còn lại → đóng dropdown
  open.value = false;
};

// Gắn listener khi component mount
onMounted(() => {
  document.addEventListener('click', handleClickOutside);
});

// Gỡ listener trước khi component bị hủy
onBeforeUnmount(() => {
  document.removeEventListener('click', handleClickOutside);
});
</script>

<style scoped>
.base-field {
  /* width: 50%; */
  display: flex;
  column-gap: 15px;
}
.req {
  color: red;
}
.full-width {
  justify-content: space-between;
}

.base-label {
  width: 150px;
  font-weight: 500;
  color: #262626;
  display: block;
}
.combobox {
  padding: 5px 0 5px 12px;
  position: relative;
  display: flex;
  max-width: 122px;
  column-gap: 5px;
  flex: 1 1 0;
  border: 1px solid #d1d5db;
  border-radius: 4px;
}

.combobox.error {
  border: 1px solid red;
}

.combobox:focus-within {
  border-color: #16a34a;
}

.combobox__input {
  width: 78px;
  height: 16px;
}
.combobox__input::placeholder {
  color: #33333389;
  font-weight: 500;
}
.combobox__button {
  padding: 0;
}
.combobox__data {
  position: absolute;
  top: calc(100% + 2px);
  width: 100%;
  left: 0;
  display: flex;
  flex-direction: column;
  padding: 12px 13px 7px 12px;
  z-index: 109;
  border-radius: 4px;
  background: #fff;
  border: 1px solid #d5dfe2;
  overflow: auto;
  max-height: 215px;
  overflow-y: auto;
  overflow-x: hidden;
}
.combobox__data::-webkit-scrollbar {
  width: 6px;
}

.combobox-item {
  border-radius: 3.5px;
  padding: 6px 10px;
  display: flex;
  justify-content: center;
  color: #111827;
}
.combobox-item:hover {
  background: #efefef;
  cursor: pointer;
}
</style>
