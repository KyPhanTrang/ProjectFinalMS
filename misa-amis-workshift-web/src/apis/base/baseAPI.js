import api from '@/apis/config/APIConfig.js';

export default class BaseAPI {
    constructor() {
        this.controller = null;
    }
    /**
     * Phương thức lấy tất cả dữ liệu
     */
    getAll() {
        return api.get(`${this.controller}`);
    }
    /**
     * Hàm lấy dữ liệu phân trang
     * @param {*} payload 
     */
    paging(params) {
        return api.get(`${this.controller}/paging`, { params });
    }

    /**
        * Thêm mới bản ghi
        * @param {Object} body
        */
    create(body) {
        return api.post(`${this.controller}`, body);
    }

    /**
     * Hàm cập nhật dữ liệu
     * @param {*} id 
     * @param {*} body 
     */
    update(id, body) {
        return api.put(`${this.controller}/${id}`, body);
    }
    /**
     * Hàm xóa bản ghi
     * @param {*} id 
     */
    delete(id) {
        return api.delete(`${this.controller}/${id}`);
    }
}