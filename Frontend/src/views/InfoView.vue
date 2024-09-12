<template>
  <div class="mainContainer">
    <div id="child1">
      <router-link :to="{ path: '/tracker', query: { data: inputData } }">
        <button>Jag vill förändra mitt liv nu!</button>
       
      </router-link>
      <div id="app">
    <BarChart :data="chartData" :categories="chartCategories" />
  </div>
    </div>
    <div id="child2">
      <h1 :style="{ color: 'white' }">
        Your life expecancy has decreased by
        {{ decreasedLifeExpectancy }} minutes.
      </h1>
      <h1 :style="{ color: 'white' }">You have spent {{ moneySpent }} SEK.</h1>

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

      <input
        v-model="sliderValueAmountInfo"
        type="range"
        min="0"
        max="50"
        class="slider"
      />
      <p v-if="selectedButton">
        {{
          selectedButton === "Cigarette"
            ? `You have smoked ${sliderValueAmountInfo} Cigarettes`
            : `You have smoked ${sliderValueAmountInfo} E-Cigarettes`
        }}
      </p>

      <input
        v-model="sliderValueYearsInfo"
        type="range"
        min="0"
        max="50"
        class="slider"
      />
      <p>You have smoked for {{ sliderValueYearsInfo }} years</p>
    </div>
    <div id="goBtn"></div>
  </div>
</template>

<script>

import BarChart from '/src/components/BarChart.vue';
import axios from 'axios';

const URL_FOR_LOCAL_HOST = "https://localhost:7197/";


export default {
  name: "InfoView",
  components: {
    BarChart

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
      inputData: "", // This is your original input data
      selectedButton: "Cigarette", // Cigarette button by default, then it changes to the selected by user
      sliderValueAmountInfo: this.data || "No data", // Default slider value
      sliderValueYearsInfo: this.years|| "No data" ,
      reduceLifeRate: 11, // Default value used, we assume 11 min for cigarette and 9 min for e-cigarette
      moneySpendingRate: 4, // Cost in SEK per cigarette, we assume 4 kr for cigarette and 2 kr for e-cigarette
      toxinNames: [],
      chartData: [40, 43, 44, 47, 54, 58, 69, 110, 120, 138],
      chartCategories: [],
      fetchError: false
    };
  },
  methods: {
    toggleButton(button) {
      this.selectedButton = button; // Set the button as pressed
      this.reduceLifeRate = this.selectedButton === "Cigarette" ? 11 : 9;
      this.moneySpendingRate = this.selectedButton === "Cigarette" ? 4 : 2;
    },
    async getData() {
      try {
        await new Promise(resolve => setTimeout(resolve, 1000));
        const response = await axios.get(URL_FOR_LOCAL_HOST + "api/TodoApp/GetSubstanceCategoryAndDescription");
        this.toxinNames = Object.keys(response.data).map(key => response.data[key]).filter(value => value !== null)[0];
        this.chartCategories = this.toxinNames.map(toxin => toxin.toxinName);
    
      } catch (error) {
        console.error('Error fetching data:', error);
        this.fetchError = true;
      }
    }
  },
  computed: {
    receivedData() {
      return this.$route.query.data || "No data received";
    },
    decreasedLifeExpectancy() {
      // Calculate reduction in life expectancy

      return (
        this.reduceLifeRate *
        this.sliderValueAmountInfo *
        365 *
        this.sliderValueYearsInfo
      );
    },
    moneySpent() {
      // Calculate amount of money spent
      return (
        this.moneySpendingRate *
        this.sliderValueAmountInfo *
        365 *
        this.sliderValueYearsInfo
      );
    },
  },
  mounted: function() {
    this.getData();
  }
};
</script>

<style>
.mainContainer {
  padding: 1rem;
  display: flex;
}
#child1 {
  background-color: burlywood;
  flex: 1.2;
}
#child2 {
  flex: 1;
  background-color: chartreuse;
}
#goBtn {
  display: inline;
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
