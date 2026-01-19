<template>
  <div v-show="item.flag" class="menu-item-line"></div>
  <RouterLink
    :to="item.to"
    class="sidebar-item sidebar-item--root"
    :class="{ 'is-active': isActive }"
  >
    <div class="sidebar__icon icon--sidebar" :class="item.icon"></div>
    <div v-show="!collapsed" class="sidebar__text">
      {{ item.label }}
    </div>
  </RouterLink>
  <div v-show="item.flag" class="menu-item-line"></div>
</template>

<script setup>
import { computed } from 'vue';
import { useRoute } from 'vue-router';

const props = defineProps({
  item: Object,
  collapsed: Boolean,
});

const route = useRoute();

const isActive = computed(() => route.path === props.item.to);
</script>

<style scoped>
.menu-item-line {
  margin: 8px auto;
  width: 100%;
  min-width: 36px;
  border-bottom: 1px solid rgba(209, 213, 219, 0.3);
}

/* Active */
.is-active {
  background-color: #009b71;
}

.sidebar-item.is-active:hover {
  background-color: #009b71;
}

.sidebar-item.is-active .sidebar__icon {
  background-color: #fff;
}

.sidebar-item.is-active .sidebar__text {
  color: #fff;
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

.sidebar__icon {
  margin-left: 8px;
}

.sidebar__text {
  color: #9ca3af;
}
</style>
