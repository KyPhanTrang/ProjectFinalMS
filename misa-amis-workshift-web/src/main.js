import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import './assets/styles/reset.css'
import './assets/styles/icon.css'
import './assets/styles/base.css'

const app = createApp(App)

app.use(router)

app.mount('#app')
