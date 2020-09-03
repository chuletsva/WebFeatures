<template>
  <div class="catalog">
    <div class="catalog-item" v-for="product in products" :key="product.id">
      <form>
        <img
          class="catalog-item-image"
          v-bind:src="'https://localhost:5001/api/files/' + product.pictureId"
          width="400px"
          height="auto"
        />
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
.catalog {
  padding-right: 15px;
  padding-left: 15px;
  margin-right: auto;
  margin-left: auto;
}
</style>