<!-- src/components/layout/TheSidebar.vue -->
<template>
  <aside class="app-sidebar" :class="{ 'app-sidebar--collapsed': collapsed }">
    <!-- Sidebar content -->
    <nav class="sidebar">
      <!-- Menu list -->
      <div class="sidebar__menu">
        <component
          v-for="item in SIDEBAR_MENU"
          :key="item.key"
          :is="resolveComponent(item)"
          :item="item"
          :collapsed="collapsed"
        />
        <div class="sidebar__footer">
          <div class="sidebar__collapse-btn" @click="toggleSidebar">
            <div
              class="sidebar__icon icon--sidebar icon--bottom"
              :class="{ 'icon--rotate': collapsed }"
            ></div>
            <div class="sidebar__text footer-text-bottom" title="Thu gọn" v-show="!collapsed">
              Thu gọn
            </div>
          </div>
        </div>
      </div>
    </nav>
  </aside>
</template>

<script setup>
import { ref, onMounted } from 'vue';
// import { useRoute } from 'vue-router';
import SidebarItem from '@/components/ms-sidebar/SidebarItem.vue';
import SidebarGroup from '@/components/ms-sidebar/SidebarGroup.vue';
import { SIDEBAR_MENU } from '@/utils/constants';

onMounted(() => {
  const saved = localStorage.getItem('sidebar-collapsed');

  if (saved !== null) {
    collapsed.value = JSON.parse(saved);
  }
});

/**
 * Render ra type của item thuộc sidebar
 * @param item (có 2 kiểu) - 1 Group "có dropdown", 2 Item không có dropdown
 */
const resolveComponent = (item) => {
  return item.type === 'group' ? SidebarGroup : SidebarItem;
};
/**
 * Xử lý sidebar co dãn
 */
const collapsed = ref(false);
const toggleSidebar = () => {
  collapsed.value = !collapsed.value;
  // Lưu trạng thái vào localStorage
  localStorage.setItem('sidebar-collapsed', JSON.stringify(collapsed.value));
};
</script>

<style>
.app-sidebar {
  display: flex;
  position: relative;
  width: 234px;
  min-height: calc(100vh - 56px);
  height: 100%;
  background-color: #111827;
  overflow-y: auto;
  scrollbar-gutter: stable;
  overflow-x: hidden;
}

::-webkit-scrollbar {
  width: 8px;
  height: 8px;
  background-color: transparent;
  -webkit-transition: all 0.2s cubic-bezier(0.4, 0, 0.2, 1);
  transition: all 0.2s cubic-bezier(0.4, 0, 0.2, 1);
}

::-webkit-scrollbar-thumb {
  background-color: #b8bcc3;
}

::-webkit-scrollbar-track {
  background-color: #f1f1f1;
  width: 20px;
}

/* State Hidden */
.app-sidebar--collapsed {
  width: 60px;
  scrollbar-width: none;
}

.app-sidebar--collapsed .sidebar__text {
  display: none;
}

/* Sidebar */
.sidebar {
  flex-shrink: 0;
  flex: 1;
  position: relative;
  display: flex;
  flex-direction: column;
  min-width: 60px;
}

a {
  outline: none;
  text-decoration: none;
}

/* Sidebar Menu */
.sidebar__menu {
  position: relative;
  flex-shrink: 0;
  flex: 1;
  display: flex;
  flex-direction: column;
  padding: 12px 12px 0 12px;
  row-gap: 4px;
  min-height: 0;
}

/* Sidebar Bottom */
.sidebar__footer {
  width: 100%;
  height: 56px;
  margin-top: 10px;
  display: flex;
  align-items: center;
  background: #111827;
  border-top: 1px solid rgba(209, 213, 219, 0.3);
}

.sidebar__collapse-btn {
  width: 100%;
  min-width: 36px;
  height: 36px;
  min-height: 36px;
  border-radius: 4px;
  background: #111827;
  color: #9ca3af;
  z-index: 2;
  display: flex;
  justify-content: center;
  align-items: center;
  padding-right: 5px;
}

.sidebar__collapse-btn:hover {
  background-color: #2b313ec7;
  cursor: pointer;
}

.footer-text-bottom {
  margin-left: 8px;
  font-size: 13px;
}

.icon--bottom {
  width: 20px;
  height: 20px;
  background-color: white;
}
</style>
