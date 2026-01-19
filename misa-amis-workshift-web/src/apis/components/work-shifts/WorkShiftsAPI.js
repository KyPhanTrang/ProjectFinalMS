import api from '@/apis/config/APIConfig.js';
import BaseAPI from '@/apis/base/baseAPI.js';

class WorkShiftsAPI extends BaseAPI {
    constructor() {
        super();
        this.controller = "workshifts";
    }

    /**
         * Hàm kiểm tra trùng lặp mã ca làm việc
         * @param {*} payload 
         * @returns 
         */
    checkDuplicate(payload) {
        return api.post(`${this.controller}/check-duplicate`, payload);
    }

    /**
     * Hàm kiểm tra trùng lặp mã ca làm việc
     * @param {*} payload 
     * @returns 
     */
    checkDuplicate(payload) {
        return api.post(`${this.controller}/check-duplicate`, payload);
    }
}

export default new WorkShiftsAPI();