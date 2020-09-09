import client from "../utils/httpClient";
import paginationQueryBuilder from "../utils/paginationQueryBuilder";

export default new class ProductService {
  getById(id) {
    return client.get(`/products/${id}`);
  }
  get(pagination) {
    let subPath = "/products";

    if (pagination !== undefined) {
      let query = paginationQueryBuilder.build(pagination);
      subPath += query;
    }

    return client.get(subPath);
  }
  create(product) {
    return client.post("/products", product);
  }
}
