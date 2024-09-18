<template>
  <div>
    <div class="dashboard">
    <DashMeter></DashMeter>
  </div>
  
  <div class="ribbonContainer ">
    <div class="graphContainer">
      <h2>Moas brutalt feta graf</h2>
    </div>

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
      <p>{{ sliderValue + " " + this.selectedButton.toLowerCase() + "s"}}</p>

      <button  @click="handleSubmit" class="std-button">Submit</button>
    </div>
  </div>
</div>
</template>

<script>
import DashMeter from '@/components/DashMeter.vue';
import Backend from '@/services/backend.js';

export default {
  name: 'TrackerView',
  components:{
    DashMeter,
  },
  props: {
    data: {
      type: String,
      default: "No data received"
    },
    years: {
      type: String,
      default: "No years received"
    }
  },
  data() {
    return {
      //typeOfConsumable: this.consumableType || "No data",
      selectedButton: "Cigarette", // Cigarette button by default, then it changes to the selected by user
      numberOfCig: this.data || "No data",
      yearsOfSmoking: this.years || "No data",
      sliderValue: 50, // Default slider value
    };
  },
  computed: {
    receivedData() {
      return this.$route.query.data || "No data received";
    }
  },
  methods: {
    toggleButton(button) {
      this.selectedButton = button; // Set the button as pressed
      this.reduceLifeRate = this.selectedButton === "Cigarette" ? 11 : 9;
      this.moneySpendingRate = this.selectedButton === "Cigarette" ? 4 : 2;
    },
    handleSubmit() {
      // Create formData object
      let formData = new FormData();
      formData.append('userName', 'ruben'); // Example username
      formData.append('consumableName', this.selectedButton === 'Cigarette' ? 'cig' : 'e-cig'); // Example consumable type  === 'Cigarette' ? 'cig' : 'e-cig'
      formData.append('date', new Date().toISOString().split('T')[0]); // Today's date
      formData.append('amount', this.sliderValue); // Number of cigarettes from the slider

      // Send the data to the backend
      Backend.postNewUserTrackingData(formData)
        .then(response => {
          console.log("Data successfully submitted:", response);
          alert("Data successfully submitted!");
        })
        .catch(error => {
          console.error("Error submitting data:", error);
          alert("Error: You have already submitted data for today!");
        });
    }
  }
};
</script>

<style scoped>
.ribbonContainer {
  margin-top:0px;
  flex-direction: column;
}
.ribbonContainer > div{
  margin-top: 1rem;
}
.mainContainer > div{
  margin-top: 0;
}
.graphContainer{
  border: var(--border);
  text-align: center;
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
  width: 80%;
  margin-right: 20px;
}
.buttonContainer{
  
}
.buttonContainer button{
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

.dashboard{
  display: block;
  margin:0px;
  padding: 0px;
  
}

.button-custom-1 {
  background-color: #3498db; /* Constant color */
  color: white;
  padding: 10px 20px;
  border: none;
  border-radius: 5px;
  font-size: 16px;
  cursor: pointer;
  transition: background-color 0.3s; /* Smooth transition for color change */
  margin-right: 0px; /* Corrected spacing */
  margin-top: 0px;
}

.button-pressed-1 {
  background-color: #2980b9; /* Darker color when pressed */
}

.button-custom-2 {
  background-color: #3498db; /* Constant color */
  color: white;
  padding: 10px 20px;
  border: none;
  border-radius: 5px;
  font-size: 16px;
  cursor: pointer;
  transition: background-color 0.3s; /* Smooth transition for color change */
  margin-right: 0px; /* Corrected spacing */
  margin-top: 0px;
}

.button-pressed-2 {
  background-color: #2980b9; /* Darker color when pressed */
}
</style>
