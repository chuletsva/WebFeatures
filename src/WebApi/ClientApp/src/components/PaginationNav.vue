<template>
  <div>
    <button v-show="currentPage != 1" v-on:click="movePrev">Prev</button>
    <button v-for="number in pagesCount" :key="number" v-on:click="onClick(number)">
      {{ number }}
    </button>
    <button v-on:click="moveNext" v-show="currentPage != pagesCount">
      Next
    </button>
  </div>
</template>
<script>
export default {
  props: {
    pagesCount: Number,
    currentPage: Number,
    onClick: Function
  },
  data() {
    return {};
  },
  methods: {
    movePrev() {},
    moveNext() {}
  }
};
</script>
