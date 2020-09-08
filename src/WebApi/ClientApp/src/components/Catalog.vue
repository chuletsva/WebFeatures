<template>
  <div class="catalog">
    <paginate
      :page-count="pagesCount"
      :page-range="2"
      :click-handler="fetchProducts"
      :prev-text="'Prev'"
      :next-text="'Next'"
      :container-class="'pagination'"
      :page-class="'page-item'"
    ></paginate>

    <div class="d-flex flex-wrap align-content-md-center justify-content-center">
      <div class="p-2" v-for="product in products" :key="product.id">
        <b-card
          :title="product.name"
          :img-src="product.pictureUrl"
          :img-alt="product.name + ' image'"
          img-top
          tag="article"
        >
          <b-link to="/">
            <b-button variant="success">ADD TO BASKET</b-button>
          </b-link>
        </b-card>
      </div>
    </div>
  </div>
</template>

<script>
import productService from "../services/productService";
import fileService from "../services/fileService";

export default {
  data() {
    return {
      perPage: 8,
      pagesCount: null,
      products: []
    };
  },
  methods: {
    fetchProducts(pageNum) {
      let pagination = {
        skip: (pageNum - 1) * this.perPage,
        top: this.perPage
      };

      productService.get(pagination).then(response => {
        this.products = response.data.items;

        this.products.forEach(product => {
          if (product.pictureId === null) {
            product.pictureUrl = require("../assets/missing.png");
          } else {
            product.pictureUrl = fileService.getDownloadLink(product.pictureId);
          }
        });

        console.log(response.data.lastPage);

        this.pagesCount = response.data.lastPage;
      });
    }
  },
  created() {
    this.fetchProducts(1);
  }
};
</script>
