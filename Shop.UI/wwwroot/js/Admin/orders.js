var app = new Vue({
    el: "#app",
    data: {
        loading: false,
        orders: [],
        selectedOrder: null,
        status: 0
    },
    mounted() {
        this.getOrders();
    },
    watch: {
        status: function ()
        {
            this.getOrders();
        }
    },
    methods: {
        getOrders: function () {
            this.loading = true;
            axios.get("/orders?status="+this.status)
                .then(res => {
                    console.log(res);
                    this.orders = res.data;
                })
                .catch(err => {
                    console.error(err)
                })
                .then(() => {
                    this.loading = false;
                });
        },
        selectOrder: function (id) {
            this.loading = true;
            axios.get("/orders/" + id)
                .then(res => {
                    console.log(res);
                    this.selectedOrder = res.data;
                })
                .catch(err => {
                    console.error(err)
                })
                .then(() => {
                    this.loading = false;
                });
        },
        updateOrder() {
            this.loading = true;
            axios.put('/orders/' + this.selectedOrder.id, null)
                .then(res => {
                    console.log(res);
                })
                .catch(err => {
                    console.error(err)
                })
                .then(() => {
                    this.loading = false;
                });
        },
        exitOrder(){
            this.selectedOrder = null
        } 
    }
})