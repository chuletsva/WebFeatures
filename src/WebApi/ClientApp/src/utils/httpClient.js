import axios from "axios";

export default new class HttpClient {
  baseUrl = "https://localhost:5001/api";

  get(path) {
    return axios.get(this.baseUrl + path);
  }

  post(path, body) {
    return axios.post(this.baseUrl + path, body);
  }
}
