import { createRouter, createWebHistory } from 'vue-router'
import MainView from '@/views/MainView.vue'
import TrackerView from '@/views/TrackerView.vue'
import InfoView from '@/views/InfoView.vue'


const routes = [
  {
    path: '/',
    name: 'home',
    component: MainView
  },
  {
    path: '/main',
    name: 'main',
    component: MainView
  },
  {
    path: '/info',
    name: 'info',
    component: InfoView
  },
  {
    path: '/tracker',
    name: 'tracker',
    component: TrackerView
  }
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

export default router
