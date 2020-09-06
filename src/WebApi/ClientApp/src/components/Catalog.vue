<template>
  <div class="d-flex flex-wrap align-content-md-center justify-content-center">
    <div class="p-2" v-for="product in products" v-bind:key="product.id">
      <b-card
        v-bind:title="product.name"
        v-bind:img-src="product.pictureUrl"
        v-bind:img-alt="product.name + ' image'"
        img-top
        tag="article"
      >
        <b-card-text>{{ product.description }}</b-card-text>

        <router-link to="/">
          <b-button variant="success">ADD TO BASKET</b-button>
        </router-link>
      </b-card>
    </div>
  </div>
</template>

<script>
import productService from "../services/productService";
import fileService from "../services/fileService";

export default {
  data() {
    return {
      products: []
    };
  },
  created() {
    productService.get().then(response => {
      this.products = response.data;

      this.products.forEach(product => {
        if (product.pictureId === null) {
          product.pictureUrl = require("../assets/missing.png");
        } else {
          product.pictureUrl = fileService.baseUrl + "/" + product.pictureId;
        }
      });
    });
  }
};
</script>
