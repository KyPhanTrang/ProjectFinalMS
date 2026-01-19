export const workShiftFields = [
    // Checkbox
    { key: 'select', label: '--', type: 'custom', width: '40' },

    { key: 'shiftCode', label: 'Mã ca', type: 'text', width: '120' },
    { key: 'shiftName', label: 'Tên ca', type: 'text', width: '250' },

    { key: 'startTime', label: 'Giờ vào ca', type: 'time', width: '130' },
    { key: 'endTime', label: 'Giờ hết ca', type: 'time', width: '130' },
    { key: 'breakStartTime', label: 'Bắt đầu nghỉ giữa ca', type: 'time', width: '210' },
    { key: 'breakEndTime', label: 'Kết thúc nghỉ giữa ca', type: 'time', width: '210' },

    {
        key: 'workingTime',
        label: 'Thời gian làm việc (giờ)',
        type: 'number',
        width: '210',
        align: 'right',
    },
    {
        key: 'breakingTime',
        label: 'Thời gian nghỉ giữa ca (giờ)',
        type: 'number',
        width: '210',
        align: 'right',
    },

    { key: 'isActive', label: 'Trạng thái', type: 'custom', width: '200' },

    { key: 'createdBy', label: 'Người tạo', type: 'text', width: '160' },
    { key: 'createdDate', label: 'Ngày tạo', type: 'date', width: '160' },
    { key: 'modifiedBy', label: 'Người sửa', type: 'text', width: '160' },
    { key: 'modifiedDate', label: 'Ngày sửa', type: 'date', width: '160' },
];