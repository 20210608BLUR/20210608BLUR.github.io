let cart = new Vue({
    el: '#cartApp',
    data: {
        product:
            {}
    },
    computed: {
        totalSum() {
            let totalPrice = 0;
            for (var i = 0; i < this.product.length; i++) {
                let addAmount = 0;
                if (this.product[i].StoreProductOptions != null) {
                    for (var j = 0; j < this.product[i].StoreProductOptions.length; j++) {
                        for (var k = 0; k < this.product[i].StoreProductOptions[j].ProductOptionDetails.length; k++) {
                            if (this.product[i].StoreProductOptions[j].SelectOption == this.product[i].StoreProductOptions[j].ProductOptionDetails[k].StoreProductOptionDetailId) {
                                addAmount += this.product[i].StoreProductOptions[j].ProductOptionDetails[k].AddPrice;
                            }
                        }
                    }
                }
                totalPrice += this.product[i].Quantity * (this.product[i].UnitPrice + addAmount);
            }
            return totalPrice;
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
        changeItem(product, index) {
            //刪除
            if (product.Quantity == 0) {
                this.product.splice(index, 1);
                console.log(this.product)
            }
            console.log(this.product)
            $.ajax({
                type: 'POST',
                url: '/api/Cart/ChangeCartItem',
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(this.product),
                success: function (response) {
                    console.log(response)
                    console.log(response.Result)
                    cart.product = response.Result;
                }
            });
        },
        closeCart() {
            $('#cart-check').click();
        }
    }
});

