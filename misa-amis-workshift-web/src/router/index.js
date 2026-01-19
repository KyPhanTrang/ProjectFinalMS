import { createRouter, createWebHistory } from 'vue-router'
import AppLayout from '@/layouts/AppLayout.vue'

// Dashboard
import Dashboard from '@/views/dashboard/Index.vue'

// Work
import WorkShift from '@/views/work-shift/Index.vue'
import WorkOrder from '@/views/work-order/Index.vue'

// Order & Plan
import Order from '@/views/order/Index.vue'
import Plan from '@/views/plan/Index.vue'

// Production
import ProductionSystem from '@/views/production-system/Index.vue'
import ProductionCapacity from '@/views/production-capacity/Index.vue'
import ProductionReport from '@/views/production-report/Index.vue'

// Material
import MaterialFactory from '@/views/material-factory/Index.vue'
import ProductMaterial from '@/views/product-material/Index.vue'

// Process & Quality
import Process from '@/views/process/Index.vue'
import QualityControl from '@/views/quality-control/Index.vue'

// Others
import Coordinate from '@/views/coordinate/Index.vue'
import FileSearch from '@/views/file-search/Index.vue'



const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [{
    path: '/',
    component: AppLayout,
    children: [
      {
        path: '',
        redirect: '/work-shifts'
      }, {
        path: 'work-shifts',
        name: 'work-shifts',
        component: WorkShift
      },
      {
        path: 'dashboard',
        name: 'dashboard',
        component: Dashboard
      },
      {
        path: 'work-orders',
        name: 'work-orders',
        component: WorkOrder
      },

      {
        path: 'orders',
        name: 'orders',
        component: Order
      },
      {
        path: 'plans',
        name: 'plans',
        component: Plan
      },

      {
        path: 'production-system',
        name: 'production-system',
        component: ProductionSystem
      },
      {
        path: 'production-capacity',
        name: 'production-capacity',
        component: ProductionCapacity
      },
      {
        path: 'production-report',
        name: 'production-report',
        component: ProductionReport
      },

      {
        path: 'material-factory',
        name: 'material-factory',
        component: MaterialFactory
      },
      {
        path: 'product-material',
        name: 'product-material',
        component: ProductMaterial
      },

      {
        path: 'process',
        name: 'process',
        component: Process
      },
      {
        path: 'quality-control',
        name: 'quality-control',
        component: QualityControl
      },

      {
        path: 'coordinate',
        name: 'coordinate',
        component: Coordinate
      },
      {
        path: 'file-search',
        name: 'file-search',
        component: FileSearch
      }
    ]
  }],
})

export default router
