<!-- ================= ShiftForm.vue ================= -->
<template>
  <div
    class="wrapper-modal"
    ref="modalRef"
    tabindex="-1"
    @keydown.enter.prevent="$emit('form-submit')"
    @keydown.esc="$emit('close')"
    @keydown.ctrl.s.prevent="$emit('form-submit')"
  >
    <div class="modal">
      <div class="modal__header">
        <div class="title">{{ mode == 'create' ? 'Thêm Ca ' : 'Sửa Ca ' }}làm việc</div>
        <div class="title-right">
          <div class="icon--mask icon--help cursor-pointer"></div>
          <div class="icon--mask icon--close cursor-pointer" @click="$emit('close')"></div>
        </div>
      </div>

      <div class="modal__body">
        <div class="form-row">
          <ms-input
            ref="shiftCodeRef"
            label="Mã ca"
            required
            v-model="form.shiftCode"
            :error="errors.shiftCode"
          />
        </div>
        <div class="form-row">
          <ms-input label="Tên ca" required v-model="form.shiftName" :error="errors.shiftName" />
        </div>

        <div class="modal__rows">
          <div class="form-row">
            <div class="form-col form-col--half left">
              <ms-time-select
                label="Giờ vào ca"
                required
                v-model="form.startTime"
                :error="errors.startTime"
              />
            </div>
            <div class="form-col form-col--half">
              <ms-time-select
                label="Giờ hết ca"
                required
                v-model="form.endTime"
                :full-width="true"
                :error="errors.endTime"
              />
            </div>
          </div>

          <div class="form-row">
            <div class="form-col form-col--half left">
              <ms-time-select
                label="Bắt đầu nghỉ giữa ca"
                v-model="form.breakStartTime"
                :error="errors.breakStartTime"
              />
            </div>
            <div class="form-col form-col--half">
              <ms-time-select
                label="Kết thúc nghỉ giữa ca"
                v-model="form.breakEndTime"
                :error="errors.breakEndTime"
                :full-width="true"
              />
            </div>
          </div>

          <div class="form-row">
            <div class="form-col form-col--half left">
              <ms-input
                label="Thời gian làm việc (giờ)"
                disabled
                v-model="form.workingTime"
                :maxWidth="true"
              />
            </div>
            <div class="form-col form-col--half">
              <ms-input
                label="Thời gian nghỉ giữa ca (giờ)"
                disabled
                v-model="form.breakingTime"
                :full-width="true"
                :maxWidth="true"
                :fullWidth="true"
              />
            </div>
          </div>
        </div>
        <ms-textarea label="Mô tả" v-model="form.description" />
        <div v-show="mode === 'update'">
          <div class="wrapper-status-info">
            <div class="info-label">Trạng thái</div>

            <div class="info-action">
              <label class="radio">
                <input type="radio" :value="true" v-model="form.isActive" />
                <span class="radio__check"> </span>
                <span class="radio__label"> Đang sử dụng</span>
              </label>

              <label class="radio">
                <input type="radio" :value="false" v-model="form.isActive" />
                <span class="radio__check"></span>
                <span class="radio__label">Ngừng sử dụng</span>
              </label>
            </div>
          </div>
        </div>
      </div>

      <div class="modal__footer">
        <button class="btn btn-close" @click="$emit('close')">Hủy</button>
        <button class="btn">Lưu và Thêm</button>
        <button class="btn btn-primary" @click="$emit('form-submit')">Lưu</button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, nextTick } from 'vue';
import MsInput from '@/components/ms-input/MsInput.vue';
import MsTextarea from '@/components/ms-input/MsTextarea.vue';
import MsTimeSelect from '@/components/ms-input/MsTimeSelect.vue';

/**
 * Auto focus
 */
const shiftCodeRef = ref(null);
const modalRef = ref(null);
onMounted(async () => {
  await nextTick();
  shiftCodeRef.value?.focus();
});

/**
 * DEFINE EMIT
 */
defineEmits(['close', 'form-submit']);

/**
 * DEFINE PROPS
 */
defineProps({
  form: Object,
  errors: Object,
  mode: String,
});
</script>

<style scoped>
.wrapper-modal {
  top: 0;
  right: 0;
  bottom: 0;
  left: 0;
  position: absolute;
  width: 100%;
  height: 100%;
  z-index: 999;
  background-color: #0000003b;
  display: flex;
  justify-content: center;
  align-items: center;
}

.modal {
  width: 680px;
  background: #fff;
  border-radius: 4px;
  display: flex;
  flex-direction: column;
}
.modal__header {
  padding: 16px 20px;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.title {
  font-size: 24px;
  font-weight: 700;
  color: #111827;
  margin-right: 32px;
  white-space: nowrap;
  cursor: text;
}
.title-right {
  display: flex;
  align-items: center;
  column-gap: 8px;
}
.modal__body {
  padding: 20px;
}

.form-row {
  display: flex;
  margin-bottom: 16px;
}

.form-col--half {
  width: 50%;
}

.form-col--half.left {
  margin-right: 12px;
}

/* Status */
.wrapper-status-info {
  margin-top: 16px;
  display: flex;
  align-items: center;
  column-gap: 16px;
}

.info-label {
  width: 150px;
  color: #262626;
  font-weight: 600;
}
.info-action {
  display: flex;
  align-items: center;
  column-gap: 16px;
}
.radio {
  display: flex;
  align-items: center;
}

.radio input {
  display: none;
}

/* vòng tròn ngoài */
.radio__check {
  width: 16px;
  height: 16px;
  border: 1px solid #ccc;
  border-radius: 50%;
  position: relative;
}

/* vòng tròn ngoài */
.radio__check:hover {
  border: 3px solid #009b71;
}

/* khi checked → đổi viền */
.radio input:checked + .radio__check {
  border-color: #009b71;
}

/* chấm tròn bên trong */
.radio input:checked + .radio__check::after {
  content: '';
  width: 8px;
  height: 8px;
  background-color: #009b71;
  border-radius: 50%;
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
}

.radio__label {
  font-weight: 500;
  margin-left: 8px;
}

.modal__footer {
  display: flex;
  justify-content: flex-end;
  gap: 12px;
  padding: 12px 20px;
  border-top: 1px solid #e5e7eb;
}
.btn {
  cursor: pointer;
  padding: 6px 12px;
  border-radius: 4px;
  border: 1px solid #d1d5db;
  background-color: #fff;
  color: #111827;
  font-weight: 500;
}
.btn:hover {
  background-color: #e0e0e098;
}
.btn.btn-primary {
  background: #16a34a;
  color: #fff;
  border-color: #16a34a;
}

.btn.btn-primary:hover {
  background: #0a4e23ee;
  color: #fff;
  border-color: #0a4e23ee;
}
</style>
