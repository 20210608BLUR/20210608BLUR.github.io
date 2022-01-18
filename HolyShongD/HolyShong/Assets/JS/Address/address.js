let addressApp = new Vue({
    el: '#addressApp',
    data: {
        map: null,
        // GOOGLE自動搜尋完成方法
        autocomplete: null,
        // api要綁定的搜尋框
        site: '',
        //存place確定後回傳的資料
        place: null,
        allAddresses: [],
        currentAddress: '',
        defaultAddress: '', 
    },
    mounted() {
        this.test()
        this.siteAuto()
        this.getAllAddresses()
    },
    methods: {
         //地圖初始化
        test() {
            let location = {
                lat: 25.0416806,
                lng: 121.5352095
            }
            this.map = new google.maps.Map(document.getElementById('mapAddress'), {
                center: location,
                zoom: 15

            })
        },

        // 地址自動完成 + 地圖中心變更到輸入的地址上
        siteAuto() {
            // 限制在台灣
            let options = {
                componentRestrictions: { country: 'tw' }
            }
            this.autocomplete = new google.maps.places.Autocomplete(this.$refs.site, options)

            // 地址輸入框變動執行
            this.autocomplete.addListener('place_changed', () => {
                //地點資料存進place
                this.place = this.autocomplete.getPlace();

                //確認回來資料有經緯度
                if (this.place.geometry) {
                    //改變地圖中心點
                    let searchCenter = this.place.geometry.location
                    // panto平滑移動
                    this.map.panTo(searchCenter)
                    //放標記
                    let marker = new google.maps.Marker({
                        position: searchCenter,
                        map: this.map
                    })

                }
            })
        },
        getAllAddresses(callback = () => {}) {
            axios.get('/api/AddressApi/GetALLAddresses')
            .then(res => {
                console.log(res.data)                
                this.allAddresses = res.data
                if(res.data.AddressDetails){
                    this.currentAddress = res.data.AddressDetails.find(x => x.IsDefault).AllAddress
                    callback()
                }
                
            })
        },
        createAddresses(){
        
            const address = this.place.formatted_address + this.place.name 

            console.log(address)


            if( address === ''){
               
                return
            }
            
            console.log(address)
        
            const data = {
            MemberId:'',
            IsDefault:false,
            CreateTime:" ",
            AddressDetails:[{IsDefault: true,AllAddress: address, AddressId: '' }]
            }
            axios.post('/api/AddressApi/CreateAddress', data)
            .then(res =>{

            
                this.getAllAddresses(() => {
                    
                    this.currentAddress = address
                                   
                })
            
                this.place.formatted_address = ''
                this.place.name= ''
                this.site = ''
            })
        },
        deleteAddresses(address){
            if (address.IsDefault == true) {
            toastr.error('這是你的預設地址 不能刪歐')  
            return
            }
            // axios.post(url[, data[, config]])
            axios.post('/api/addressapi/DeleteAddress', null,{ params:{ request:address.AddressId }})
            .then(res =>{
                // const index = this.allAddresses.AddressDetails.findIndex(x => x.AddressId == address.AddressId)
                // this.allAddresses = this.allAddresses.AddressDetails.splice(index, 1)
            
                this.getAllAddresses()
            } )
        },
        updateAddresses(address){
            toastr.success('地址更新成功!')
            console.log(address)
            const data = {
                MemberId:'',
                IsDefault:true,
                AddressDetails:[{IsDefault: address.IsDefault,AllAddress: address.AllAddress, AddressId: address.AddressId }] 
            } 
        

            axios.post('/api/AddressApi/UpdateAddress', data)
            .then((res) =>{
                this.getAllAddresses(() => {
                this.currentAddress = address.AllAddress  
                })
            })
        }
    }
})