<template>
  <div class="toast toast--success">
    <div class="toast-edge"></div>
    <div class="wrapper">
      <div class="toast__icon"></div>
      <div class="toast__message">{{ msg }}</div>
    </div>
    <div class="icon--mask icon--close cursor-pointer" @click="$emit('close')"></div>
  </div>
</template>

<script setup>
/**
 * Define props
 */

defineProps({
  msg: String,
});

/**
 * Define emit
 */
defineEmits(['close']);
</script>

<style scoped>
.toast {
  position: fixed;
  top: 8%;
  left: 50%;
  transform: translate(-50%, -50%);

  min-width: 300px;
  max-width: 360px;
  height: 48px;

  background-color: #fff !important;
  box-shadow: 0 4px 16px #0000003d;
  color: #fff;

  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 8px;
  padding-right: 16px;

  border-radius: 4px;
  z-index: 9999;
}

.toast-edge {
  width: 8px;
  height: 100%;
  border-top-left-radius: 4px;
  border-bottom-left-radius: 4px;
  background: #009b71;
  z-index: -1;
}

.wrapper {
  flex: 1;
  display: flex;
  column-gap: 8px;
}

.toast__icon {
  background-image: url(@/assets/icons/icon_sidebar.svg);
  background-repeat: no-repeat;
  background-position: -99px -169px;
  width: 18px;
  height: 18px;
}

.toast__message {
  color: rgb(17, 24, 39);
}
</style>
