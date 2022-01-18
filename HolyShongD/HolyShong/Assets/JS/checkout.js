let checkOut = new Vue({

    el: '#checkApp',
    data: {
        store: checkInfo,
        options: [
            { value: '門口碰面', text: '門口碰面' },
            { value: '在外碰面', text: '在外碰面' },
            { value: '放在門口', text: '放在門口' }
        ],
        selectDiscount: {
            DiscountName: '尚無選擇優惠卷'
        }
    },
    computed: {
        totalSum() {
            let totalPrice = 0;
            for (var i = 0; i < this.store.CartItems.length; i++) {
                let addAmount = 0;
                if (this.store.CartItems[i].StoreProductOptions != null) {
                    for (var j = 0; j < this.store.CartItems[i].StoreProductOptions.length; j++) {
                        for (var k = 0; k < this.store.CartItems[i].StoreProductOptions[j].ProductOptionDetails.length; k++) {
                            if (this.store.CartItems[i].StoreProductOptions[j].SelectOption == this.store.CartItems[i].StoreProductOptions[j].ProductOptionDetails[k].StoreProductOptionDetailId) {
                                addAmount += this.store.CartItems[i].StoreProductOptions[j].ProductOptionDetails[k].AddPrice;
                            }
                        }
                    }
                }
                totalPrice += this.store.CartItems[i].Quantity * (this.store.CartItems[i].UnitPrice + addAmount)
            }
            return totalPrice;
        },
        discountPrice() {
            let discountType = this.selectDiscount.DiscountType;
            if (discountType == null) {
                return 0;
            }
            else if (discountType == 1) {
                return Math.round(this.totalSum * (1 - this.selectDiscount.DiscountAmount));
            }
            else {
                return this.selectDiscount.DiscountAmount;
            }
        },
        finalPrice() {
            return this.totalSum - this.discountPrice + this.store.DeliverFee;
        }
    },
    methods: {
        sum(p) {
            console.log(p)
            let addAmount = 0;
            if (p.StoreProductOptions == null) { return p.UnitPrice * p.Quantity; }
            for (var i = 0; i < p.StoreProductOptions.length; i++) {
                for (var j = 0; j < p.StoreProductOptions[i].ProductOptionDetails.length; j++) {
                    if (p.StoreProductOptions[i].ProductOptionDetails[j].StoreProductOptionDetailId == p.StoreProductOptions[i].SelectOption) {
                        addAmount += p.StoreProductOptions[i].ProductOptionDetails[j].AddPrice;
                    }
                }
            }
            return (p.UnitPrice + addAmount) * p.Quantity;
        },
        sendForm() {
            let btn = document.querySelector('#sendFormToOrder');
            btn.click();
        },
        openCheckOutDiscount() {
            console.log(checkOut.store.CartItems)
            $.ajax({
                type: 'POST',
                url: '/api/Discount/MatchDiscountStore',
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(checkOut.store.CartItems),
                success: function (res) {
                    if (checkOut.totalSum >= 100) {
                        console.log(res);
                        discountModel.product = res.Result;
                    }
                }
            });
        }
    }
});

