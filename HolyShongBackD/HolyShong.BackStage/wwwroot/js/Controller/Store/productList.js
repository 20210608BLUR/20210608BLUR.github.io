// Create our number formatter.
var formatter = new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency: 'USD',
    minimumFractionDigits: 0,
    maximumFractionDigits: 0,
});

let app = new Vue({
    el: "#productlist",
    data() {
        return {
            items: [],
            fields: [
                { key: 'productId', label: '餐點編號', sortable: true, sortDirection: 'desc' },
                { key: 'productName', label: '餐點名稱', sortable: false, sortDirection: 'desc' },
                { key: 'storeId', label: '餐廳編號', sortable: true, sortDirection: 'desc' },
                { key: 'storeName', label: '餐廳名稱', sortable: false, sortDirection: 'desc' },
                { key: 'productCategoryName', label: '餐點類別', sortable: false, sortDirection: 'desc' },
                { key: 'productImg', label: '餐點圖片' },
                { key: 'unitPrice', label: '餐點單價', sortable: true, sortDirection: 'desc', formatter: (value, key, item) => { return value ? formatter.format(value) : 0 } },
                { key: 'actions', label: '商品管理' }
            ],
            enableEdit: false,
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
            productCateOptions: [],
            productOptions: [
                { value: '尺寸', text: '尺寸' },
                { value: '甜度', text: '甜度' },
                { value: '冰塊', text: '冰塊' },
                { value: '加購', text: '加購' },
                { value: '套餐', text: '套餐' },
                { value: '選擇', text: '選擇' },
            ],
            productOptionsModel: {
                id: 'optionModal',
                title: '',
                item: {
                    storeName: '',
                    productName: '',
                    unitPrice: '',
                    productCategoryName: '',
                },
                fields: [
                    { key: 'productImg', label: '餐點圖片', value: '' },
                    { key: 'productName', label: '餐點名稱', sortable: false, sortDirection: 'desc', value: '' },
                    { key: 'storeName', label: '餐廳名稱', sortable: false, sortDirection: 'desc', value: '' },
                    { key: 'productCategoryName', label: '餐點類別', sortable: false, sortDirection: 'desc', value: '' },
                    { key: 'unitPrice', label: '餐點單價', sortable: true, sortDirection: 'desc', formatter: (value, key, item) => { return value ? formatter.format(value) : 0 } },
                    { key: 'productDescription', label: '餐點描述', value: '' },
                ]
            },
            productDetailsModel: {
                item: [],
                fields: [
                    { key: 'productOptionName', label: '餐點規格', sortable: false, sortDirection: 'desc' },
                    { key: 'productOptionDetailName', label: '餐點選項', sortable: false, sortDirection: 'desc' },
                    { key: 'addPrice', label: '加購價', sortable: true, sortDirection: 'desc', formatter: (value, key, item) => { return value ? formatter.format(value) : 0 } },
                    { key: 'actions', label: '商品編輯' }
                ]
            },
            editDetailModel: {
                id: 'detailModel',
                title: '',
                fields: [
                    { key: 'productOptionName', label: '餐點規格', sortable: false, sortDirection: 'desc' },
                    { key: 'productOptionDetailName', label: '餐點選項', sortable: false, sortDirection: 'desc' },
                    { key: 'addPrice', label: '加購價', sortable: true, sortDirection: 'desc', formatter: (value, key, item) => { return value ? formatter.format(value) : 0 } },
                ]
            },
            restaurant: {
                restaurantInfo: {
                    restaurantName: ''
                },
                restaurantInfoCheck: {
                    restaurantNameError: false,
                    restaurantNameErrorMsg: '',
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

            //描述頁面是否忙碌中，EX:進行非同步作業
            isOnSaleBusy: { PageBusy: false, DetailsBusy: false },
            isNonSaleBusy: { PageBusy: false, DetailsBusy: false },

            //url列表
            urllist: {
                getProduct: '/api/Store/GetProductList',
                productDetails: '/api/Store/GetProductDetails',
                productOptions: '/api/Store/GetProductOptions',
                UpdateProductSalesStatus: '/api/Store/UpdateProductSalesStatus',
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

            //商品上下架MessageBox參數
            SalesConfirmBoxProps: {
                onSale: { message: '請再次確認是否要上架商品', data: { SaleStatus: true } },
                nonSale: { message: '請再次確認是否要下架商品', data: { SaleStatus: false } }
            },

            //麵包屑
            //breadCrumbItems: [
            //    {
            //        text: '首頁',
            //        href: '/Home/Index'
            //    },
            //    {
            //        text: '商品列表',
            //        active: true
            //    }
            //]
        }
    },
    computed: {
        
    },
    watch: {
        'currentPage': {
            handler() {
                this.SetDefaultWithoutPage();
                if (this.tabIndex == 0) {
                    this.getSimplifyProducts(this.urllist.getProduct, this.isOnSaleBusy, true);
                }
                else {
                    this.getSimplifyProducts(this.urllist.getProduct, this.isOnSaleBusy, false);
                }
            }
        },
        'perPage': {
            handler() {
                this.SetDefaultWithoutPage();
                this.currentPage = 1;
                if (this.tabIndex == 0) {
                    this.getSimplifyProducts(this.urllist.getProduct, this.isOnSaleBusy, true);
                }
                else {
                    this.getSimplifyProducts(this.urllist.getProduct, this.isOnSaleBusy, false);
                }
            }
        },
        'restaurant.restaurantInfo': {
            immediate: true,
            deep: true,
            handler() {
                this.checkRestaurantInfo();
            }
        },
        'createOption.optionInfo': {
            immediate: true,
            deep: true,
            handler() {
                this.createOptionInfo();
            }
        },
        'productOptionsModel.item': {
            //immediate: true,
            deep: true,
            handler() {
                this.createProductInfo();
            }
        },
        tabIndex: function () {
            switch (this.tabIndex) {
                case 0:
                    this.OnSalePage();
                    break;
                case 1:
                    this.NonSalePage();
                    this.enableEdit == true;
                    break;
                default:
                    break;
            }
        }
    },
    created() {
        //初始化頁面
        this.tabIndex = 0;
        this.getSimplifyProducts(this.urllist.getProduct, this.isOnSaleBusy, true);
    },
    methods: {

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
        //設定預設狀態(無頁面資料還原)
        SetDefaultWithoutPage() {
            this.items = [];
            this.sortBy = '';
            this.sortDesc = false;
            this.filter = null;
        },
        //切換上架商品頁
        OnSalePage() {
            this.SetPageDefault();
            this.getSimplifyProducts(this.urllist.getProduct, this.isOnSaleBusy, true);
        },
        //切換下架商品頁
        NonSalePage() {
            this.SetPageDefault();
            this.getSimplifyProducts(this.urllist.getProduct, this.isNonSaleBusy, false);
        },
        //取得商品(onsale判斷為上下架)
        getSimplifyProducts(uri, busyobj, onsale) {
            busyobj.PageBusy = true;
            let cfg = {
                method: 'get',
                url: `${uri}?isOnSale=${onsale}&perPage=${this.perPage}&currentPage=${this.currentPage}`,
            };
            axios(cfg)
                .then(res => {
                    if (res.data.isSuccess == true) {
                        this.items = res.data.result.items;
                        this.currentPage = res.data.result.currentPage;
                        this.totalRows = res.data.result.totalRows;
                    }
                })
                .catch(err => {
                    console.dir(err);
                })
                .finally(() => {
                    busyobj.PageBusy = false;
                });
        },
        //取得商品明細
        getProductDetails(uri, id) {
            let cfg = {
                method: 'GET',
                url: `${uri}?id=${id}`
            };
            return axios(cfg);
        },
        getProductOptions(uri, id) {
            let cfg = {
                method: 'GET',
                url: `${uri}?id=${id}`
            };
            return axios(cfg);
        },

        //更新商品銷售狀態
        UpdateProductSalesStatus(uri, data) {
            let cfg = {
                method: 'post',
                headers: {
                    'Content-type': 'application/json',
                },
                data: {
                    ID: data.ID,
                    SaleStatus: data.SaleStatus
                },
                url: uri
            };

            axios(cfg)
                .then(res => {
                    if (res.data.isSuccess == true) {
                        let index = this.items.findIndex(x => x.productId === data.ID)
                        if (index >= 0) {
                            toastr.success('商品操作成功');
                            this.items.splice(index, 1);
                            if (res.data.result == true) {
                                tabIndex = 1;
                            }
                            else {
                                tabIndex = 0;
                            }
                        }
                    }
                })
                .catch(err => {
                    toastr.error('商品操作失敗');
                })
                .finally(() => {
                });
        },
        //顯示商品明細
        async info(item, index, button) {
            try {
                this.isOnSaleBusy.DetailsBusy = true;
                if (item.isEnable == false) {
                    this.enableEdit = true;
                }
                else {
                    this.enableEdit = false;
                }

                this.productOptionsModel.title = `商品明細 ${item.productName}`
                this.$root.$emit('bv::show::modal', this.productOptionsModel.id, button);
                //找完整商品明細
                let response = await this.getProductDetails(this.urllist.productDetails, item.productId);
                console.log(response)
                if (response.data.isSuccess == true) {
                    this.productOptionsModel.item = response.data.result;

                    if (response.data.result.optionList == null || response.data.result.optionList.length == 0) {
                        this.productDetailsModel.item = [];
                    }
                    else {
                        this.productDetailsModel.item = [];
                        this.productDetailsModel.item = response.data.result.optionList.map(x => x.optionDetailList).reduce((a, b) => a.concat(b));
                    }

                    //撈資料庫的productCategory
                    let responseOptions = await this.getProductOptions(this.urllist.productOptions, item.productId);
                    if (responseOptions.data.isSuccess == true) {
                        this.productCateOptions = [];
                        this.productCateOptions = responseOptions.data.result;
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
        newInfo(button) {
            this.productDetailsModel.item = [];
            this.productCateOptions = [];
            this.$root.$emit('bv::show::modal', 'modal-multi-1', '#btnShow');

        },
        showCreateProduct() {
            this.enableEdit = true;
            this.productOptionsModel.title = `商品明細`
            this.$root.$emit('bv::show::modal', this.productOptionsModel.id, button);
        },
        //顯示編輯商品規格
        showEditOptipn(item, index, button) {
            this.editDetailModel.item = item;
            this.editDetailModel.title = `編輯規格 - ${item.productOptionName}`
            this.$root.$emit('bv::show::modal', this.editDetailModel.id, button);

        },
        resetProductOptionsModel() {
            this.productOptionsModel.title = ''
            this.productOptionsModel.fields.map(x => x.value = '');
            this.productDetailsModel.title = ''
            this.productDetailsModel.fields.map(x => x.value = '');
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
        //顯示上下架確認視窗
        ShowUpdateSaleConfirm(productId, cfg) {
            this.$bvModal.msgBoxConfirm(cfg.message, {
                title: '操作確認',
                size: 'md',
                buttonSize: 'md',
                okVariant: 'warning ',
                okTitle: '確認',
                cancelTitle: '取消',
                footerClass: 'p-2',
                hideHeaderClose: true,
                centered: true,
                noCloseOnEsc: true,
                noCloseOnBackdrop: true
            })
                .then(value => {
                    if (value) {
                        let data = {
                            ID: productId,
                            SaleStatus: cfg.data.SaleStatus,
                        }
                        this.UpdateProductSalesStatus(this.urllist.UpdateProductSalesStatus, data)
                    }
                })
                .catch(err => {
                    // An error occurred
                })
        },
        CreateProduct() {
            let isVerify = this.checkProductVerify();
            if (isVerify == false) {
                toastr.error('商品操作失敗，請確認所有必填欄位皆填寫');
                return false;
            }


            for (var index in this.productDetailsModel.item) {
                this.productOptionsModel.item.optionList.push({
                    productOptionName: `${this.productDetailsModel.item[index].productOptionName}`,
                    productOptionDetailName: `${this.productDetailsModel.item[index].productOptionDetailName}`,
                    addPrice: `${this.productDetailsModel.item[index].addPrice}`
                });
            }


            //CREATE AXIOS
            let cfg = {
                method: 'POST',
                headers: {
                    'Content-type': 'application/json',
                },
                data: JSON.stringify(this.productOptionsModel.item),
                url: '/api/Store/CreateProduct'
            };
            axios(cfg)
                .then(res => {
                    if (res.data.isSuccess == true) {
                        this.SetDefaultWithoutPage();
                        this.getSimplifyProducts(this.urllist.getProduct, this.isOnSaleBusy, true);
                        toastr.success('商品操作成功');
                    }
                })
                .catch(err => {
                    toastr.error('商品操作失敗');
                })
                .finally(() => {
                });
        },
        ProductModify(item) {
            console.log(item)
            let isVerify = this.checkProductVerify();
            if (isVerify == false) {
                toastr.error('商品操作失敗');
                return false;
            }

            let cfg = {
                method: 'POST',
                headers: {
                    'Content-type': 'application/json',
                },
                data: JSON.stringify(item),
                url: '/api/Store/ProductModify'
            };
            axios(cfg)
                .then(res => {
                    if (res.data.isSuccess == true) {
                        this.SetDefaultWithoutPage();

                        this.getSimplifyProducts(this.urllist.getProduct, this.isOnSaleBusy, false);
                        toastr.success('商品操作成功');
                        this.$bvModal.hide(`${this.productOptionsModel.id}`)
                    }
                })
                .catch(err => {
                    toastr.error('商品操作失敗');
                })
                .finally(() => {
                });
        },
        CancleModify(item) {
            this.$bvModal.hide(`${this.productOptionsModel.id}`)
            //for (let prop in this.createProduct.productInfo) {
            //    this.createProduct.productInfo[prop] = '';
            //}
            this.productOptionsModel.item = {};
        },
        //刪除選項
        DeleteOptions(item, index) {

            this.$bvModal.msgBoxConfirm('刪除後將無法復原', {
                title: '確認刪除',
                size: 'sm',
                buttonSize: 'sm',
                okVariant: 'danger',
                okTitle: '確定',
                cancelTitle: '取消',
                footerClass: 'p-2',
                hideHeaderClose: false,
                centered: true
            })
                .then(value => {
                    if (value == true) {
                        let cfg = {
                            method: 'post',
                            headers: {
                                'Content-type': 'application/json',
                            },
                            data: JSON.stringify(item),
                            url: '/api/Store/DeleteOption'
                        };

                        axios(cfg)
                            .then(res => {
                                if (res.data.isSuccess == true) {
                                    let index = this.productDetailsModel.item.findIndex(x => x.productOptionDetailId === item.productOptionDetailId)
                                    if (index >= 0) {
                                        toastr.success('商品操作成功');
                                        //刪除商品
                                        //刷新頁面
                                        this.productDetailsModel.item.splice(index, 1);
                                        this.productOptionsModel.item.optionList.splice(index, 1);
                                        this.getSimplifyProducts(this.urllist.getProduct, this.isOnSaleBusy, false);
                                    }
                                }
                            })
                            .catch(err => {
                                toastr.error('商品操作失敗');
                            })
                            .finally(() => {
                            });
                    }
                })
                .catch(err => {
                    // An error occurred
                })



        },
        //上傳照片
        submitPic(inputClass) {
            var cloud_url = 'https://api.cloudinary.com/v1_1/dvyxx4jau/image/upload';
            var cloud_upload_preset = 'di5kimai';
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
                        app.productOptionsModel.item.productImg = res.data.secure_url;
                    })
            }
        },
        //餐廳檢核
        checkRestaurantInfo() {
            if (this.restaurant.restaurantInfo.restaurantName == '') {
                this.restaurant.restaurantInfoCheck.restaurantNameError = true;
                this.restaurant.restaurantInfoCheck.restaurantNameErrorMsg = '必填';

            }
            else {
                this.restaurant.restaurantInfoCheck.restaurantNameError = false;
                this.restaurant.restaurantInfoCheck.restaurantNameErrorMsg = '';
            }
        },
        checkRestaurantVerify() {
            if (this.restaurant.restaurantInfoCheck.restaurantNameError == true) {
                return false;
            }
            return true;
        },//新增選項
        createOptionInfo() {
            for (let prop in this.createOption.optionInfo) {
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
        //新增產品檢核
        createProductInfo() {
            for (let prop in this.productOptionsModel.item) {
                if (prop == "storeName") continue;
                if (this.productOptionsModel.item[prop] == '') {
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
                if (prop != 'productDescriptionError' && prop != 'productImgError' && prop != 'isEnableError' && prop !='optionListError') {
                    if (this.createProduct.productInfoCheck[prop] == true) {
                        return false;
                    }
                }
            }
            return true;
        },
        //更新產品檢核
        modifyProductInfo() {
            for (let prop in this.createProduct.productInfo) {
                if (prop == "isEnable" || prop == "optionList"){
                    continue;
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
        checkProductModifyVerify() {
            for (let prop in this.createProduct.productInfoCheck) {
                if (prop != 'productDescriptionError' && prop != 'productImgError' && prop != "") {
                    if (this.createProduct.productInfoCheck[prop] == true) {
                        return false;
                    }
                }
            }
            return true;
        },
        SearchStore() {
            //先檢查有沒有驗證通過
            let isVerify = this.checkRestaurantVerify();
            if (isVerify == false) return false;

            let cfg = {
                method: 'POST',
                headers: {
                    'Content-type': 'application/json',
                },
                data: JSON.stringify({ storeName: this.restaurant.restaurantInfo.restaurantName }),
                url: '/api/Store/SearchStore'
            };

            axios(cfg)
                .then(res => {
                    if (res.data.isSuccess == true) {
                        if (res.data.result.responseMsg == "找不到餐廳") {
                            toastr.error('找不到餐廳');
                        }
                        else if (res.data.result.responseMsg == "查詢到多間餐廳，請輸入完整店名") {
                            toastr.error('查詢到多間餐廳，請輸入完整店名');
                        }
                        else {
                            this.restaurant.restaurantInfo.restaurantName = '';
                            toastr.success('成功找到店家');
                            this.productCateOptions = res.data.result.productCateArray;
                            this.$bvModal.hide('modal-multi-1');
                            this.enableEdit = true;
                            this.$root.$emit('bv::show::modal', `${this.productOptionsModel.id}`, '#btnShow');
                            this.productOptionsModel.item.optionList = [];
                            this.productOptionsModel.item.storeName = '';
                            this.productOptionsModel.item.productName = '';
                            this.productOptionsModel.item.unitPrice = '';
                            this.productOptionsModel.item.productImg = '';
                            this.productOptionsModel.item.productDescription = '';
                            this.productOptionsModel.item.productCategoryName = '';
                            this.productOptionsModel.item.storeName = res.data.result.storeName;
                            this.createProductInfo();
                        }
                    }
                })
                .catch(err => {
                    toastr.error('商品操作失敗');
                })
                .finally(() => {
                });

        },
        Cancle() {
            this.$bvModal.msgBoxConfirm('刪除後將無法復原', {
                title: '確認刪除',
                size: 'sm',
                buttonSize: 'sm',
                okVariant: 'danger',
                okTitle: '確定',
                cancelTitle: '取消',
                footerClass: 'p-2',
                hideHeaderClose: false,
                centered: true
            })
                .then(value => {
                    this.$bvModal.hide('modal-2');
                    for (let item in this.createOption.optionInfo) {
                        this.createOption.optionInfo[item] = '';
                    }
                })
        },
        //加入
        AddOptions() {
            let isVerify = this.checkOptionVerify();
            if (isVerify == false) return false;

            this.productDetailsModel.item.push({
                productOptionName: `${this.createOption.optionInfo.productOptionName}`,
                productOptionDetailName: `${this.createOption.optionInfo.productOptionDetailName}`,
                addPrice: `${this.createOption.optionInfo.addPrice}`
            });

            if (this.tabIndex == 1) {
                this.productOptionsModel.item.optionList[0].optionDetailList.push({
                    productOptionName: `${this.createOption.optionInfo.productOptionName}`,
                    productOptionDetailName: `${this.createOption.optionInfo.productOptionDetailName}`,
                    addPrice: `${this.createOption.optionInfo.addPrice}`
                });
            }

            this.$bvModal.hide('modal-2')
            for (let item in this.createOption.optionInfo) {
                this.createOption.optionInfo[item] = '';
            }
        }
    }
});
