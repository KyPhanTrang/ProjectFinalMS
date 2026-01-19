<template>
  <div class="ms-select" ref="root">
    <div class="ms-select__control" ref="control" @click="toggle">
      <input class="ms-select__value" readonly />
      <span class="ms-select__arrow"></span>
    </div>

    <div v-if="isOpen" class="ms-select__dropdown" ref="dropdown" :style="dropdownStyle">
      <div v-for="opt in [10, 20, 30, 50, 100]" :key="opt" class="ms-select__option">
        {{ opt }}
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, nextTick } from 'vue';
const root = ref(null);
const control = ref(null);
const dropdown = ref(null);

const isOpen = ref(false);
const dropdownStyle = ref({});

async function calculateDropdownPosition() {
  debugger;
  await nextTick();

  const controlRect = control.value.getBoundingClientRect();
  const dropdownRect = dropdown.value.getBoundingClientRect();

  const viewportHeight = window.innerHeight;

  const spaceBelow = viewportHeight - controlRect.bottom;
  const spaceAbove = controlRect.top;

  // Reset
  dropdown.value = {};

  if (spaceBelow >= dropdownRect.height) {
    // xổ xuống
    dropdownStyle.value = {
      top: `${controlRect.height + 4}px`,
      bottom: 'auto',
    };
  } else if (spaceAbove >= dropdownRect.height) {
    // xổ lên
    dropdownStyle.value = {
      bottom: `${controlRect.height + 4} px`,
      top: 'auto',
    };
  } else {
    // không đủ chỗ cả hai → chọn phía rộng hơn
    if (spaceBelow >= spaceAbove) {
      dropdownStyle.value = {
        top: `${controlRect.height + 4}px`,
        maxHeight: `${spaceBelow - 8}px`,
        overflowY: 'auto',
      };
    } else {
      dropdownStyle.value = {
        bottom: `${controlRect.height + 4}px`,
        maxHeight: `${spaceAbove - 8}px`,
        overflowY: 'auto',
      };
    }
  }
}

async function toggle() {
  isOpen.value = !isOpen.value;
  if (isOpen.value) {
    await calculateDropdownPosition();
  }
}
</script>

<style scoped>
.ms-select {
  position: relative;
}
.ms-select__control {
  background-color: azure;
  padding: 5px 8px 5px 12px;
  background: #fff;
  border: 1px solid #d1d5db;
  border-radius: 4px;
  width: 80px;
  display: flex;
}
.ms-select__control input {
  width: 100%;
  outline: 1px;
  border: 1px solid #ccc;
}
.ms-select__value {
}
.ms-select__arrow {
}
.ms-select__dropdown {
  position: absolute;
  top: -50%;
  width: 100%;
}
.ms-select__option {
}
</style>
