<template>
  <div class="header-right">
    <!-- Header content left -->
    <div class="header-content-left">
      <span class="header-name-company">Công ty cổ phần Misa 234</span>
      <span class="icon--mask icon--down-small"></span>
    </div>
    <!-- Header content right -->
    <div class="header-content-right">
      <!-- User Action -->
      <div class="user-action user-action--sp" title="Xuất Excel">
        <div class="icon--mask icon--download-status"></div>
      </div>
      <div class="header-separator"></div>
      <div class="user-action" title="Tra cứu tồn kho">
        <div class="icon--24 icon--package"></div>
      </div>
      <div class="user-action sp" title="Mời tham gia MISA AMIS">
        <div class="icon--amis-invite"></div>
      </div>
      <div class="user-action" title="Thông báo">
        <div class="icon--mask icon--24 icon--bell"></div>
      </div>
      <div class="user-action" title="Tính năng khác">
        <div class="icon--mask icon--24 icon--question-circle"></div>
      </div>
      <div class="user-action" title="Kiến thức hữu ích">
        <div class="icon--24 icon--dots-circle"></div>
      </div>
      <div class="user-action sp" title="Kiến thức hữu ích">
        <div class="icon--knowledge"></div>
      </div>
      <!-- User Avatar -->
      <div class="user-avatar">
        <img
          class="avatar"
          src="https://amisplatform.misacdn.net/APIS/PlatformAPI/api/Avatar/77b8feac-dfe5-4b47-b1ff-ac9a2eaced5a/LT0YJBOP.jpg?avatarID=03762a31-c8c5-45ae-887b-17bcdc47043b&amp;width=64&amp;height=64"
          alt="User avatar"
        />
      </div>
    </div>
  </div>
</template>

<script setup></script>
<style scoped>
/* Container */
.header-right {
  flex: 1;
  display: flex;
  justify-content: space-between;
  height: 100%;
  padding-left: 20px;
  padding-right: 16px;
}

/* Header content left */
.header-content-left {
  display: flex;
  align-items: center;
  padding-top: 2px;
}

.header-name-company {
  color: rgb(17, 24, 39);
  font-size: 13px;
  font-weight: 700;
  margin-right: 4px;
  white-space: nowrap;
}

/* Header content right */
.header-content-right {
  display: flex;
  align-items: center;
}
.header-actions {
  display: flex;
  justify-content: space-between;
}

.header-separator {
  width: 1px;
  height: 16px;
  background: #d1d5db;
  align-self: center;
  border-radius: 1px;
}

/* User Action */
.user-action {
  display: flex;
  align-items: center;
  justify-content: center;
  height: 24px;
  width: 24px;
  margin-left: 12px;
  border-radius: 50%;
  transition: background-color 0.1s ease;
}
.user-action:hover {
  background-color: rgba(0, 0, 0, 0.1);
  cursor: pointer;
}
.user-action--sp {
  margin-right: 12px;
  margin-left: unset;
}
.user-action.sp {
  width: 32px;
  height: 32px;
}

/* User avatar */
.user-avatar {
  margin-left: 19px;
  border-radius: 50%;
  cursor: pointer;
  width: 32px;
  height: 32px;
  overflow: hidden;
  border: none;
}

.avatar {
  width: 100%;
  height: 100%;
  max-width: 32px;
  max-height: 32px;
}
</style>
