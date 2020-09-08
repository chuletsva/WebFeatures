import client from "../utils/httpClient";

class FileService {
  getDownloadLink(id){
    return `${client.baseUrl}/files/${id}`;
  }
}

export default new FileService();
