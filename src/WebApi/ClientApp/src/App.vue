<template>
  <div id="app">
    <!-- <app-header></app-header> -->
    <router-view></router-view>
  </div>
</template>

<script>
// import Header from "./components/Header.vue";

export default {
  components: {
    // "app-header": Header
  },
  data() {
    return {};
  }
};
</script>

<style>
html,
body {
  font-family: Montserrat, sans-serif;
  font-size: 16px;
  font-weight: 400;
  z-index: 10;
}

*,
*::after,
*::before {
  box-sizing: border-box;
}

.preloading {
  color: #00a69c;
  display: block;
  font-size: 1.5rem;
  left: 50%;
  position: fixed;
  top: 50%;
  transform: translate(-50%, -50%);
}

select::-ms-expand {
  display: none;
}

@media screen and (min-width: 992px) {
  .form-input {
    max-width: 360px;
    width: 360px;
  }
}
.form-input {
  border-radius: 0;
  height: 45px;
  padding: 10px;
}
.form-input-small {
  max-width: 100px !important;
}
.form-input-medium {
  width: 150px !important;
}
.alert {
  padding-left: 0;
}
.alert-danger {
  background-color: transparent;
  border: 0;
  color: #fb0d0d;
  font-size: 12px;
}
a,
a:active,
a:hover,
a:visited {
  color: #000;
  text-decoration: none;
  transition: color 0.35s;
}

a:hover,
a:active {
  color: #75b918;
  transition: color 0.35s;
}
</style>