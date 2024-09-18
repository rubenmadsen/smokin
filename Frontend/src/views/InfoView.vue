<template>
  <div class="ribbonContainer">
    <div id="left">
      <router-link
        :to="{
          path: '/tracker',
          query: {
            data: this.sliderValueAmountInfo,
            years: this.sliderValueYearsInfo,
          },
        }"
      >
        <button>Jag vill förändra mitt liv nu!</button>
      </router-link>
      <div id="app">
        <Bar
          ref="myChartRef"
          id="my-chart-id"
          :options="chartOptions"
          :data="chartData"
        />
      </div>
    </div>
    <div id="right">
      <div class="labelContainer">
        <h1 :style="{ color: 'white' }">
          Your life expecancy has decreased by
          {{ decreasedLifeExpectancy }} minutes.
        </h1>
        <h1 :style="{ color: 'white' }">
          You have spent {{ moneySpent }} SEK.
        </h1>
      </div>

      <div class="btnContainer">
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

      <div class="sliderContainer">
        <p v-if="selectedButton">
          {{
            selectedButton === "Cigarette"
              ? `You have smoked ${sliderValueAmountInfo} Cigarettes`
              : `You have smoked ${sliderValueAmountInfo} E-Cigarettes`
          }}
        </p>
        <input
          v-model="sliderValueAmountInfo"
          type="range"
          min="0"
          max="50"
          class="slider"
        />

        <p>You have smoked for {{ sliderValueYearsInfo }} years</p>
        <input
          v-model="sliderValueYearsInfo"
          type="range"
          min="0"
          max="50"
          class="slider"
        />
      </div>
    </div>
    <div id="goBtn"></div>
  </div>
</template>

<script>
// import BarChart from "/src/components/BarChart.vue";
import axios from "axios";
import { Bar } from "vue-chartjs";
import {
  Chart as ChartJS,
  Title,
  Tooltip,
  Legend,
  BarElement,
  CategoryScale,
  LinearScale,
  LogarithmicScale,
} from "chart.js";

ChartJS.register(
  Title,
  Tooltip,
  Legend,
  BarElement,
  CategoryScale,
  LinearScale,
  LogarithmicScale
);

const URL_FOR_LOCAL_HOST = "https://localhost:7197/api/TodoApp/";

export default {
  name: "InfoView",

  components: {
    Bar,
  },

  props: {
    data: {
      type: String,
      default: "No data received",
    },
    years: {
      type: String,
      default: "No years received",
    },
  },
  data() {
    return {
      inputData: "", 
      selectedButton: "Cigarette", // Cigarette button by default, then it changes to the selected by user
      sliderValueAmountInfo: this.data || "No data", // Default slider value
      sliderValueYearsInfo: this.years || "No data",
      reduceLifeRate: 11, // Default value used, we assume 11 min for cigarette and 9 min for e-cigarette
      moneySpendingRate: 4, // Cost in SEK per cigarette, we assume 4 kr for cigarette and 2 kr for e-cigarette
      toxinNames: [],
      chartCategories: [],
      fetchError: false,
      chartData: {
        labels: [],
        datasets: [{ data: [] }, {descriptions: []}],
      },
      chartOptions: {
       
        responsive: true,
        plugins: {
          legend: { display: false },
          tooltip: {
      callbacks: {
        label: (context) => this.tooltipLabel(context), 
      },
    },


        },
        scales: {
          x: {
            display: true, // Ensure x-axis is displayed
            title: {
              display: true,
            },
            ticks: {
              autoSkip: false, // Ensure all labels are shown
              maxRotation: 90, // Rotate labels if necessary
              minRotation:70,
              font: {
                        size: 16
                    }
            },
            grid: {
              display: false, // Hide grid lines if not needed
            },
          },
          y: {
            display: true,
            beginAtZero: true,
            type: "logarithmic", // Assuming you need logarithmic scale
            grid: {
              display: true,
              drawBorder: false,
            },
            ticks: {
              maxTicksLimit: 15,
              callback: function (value) {
                if (Math.abs(value) < 1) {
                  return Math.abs(value) < 0.01
                    ? value.toExponential(0)
                    : value.toFixed(2);
                } else {
                  return Math.round(value).toString();
                }
              },
            },
          },
        },
      },
    };
  },

  methods: {
    tooltipLabel(context) {
    const datasetLabel = context.dataset.label;
    const value = context.raw;
    const description = context.chart.data.datasets[1].descriptions[context.dataIndex] || '';

    const descriptionLines = this.splitDescription(description, 30);
    return [`${datasetLabel}: ${value}`, ...descriptionLines];
  },
    toggleButton(button) {
      if (button == "Cigarette") {
        this.getData("cig");
      } else {
        this.getData("e-cig");
      }
      this.selectedButton = button; // Set the button as pressed
      this.reduceLifeRate = this.selectedButton === "Cigarette" ? 11 : 9;
      this.moneySpendingRate = this.selectedButton === "Cigarette" ? 4 : 2;
    },

    splitDescription(description, maxChars) {
      const words = description.split(' ');
      let currentLine = '';
      const lines = [];

      words.forEach((word) => {
        if ((currentLine + word).length > maxChars) {
          lines.push(currentLine.trim());
          currentLine = word + ' ';
        } else {
          currentLine += word + ' ';
        }
      });

      if (currentLine) {
        lines.push(currentLine.trim());
      }

      return lines;
    },

    async getData(cigarrette_type) {
      try {
        await new Promise((resolve) => setTimeout(resolve, 1000));
        const responseSubstanceConcentration = await axios.get(
          URL_FOR_LOCAL_HOST +
            "GetSubstancesAndConcentrations/" +
            cigarrette_type
        );
        // Make the API call
        
        await new Promise((resolve) => setTimeout(resolve, 1000));
        const responseDescription = await axios.get(
          URL_FOR_LOCAL_HOST + 'GetSubstanceCategoryAndDescription'
        );
        // Make the API call

        const toxinsDescription = responseDescription.data.Value;

        // Create lists for toxin names and amounts
        const descriptions = toxinsDescription.map((toxin) => toxin.description);
      

        console.log("descriptionLisr" + descriptions);
        

   
      


        const toxins = responseSubstanceConcentration.data.Value;

        // Create lists for toxin names and amounts
        const toxinNames = toxins.map((toxin) => toxin.toxinName);
        const amounts = toxins.map((toxin) => parseFloat(toxin.amount)); // Ensure amounts are floats

        console.log("toxinNames" + toxinNames);
        console.log("amount" + amounts);

        // Set chartData
        this.chartData = {
          labels: toxinNames,
          datasets: [
            {
              label: "Concentration",
              data: amounts,
             
            },
            {
              label: "Description",
              descriptions: descriptions,
            }
          ],
        };
        // Return the transformed list
      } catch (error) {
        console.error("Error fetching data:", error);
        this.fetchError = true;
      }
    },

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

  mounted: function () {
    this.getData("cig");

  },

  trackerLink() {
    return {
      path: "/tracker",
      query: {
        data: this.sliderValueAmount,
        years: this.sliderValueYears,
      },
    };
  },
};
</script>

<style scoped>
.viewContainer {
  padding: 1rem;
  display: flex;
}
#left {
  flex: 1.2;
  padding: 1rem;
}
#right {
  flex: 1;
  padding: 1rem;
  background-color: var(--text-color);
  text-align: left;
}
#right > div{
  border: var(--border);
  padding-bottom: 1rem;
}
.labelContainer {
}
.btnContainer {
}
.sliderContainer {
  color: var(--primary-color);
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
