<template>
  <teleport to="body">
    <div
      class="filter-popup"
      :style="style"
      ref="popupRef"
      tabindex="-1"
      @keydown.enter.prevent="apply"
      @keydown.esc="emit('close')"
    >
      <!-- Header -->
      <div class="filter-popup__header">
        <span class="filter-popup__header-label">Lọc {{ field.label }}</span>
        <span
          class="close filter-popup__header-icon icon--mask icon--close"
          @click="emit('close')"
        ></span>
      </div>

      <!-- Content -->

      <div class="filter-popup__content">
        <!-- String or Number -->
        <div
          class="wrapper-input"
          v-if="field.filterType === 'number' || field.filterType === 'string'"
        >
          <input type="text" value="Chứa hoặc bằng" tabindex="-1" />
        </div>
        <!-- Status -->
        <select v-if="field.filterType === 'status'" v-model="local.isActive" ref="inputRef">
          <option :value="null">Tất cả</option>
          <option :value="true">Đang sử dụng</option>
          <option :value="false">Ngừng sử dụng</option>
        </select>

        <!-- Number string -->
        <div
          class="wrapper-input"
          v-if="field.filterType === 'number' || field.filterType === 'string'"
        >
          <input
            v-if="field.filterType === 'number'"
            ref="inputRef"
            type="number"
            v-model.number="local.workingTime"
            placeholder="Nhập số giờ"
          />
          <input
            v-if="field.filterType === 'string'"
            ref="inputRef"
            type="text"
            v-model="local.keyword"
            placeholder="Nhập giá trị lọc"
          />
        </div>
      </div>

      <!-- Footer -->
      <div class="filter-popup__footer">
        <div class="wrapper-btn reset" @click="reset">
          <div class="reset-label">Bỏ lọc</div>
        </div>
        <div class="wrapper-btn primary" @click="apply">
          <div class="primary-label">Áp dụng</div>
        </div>
      </div>
    </div>
  </teleport>
</template>

<script setup>
import { ref, reactive, computed, onMounted, nextTick } from 'vue';

const popupRef = ref(null);
const inputRef = ref(null);

onMounted(async () => {
  await nextTick();
  inputRef.value?.focus();
});

const props = defineProps({
  field: Object,
  filter: Object,
  position: Object,
});

const emit = defineEmits(['apply', 'close']);

const local = reactive({
  isActive: props.filter.isActive,
  workingTime: props.filter.workingTime,
  keyword: props.filter.keyword,
});

const style = computed(() => ({
  position: 'fixed',
  top: props.position.top + 'px',
  left: props.position.left + 'px',
}));

function apply() {
  emit('apply', { ...local });
}

function reset() {
  emit('apply', {
    isActive: null,
    workingTime: null,
    keyword: '',
  });
}
</script>

<style scoped>
.filter-popup {
  display: flex;
  flex-direction: column;
  gap: 16px;
  min-width: 350px;
  background: #fff;
  border-radius: 4px;
  padding: 16px;
  box-shadow:
    0 0 8px #0000001a,
    0 8px 16px #0000001a;
  z-index: 9999;
}

.filter-popup:focus-visible {
  outline: none;
}

.filter-popup__header {
  display: flex;
  justify-content: space-between;
  font-weight: 600;
}

.filter-popup__header-label {
  font-weight: 600;
  font-size: 16px;
}
.filter-popup__header-icon:hover {
  background-color: rgb(0, 0, 0);
  cursor: pointer;
}

.filter-popup__content {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.wrapper-input {
  padding: 5px 8px 5px 12px;
  -moz-column-gap: 0px;
  column-gap: 0px;
  background: #fff;
  border: 1px solid #d1d5db;
  border-radius: 4px;
}

.wrapper-input:focus-within {
  border: 1px solid #009b71;
}

.filter-popup__content select {
  outline: 1px;
  border: 1px solid #ccc;
  border-radius: 4px;
  padding: 5px;
}

.filter-popup__content select,
.filter-popup__content input {
  width: 100%;
}

.filter-popup__footer {
  display: flex;
  justify-content: flex-end;
  gap: 8px;
}

.wrapper-btn {
  padding: 6px 12px;
  border: 1px solid #d1d5db;
  color: #111827;
  background-color: #fff;
  border-radius: 4px;
  cursor: pointer;
}

.wrapper-btn.reset {
  background-color: #ffffff;
}
.wrapper-btn.reset:hover {
  background-color: #f3f4f6;
}

.reset-label {
  font-weight: 500;
  color: #111827;
}
.wrapper-btn.primary {
  background-color: #009b71;
}
.wrapper-btn.primary:hover {
  background-color: #007b5d;
}

.primary-label {
  color: #ffffff;
}
</style>
