import Vue from "vue";
import VueRouter from "vue-router";
import Catalog from "./components/Catalog.vue";

Vue.use(VueRouter);

export default new VueRouter({
  mode: "history",
  routes: [
    {
      path: "/",
      name: "catalog",
      component: Catalog
    }
  ]
});
