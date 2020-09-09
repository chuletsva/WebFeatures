<template>
  <div>
    <b-button :disabled="!hasPrevPage()" @click="changePage(prevPage)">
      Prev
    </b-button>
    <b-button
      v-for="page in pages"
      :key="page"
      :variant="page === currentPage ? 'primary' : ''"
      @click="changePage(page)"
      >{{ page }}</b-button
    >
    <b-button :disabled="!hasNextPage()" @click="changePage(nextPage)">
      Next
    </b-button>
  </div>
</template>
<script>
export default {
  props: {
    pagesCount: {
      type: Number,
      required: true,
      default: 0
    },
    perPage: {
      type: Number,
      required: true,
      default: 4
    },
    currentPage: {
      type: Number,
      required: true,
      default: 1
    },
    pageRange: {
      type: Number,
      default: 2
    }
  },
  methods: {
    changePage(pageNum) {
      this.$emit("page-changed", pageNum);
    },
    hasPrevPage() {
      return this.currentPage > 1;
    },
    hasNextPage() {
      return this.currentPage < this.pagesCount;
    }
  },
  computed: {
    pages() {
      const start = Math.max(this.currentPage - this.pageRange, 1);
      const finish = Math.min(this.currentPage + this.pageRange, this.pagesCount);

      let pages = [];

      for (let i = start; i <= finish; i++) {
        pages.push(i);
      }
      
      return pages;
    },
    prevPage() {
      return this.currentPage - 1;
    },
    nextPage() {
      return this.currentPage + 1;
    }
  },
  created() {
    this.changePage(this.currentPage);
  }
};
</script>
