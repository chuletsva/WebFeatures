import client from "../utils/httpClient";

export default new class FileService {
  getDownloadLink(id){
    return `${client.baseUrl}/files/${id}`;
  }
}
