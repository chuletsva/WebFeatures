<template>
  <div class="catalog">
    <div class="catalog-item" v-for="product in products" :key="product.id">
      <form>
        <img class="catalog-item-image" src="../assets/missing.png" />
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
.catalog-item {
  vertical-align: top;
  display: inline-block;
  text-align: center;
  width: 300px;
}
.catalog-item-image {
  width: 100%;
  height: auto;
}
</style>