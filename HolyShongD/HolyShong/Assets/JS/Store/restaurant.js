let app = new Vue({
    el: '#app',
    data: {
        isVerify: true,
        select: '',
        product: {
            StoreName: '',
            ProductId: 1,
            ProductName: '',
            ProductDescription: '',
            UnitPrice: 69.0,
            ProductImg: '',
            Quantity: 1,
            StoreProductOptions: [
                {
                    SelectOption: '',
                    SelectOptionPrice: '',
                    ProductOptionName: '',
                    ProductOptionDetails: [
                        {
                            StoreProductOptionDetailId: '',
                            StoreProductOptioinDetailName: '',
                            AddPrice: 0
                        }
                    ]
                },
            ]
        }
    },
    methods: {
        changeAmount(value) {
            if (this.product.Quantity + value == 0) { return; }
            this.product.Quantity += value;
        },
        addToCart() {
            let selectOptions = this.product.StoreProductOptions.map(x => x.SelectOption);
            if (selectOptions.includes(null)) {
                return toastr.error('還有必填選項喔');
            }
            $.ajax({
                type: 'POST',
                url: '/api/Cart/AddToCart',
                data: this.product,
                success: function (res) {
                    console.log(res)
                    console.log(res.Result)
                    $('#cardModal').modal('hide');
                    if (document.querySelector('#cart-check').checked == true) {
                        $('#cart-check').click();
                    }
                    $('#cart-check').click();
                }
            });
        },
    },
    computed: {
        sum() {
            let optionSource = this.product.StoreProductOptions.map(x => x.ProductOptionDetails);
            let options = optionSource.length == 0 ? [] : optionSource.reduce((a, b) => a.concat(b));
            let selectOptions = this.product.StoreProductOptions.map(x => x.SelectOption);
            let bonus = options.filter(x => selectOptions.includes(x.StoreProductOptionDetailId)).map(x => x.AddPrice);
            let bonusPrice = bonus.length == 0 ? 0 : bonus.reduce((a, b) => a + b);

            return (this.product.UnitPrice + bonusPrice) * this.product.Quantity;
        }
    }
});


//愛心 
let heartSolid = document.querySelector(".heartSolid");
let heartEmpty = document.querySelector(".heartEmpty");
let heart = document.querySelector(".heart")

heart.onclick = function () {
    //空心轉實心
    if ($('.heartEmpty').css('display') === "block") {        
        heartEmpty.style.display = "none";
        heartSolid.style.display = "block";
        //前端傳遞到後端
        $.ajax({
            type: 'POST',
            url: '/Store/CreateFavoriteStore',
            data: { UrlName: location.href,StoreId:0 },
            //如果>0 沒登入(傳網址)
            success: function (res) {
                if (res.length > 0) {
                    window.location.href=res;
                }
            }
        });
    }

    else {
        heartEmpty.style.display = "block";
        heartSolid.style.display = "none";

        $.ajax({
            type: 'POST',
            url: '/Store/DeleteFavoriteStore',
            success: function (res) {
                if (res.length > 0) {
                    window.location.href = res;
                }
            }
        });
    }
}

//重整頁面後,愛心不會消失
console.log('555');
$(document).ready(function () {
    readHeart();
});
function readHeart() {

    //愛心 
    let heartSolid = document.querySelector(".heartSolid");
    let heartEmpty = document.querySelector(".heartEmpty");
    console.log('321')
    $.ajax({
        type: 'GET',
        url: '/Store/ReadMemberHeart',
        //有讀到的話 愛心是實心
        success: function (res) {
            if (res == "True") {
                heartEmpty.style.display = "none";
                heartSolid.style.display = "block";
            } else {
                heartEmpty.style.display = "block";
                heartSolid.style.display = "none";
            }
        }
    });
}

//按卡片取product內容
let productCards = document.querySelectorAll('.cardProduct');
productCards.forEach((card, index) => {
    card.addEventListener('click', function (event) {
        let cardid = card.getAttributeNode('data-id').value;

        $.ajax({
            type: 'POST',
            url: '/api/Store/GetProductModal',
            contentType: 'application/json',
            data: JSON.stringify({ ProductId: cardid }),
            success: function (res) {
                app.product = res;
                $('#cardModal').modal('show')
            }
        });
    });
});

