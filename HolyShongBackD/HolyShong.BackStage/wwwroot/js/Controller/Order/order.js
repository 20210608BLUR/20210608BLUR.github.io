
//金額千分位逗號
function CurrencyFormat(num) {
    var parts = num.toString().split('.');
    parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ',');
    return parts.join('.');
}

//日期時間格式化
//function DateFormat(value, format) {
//    return moment(value).format(format || 'YYYY/MM/DD HH:mm:ss');
//}

//超過指定內容長度以...取代
function StringContentFormat(content, maxlength = 1) {
    if (maxlength <= 0 || content.length <= maxlength) {
        return content;
    }
    else {
        return content.substring(0, maxlength - 1) + '...';
    }
}

let orderVue = new Vue({
    el: '#order',
    data: {
        isBusy: true,
        tabIndex: 0,
        preparingFields: [
            { key: 'orderId', label: '訂單編號', sortable: true, class: 'text-center' },
            { key: 'memberId', label: '會員編號', sortable: true, class: 'text-center' },
            { key: 'storeId', label: '店家編號', sortable: true, class: 'text-center' },
            { key: 'storeName', label: '店家名稱', sortable: true, class: 'text-center' },
            { key: 'discountName', label: '使用優惠', sortable: true, class: 'text-center' },
            { key: 'orderStatus', label: '訂單狀態', sortable: true, class: 'text-center' },
            { key: 'updateStatus', label: '修改狀態', sortable: true, class: 'text-center' },
            { key: 'Action', label: '功能', sortable: true, class: 'text-center' }
        ],
        historyFields: [
            { key: 'orderId', label: '訂單編號', sortable: true, class: 'text-center' },
            { key: 'memberId', label: '會員編號', sortable: true, class: 'text-center' },
            { key: 'storeId', label: '店家編號', sortable: true, class: 'text-center' },
            { key: 'storeName', label: '店家名稱', sortable: true, class: 'text-center' },
            { key: 'discountName', label: '使用優惠', sortable: true, class: 'text-center' },
            { key: 'orderStatus', label: '訂單狀態', sortable: true, class: 'text-center' },
            { key: 'Action', label: '功能', sortable: true, class: 'text-center' }
        ],
        preparingOptions: [
            { value: '1', text: '已付款' },
            { value: '2', text: '餐點準備中' },
            { value: '3', text: '餐點完成' }
        ],
        historyOptions: [
            { value: '4', text: '等待配送' },
            { value: '5', text: '配送中' },
            { value: '6', text: '配送完成' }
        ],
        //表格所有資料
        preparingItems: [],
        orderList: [],
        historyItems: [],
        //頁碼
        preparingCurrentPage: 1,
        preparingPerPage: 10,
        historyCurrentPage: 1,
        historyPerPage: 10,
        filter: '',
        infoModal: {
            id: 'info-modal',
            title: '',
            content: ''
        }
    },
    created() {
        this.getPreparingOrders();
        this.getHistoryOrders();
    },
    methods: {
        getPreparingOrders() {            
            axios.get('/api/Order/GetPreparingOrders')
                .then(res => {
                    console.log(res);
                    if (res.data.isSuccess == true) {
                        this.preparingItems = res.data.result;
                        this.isBusy = false;
                    }
                    else {
                        console.log(res.data.exception)
                    }
                });
        },
        getHistoryOrders() {
            axios.get('/api/Order/GetHistoryOrders')
                .then(res => {
                    console.log(res);
                    if (res.data.isSuccess == true) {
                        this.historyItems = res.data.result;
                        this.isBusy = false;
                    }
                    else {
                        console.log(res.data.exception)
                    }
                });
        },
        //InfoModal
        resetInfoModal() {
            this.infoModal.title = '',
                this.infoModal.content = '';
        },
        preparingOrderInfo(item, index, button) {
            this.infoModal.title = `訂單資訊`;
            this.infoModal.content = JSON.stringify(item, null, 2);
            this.$root.$emit('bv::show::modal', this.infoModal.id, button);
            console.log(this.infoModal.id)
            this.orderList = item;
            this.orderList.orderPrice = CurrencyFormat(item.orderPrice);
        },
        historyOrderInfo(item, index, button) {
            this.infoModal.title = `訂單資訊`;
            this.infoModal.content = JSON.stringify(item, null, 2);
            this.$root.$emit('bv::show::modal', this.infoModal.id, button);
            this.orderList = item;
            this.orderList.orderPrice = CurrencyFormat(item.orderPrice);
        },
        updateOrderStatus(item) {
            axios.post('/api/Order/UpdateOrder', {
                OrderId: item.orderId,
                OrderStatus: item.orderStatus
            })
                .then(res => {
                    if (res.data.isSuccess == true) {
                        alert('修改成功');
                        orderVue.getPreparingOrders();
                        orderVue.getHistoryOrders();
                    }
                    else {
                        throw new UserException('Unknown Error');
                    }
                })
                .catch(ex => {
                    this.IsSuccess = false;
                    this.Exception = ex.toString();
                    console.log(item);
                });
        },
    },
    computed: {
        preparingTotalRows() {
            return this.preparingItems.length;
        },
        historyTotalRows() {
            return this.historyItems.length;
        }
    },
    watch: {

    },
});