var app = new Vue({
    el: "#app",
    data: {
        editing: false,
        loading: false,
        objectIndex: 0,
        products: [],
        productModel: {
            id: 0,
            name: "",
            description: "",
            value: 0
        }
    },
    mounted() {
        this.getProducts();
    },
    methods: {
        getProduct: function (id) {
            this.loading = true;
            axios.get("/products/" + id)
                .then(res => {
                    console.log(res);
                    let product = res.data;
                    this.productModel = {
                        id: product.id,
                        name: product.name,
                        description: product.description,
                        value: product.value,
                    }
                })
                .catch(err => {
                    console.error(err)
                })
                .then(() => {
                    this.loading = false;
                });
        },
        getProducts: function () {
            this.loading = true;
            axios.get("/products")
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
        createProduct: function () {
            this.loading = true;
            axios.post('/products', this.productModel)
                .then(res => {
                    console.log(res.data);
                    this.products.push(res.data);
                })
                .catch(err => {
                    console.error(err)
                })
                .then(() => {
                    this.loading = false;
                    this.editing = false;
                });
        },
        updateProduct: function () {
            this.loading = true;
            axios.put('/products', this.productModel)
                .then(res => {
                    console.log(res.data);
                    this.products.splice(this.objectIndex, 1, res.data);
                })
                .catch(err => {
                    console.error(err)
                })
                .then(() => {
                    this.loading = false;
                    this.editing = false;
                });
        },
        editProduct: function (id, index) {
            this.editing = true;
            this.objectIndex = index;
            this.getProduct(id);
        },
        deleteProduct: function (id, index) {
            this.loading = true;
            axios.delete("/products/" + id)
                .then(res => {
                    console.log(res);
                    this.products.splice(index, 1);
                })
                .catch(err => {
                    console.error(err)
                })
                .then(() => {
                    this.loading = false;
                });
        },
        cancel: function () {
            this.editing = false;
        }
    },
    computed: {}
})