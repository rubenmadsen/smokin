// services/Backend.js
import axios from 'axios';

const API_BASE_URL = 'https://localhost:7197/api/'; 
const API_URL = API_BASE_URL + 'TodoApp/'
class Backend {    
  constructor() {
    if(Backend.instance)
        return Backend.instance;

    Backend.instance = this;
    
    this.axiosInstance = axios.create({
        baseURL: API_URL, 
        timeout: 10000, 
        headers: {
          'Content-Type': 'application/json',
        },
      });

  }

  static getAxios() {
    const instance = Backend.instance || new Backend();
    return instance.axiosInstance;
  }

  static getToxinData() {
    const URL = API_URL + 'GetToxinData/';
    return Backend.getAxios().get(URL)
      .then((response) => {
        return response.data;
      })
      .catch((error) => {
        throw error;
      });
  }

  // Endpoint: GP-26
  static getSubstancesAndConcentrations(cigarrette_type) {
    const URL = API_URL + 'GetSubstancesAndConcentrations/' + cigarrette_type + '/';
    return Backend.getAxios().get(URL)
      .then((response) => {
        return response.data;
      })
      .catch((error) => {
        throw error;
      });
  }

  // Endpoint: GP-29
  static getSubstanceCategoryAndDescription() {
    const URL = API_URL + 'GetSubstanceCategoryAndDescription/';
    return Backend.getAxios().get(URL)
      .then((response) => {
        return response.data;
      })
      .catch((error) => {
        throw error;
      });
  }

// Endpoint: GP-27
static postNewUserTrackingData(formData) {
  const URL = API_URL + 'PostNewUserTrackingData/';
  return Backend.getAxios().post(URL, formData, {
    headers: {
      'Content-Type': 'multipart/form-data',
    },
  })
  .then((response) => {
    return response.data;
  })
  .catch((error) => {
    throw error;
  });
}

// Endpoint: GP-28
static getTrackedUserData() {
  const URL = API_URL + 'GetTrackedUserData/';
  return Backend.getAxios().get(URL)
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      throw error;
    });
}

}
export default Backend;