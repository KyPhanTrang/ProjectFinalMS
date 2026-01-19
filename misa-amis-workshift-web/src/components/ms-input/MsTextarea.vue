<!-- ========== BaseTextarea.vue ========== -->
<template>
  <div class="base-field">
    <label class="base-label">{{ label }}</label>
    <div class="base-input">
      <textarea
        :value="modelValue"
        rows="4"
        @input="$emit('update:modelValue', $event.target.value)"
      ></textarea>
    </div>
  </div>
</template>

<script setup>
defineProps({ label: String, modelValue: String });
</script>

<style scoped>
.base-field {
  width: 100%;
  display: flex;
  column-gap: 15px;
}
.base-label {
  width: 150px;
  font-weight: 500;
  color: #262626;
  display: block;
}

.base-input {
  flex: 1;
  background: #fff;
  overflow: auto;
  border-radius: 4px;

  border: 1px solid #d5dfe2;
  resize: none;
}

.base-input:focus-within {
  border-color: #16a34a;
}

textarea {
  padding: 6px 10px;
  height: 68px;
  max-height: 68px;
  width: 100%;
  border: none;
  outline: none;
  resize: none;
}
</style>
