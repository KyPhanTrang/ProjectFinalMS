<template>
  <section class="work-shift-header">
    <!-- Title -->
    <div class="work-shift-header__title">Ca làm việc</div>

    <!-- Actions -->
    <div class="work-shift-header__actions">
      <!-- Add work-shift button -->
      <button @click="$emit('open')" class="work-shift-header__add-btn">
        <div class="work-shift-header__add-icon icon--mask icon--add"></div>
        <span class="work-shift-header__add-text">Thêm</span>
      </button>
    </div>
  </section>
</template>

<script setup>
// Emit
defineEmits(['open']);
</script>
<style scoped>
.work-shift-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 16px;
}
.work-shift-header__title {
  font-size: 24px;
  font-weight: 700;
  font-family: Inter;
  color: #111827;
}
.work-shift-header__actions {
  display: flex;
}
.work-shift-header__add-btn {
  position: relative;
  background-color: #009b71;
  height: 28px;
  border: none;
  display: flex;
  align-items: center;
  transition: all 0.2s ease;
  cursor: pointer;
  outline: none;
  padding: 6px 12px;
  border-radius: 4px;
  font-weight: 500;
  font-size: 13px;
  color: #fff;
}

.work-shift-header__add-btn:hover {
  background-color: #007b5d;
}
.work-shift-header__add-text {
  padding-left: 4px;
}
</style>
