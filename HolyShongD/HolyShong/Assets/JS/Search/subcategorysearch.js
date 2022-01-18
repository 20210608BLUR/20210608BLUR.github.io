let searchapp = new Vue({
    el: '#subcategory',
    data: {
        Keyword: '',
        SearchCount: 0,
        Type: 1,
        Price: '',
        DeliveryFee:0
    },
    methods: {
        selectPriceRange(val) {
            this.Price = val
        },
        search() {
            let request = {
                Keyword: this.Keyword,
                SearchCount: this.SearchCount,
                Type: this.Type,
                Price: this.Price,
                DeliveryFee: this.DeliveryFee
            }

            window.location.href = '/home/search?' + new URLSearchParams(request)
            //fetch('/home/search?' + new URLSearchParams(request), {
            //    method: 'GET',
            //    //headers: {
            //    //    'content-type': 'application/json'
            //    //},
            //    //body: JSON.stringify(request)
            //}).then(res => {
            //    console.log(res);
            //    window.location.href = res.url
            //});
        }
    }
})

document.querySelector('#keyword').oninput = () => { searchapp.Keyword = document.querySelector('#keyword').value }

function uberSearch(e) {
    if (e.code == 'Enter') {
        searchapp.search();
    }
}