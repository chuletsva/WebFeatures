import client from "./httpClient";

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

export default new ProductService();
