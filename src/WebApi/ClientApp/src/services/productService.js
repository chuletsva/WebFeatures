import client from "../utils/httpClient";

class ProductService {
  get(id) {
    let path = "/products";
    if (arguments.length === 1) {
      path += id;
    }
    return client.get(path);
  }
  create(product) {
    return client.post("/products", product);
  }
}

const service = new ProductService();

export default service;
