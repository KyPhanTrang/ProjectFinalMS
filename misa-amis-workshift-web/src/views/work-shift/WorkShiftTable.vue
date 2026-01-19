<template>
  <div class="wrapper-table">
    <!-- Table -->
    <ms-table
      :fields="fields"
      :rows="data"
      @edit="onEdit"
      @delete="onDelete"
      @change-status="onChangeStatus"
      @duplicate="onDuplicate"
    >
      <template #select="{ row }">
        <label class="checkbox">
          <!-- <input type="checkbox" 
          v-model="checked" 
          class="checkbox__input" /> -->
          <input type="checkbox" class="checkbox__input" />
          <span class="checkbox__box"></span>
        </label>
      </template>
      <template #isActive="{ value }">
        <div class="wrapper-custom-status">
          <div class="inactive custom-status" :class="{ active: value }">
            {{ formatStatus(value) }}
          </div>
        </div>
      </template>
    </ms-table>
    <!-- Pagination -->
    <div class="work-shift-table__pagination">
      <div class="pagination">
        <!-- Content Left -->
        <div class="pagination__info">
          Tổng số:
          <b class="text-total">
            {{ total }}
          </b>
        </div>

        <!-- Content Right -->
        <div class="pagination__controls">
          <!-- Page size -->
          <span class="pagination__label">Số dòng/trang</span>
          <select class="pagination__select" :value="pageSize" @change="onPageSizeChange">
            <option :value="10">10</option>
            <option :value="20">20</option>
            <option :value="30">30</option>
            <option :value="50">50</option>
            <option :value="100">100</option>
          </select>

          <!-- Page info -->
          <span class="pagination__page-info"> {{ startRecord }} - {{ endRecord }}</span>

          <!-- Button First -->
          <div
            class="pagination__btn icon--mask icon--first"
            :class="{ disabled: pageIndex === 1 }"
            @click="pageIndex !== 1 && goFirstPage()"
          ></div>

          <!-- Button Prev -->
          <div
            class="pagination__btn icon--mask icon--left"
            :class="{ disabled: pageIndex === 1 }"
            @click="pageIndex !== 1 && prevPage()"
          ></div>

          <!-- Button Next -->
          <div
            class="pagination__btn icon--mask icon--right"
            :class="{ disabled: pageIndex === totalPages }"
            @click="pageIndex !== totalPages && nextPage()"
          ></div>

          <!-- Button Last -->
          <div
            class="pagination__btn icon--mask icon--last"
            :class="{ disabled: pageIndex === totalPages }"
            @click="pageIndex !== totalPages && goLastPage()"
          ></div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { computed } from 'vue';
import { formatStatus } from '@/utils/formatter';
import MsTable from '@/components/ms-table/MsTable.vue';
import MsSelect from '@/components/ms-input/MsSelect.vue';

/**
 * EMITS
 */
const emit = defineEmits([
  'edit',
  'delete',
  'duplicate',
  'change-status',
  'page-change',
  'page-size-change',
]);

/**
 * PROPS
 */
const props = defineProps({
  data: { type: Array, required: true },
  fields: {
    type: Array,
  },

  // paging
  pageIndex: Number,
  pageSize: Number,
  total: Number,
  loading: Boolean,
});

const totalPages = computed(() => {
  if (!props.total) return 1;
  return Math.ceil(props.total / props.pageSize);
});

const startRecord = computed(() => {
  if (props.total === 0) return 0;
  return (props.pageIndex - 1) * props.pageSize + 1;
});

const endRecord = computed(() => {
  const end = props.pageIndex * props.pageSize;
  return end > props.total ? props.total : end;
});

function nextPage() {
  emit('page-change', props.pageIndex + 1);
}

function prevPage() {
  emit('page-change', props.pageIndex - 1);
}

function goFirstPage() {
  emit('page-change', 1);
}

function goLastPage() {
  emit('page-change', totalPages.value);
}

function onPageSizeChange(event) {
  emit('page-size-change', Number(event.target.value));
}

/**
 * ACTION (FORWARD EMIT TO CHILD)
 */

function onEdit(row) {
  emit('edit', row);
}

function onDelete(row) {
  emit('delete', row);
}

function onDuplicate(row) {
  emit('duplicate', row);
}

function onChangeStatus(row) {
  emit('change-status', row);
}
</script>

<style scoped>
/* Custom checkbox */
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

/* Custom style */
.wrapper-custom-status {
  display: flex;
  align-items: center;
}

.custom-status {
  height: 25px;
  line-height: 15px;
  width: fit-content;
  width: -moz-fit-content;
  padding: 5px 8px;
  border-radius: 999px;
}

.inactive {
  color: #dc2626;
  background-color: #fee2e2;
}

.active {
  color: #009b71;
  background-color: #ebfef6;
}

/* Wrapper table */
.wrapper-table {
  flex: 1;
  overflow: hidden;
  display: flex;
  flex-direction: column;
}

/* Pagination part */
.work-shift-table__pagination {
  height: 44px;
  min-height: 44px;
  align-items: center;
  background-color: #f3f4f6;
  width: 100%;
  padding: 8px 16px;
  border-radius: 0 0 4px 4px;
}

/* Pagination container */
.pagination {
  flex: 1;
  display: flex;
  justify-content: space-between;
  align-items: center;
  color: rgb(30, 38, 51);
}

/* Total Text */
.text-total {
  font-weight: 700;
}

/* Pagination container button */
.pagination__controls {
  display: flex;
  align-items: center;
  gap: 8px;
}

/* Pagination page info */
.pagination__page-info {
  font-weight: 700;
  margin: 0 5px;
}

/* Pagination select */
.pagination__select {
  padding: 5px 25px;
  border: 1px solid #e0e0e0;
  background-color: #ffffff;
  border-radius: 4px;
  margin: 0 5px;
}

/* Pagination btn */
.pagination__btn {
  background-color: #4b5563;
  width: 16px;
  height: 16px;
  margin: 0 12px;
  cursor: pointer;
}

/* Pagination button style when disable */
.pagination__btn.disabled {
  opacity: 0.4;
  pointer-events: none;
  background-color: #9ca3af;
}
</style>
