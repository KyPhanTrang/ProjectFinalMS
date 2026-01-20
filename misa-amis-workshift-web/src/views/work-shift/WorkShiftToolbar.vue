<template>
  <section class="work-shift-toolbar">
    <!-- Left: search -->
    <div class="work-shift-toolbar__left">
      <div class="work-shift-toolbar__search">
        <div class="search-box">
          <div class="work-shift-toolbar__search-icon icon--mask icon--search"></div>
          <div class="wrapper-search-input">
            <input
              class="search-input"
              type="text"
              placeholder="Tìm kiếm"
              v-model="keyword"
              @input="onSearch"
            />
          </div>
        </div>
      </div>
      <!-- Chips -->
      <div
        class="filter-chips"
        v-if="chips.length"
        title="Tìm trong: mã, tên, người tạo, người sửa"
      >
        <div class="filter-chip" v-for="chip in chips" :key="chip.key">
          <div class="chip-text">
            {{ chip.label }}:
            <b>{{ chip.value }}</b>
          </div>
          <div
            class="chip-remove icon--mask icon--close"
            @click="removeFilterByKey(chip.key)"
          ></div>
        </div>

        <div class="clear-filter-label" @click="clearAllFilter">Bỏ lọc</div>
      </div>
    </div>

    <!-- Right: actions -->
    <div class="work-shift-toolbar__right">
      <button class="work-shift-toolbar__button" @click="onReload">
        <div class="work-shift-toolbar__action-icon icon--mask icon--reload"></div>
      </button>
    </div>
  </section>
</template>

<script setup>
import { ref } from 'vue';
/**
 * EMIT
 */
const emit = defineEmits(['reload', 'search', 'remove-filter-by-id', 'clear-filter']);
const keyword = ref('');

/**
 * PROPS
 */
defineProps({
  chips: Array,
});

/**
 * Gửi sự kiện search mỗi khi input thay đổi
 */
function onSearch() {
  emit('search', keyword.value);
}

/**
 * Reload / reset toolbar
 * - Xóa search input
 * - Emit reload event
 */
function onReload() {
  keyword.value = null;
  emit('reload');
}

/**
 * Xóa 1 chip filter
 * @param {string} key - key của chip (key: 'keyword', 'isActive', 'workingTime')
 */
function removeFilterByKey(key) {
  if (key === 'keyword') keyword.value = null;
  emit('remove-filter-by-id', key);
}

/**
 * Xóa tất cả filter
 */
function clearAllFilter() {
  keyword.value = null;
  emit('clear-filter');
}
</script>

<style scoped>
.work-shift-toolbar {
  flex-shrink: 0;
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 8px 16px;
  background: #fff;
  border-top-left-radius: 4px;
  border-top-right-radius: 4px;
}

.work-shift-toolbar__left {
  display: flex;
  align-items: center;
}
.work-shift-toolbar__search {
  width: 200px;
}

.search-box {
  background-color: #fff;
  border: 1px solid #d1d5db;
  border-radius: 4px;
  width: 100%;
  display: flex;
  align-items: center;
  padding: 5px 12px;
  column-gap: 3px;
}
.search-box:hover {
  border-color: #9ca3af;
}
.search-box:focus-within {
  border-color: #009b71;
}

.work-shift-toolbar__search-icon {
  margin: 0 4px 0 0;
  flex-shrink: 0;
  cursor: pointer;
}
.wrapper-search-input {
  flex: 1;
  height: 16px;
}

.search-input {
  width: 100%;
}

.filter-chips {
  margin-left: 5px;
  display: flex;
  align-items: center;
}

.filter-chip {
  display: flex;
  gap: 8px;
  height: 24px;
  padding: 0 8px;
  border-radius: 4px;
  position: relative;
  margin-right: 8px;
  white-space: normal;
  align-items: center;
  background-color: #f3f4f6;
}

.chip-text {
  white-space: nowrap;
}

.chip-remove {
  cursor: pointer;
}

.clear-filter-label {
  color: #f06666;
  cursor: pointer;
  white-space: nowrap;
}

.work-shift-toolbar__button {
  padding: 6px 12px;
  display: flex;
  justify-content: center;
  align-items: center;
  border: 1px solid #d1d5db;
  color: #111827;
  background-color: #fff;
  transition: all 0.2s ease;
  cursor: pointer;
  border-radius: 4px;
  position: relative;
  font-size: 13px;
  height: 28px;
  font-weight: 500;
}
.work-shift-toolbar__button:hover {
  background-color: #f3f4f6;
}
</style>
