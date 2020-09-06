import client from "./httpClient";

class FileService {
  baseUrl = client.baseUrl + "/files";
}

export default new FileService();
