let discountModel = new Vue({
    el: '#discountModal',
    data: { product: {} },
    methods: {
        hideModal() {
            let discountModal = document.getElementById('discountModal');
            console.log(discountModal);
            discountModal.classList.remove('show');
        },
        selectDiscount(discount) {
            console.log(discount)
            checkOut.selectDiscount = discount;
        }
    }
});


//新增並讀取優惠卷
function inputDiscount() {
    let addDiscount = document.querySelector('.ONdiscount-body input');
    $.ajax({
        type: 'POST',
        url: '/api/Discount/inputDiscountCode',
        data: { discountCode: addDiscount.value },
        success: function (res) {
            let discountError = document.querySelector('.discountError');
            discountError.innerText = '';
            if (res.Result != "完成新增") {
                discountError.innerText = res.Result;
            }
            else {
                discountError.innerText = '';
                $('.addDiscountClose').click();
                let discountModal = document.getElementById('discountModal');
                discountModal.classList.add('show');
                //成功新增後篩出所有領用優惠

                if (typeof checkOut == "undefined") {
                    $.ajax({
                        type: 'POST',
                        url: '/api/Discount/GetDiscount',
                        success: function (response) {
                            response.result.forEach(discount => {
                                discount.EndTime = discount.EndTime.split('T')[0];
                            });
                            discountModel.product = response.Result;
                        }
                    });
                }
                else {
                    $.ajax({
                        type: 'POST',
                        url: '/api/Discount/MatchDiscountStore',
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(checkOut.store.CartItems),
                        success: function (response) {
                            discountModel.product = response.Result;
                        }
                    });
                }
            }
        }
    });
}

//新增優惠卷按X
let onCloseBtn = document.querySelector('#ONdiscountModal .close');
onCloseBtn.addEventListener('click', function(){
    let discountModal = document.getElementById('discountModal');
    discountModal.classList.add('show');
});


// 首頁讀取全部優惠卷
let discount = document.querySelector('.discount');
discount.addEventListener('click', function () {
    $.ajax({
        type: 'POST',
        url: '/api/Discount/GetDiscount',
        success: function (res) {
            res.Result.forEach(discount => {
                discount.EndTime = discount.EndTime.split('T')[0];
            });
            discountModel.product = res.Result;
        }
    });
});
