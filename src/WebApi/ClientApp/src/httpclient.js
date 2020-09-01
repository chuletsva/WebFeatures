import axios from "axios";

class HttpClient {
  baseUrl = "https://localhost:5001/api"; // process.env.BASE_URL

  get(path) {
    return axios.get(this.baseUrl + path);
  }

  post(path) {
    return axios.post(this.baseUrl + path);
  }
};

const client = new HttpClient();

export default client;