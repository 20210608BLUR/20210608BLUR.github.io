
        let menuCheck = document.querySelector('#menu-check');
        let bg = document.querySelector('.bg');
        let body = document.querySelector('body');

        //加入menu背景
        menuCheck.addEventListener('click', function(){
            //若漢堡按下，加入背景
            if(menuCheck.checked)
            {
                bg.classList.add('bg-style');
                body.setAttribute('style','overflow:hidden;');
                bg.addEventListener('click', function(){
                    menuCheck.click();
                    event.stopImmediatePropagation();
                });
            }
            //漢堡關起，移除背景
            else{
                bg.classList.remove('bg-style');
                body.removeAttribute('style','overflow:hidden;');
            }
        });

let btn = document.querySelector('#findDeliver');
btn.addEventListener('click', function () {
    $.ajax({
        type: 'POST',
        url: '/Deliver/OrderStateChange',
        data: { OrderCode: "EAT00001", OrderStatus: 4 },
        success: function (response) {
            toastr.success('成功完成訂單')
        }
    });
});