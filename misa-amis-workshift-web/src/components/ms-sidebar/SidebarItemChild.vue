<template>
  <div v-show="!collapsed">
    <RouterLink
      :to="{ path: item.to, query: { tab: item.tab } }"
      class="sidebar-item sidebar-item--child"
      :class="{ 'is-active': isActive }"
    >
      <!-- icon ẩn -->
      <div class="sidebar__icon icon--sidebar child-icon"></div>
      <span class="sidebar__text">
        {{ item.label }}
      </span>
    </RouterLink>
  </div>
</template>

<script setup>
import { computed } from 'vue';
import { useRoute } from 'vue-router';

const props = defineProps({
  item: Object,
  collapsed: Boolean,
});

const route = useRoute();
const isActive = computed(() => {
  return route.path === props.item.to && route.query.tab === props.item.tab;
});
</script>

<style scoped>
.sidebar-item--child {
  margin-top: 4px;
}

/* Active */
.is-active {
  background-color: #4b5563;
}

.sidebar-item.is-active:hover {
  background-color: #4b5563;
}

.sidebar-item.is-active .sidebar__icon {
  background-color: #fff;
}

.sidebar-item.is-active .sidebar__text {
  color: #fff;
}

.sidebar-item.is-active .icon--dropdown {
  background-color: #fff;
}

/* Normal */
.sidebar-item {
  width: 100%;
  min-width: 36px;
  height: 36px;
  min-height: 36px;
  display: flex;
  align-items: center;
  column-gap: 8px;
  padding: 9px 0;
  border-radius: 4px;
}

.sidebar-item:hover {
  background-color: #1f283c;
}

.sidebar-item:hover .sidebar__icon {
  background-color: #fff;
}

.sidebar-item:hover .sidebar__text {
  color: #fff;
}

.sidebar-item:hover .icon--dropdown {
  background-color: #fff;
}

.sidebar__icon {
  margin-left: 8px;
}

.sidebar__text {
  color: #9ca3af;
}
</style>
