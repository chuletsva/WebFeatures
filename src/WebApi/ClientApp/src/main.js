import Vue from "vue";
import Paginate from 'vuejs-paginate'
import App from "./App.vue";
import router from "./router";
import { BootstrapVue, IconsPlugin } from "bootstrap-vue";
import "bootstrap/dist/css/bootstrap.css";
import "bootstrap-vue/dist/bootstrap-vue.css";

Vue.use(BootstrapVue);
Vue.use(IconsPlugin);
Vue.component('paginate', Paginate);

new Vue({
  el: "#app",
  render: h => h(App),
  router
});
