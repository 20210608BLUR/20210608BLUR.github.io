let discountA = new Vue({
    el: '#discountA',
    data:{
        isBusy: true,
        tabIndex: 0,
        effectiveItems:[],
        invalidItems:[],
        dicountList: [],
        fields:[
            {key: 'discountCode', label: '優惠名稱', sortable: true},
            {key: 'displayName', label: '優惠內容', sortable: true},
            {key: 'amount', label: '優惠折扣', sortable: true},
            {key: 'startTime', label: '優惠開始', sortable: true},
            {key: 'endTime', label: '優惠結束', sortable: true},
            {key: 'Action', label: '功能', sortable: true}
           
        ],
        currentPage: 1,
        perPage: 4,
        filter: '',
        infoModal: {
            id:'info-modal',
            title:'',
            content:''
        },

        currentDiscount:{
            DiscountId: '',
            DiscountCode: '',
            DisplayName: '',
            Amount: '',
            Contents: '',
            StartTime: '',
            EndTime: '',
            Img: '',
            IsAllStore: '',
            Title: '',
            Type: '',
            UseLimit: ''
        },
        discountList:[],

        // 表單驗證
        isVerify: true,
        createDiscountData:{
            id:'',
            name:'', 
            startTime:'',
            EndTime:'',
        },
        createDiscountDataCheck:{
            idError: false,
            idErrorMsg:'',
            nameError: false, 
            nameErrorMsg:'', 
        },
    
        selected: true,
        optionsResaurant:[
            {text:'是', value: true},
            {text:'否', value: true}

        ],
        selectedDiscount: 1,
        optionsDiscount:[
            {text:'折數', value: 1},
            {text:'金額', value: 2}

        ],
        valueStart: '',
        valueEnd:'',
        selectedAmountA: 95,
        optionsA: [   
            { value: 95, text: '95折' },
            { value: 9, text: '9折' },
            { value: 85, text: '85折' }
        ],
        selectedAmountB: 30,
        optionsB: [                
            { value: 30, text: '30' },
            { value: 50, text: '50' },
            { value: 80, text: '80' }
        ],
        value6:'',
        setDisabled: {
            disabledDate(time) {
                return time.getTime() < Date.now();  
            }
        },
        setDisabledEnd :{
            disabledDate(time){
                return time.getTime() < Date.parse(discountA.currentDiscount.StartTime)
            }
        }



    },
    created(){
        
        this.getEffectiveDiscount()
        this.getInvalidDiscount()

    },
    
    methods:{      
        getEffectiveDiscount(){
            axios.get('/api/DiscountA/GetEffectiveDiscount')
            .then(res=>{
                console.log(res)
                if(res.data.isSuccess == true){
                    this.effectiveItems = res.data.result.reverse()
                    this.isBusy = false
                    res.data.result  

                
                }else{
                    console.log(res.data.exception)
                }
            })

        },
        getInvalidDiscount(){
            axios.get('/api/DiscountA/GetInvalidDiscount')
            .then(res=>{
                if(res.data.isSuccess == true){
                    this.invalidItems = res.data.result
                    this.isBusy = false
                }else{
                    console.log(res.data.exception)
                }
            })

        },
        createDiscount(){
            let amount
            if(this.selectedDiscount == 1){
                amount = this.selectedAmountA
            }else{
                amount = this.selectedAmountB
            }
            const data = {
                Discountcode: this.createDiscountData.id,
                DisplayName: this.createDiscountData.name,
                IsAllStore: this.selected, 
                Type: this.selectedDiscount,
                Amount: amount,
                StartTime: this.currentDiscount.StartTime,
                EndTime: this.currentDiscount.EndTime,
                Title:'',
                Contents:'',
                Img:'',
                UseLimit:''

            }
            axios.post('/api/DiscountA/CreateDiscount', data)
            .then(res=>{
                console.log(res)
                
                if(res.data.isSuccess == true && res.data.result == true){
                
                
                    toastr.success('新增成功')
                    this.getEffectiveDiscount()
                    this.getInvalidDiscount()
                
                    this.$bvModal.hide('modal-create')
                } else{
                    toastr.error('新增失敗')
                }
                
            })
        
        },
        updateDiscount(){
            axios.post('/api/DiscountA/UpdateDiscount')
            .then(res=>{
                console.log(res)
            })

        },
        deleteDiscount(item, index, button){
            console.log(item)   
            
            let request = {discountId: item.discountId }
            axios.post('/api/DiscountA/DeleteDiscount' ,request)
            .then(res=>{
                if(res.data.isSuccess == true && res.data.result == true){
                    toastr.success('刪除成功')
                    this.getAllDiscount() 
                    alert('刪除成功')
                } else{
                    toastr.error('刪除失敗')
                }
            })

        },
        
        // checkDiscountId(){
        //     if(this.createDiscountData.id == ''){
        //         this.createDiscountDataCheck.idError = true;
        //         this.createDiscountDataCheck.idErrorMsg ='必填欄位'
        //     }
        //     else{
        //         this.createDiscountDataCheck.idError = false;
        //         this.createDiscountDataCheck.idErrorMsg =''

        //     }
        // },
        checkDiscountVerify(){
            for (let prop in this.createDiscountDataCheck){
                if(this.createDiscountDataCheck[prop] == true){
                    this.isVerify = false
                    return 
                }
                this.isVerify = true
            }
        },
        watch:{
            'createDiscountData.id':{
                immediate: true,
                deep: true,
                handler(){
                    this.checkDiscountId()
                    this.checkDiscountVerify()
                }
            },
        },
        

        info(item, index, button){
            this.infoModal.title = '優惠明細',
            this.infoModal.content = JSON.stringify(item, null, 2);
            this.$root.$emit('bv::show::modal', this.infoModal.id, button);
            this.discountList = item

        },
        resetInfoModal(){
            this.infoModal.title ='',
            this.infoModal.content =''
        },

        dateDisabled(time) {
            return time.getTime() < Date.now();  
        },
        hideModal() {
            this.$refs['my-modal'].hide()
          },

    },
    

    computed:{
        totalRowsNow(){
            return this.effectiveItems.length
        },
        totalRowsPast(){
            return this.invalidItems.length
        },

        stateId() {
            return this.createDiscountData.id.length >= 4
        },
        stateIdFeedback() {
            if (this.createDiscountData.id.length > 0) {
                return '字數不能少於三位數'
            }
            return '此欄位必填'
        },
        stateName() {
            return this.createDiscountData.name.length >= 4
        },
        stateNameFeedback() {
            if (this.createDiscountData.name.length > 0) {
                return '字數不能少於五位數'
            }
            return '此欄位必填'
        },
        stateResturant() {
            return Boolean(this.selected)
        },
        stateDiscount(){
            return Boolean(this.selectedDiscount)

        },

        startTime(){
        
            if(this.currentDiscount.StartTime < this.currentDiscount.EndTime)
            return 
        },
        startTimeFeedBack(){
        
            return '結束時間不能小於開始時間'

        }

        
        
    
    },
    
}) 