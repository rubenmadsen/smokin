<template>
  <div>
    <div class="dashboard">
      <DashMeter ref="dashMeter"></DashMeter>
    </div>
    
    <div class="ribbonContainer">
      <div class="graphContainer">
       
        <div class="scatter-plot-container">
          <ScatterChart :data="chartData" :options="chartOptions" />
        </div>
      </div>
      <div class="separator"></div>
      <div class="sliderContainer">
        <div class="buttonContainer">
          <button
            class="button-custom-1"
            :class="{ 'button-pressed-1': selectedButton === 'Cigarette' }"
            @click="toggleButton('Cigarette')"
          >
            Cigarette
          </button>
          <button
            class="button-custom-2"
            :class="{ 'button-pressed-2': selectedButton === 'E-Cigarette' }"
            @click="toggleButton('E-Cigarette')"
          >
            E-Cigarette
          </button>
        </div>
        <p>How many {{ this.selectedButton.toLowerCase() + "s" }} did you smoke today?</p>
        <input v-model="sliderValue" type="range" min="0" max="100" class="slider" />
        <p>{{ sliderValue + " " + this.selectedButton.toLowerCase() + "s" }}</p>
  
        <button @click="handleSubmit" class="std-button">Submit</button>
      </div>
    </div>
  </div>
</template>

<script>
import { defineComponent } from 'vue';
import { Scatter } from 'vue-chartjs';
import { Chart as ChartJS, Title, Tooltip, Legend, PointElement, LinearScale } from 'chart.js';
import DashMeter from '@/components/DashMeter.vue';
import Backend from '@/services/backend.js';

// Register chart.js components
ChartJS.register(Title, Tooltip, Legend, PointElement, LinearScale);

export default defineComponent({
  name: 'TrackerView',
  components: {
    DashMeter,
    ScatterChart: Scatter,
  },
  data() {
    return {
      selectedButton: "Cigarette",
      numberOfCig: this.data || "No data",
      yearsOfSmoking: this.years || "No data",
      sliderValue: 50,
      
      chartData: {
        datasets: [
          {
            
            
            label: 'Data',
            data: [{x:1, y:10},{x:2, y:7},{x:3, y:7},{x:4, y:10},{x:5, y:2},{x:6, y:3},{x:7, y:3},{x:8, y:3}],
            backgroundColor:'rgb(255, 255, 255)',
            borderColor: 'rgb(255, 255, 255)',
           
            pointRadius: 5,
            
          },
        ],
      },
      chartOptions: {
        responsive: true,
        maintainAspectRatio: false,
        scales: {
          x: {
            ticks:{
              color: 'rgb(255, 255, 255)'

            },
            type: 'linear',
            position: 'bottom',
            color: 'rgb(255, 255, 255)',
            title: {
              display: true,
              text: 'Days',
              color: 'rgb(255, 255, 255)',
            },
          },
          y: {
            ticks:{
              color: 'rgb(255, 255, 255)'

            },
            title: {
              display: true,
              text: 'Amount',
              color: 'rgb(255, 255, 255)',
            },
          },
        },
      },
    };
  },
  computed: {
    receivedData() {
      return this.$route.query.data || "No data received";
    }
  },
  methods: {
   
    toggleButton(button) {
      this.selectedButton = button;
      this.reduceLifeRate = this.selectedButton === "Cigarette" ? 11 : 9;
      this.moneySpendingRate = this.selectedButton === "Cigarette" ? 4 : 2;
    },
    handleSubmit() {
      let formData = new FormData();
      formData.append('userName', 'ruben');
      formData.append('consumableName', this.selectedButton === 'Cigarette' ? 'cig' : 'e-cig');
      formData.append('date', new Date().toISOString().split('T')[0]);
      formData.append('amount', this.sliderValue);

      Backend.postNewUserTrackingData(formData)
        .then(response => {
          console.log("Data successfully submitted:", response);
          alert("Data successfully submitted!");
          this.$refs.dashMeter.getData();
        })
        .catch(error => {
          console.error("Error submitting data:", error);
          alert("Error: You have already submitted data for today!");
        });
    }
  }
});
</script>

<style scoped>
.ribbonContainer {
  margin-top: 0px;
  flex-direction: column;
}
.ribbonContainer > div {
  margin-top: 1rem;
}
.mainContainer > div {
  margin-top: 0;
}
.graphContainer {
  border: var(--border);
  text-align: center;
  display: flex; /* Add Flexbox */
  justify-content: center; /* Center horizontally */
  align-items: center; /* Center vertically */
  height: 100%; /* Ensure the container takes full height */
  min-height: 300px; /* Set a minimum height if needed */
}
.scatter-plot-container {
  height:200px; /* Adjusted height for the chart */
  width: 100%; /* Full width of the container */
  max-width: 600px; /* Limit the maximum width */
  margin: 0; /* Remove any default margins */
}
.sliderContainer {
  text-align: center;
  display: inline;
  align-items: center;
}
.sliderContainer > * {
  margin-top: 1rem;
}
.slider {
  width: 60%;
  margin-right: 20px;
}
.buttonContainer {
}
.buttonContainer button {
  margin: 0.5rem;
}
.submitButton {
  padding: 10px 20px;
  background-color: #3498db;
  color: white;
  border: none;
  border-radius: 5px;
  cursor: pointer;
  font-size: 16px;
  transition: background-color 0.3s;
}
.submitButton:hover {
  background-color: #2980b9;
}
.dashboard {
  display: block;
  margin: 0px;
  padding: 0px;
}
.button-custom-1 {
  background-color: #3498db;
  color: white;
  padding: 10px 20px;
  border: none;
  border-radius: 5px;
  font-size: 16px;
  cursor: pointer;
  transition: background-color 0.3s;
  margin-right: 0px;
  margin-top: 0px;
}
.button-pressed-1 {
  background-color: #2980b9;
}
.button-custom-2 {
  background-color: #3498db;
  color: white;
  padding: 10px 20px;
  border: none;
  border-radius: 5px;
  font-size: 16px;
  cursor: pointer;
  transition: background-color 0.3s;
  margin-right: 0px;
  margin-top: 0px;
}
.button-pressed-2 {
  background-color: #2980b9;
}
</style>
