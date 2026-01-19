<!-- src/layouts/AppLayout.vue -->
<template>
  <div class="app">
    <the-header></the-header>
    <div class="app-body">
      <the-sidebar></the-sidebar>

      <main class="app-main">
        <router-view />
      </main>
    </div>

    <!-- <app-modal></app-modal> -->
  </div>
</template>

<script setup>
import TheHeader from './header/Index.vue';
import TheSidebar from './TheSidebar.vue';
</script>

<style scoped>
.app {
  width: 100%;
  height: 100vh;
  display: flex;
  flex-direction: column;
  overflow: hidden;
}

.app-body {
  flex: 1;
  display: flex;
  overflow: hidden;
}

.app-main {
  flex: 1;
  background-color: #e5e7eb;
  overflow: hidden;
  display: flex;
}
</style>
