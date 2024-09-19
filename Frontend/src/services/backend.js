// services/Backend.js
import axios from 'axios';

const API_BASE_URL = 'https://localhost:7197/api/'; 
const API_URL = API_BASE_URL + 'TodoApp/'
class Backend {
  constructor() {
    if (Backend.instance)
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
  static getTrackedUserData(username) {
    const URL = API_URL + 'GetTrackedUserData/' + username + '/';
    return Backend.getAxios().get(URL)
        .then((response) => {
          return response.data;
        })
        .catch((error) => {
          throw error;
        });
  }

  /*
  let formData = new FormData();
                  formData.append('userName', "Dolk Lundgren");
                  formData.append('consumableName', "Cigarette");
                  formData.append('date', "2000-01-01");
                  formData.append('amount', 3);
  */

  static minutesTo(minutes) {
    if (minutes < 60) {
      return `${minutes} minute${minutes !== 1 ? 's' : ''}`;
    } else if (minutes < 1440) {
      const hours = (minutes / 60).toFixed(1);
      return `${hours} hour${hours !== '1.0' ? 's' : ''}`;
    } else {
      const days = (minutes / 1440).toFixed(1);
      return `${days} day${days !== '1.0' ? 's' : ''}`;
    }
  }
  static krTo(kr) {
    if (kr < 1000) {
      return `${kr} kr`;
    } else if (kr < 1000000) {
      const tkr = (kr / 1000).toFixed(1);
      return `${tkr} tkr`;
    } else {
      const mkr = (kr / 1000000).toFixed(1);
      return `${mkr} Mkr`;
    }
  }
}
export default Backend;