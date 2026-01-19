<template>
  <div class="sidebar-group" :class="{ 'is-collapsed': collapsed }">
    <div
      class="sidebar-item sidebar-item--group cursor-pointer"
      @click="toggle"
      :class="{ 'is-active': open }"
    >
      <div class="sidebar__icon icon--sidebar" :class="item.icon"></div>
      <span v-show="!collapsed" class="sidebar__text">
        {{ item.label }}
      </span>
      <div v-show="!collapsed" class="wrapper-icon-dropdown">
        <div class="icon--sidebar icon--dropdown" :class="{ rotate: open }"></div>
      </div>
    </div>

    <div v-show="open" class="sidebar-group__children">
      <SidebarItemChild
        v-for="child in item.children"
        :key="child.key"
        :item="child"
        :collapsed="collapsed"
      />
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue';
import SidebarItemChild from './SidebarItemChild.vue';

const props = defineProps({
  item: Object,
  collapsed: Boolean,
});

const open = ref(false);

const toggle = () => (open.value = !open.value);
</script>

<style scoped>
/* If open */
.rotate {
  mask-position: -180px -16px;
  margin-bottom: 2px;
}

/* Header */
.sidebar-group.is-collapsed {
  max-height: 36px;
}

/* Children */
.sidebar-group__children {
  margin-top: 4px;
  row-gap: 4px;
}

.sidebar-item--group {
  display: flex;
}

.wrapper-icon-dropdown {
  flex: 1;
  display: flex;
  justify-content: flex-end;
}

.icon--dropdown {
  margin-right: 8px;
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
  white-space: nowrap;
}
</style>
