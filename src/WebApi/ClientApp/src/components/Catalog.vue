<template>
  <div id="catalog">
    <div id="catalog-item" v-for="product in products" :key="product.id">
      <form>
        <img v-bind:src="product.imageUrl" />
        <input type="submit" value="[ADD TO BASKET]" />
        <span>{{ product.name }}</span>
        <span>{{ product.price }}</span>
      </form>
    </div>
  </div>
</template>

<script>
import client from "../httpclient";

export default {
  data() {
    return {
      products: []
    };
  },
  created() {
    client.get("/products").then(response => (this.products = response.data));
  }
};
</script>

<style scoped>
</style>