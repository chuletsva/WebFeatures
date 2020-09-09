<template>
  <div class="catalog">
    <pagination-nav :pagesCount="pagesCount" :onClick="fetchProducts"></pagination-nav>

    <div class="d-flex flex-wrap align-content-md-center justify-content-center">
      <div class="p-2" v-for="product in products" :key="product.id">
        <b-card
          :title="product.name"
          :img-src="product.pictureUrl"
          :img-alt="product.name + ' image'"
          img-top
          tag="article"
        >
          <b-button variant="success">ADD TO BASKET</b-button>
        </b-card>
      </div>
    </div>
  </div>
</template>

<script>
import productService from "../services/productService";
import fileService from "../services/fileService";
import PaginationNav from "./PaginationNav.vue";

export default {
  components:{
    'pagination-nav': PaginationNav
  },
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

        this.pagesCount = response.data.lastPage;
      });
    }
  },
  created() {
    this.fetchProducts(1);
  }
};
</script>
