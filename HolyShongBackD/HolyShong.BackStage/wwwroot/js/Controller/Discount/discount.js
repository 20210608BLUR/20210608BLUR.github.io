let discountVue = new Vue({
    el: '#discount',
    data: {
        isBusy: true,
        fields: [
            { key: 'discountCode', label: '優惠代碼'},
            { key: 'displayName', label: '優惠名稱' },
            { key: 'amount', label: '優惠折扣' },
            { key: 'startTime', label: '優惠結束', formatter: (value,key,item)=>{ return value ? value.split('T')[0] : '' }},
            { key: 'endTime', label: '優惠結束', formatter: (value, key, item) => { return value ? value.split('T')[0] : '' }},
            { key: 'Action', label: '功能' }
        ],
        //表格所有資料
        items: [],
        currentPage: 1,
        perPage: 5,
        filter: '',
        infoModal: {
            id: 'info-modal',
            title: '',
            content: ''
        }
    },
    created() {
        this.getAllDiscount();
    },
    methods: {
        info(item, index, button) {
            this.infoModal.title = `Row index: ${index}`;
            this.infoModal.content = JSON.stringify(item, null, 2);
            this.$root.$emit('bv::show::modal', this.infoModal.id, button);
        },
        resetInfoModal() {
            this.infoModal.title = '',
                this.infoModal.content = '';
        },
        getAllDiscount() {
            axios.get('/api/Discount/GetDiscount')
                .then(res => {
                    console.log(res);
                    if (res.data.isSuccess == true) {
                        this.items = res.data.result;
                        this.isBusy = false;
                    }
                    else {
                        console.log(res.data.exception)
                    }
                });
        },
    },
    computed: {
        totalRows() {
            return this.items.length;
        }
    }
});