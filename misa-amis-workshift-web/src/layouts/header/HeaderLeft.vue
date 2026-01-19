<template>
  <!-- Header Left -->
  <div class="header-left">
    <!-- Header Brand -->
    <div class="header-brand">
      <!-- Brand Switch -->
      <div class="brand-switch">
        <div class="icon--mask icon--switch-app"></div>
      </div>
      <!-- Brand Logo -->
      <div class="brand-logo">
        <div class="icon--logo-item"></div>
      </div>
      <!-- Brand Title -->
      <div class="brand-title">Sản xuất</div>
    </div>
  </div>
</template>

<script setup>
// import BaseSearchBox from '@/components/base/BaseSearchBox.vue'
</script>
<style scoped>
/* Header left */
.header-left {
  min-width: 196px;
  height: 100%;
  display: flex;
  align-items: center;
  padding-left: 16px;
  margin-right: 16px;
  background-color: #fff;
}

/* Header brand */
.header-brand {
  display: flex;
  align-items: center;
}

/* Brand switch */
/* .brand-switch {
} */

/* Brand Logo */
.brand-logo {
  margin-left: 16px;
}

/* Brand Title */
.brand-title {
  font-size: 18px;
  margin-left: 12px;
  font-weight: 700;
  color: #262626;
  white-space: nowrap;
}
</style>
