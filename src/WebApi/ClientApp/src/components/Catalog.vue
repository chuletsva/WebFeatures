<template>
  <div class="catalog">
    <div class="catalog-items">
      <div class="catalog-item" v-for="product in products" :key="product.id">
        <form>
          <img
            class="catalog-item-image"
            v-bind:src="'https://localhost:5001/api/files/' + product.pictureId"
            width="400px"
            height="auto"
          />
          <input
            class="catalog-item-button"
            type="submit"
            value="[ADD TO BASKET]"
          />
          <div class="catalog-item-name">
            <span>{{ product.name }}</span>
          </div>
          <div class="catalog-item-price">
            <span>{{ product.price }}</span>
          </div>
        </form>
      </div>
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
.catalog-items {
  padding-right: 15px;
  padding-left: 15px;
  margin-right: auto;
  margin-left: auto;
}
.catalog-item {
  margin-bottom: 1.5rem;
  text-align: center;
  width: 33%;
  display: inline-block;
  float: none !important;
}
@media screen and (max-width: 1024px) {
  .catalog-item {
    width: 50%;
  }
}
@media screen and (max-width: 768px) {
  .catalog-item {
    width: 100%;
  }
}
.catalog-item-image {
  max-width: 370px;
  width: 100%;
}
.catalog-item-button {
  background-color: #83d01b;
  border: 0;
  color: #ffffff;
  cursor: pointer;
  font-size: 1rem;
  height: 3rem;
  margin-top: 1rem;
  transition: all 0.35s;
  width: 80%;
}
.catalog-item-button.is-disabled {
  opacity: 0.5;
  pointer-events: none;
}
.catalog-item-button :hover {
  background-color: #4a760f;
  transition: all 0.35s;
}
.catalog-item-name {
  font-size: 1rem;
  font-weight: 300;
  margin-top: 0.5rem;
  text-align: center;
  text-transform: uppercase;
}
.catalog-item-price {
  font-size: 28px;
  font-weight: 900;
  text-align: center;
}
.catalog-item-price::before {
  content: "$";
}
</style>