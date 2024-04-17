<template>
<div class="home-container">
  <h1 class="page-title">Home Page</h1>
  
  <p class="page-description">
    This page serves as the home page. Assuming the backend has been correctly implemented and successfully connected, it should present the toxins data within a table format.
  </p>

  <div v-if="toxins.length > 0" class="toxins-table-container">
    <table class="toxins-table">
      <thead>
        <tr>
          <th>Compound</th>
          <th>Tobacco Concentration</th>
          <th>Electronic Concentration</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="(toxin, index) in toxins" :key="index">
          <td>{{ toxin.Compound }}</td>
          <td>{{ toxin.Tobacco }}</td>
          <td>{{ toxin.Electronic }}</td>
        </tr>
      </tbody>
    </table>
  </div>

  <div v-else-if="fetchError" class="error-message">
    Failed to fetch toxins data.
    <button @click="reloadData" class="reload-button">Reload Data</button>
  </div>

  <div v-else class="loading-message">
    Loading toxins data...
  </div>
</div>
</template>


<script>
import '../components/toxinsList.css'
import axios from 'axios';

const URL_FOR_LOCAL_HOST = "https://localhost:7197/"
//const URL_FOR_LOCAL_HOST = "TODO"


export default {
  name: 'HomeView',
  data() {
    return {
      toxins: [],
      fetchError: false,
    }
  },
  methods: {
    async getData() {
      try {
        await new Promise(resolve => setTimeout(resolve, 1000));
        const response = await axios.get(URL_FOR_LOCAL_HOST + "api/TodoApp/GetToxinData");
        this.toxins = Object.keys(response.data).map(key => response.data[key]);
        console.log(this.toxins);
      } catch (error) {
        console.error('Error fetching data:', error);
        this.fetchError = true;
      }
    },
    async reloadData() {
      this.fetchError = false; 
      await this.getData();
    }
  },

  mounted:function(){
        this.getData()
      }
}

</script>

<style>

.reload-button {
  background-color: #4CAF50;
  border: none;
  color: white;
  padding: 10px 20px;
  text-align: center;
  text-decoration: none;
  display: inline-block;
  font-size: 16px;
  margin-top: 10px;
  cursor: pointer;
}

.reload-button:hover {
  background-color: #45a049;
}
</style>
