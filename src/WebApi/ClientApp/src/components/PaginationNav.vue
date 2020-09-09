<template>
  <div>
    <button v-on:click="movePrev" :disabled="currentPage === 1">
      Prev
    </button>
    <button
      v-for="number in pagesCount"
      :key="number"
      v-on:click="onClick(number)"
    >
      {{ number }}
    </button>
    <button v-on:click="moveNext" :disabled="currentPage === pagesCount">
      Next
    </button>
  </div>
</template>
<script>
export default {
  props: {
    pagesCount: Number,
    onClick: Function
  },
  data() {
    return {
      currentPage: 1
    };
  },
  methods: {
    movePrev() {
      this.currentPage--;
      this.onClick(this.currentPage);
    },
    moveNext() {
      this.currentPage++;
      this.onClick(this.currentPage);
    }
  },
  created() {
    this.onClick(this.currentPage);
  }
};
</script>
