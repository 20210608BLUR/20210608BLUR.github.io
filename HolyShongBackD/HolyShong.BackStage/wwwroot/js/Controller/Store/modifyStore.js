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
            { value: 'c', text: 'Please select an option' },
            { value: 'a', text: 'This is First option' },
            { value: 'b', text: 'Selected Option' },
        ],
        signUpBusinessTime: {
            businessIsVerify: true,
            timeInfo: {

            }
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
        //開啟以編輯
        enableEdit: false,
        enableEditTime: false,
        //營業時間
        restMonday: false,
        startMonday: '',
        endMonday: '',
        status: ''

    },
    watch: {
        'signupStore.storeInfo': {
            immediate: true,
            deep: true,
            handler() {
                this.checkStoreInfo();
            }
        }
    },
    computed: {
        nextable() {
            //餐廳基本資料
            if (this.tabIndex == 0) {
                return this.checkStoreInfoVerify();
            }
            else {
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
        //餐廳基本資料檢核
        checkStoreInfoVerify() {
            for (let prop in this.signupStore.storeInfoCheck) {
                if (this.signupStore.storeInfoCheck[prop] == true) {
                    return false;
                }
            }
            return true;
        },
        submitPic() {
            let sendPic = document.querySelector('.sendPic');
            sendPic.click();
        }
    },
});