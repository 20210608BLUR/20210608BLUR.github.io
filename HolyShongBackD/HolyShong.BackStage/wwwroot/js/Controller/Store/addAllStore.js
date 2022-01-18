var cloud_url = 'https://api.cloudinary.com/v1_1/dvyxx4jau/image/upload';
var cloud_upload_preset = 'di5kimai';

let app = new Vue({
    el: '#storeVue',
    data: {
        tabIndex: 0,
        signupStore: {
            signupIsVarify: true,
            //基本資料填寫                    
            storeInfo: {
                name: '',
                category: '',
                address: '',
                phone: '',
                chargeName: '',
                chargePhone: '',
                keyword: '',
                img: ''
            },
            storeInfoCheck: {
                nameError: false,
                nameErrorMsg: '',
                categoryError: false,
                categoryErrorMsg: '',
                addressError: false,
                addressErrorMsg: '',
                phoneError: false,
                phoneErrorMsg: '',
                chargeNameError: false,
                chargeNameErrorMsg: '',
                chargePhoneError: false,
                chargePhoneErrorMsg: '',
                keywordError: false,
                keywordErrorMsg: '',
                imgError: false,
                imgErrorMsg: '',
            }
        },
        //storeCategory
        options: [
            { value: '1', text: '健康飲食' },
            { value: '2', text: '日本美食' },
            { value: '3', text: '台灣美食' },
            { value: '4', text: '韓國美食' },
            { value: '5', text: '中式美食' },
            { value: '6', text: '泰國美食' },
            { value: '7', text: '美式料理' },
            { value: '8', text: '義式料理' },
            { value: '9', text: '墨西哥美食' },
            { value: '10', text: '印度美食' },
            { value: '11', text: '純素料理' },
            { value: '12', text: '甜點' },
            { value: '13', text: '飲料' },
            { value: '14', text: '早午餐' },
            { value: '15', text: '速食' },
        ],
        // productCategory
        productCateOptions: [],
        signUpBusinessTime: {
            businessIsVerify: true,
            timeInfo: {
                monday: { startTime: '', endTime: '' },
                tuesday: { startTime: '', endTime: '' },
                wednesday: { startTime: '', endTime: '' },
                thursday: { startTime: '', endTime: '' },
                friday: { startTime: '', endTime: '' },
                saturday: { startTime: '', endTime: '' },
                sunday: { startTime: '', endTime: '' },
            },
            timeInfoCheck: {
                mondayError: false,
                mondayErrorMsg: '',
                tuesdayError: false,
                tuesdayErrorMsg: '',
                wednesdayError: false,
                wednesdayErrorMsg: '',
                thursdayError: false,
                thursdayErrorMsg: '',
                firdayError: false,
                firdayErrorMsg: '',
                saturdayError: false,
                saturdayErrorMsg: '',
                sundayError: false,
                sundayErrorMsg: '',
            },
            restInfo: {
                mondayRest: false,
                tuesdayRest: false,
                wednesdayRest: false,
                thursdayRest: false,
                fridayRest: false,
                saturdayRest: false,
                sundayRest: false,
            }
        },
        signUpCategory: {
            categoryIsVarify: true,
            categoryInfo: {
                first: '',
                second: '',
                third: '',
                forth: ''
            },
            categoryInfoCheck: {
                firstError: false,
                firstErrorMsg: '',
                secondError: false,
                secondErrorMsg: '',
                thirdError: false,
                thirdErrorMsg: '',
                forthError: false,
                forthErrorMsg: ''
            }
        },
        createProduct: {
            productInfo: {
                productName: '',
                productCategoryName: '',
                productDescription: '',
                unitPrice: 0,
                productImg: ''
            },
            productInfoCheck: {
                productNameError: false,
                productNameErrorMsg: '',
                productCategoryNameError: false,
                productCategoryNameErrorMsg: '',
                unitPriceError: false,
                unitPriceErrorMsg: ''
            }
        },
        createOption: {
            optionInfo: {
                productOptionName: '',
                productOptionDetailName: '',
                addPrice: ''
            },
            optionInfoCheck: {
                productOptionNameError: false,
                productOptionNameErrorMsg: '',
                productOptionDetailNameError: false,
                productOptionDetailNameErrorMsg: '',
                addPriceError: false,
                addPriceErrorMsg: ''
            }
        },
        item: [],
        fields: [
            { key: 'productName', label: '餐點名稱', sortable: false, sortDirection: 'desc' },
            { key: 'productCategoryName', label: '餐點類別', sortable: false, sortDirection: 'desc' },
            { key: 'productImg', label: '餐點圖片' },
            { key: 'unitPrice', label: '餐點單價', sortable: true, sortDirection: 'desc' },
            { key: 'actions', label: '商品管理' }
        ],
        productOptionsModel: {
            id: 'optionModal',
            title: '',
            item: [],
            fields: [
                { key: 'productImg', label: '餐點圖片', value: '' },
                { key: 'productName', label: '餐點名稱', sortable: false, sortDirection: 'desc', value: '' },
                { key: 'storeName', label: '餐廳名稱', sortable: false, sortDirection: 'desc', value: '' },
                { key: 'productCategoryName', label: '餐點類別', sortable: false, sortDirection: 'desc', value: '' },
                { key: 'unitPrice', label: '餐點單價', sortable: true, sortDirection: 'desc' },
                { key: 'productDescription', label: '餐點描述', value: '' },
            ]
        },
        productDetailsModel: {
            item: [],
            fields: [
                { key: 'productOptionName', label: '餐點規格', sortable: false, sortDirection: 'desc' },
                { key: 'productOptionDetailName', label: '餐點選項', sortable: false, sortDirection: 'desc' },
                { key: 'addPrice', label: '加購價', sortable: true, sortDirection: 'desc' },
                { key: 'actions', label: '商品編輯' }
            ]
        },
        editDetailModel: {
            id: 'detailModel',
            title: '',
            item: [],
            fields: [
                { key: 'productOptionName', label: '餐點規格', sortable: false, sortDirection: 'desc' },
                { key: 'productOptionDetailName', label: '餐點選項', sortable: false, sortDirection: 'desc' },
                { key: 'addPrice', label: '加購價', sortable: true, sortDirection: 'desc', formatter: (value, key, item) => { return value ? formatter.format(value) : 0 } },
            ]
        },
        currentitem: null,
        tabIndex: 0,
        totalRows: 1,
        currentPage: 1,
        perPage: 10,
        pageOptions: [5, 10, 15],
        sortBy: '',
        sortDesc: false,
        sortDirection: 'asc',
        filter: null,
        filterOn: [],

        //描述頁面是否忙碌中，EX:進行非同步作業
        isOnSaleBusy: { PageBusy: false, DetailsBusy: false },
        isNonSaleBusy: { PageBusy: false, DetailsBusy: false },

        //url列表
        urllist: {
            getProduct: '/api/Store/GetProductList',
            productDetails: '/api/Store/GetProductDetails',
            UpdateProductSalesStatus: '/api/Store/UpdateProductSalesStatus',
            createStore: '/api/Store/CreateStore'
        },

        //商品精簡清單圖片延遲載入的參數
        simplifyproductimgProps: {
            blank: true,
            blankColor: '#bbb',
            height: 60
        },

        //商品明細圖片延遲載入的參數
        productDetailsimgProps: {
            blank: true,
            blankColor: '#bbb',
            width: 240,
            height: 340
        },

        //麵包屑
        items: [
            {
                text: '首頁',
                href: '~/Home/Index'
            },
            {
                text: '餐廳管理'
            },
            {
                text: '餐廳基本資料',
                active: true
            }
        ],

    },
    watch: {
        'signupStore.storeInfo': {
            immediate: true,
            deep: true,
            handler() {
                this.checkStoreInfo();
            }
        },
        'signUpBusinessTime.timeInfo.monday.startTime': {
            immediate: true,
            handler() {
                this.checkTimeInfo();
            }
        },
        'signUpBusinessTime.timeInfo.monday.endTime': {
            handler() {
                this.checkTimeInfo();
            }
        },
        'signUpBusinessTime.timeInfo.tuesday.startTime': {
            handler() {
                this.checkTimeInfo();
            }
        },
        'signUpBusinessTime.timeInfo.tuesday.endTime': {
            handler() {
                this.checkTimeInfo();
            }
        },
        'signUpBusinessTime.timeInfo.wednesday.startTime': {
            handler() {
                this.checkTimeInfo();
            }
        },
        'signUpBusinessTime.timeInfo.wednesday.endTime': {
            handler() {
                this.checkTimeInfo();
            }
        },
        'signUpBusinessTime.timeInfo.thursday.startTime': {
            handler() {
                this.checkTimeInfo();
            }
        },
        'signUpBusinessTime.timeInfo.thursday.endTime': {
            handler() {
                this.checkTimeInfo();
            }
        },
        'signUpBusinessTime.timeInfo.friday.startTime': {
            handler() {
                this.checkTimeInfo();
            }
        },
        'signUpBusinessTime.timeInfo.friday.endTime': {
            handler() {
                this.checkTimeInfo();
            }
        },
        'signUpBusinessTime.timeInfo.saturday.startTime': {
            handler() {
                this.checkTimeInfo();
            }
        },
        'signUpBusinessTime.timeInfo.saturday.endTime': {
            handler() {
                this.checkTimeInfo();
            }
        },
        'signUpBusinessTime.timeInfo.sunday.startTime': {
            handler() {
                this.checkTimeInfo();
            }
        },
        'signUpBusinessTime.timeInfo.sunday.endTime': {
            handler() {
                this.checkTimeInfo();
            }
        },
        'signUpBusinessTime.restInfo.mondayRest': {
            handler() {
                this.checkTimeInfo();
            }
        },
        'signUpBusinessTime.restInfo.tuesdayRest': {
            handler() {
                this.checkTimeInfo();
            }
        },
        'signUpBusinessTime.restInfo.wednesdayRest': {
            handler() {
                this.checkTimeInfo();
            }
        },
        'signUpBusinessTime.restInfo.thursdayRest': {
            handler() {
                this.checkTimeInfo();
            }
        },
        'signUpBusinessTime.restInfo.fridayRest': {
            handler() {
                this.checkTimeInfo();
            }
        },
        'signUpBusinessTime.restInfo.saturdayRest': {
            handler() {
                this.checkTimeInfo();
            }
        },
        'signUpBusinessTime.restInfo.sundayRest': {
            handler() {
                this.checkTimeInfo();
            }
        },
        'signUpCategory.categoryInfo': {
            immediate: true,
            deep: true,
            handler() {
                this.checkCategoryInfo();
            }
        },
        'createProduct.productInfo': {
            immediate: true,
            deep: true,
            handler() {
                this.createProductInfo();
            }
        },
        'createOption.optionInfo': {
            immediate: true,
            deep: true,
            handler() {
                this.createOptionInfo();
            }
        }
    },
    computed: {
        nextable() {
            //餐廳基本資料
            if (this.tabIndex == 0) {
                return this.checkStoreInfoVerify();
            }
            else if (this.tabIndex == 1) {
                return this.checkTimeInfoVerify();
            }
            else if (this.tabIndex == 2) {
                return this.checkCategoryVerify();
            }
            else if (this.tabIndex == 3) {
                this.productCateOptions = [];
                this.categoryOptions();
                this.checkProductVerify();
                return true;
            }

        }
    },
    methods: {
        //餐廳基本資料檢核
        checkStoreInfo() {
            for (let prop in this.signupStore.storeInfo) {
                if (this.signupStore.storeInfo[prop] == '') {
                    this.signupStore.storeInfoCheck[`${prop}` + "Error"] = true;
                    this.signupStore.storeInfoCheck[`${prop}` + "ErrorMsg"] = '必填';

                }
                else {
                    this.signupStore.storeInfoCheck[`${prop}` + "Error"] = false;
                    this.signupStore.storeInfoCheck[`${prop}` + "ErrorMsg"] = '';
                }
            }
        },
        checkStoreInfoVerify() {
            for (let prop in this.signupStore.storeInfoCheck) {
                if (this.signupStore.storeInfoCheck[prop] == true) {
                    return false;
                }
            }
            return true;
        },
        //營業時間檢核 不得為空 不得相同 不得結束時間小於開始時間
        checkTimeInfo() {
            for (let prop in this.signUpBusinessTime.timeInfo) {
                if (this.signUpBusinessTime.restInfo[`${prop}` + "Rest"] == false) {
                    if (this.signUpBusinessTime.timeInfo[prop].startTime == '' || this.signUpBusinessTime.timeInfo[prop].endTime == '') {
                        this.signUpBusinessTime.timeInfoCheck[`${prop}` + "Error"] = true;
                        this.signUpBusinessTime.timeInfoCheck[`${prop}` + "ErrorMsg"] = '不得為空值';
                    }
                    else if (this.signUpBusinessTime.timeInfo[prop].startTime == this.signUpBusinessTime.timeInfo[prop].endTime) {
                        this.signUpBusinessTime.timeInfoCheck[`${prop}` + "Error"] = true;
                        this.signUpBusinessTime.timeInfoCheck[`${prop}` + "ErrorMsg"] = '開始與結束時間不得相同';
                    }
                    else if (this.signUpBusinessTime.timeInfo[prop].startTime > this.signUpBusinessTime.timeInfo[prop].endTime) {
                        this.signUpBusinessTime.timeInfoCheck[`${prop}` + "Error"] = true;
                        this.signUpBusinessTime.timeInfoCheck[`${prop}` + "ErrorMsg"] = '結束時間不得早於開始時間';
                    }
                    else {
                        this.signUpBusinessTime.timeInfoCheck[`${prop}` + "Error"] = false;
                        this.signUpBusinessTime.timeInfoCheck[`${prop}` + "ErrorMsg"] = '';
                    }
                }
                else {
                    this.signUpBusinessTime.timeInfo[prop].startTime = '';
                    this.signUpBusinessTime.timeInfo[prop].endTime = '';
                    this.signUpBusinessTime.timeInfoCheck[`${prop}` + "Error"] = false;
                    this.signUpBusinessTime.timeInfoCheck[`${prop}` + "ErrorMsg"] = '';
                }
            }
        },
        checkTimeInfoVerify() {
            for (let prop in this.signUpBusinessTime.timeInfoCheck) {
                if (this.signUpBusinessTime.timeInfoCheck[prop] == true) {
                    return false;
                }
            }
            return true;
        },
        //餐廳商品類別設定
        checkCategoryInfo() {
            for (let prop in this.signUpCategory.categoryInfo) {
                if (this.signUpCategory.categoryInfo[prop] == '') {
                    this.signUpCategory.categoryInfoCheck[`${prop}` + "Error"] = true;
                    this.signUpCategory.categoryInfoCheck[`${prop}` + "ErrorMsg"] = '必填';

                }
                else {
                    this.signUpCategory.categoryInfoCheck[`${prop}` + "Error"] = false;
                    this.signUpCategory.categoryInfoCheck[`${prop}` + "ErrorMsg"] = '';
                }
            }
        },
        checkCategoryVerify() {
            for (let prop in this.signUpCategory.categoryInfoCheck) {
                if (this.signUpCategory.categoryInfoCheck[prop] == true) {
                    return false;
                }
            }
            return true;
        },
        categoryOptions() {
            for (let cate in this.signUpCategory.categoryInfo) {
                this.productCateOptions.push({
                    value: `${this.signUpCategory.categoryInfo[cate]}`,
                    text: `${this.signUpCategory.categoryInfo[cate]}`
                });
            }
        },
        //新增產品檢核
        createProductInfo() {
            for (let prop in this.createProduct.productInfo) {
                let pwRegexp = /(^[1-9]([0-9]+)?(\.[0-9]{1,2})?$)|(^(0){1}$)|(^[0-9]\.[0-9]([0-9])?$)/;
                if (prop == 'unitPrice') {
                    console.log(this.createProduct.productInfo[prop])
                    console.log(pwRegexp.test(this.createProduct.productInfo[prop]))
                    if (!pwRegexp.test(this.createProduct.productInfo[prop])) {
                        this.createProduct.productInfoCheck[`${prop}` + "Error"] = true;
                        this.createProduct.productInfoCheck[`${prop}` + "ErrorMsg"] = '金額輸入錯誤';
                    }
                    else {
                        this.createProduct.productInfoCheck[`${prop}` + "Error"] = false;
                        this.createProduct.productInfoCheck[`${prop}` + "ErrorMsg"] = '';
                    }
                }
                else if (this.createProduct.productInfo[prop] == '') {
                    this.createProduct.productInfoCheck[`${prop}` + "Error"] = true;
                    this.createProduct.productInfoCheck[`${prop}` + "ErrorMsg"] = '必填';
                }
                else {
                    this.createProduct.productInfoCheck[`${prop}` + "Error"] = false;
                    this.createProduct.productInfoCheck[`${prop}` + "ErrorMsg"] = '';
                }
            }
        },
        checkProductVerify() {
            for (let prop in this.createProduct.productInfoCheck) {
                console.log("B")
                console.log(prop)
                if (prop != 'productDescriptionError' && prop != 'productImgError') {
                    if (this.createProduct.productInfoCheck[prop] == true) {
                        return false;
                    }
                }
            }
            return true;
        },
        //新增選項
        createOptionInfo() {
            for (let prop in this.createOption.optionInfo) {
                console.log("A")
                console.log(prop)
                let pwRegexp = /(^[1-9]([0-9]+)?(\.[0-9]{1,2})?$)|(^(0){1}$)|(^[0-9]\.[0-9]([0-9])?$)/;
                if (prop == 'unitPrice') {
                    if (!pwRegexp.test(this.createOption.optionInfo[prop])) {
                        this.createOption.optionInfoCheck[`${prop}` + "Error"] = true;
                        this.createOption.optionInfoCheck[`${prop}` + "ErrorMsg"] = '金額輸入錯誤';
                    }
                }
                else if (this.createOption.optionInfo[prop] == '') {
                    this.createOption.optionInfoCheck[`${prop}` + "Error"] = true;
                    this.createOption.optionInfoCheck[`${prop}` + "ErrorMsg"] = '必填';
                }
                else {
                    this.createOption.optionInfoCheck[`${prop}` + "Error"] = false;
                    this.createOption.optionInfoCheck[`${prop}` + "ErrorMsg"] = '';
                }
            }
        },
        checkOptionVerify() {
            for (let prop in this.createOption.optionInfoCheck) {
                if (this.createOption.optionInfoCheck[prop] == true) {
                    return false;
                }
            }
            return true;
        },

        //設定頁面預設狀態
        SetPageDefault() {
            this.items = [];
            this.totalRows = 1;
            this.currentPage = 1;
            this.perPage = 10;
            this.sortBy = '';
            this.sortDesc = false;
            this.filter = null;
        },
        //顯示商品明細
        async info(item, index, button) {
            try {
                this.isOnSaleBusy.DetailsBusy = true;
                this.productOptionsModel.title = `商品明細 - ID：${item.productName}`
                this.$root.$emit('bv::show::modal', this.productOptionsModel.id, button);
                let response = await this.getProductDetails(this.urllist.productDetails, item.productId);

                if (response.data.isSuccess == true) {
                    this.productOptionsModel.item = response.data.result;
                    if (response.data.result.optionList.length != 0) {
                        this.productDetailsModel.item = response.data.result.optionList[0].optionDetailList;
                    }
                    else {
                        this.productDetailsModel.item = [];
                    }

                    for (let obj of this.productOptionsModel.fields) {
                        if (item.isEnable == true && obj.key == 'actions') {
                            continue;
                        }
                        if (obj.key == 'productDescription') {
                            let tmp = this.productOptionsModel.item[`${obj.key}`];
                            obj.value = (tmp != null) ? tmp : '無';
                            continue;
                        }
                        let tmp = this.productOptionsModel.item[`${obj.key}`];
                        obj.value = (tmp != '') ? tmp : '無';
                    }
                }
                else {
                    throw new UserException('Unknown Error');
                }
            } catch (err) {
                console.error(err);
            } finally {
                this.isOnSaleBusy.DetailsBusy = false;
            }
        },
        //顯示編輯商品規格
        showEditOptipn(item, index, button) {
            this.editDetailModel.item = item;
            this.editDetailModel.title = `編輯規格 - ${item.productOptionName}`
            this.$root.$emit('bv::show::modal', this.editDetailModel.id, button);
            console.log(this.editDetailModel)
            console.log(this.$root.$emit('bv::show::modal', this.editDetailModel.id, button))

        },
        resetProductOptionsModel() {
            this.productOptionsModel.title = ''
            this.productOptionsModel.fields.map(x => x.value = '');
            this.productDetailsModel.title = ''
            this.productDetailsModel.fields.map(x => x.value = ''); 9
        },
        resetInfoModal() {
            this.editDetailModel.title = '';
            this.editDetailModel.fields.map(x => x.value = '');

        },
        onFiltered(filteredItems) {
            // Trigger pagination to update the number of buttons/pages due to filtering
            this.totalRows = filteredItems.length
            this.currentPage = 1
        },
        //新增產品
        addProduct() {
            let isVerify = this.checkProductVerify();
            if (isVerify == false) return false;
            let optionArray = [];
            for (let obj in this.productDetailsModel.item) {

                optionArray.push({
                    productOptionName: `${this.productDetailsModel.item[obj].productOptionName}`,
                    productOptionDetailName: `${this.productDetailsModel.item[obj].productOptionDetailName}`,
                    addPrice: `${this.productDetailsModel.item[obj].addPrice}`
                });
            }

            this.item.push(
                {
                    productName: `${this.createProduct.productInfo.productName}`,
                    productCategoryName: `${this.createProduct.productInfo.productCategoryName}`,
                    productImg: `${this.createProduct.productInfo.productImg}`,
                    productDescription: `${this.createProduct.productInfo.productDescription}`,
                    unitPrice: `${this.createProduct.productInfo.unitPrice}`,
                    optionList: optionArray
                });


            this.$bvModal.hide('modal-1')
            for (let prop in this.createProduct.productInfo) {
                this.createProduct.productInfo[prop] = '';
            }
            this.productDetailsModel.item = [];
        },
        cancleProduct() {
            this.$bvModal.hide('modal-1')
            for (let prop in this.createProduct.productInfo) {
                this.createProduct.productInfo[prop] = '';
            }
            this.productDetailsModel.item = [];
        },
        //新增產品規格
        addOptions() {
            let isVerify = this.checkOptionVerify();
            if (isVerify == false) return false;

            this.productDetailsModel.item.push({
                productOptionName: `${this.createOption.optionInfo.productOptionName}`,
                productOptionDetailName: `${this.createOption.optionInfo.productOptionDetailName}`,
                addPrice: `${this.createOption.optionInfo.addPrice}`
            });
            this.$bvModal.hide('modal-2')
            for (let item in this.createOption.optionInfo) {
                this.createOption.optionInfo[item] = '';
            }
        },
        cancle() {
            this.$bvModal.hide('modal-2')
            for (let item in this.createOption.optionInfo) {
                this.createOption.optionInfo[item] = '';
            }
        },
        deleteOption(item, index) {
            console.log(item)
        },
        //上傳照片
        submitPic(inputClass) {
            let sendPic = document.querySelector("." + `${inputClass}`);
            sendPic.click();
            sendPic.onchange = function () {
                var file = sendPic.files[0];
                var formData = new FormData();

                formData.append('file', file);
                formData.append('upload_preset', cloud_upload_preset);

                let cfg = {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'multipart/form-data'
                    },
                    data: formData,
                    url: cloud_url
                }
                axios(cfg)
                    .then(res => {
                        if (inputClass == 'sendPic') {
                            app.signupStore.storeInfo.img = res.data.secure_url;
                        }
                        else if (inputClass == 'sendProPic') {
                            app.createProduct.productInfo.productImg = res.data.secure_url;
                        }
                    })
            }
        },
        creatStore() {
            //整理類別array
            let cateArray = [];
            for (let prop in this.signUpCategory.categoryInfo) {
                cateArray.push(this.signUpCategory.categoryInfo[prop]);
            }

            //傳遞資料
            let cfg = {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                data: JSON.stringify({
                    storeInfo: this.signupStore.storeInfo,
                    timeInfo: this.signUpBusinessTime.timeInfo,
                    categoryInfo: cateArray,
                    productInfo: this.item
                }),
                url: this.urllist.createStore
            }
            axios(cfg)
                .then(res => {
                    console.log(res)
                    //跳轉畫面
                    //清除所有內容
                });

        }
    },
});