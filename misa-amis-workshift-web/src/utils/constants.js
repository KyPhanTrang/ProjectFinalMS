// sidebar.menu.js
export const SIDEBAR_MENU = [
    // ===== Tổng quan =====
    {
        type: 'item',
        key: 'dashboard',
        label: 'Tổng quan',
        icon: 'icon--dashboard',
        to: '/dashboard',
    },

    // ===== Đơn đặt hàng =====
    {
        type: 'item',
        key: 'order',
        label: 'Đơn đặt hàng',
        icon: 'icon--order',
        to: '/orders',
    },

    // ===== Kế hoạch sản xuất =====
    {
        type: 'group',
        key: 'plan',
        label: 'Kế hoạch sản xuất',
        icon: 'icon--plan',
        to: '/plans',
        children: [
            { key: 'plan-master', label: 'Kế hoạch tổng thể', tab: 'master', to: '/plans' },
            { key: 'plan-detail', label: 'Kế hoạch chi tiết', tab: 'detail', to: '/plans' },
            { key: 'plan-material', label: 'Kế hoạch vật liệu', tab: 'material', to: '/plans' },
            {
                key: 'plan-purchase-request',
                label: 'Yêu cầu mua NVL',
                tab: 'purchase-request',
                to: '/plans',
            },
        ],
    },

    // ===== Điều phối và thực thi =====
    {
        type: 'group',
        key: 'coordinate',
        label: 'Điều phối và thực thi',
        icon: 'icon--coordinate',
        to: '/coordinate',
        children: [
            { key: 'production-order', label: 'Lệnh sản xuất', tab: 'order', to: '/coordinate' },
            { key: 'production-schedule', label: 'Lịch sản xuất', tab: 'schedule', to: '/coordinate' },
            {
                key: 'material-export-request',
                label: 'Yêu cầu xuất vật tư',
                tab: 'export-request',
                to: '/coordinate',
            },
            {
                key: 'production-statistic',
                label: 'Thống kê sản xuất',
                tab: 'statistic',
                to: '/coordinate',
            },
            {
                key: 'finished-import-request',
                label: 'Yêu cầu nhập thành',
                tab: 'import-request',
                to: '/coordinate',
            },
            {
                key: 'semi-handover',
                label: 'Bàn giao bán thành',
                tab: 'semi-handover',
                to: '/coordinate',
            },
            {
                key: 'handover-status',
                label: 'Tình hình bàn giao',
                tab: 'handover-status',
                to: '/coordinate',
            },
        ],
    },

    // ===== Kiểm tra chất lượng =====
    {
        type: 'group',
        key: 'quality-control',
        label: 'Kiểm tra chất lượng',
        icon: 'icon--quality-control',
        to: '/quality-control',
        children: [
            { key: 'qc-request', label: 'Yêu cầu kiểm tra', tab: 'request', to: '/quality-control' },
            { key: 'qc-ticket', label: 'Phiếu kiểm tra', tab: 'ticket', to: '/quality-control' },
            { key: 'qc-standard', label: 'Tiêu chuẩn', tab: 'standard', to: '/quality-control' },
        ],
    },

    // ===== Kho vật tư =====
    {
        type: 'group',
        key: 'material-factory',
        label: 'Kho vật tư',
        icon: 'icon--material-factory',
        to: '/material-factory',
        children: [
            {
                key: 'material-issue-request',
                label: 'Đề nghị kho cấp vật tư',
                tab: 'issue-request',
                to: '/material-factory',
            },
            { key: 'material-import', label: 'Nhập kho', tab: 'import', to: '/material-factory' },
            { key: 'material-export', label: 'Xuất kho', tab: 'export', to: '/material-factory' },
            { key: 'material-transfer', label: 'Điều chuyển', tab: 'transfer', to: '/material-factory' },
            { key: 'opening-stock', label: 'Tồn kho đầu kỳ', tab: 'opening', to: '/material-factory' },
        ],
    },

    // ===== Giao việc =====
    {
        type: 'group',
        key: 'work-order',
        label: 'Giao việc',
        icon: 'icon--work-order',
        to: '/work-orders',
        children: [
            { key: 'work-order-ticket', label: 'Phiếu giao việc', tab: 'ticket', to: '/work-orders' },
            { key: 'work-order-status', label: 'Tình hình giao việc', tab: 'status', to: '/work-orders' },
        ],
    },

    // ======== Truy xuất sản phẩm
    {
        type: 'item',
        key: 'file-search',
        label: 'Truy xuất nguồn gốc',
        icon: 'icon--file-search',
        to: '/file-search',
    },

    // ===== Báo cáo =====
    {
        type: 'item',
        key: 'production-report',
        label: 'Báo cáo',
        icon: 'icon--production-report',
        to: '/production-report',
        flag: true,
    },

    // ===== Sản phẩm, NVL =====
    {
        type: 'group',
        key: 'product-material',
        label: 'Sản phẩm, NVL',
        icon: 'icon--product-material',
        to: '/product-material',
        children: [
            {
                key: 'material-product',
                label: 'Vật tư hàng hóa',
                tab: 'product',
                to: '/product-material',
            },
            {
                key: 'material-group',
                label: 'Nhóm vật tư hàng hóa',
                tab: 'group',
                to: '/product-material',
            },
            {
                key: 'material-bom',
                label: 'Định mức nguyên vật liệu',
                tab: 'bom',
                to: '/product-material',
            },
            {
                key: 'material-alternative',
                label: 'Nguyên vật liệu thay thế',
                tab: 'alternative',
                to: '/product-material',
            },
        ],
    },

    // ===== Quy trình sản xuất =====
    {
        type: 'group',
        key: 'process',
        label: 'Quy trình sản xuất',
        icon: 'icon--process',
        to: '/process',
        children: [
            { key: 'process-step', label: 'Công đoạn', tab: 'step', to: '/process' },
            { key: 'process-flow', label: 'Quy trình sản xuất', tab: 'flow', to: '/process' },
        ],
    },

    // ===== Năng lực sản xuất =====
    {
        type: 'group',
        key: 'production-capacity',
        label: 'Năng lực sản xuất',
        icon: 'icon--production-capacity',
        to: '/production-capacity',
        children: [
            { key: 'production-team', label: 'Tổ sản xuất', tab: 'team', to: '/production-capacity' },
            { key: 'machine', label: 'Máy móc', tab: 'machine', to: '/production-capacity' },
            {
                key: 'capacity-group',
                label: 'Nhóm năng lực',
                tab: 'group',
                to: '/production-capacity',
            },
            { key: 'mold', label: 'Khuôn', tab: 'mold', to: '/production-capacity' },
        ],
    },

    // ===== Danh mục khác =====
    {
        type: 'item',
        key: 'work-shifts',
        label: 'Danh mục khác',
        icon: 'icon-production-category',
        to: '/work-shifts',
    },

    // ===== Thiết lập =====
    {
        type: 'item',
        key: 'production-system',
        label: 'Thiết lập',
        icon: 'icon-production-system',
        to: '/production-system',
    },
];