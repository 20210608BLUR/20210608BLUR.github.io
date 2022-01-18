var cloud_url = 'https://api.cloudinary.com/v1_1/dvyxx4jau/image/upload';
var cloud_upload_preset = 'di5kimai';

let app = new Vue({
    el: '#storeVue',
    data: {
        tabIndex: 0,
        signupStore: {
            signupIsVarify:true,
            //基本資料填寫                    
            storeInfo: {
                name: '',
                category: '',
                address: '',
                phone: '',
                chargeName: '',
                chargePhone: '',
                keyword: '',
                img:''
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
                forth:''
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
        item: [],
        currentitem: null,
        tabIndex: 0,

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
    },
    computed: {
        nextable() {
            //餐廳基本資料
            if (this.tabIndex == 0) {
                return this.checkStoreInfoVerify();
            }
            else if (this.tabIndex == 1)
            {
                return this.checkTimeInfoVerify();
            }
            else if (this.tabIndex == 2)
            {
                this.checkCategoryVerify();
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
                    this.signupStore.storeInfoCheck[`${prop}`+ "Error"] = false;
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
        //上傳照片
        submitPic(inputClass) {
            let sendPic = document.querySelector("."+`${inputClass}`);
            sendPic.click();
            sendPic.onchange = function () {
                var file = sendPic.files[0];
                var formData = new FormData();

                formData.append('file', file);
                formData.append('upload_preset', cloud_upload_preset);

                let cfg = {
                    method: 'POST',
                    headers: {
                        'Content-Type':'multipart/form-data'
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