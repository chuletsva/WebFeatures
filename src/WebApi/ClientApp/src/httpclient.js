import axios from "axios";

export default {
  baseUrl: process.env.BASE_URL,

  get: function(path) {
    return axios.get(this.baseUrl + path);
  },
  post: function(path) {
    return axios.post(this.baseUrl + path);
  }
};
