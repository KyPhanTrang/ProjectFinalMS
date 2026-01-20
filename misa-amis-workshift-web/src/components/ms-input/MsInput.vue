<!-- ========== BaseInput.vue ========== -->
<template>
  <div class="base-field" :class="{ 'full-width': fullWidth }">
    <label class="base-label"> {{ label }} <span v-if="required" class="req">*</span> </label>
    <div class="base-input" :class="{ error, disabled, 'max-width': maxWidth }" :title="error">
      <input
        ref="inputRef"
        :value="modelValue"
        :disabled="disabled"
        :class="{ 'text-align-right': disabled }"
        @input="$emit('update:modelValue', $event.target.value)"
      />
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue';
/**
 * Props
 */
defineProps({
  label: String,
  modelValue: [String, Number],
  required: Boolean,
  disabled: Boolean,
  error: String,
  maxWidth: Boolean,
  fullWidth: Boolean,
});

const inputRef = ref(null);

const focus = () => {
  inputRef.value?.focus();
};

defineExpose({
  focus,
});
</script>

<style scoped>
.base-field {
  width: 100%;
  display: flex;
  align-items: center;
  column-gap: 15px;
}
.full-width {
  justify-content: space-between;
}

.base-label {
  width: 150px;
  font-weight: 500;
  color: #262626;
  display: block;
  white-space: nowrap;
}
.req {
  color: red;
}
.base-input {
  flex: 1;
  border: 1px solid #d1d5db;
  border-radius: 4px;
  padding: 5px 12px;
}
.base-input input {
  height: 16px;
  width: 100%;
  border: none;
  outline: none;
}
.base-input:focus-within {
  border-color: #16a34a;
}
.base-input.error {
  border-color: red;
}
.base-input.disabled {
  background: #f3f4f6;
}
.base-input.max-width {
  max-width: 122px;
}

.text-align-right {
  text-align: right;
  color: #4b5563;
}
</style>
