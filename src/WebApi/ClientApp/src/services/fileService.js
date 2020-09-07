import client from "../utils/httpClient";

class FileService {
  getDownloadLink(id){
    return `${client.baseUrl}/files/${id}`;
  }
}

const service = new FileService();

export default service;
