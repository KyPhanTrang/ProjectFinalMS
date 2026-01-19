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
const emit = defineEmits(['reload', 'search']);
const keyword = ref('');

// Action
function onSearch() {
  emit('search', keyword.value);
}

function onReload() {
  keyword.value = '';
  emit('reload');
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
