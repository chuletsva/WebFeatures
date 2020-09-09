export default new class PaginationQueryBuilder {
  build(pagination) {
    let params = [];

    if (pagination.skip !== undefined) {
      params.push(`$skip=${pagination.skip}`);
    }

    if (pagination.top !== undefined) {
      params.push(`$top=${pagination.top}`);
    }

    if (pagination.filter !== undefined) {
      params.push(`$filter=${pagination.filter}`);
    }

    if (pagination.orderby !== undefined) {
      params.push(`$orderby=${pagination.orderby}`);
    }

    if (params.length === 0) return "";

    return "?" + params.join("&");
  }
}
