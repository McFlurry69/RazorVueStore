var app = new Vue({
    el: "#app",
    data: {
        loading: false,
        products: [],
        selectedProduct: null,
        newStock: {
            productId: 0,
            description: "size",
            quantity: 10
        }
    },
    mounted() {
        this.getStock();
    },
    methods: {
        getStock: function () {
            this.loading = true;
            axios.get("/stocks")
                .then(res => {
                    console.log(res);
                    this.products = res.data;
                })
                .catch(err => {
                    console.error(err)
                })
                .then(() => {
                    this.loading = false;
                });
        },
        selectProduct: function (product) {
            this.selectedProduct = product;
            this.newStock.productId = product.id;
        },
        deleteStock (id, index) {
            this.loading = true;
            axios.delete('/stocks/' + id)
                .then(res => {
                    console.log(res);
                    this.selectedProduct.stock.splice(index, 1)
                })
                .catch(err => {
                    console.error(err)
                })
                .then(() => {
                    this.loading = false;
                });
        },
        updateStock() {
            this.loading = true;
            axios.put('/stocks', {
                stocks: this.selectedProduct.stock.map(x => {
                    return {
                        id: x.id,
                        description: x.description,
                        quantity: x.quantity,
                        productId: this.selectedProduct.id
                    };
                })
            })
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
        addStock: function (){
            this.loading = true;
            axios.post("/stocks", this.newStock)
                .then(res => {
                    console.log(res);
                    this.selectedProduct.stock.push(res.data);
                })
                .catch(err => {
                    console.error(err)
                })
                .then(() => {
                    this.loading = false;
                });
        }
    }
})