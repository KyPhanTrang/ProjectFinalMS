<template>
  <div class="msgbox-overlay">
    <div class="msgbox">
      <!-- Header -->
      <div class="msgbox__header">
        <div class="msgbox__title">
          <div class="icon--warning"></div>
          <div class="msgbox__title-msg">
            {{ title }}
          </div>
        </div>

        <div
          class="msgbox__close icon--mask icon--close cursor-pointer"
          @click="$emit('close')"
        ></div>
      </div>

      <!-- Content -->
      <div v-html="message" class="msgbox__content"></div>

      <!-- Footer -->
      <div class="msgbox__footer">
        <template v-if="type === 'confirm'">
          <div class="wrapper-btn wrapper-btn-cancel cursor-pointer" @click="$emit('cancel')">
            <div class="btn btn-cancel">Hủy</div>
          </div>
          <div class="wrapper-btn wrapper-btn-delete cursor-pointer" @click="$emit('confirm')">
            <div class="btn btn-delete">Xóa</div>
          </div>
        </template>
        <template v-else>
          <div class="wrapper-btn wrapper-btn-close cursor-pointer" @click="$emit('close')">
            <div class="btn btn-close">Đóng</div>
          </div>
        </template>
      </div>
    </div>
  </div>
</template>

<script setup>
/**
 * PROPS
 */
defineProps({
  type: { type: String, default: 'alert' }, // alert | confirm
  title: {
    type: String,
    default: 'Cảnh báo!',
  },
  message: {
    type: String,
    required: true,
  },
});
/**
 * EMIT
 */
defineEmits(['close', 'confirm', 'cancel']);
</script>

<style scoped>
.msgbox-overlay {
  position: fixed;
  inset: 0;
  background: rgba(0, 0, 0, 0.3);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 9999;
}

.msgbox {
  width: 432px;
  background: #fff;
  border-radius: 4px;
  overflow: hidden;
  font-size: 14px;
  padding: 16px;
  display: flex;
  flex-direction: column;
}

.msgbox__header {
  display: flex;
  align-items: center;
  height: 24px;
  justify-content: space-between;
}

.msgbox__title {
  display: flex;
  justify-content: center;
  align-items: center;
  column-gap: 8px;
}
.msgbox__title-msg {
  font-size: 20px;
  font-weight: 600;
}
.icon--warning {
  mask-image: url(@/assets/icons/icon_sidebar.svg);
  mask-repeat: no-repeat;
  mask-position: -188px -169px;
  background-color: #ea580c;
  height: 20px;
  width: 20px;
}

.msgbox__content {
  padding: 16px 0;
  font-size: 13px;
  max-height: 400px;
  overflow-y: auto;
  font-weight: 400;
  line-height: 20px;
  max-width: 100%;
}

.msgbox__footer {
  display: flex;
  justify-content: flex-end;
  column-gap: 8px;
}

/* Chung */
.wrapper-btn {
  padding: 6px 12px;
  border-radius: 4px;
  height: 28px;
  line-height: 16px;
}

.btn {
  font-weight: 500;
  font-size: 13px;
}
/* Cancel */
.wrapper-btn-cancel {
  border: 1px solid #d1d5db;
  background-color: #fff;
}
.wrapper-btn-cancel:hover {
  background-color: #f3f4f6;
}

.btn-cancel {
  color: #111827;
}

/* Delete */
.wrapper-btn-delete {
  background-color: #dc2626;
}
.wrapper-btn-delete:hover {
  background-color: #b91c1c;
}
.btn-delete {
  color: #fff;
}

/* CLose */
.wrapper-btn-close {
  background-color: #009b71;
}
.wrapper-btn-close:hover {
  background-color: #007b5d;
}

.btn-close {
  color: #ffff;
}
</style>
