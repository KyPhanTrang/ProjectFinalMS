<!-- src/components/layout/TheHeader.vue -->
<template>
  <header class="app-header">
    <header-left></header-left>
    <header-right></header-right>
  </header>
</template>

<script setup>
import HeaderLeft from './HeaderLeft.vue';
import HeaderRight from './HeaderRight.vue';
</script>

<style scoped>
.app-header {
  width: 100%;
  height: 48px;
  background-color: #fff;
  display: flex;
  align-items: center;
  font-family: Inter;
  border-bottom: 1px solid #d1d5db;
  position: relative;
}
</style>
