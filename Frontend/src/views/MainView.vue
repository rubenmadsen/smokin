<template>
  <div class="ribbonContainer">
    <div id="left">
      <div style="border: var(--border);">
        <h1 :style="{ color: 'white' }">You have spent {{ moneySpent }} SEK.</h1>
        <h1 :style="{ color: 'white' }">
          Your life expecancy has decreased by
          {{ decreasedLifeExpectancy }} minutes.
        </h1>

        <router-link :to="{ path: '/tracker', query :{ data : this.sliderValueAmount, years: this.sliderValueYears } }">
          <button class="std-button">Do something</button>
        </router-link>
      </div>

      <div style="border: var(--border); height: 190px;">

      </div>



      <div style="border: var(--border);">
      <router-link :to="{ path: '/info', query :{data : this.sliderValueAmount, years: this.sliderValueYears} }"
        ><h1 class="non-selectable">Jag vill veta mer hur rökning påverkar mig!!</h1></router-link
        >
      
      <router-link :to="{ path: '/tracker', query :{ data : this.sliderValueAmount, years: this.sliderValueYears } }">
        <button class="std-button">Let's go!</button>
      </router-link>
      </div>
    </div>

    
    <div id="right">
      <div class="labelContainer">
          <p>Hur mycket har du bränt?</p>
      </div>
      <div class="inputContainer">
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
      <div class="inputContainer">
        <p v-if="selectedButton">
          {{
            selectedButton === "Cigarette"
              ? `You have smoked ${sliderValueAmount} Cigarettes`
              : `You have smoked ${sliderValueAmount} E-Cigarettes`
          }}
        </p>
        <input
          v-model="sliderValueAmount"
          type="range"
          min="0"
          max="50"
          class="slider"
        />
        
        </div>
        <div class="inputContainer">
        <p>How many years have you smoked for: {{ sliderValueYears }}</p>
        <input
          v-model="sliderValueYears"
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
export default {
  name: "MainView",
  data() {
    return {
      inputData: "", // This is your original input data
      selectedButton: "Cigarette", // Cigarette button by default, then it changes to the selected by user
      sliderValueAmount: 10, // Default slider value
      sliderValueYears: 5,
      reduceLifeRate: 11, // Default value used, we assume 11 min for cigarette and 9 min for e-cigarette
      moneySpendingRate: 4, // Cost in SEK per cigarette, we assume 4 kr for cigarette and 2 kr for e-cigarette
    };
  },
  methods: {
    toggleButton(button) {
      this.selectedButton = button; // Set the button as pressed
      this.reduceLifeRate = this.selectedButton === "Cigarette" ? 11 : 9;
      this.moneySpendingRate = this.selectedButton === "Cigarette" ? 4 : 2;
    },
  },
  computed: {
    decreasedLifeExpectancy() {
      // Calculate reduction in life expectancy

      return (
        this.reduceLifeRate *
        this.sliderValueAmount *
        365 *
        this.sliderValueYears
      );
    },
    moneySpent() {
      // Calculate amount of money spent
      return (
        this.moneySpendingRate *
        this.sliderValueAmount *
        365 *
        this.sliderValueYears
      );
    },
    infoLink() {
      return {
        path: "/info",
        query: {
          data: this.sliderValueAmount ,
          years: this.sliderValueYears,
        },
      };
    },
    trackerLink(){
      return {
        path: "/tracker",
        query: {
          data: this.sliderValueAmount ,
          years: this.sliderValueYears,
        },
      }
    }
  },
  mounted() {
    console.log('Query Params:', this.$route.query);
    console.log('Received Data:', this.data);
    console.log('Received Years:', this.years);
  }
};
</script>

<style scoped>

.inputContainer{
  text-align: center;
}
#left {
  background-color: var(--primary-color);
  color: var(--text-color);
  flex: 1.2;
  padding: 1rem;
}
#right {
  flex: 1;
  background-color:var(--text-color);
  color: var(--primary-color);
  padding: 1rem;
}
#right > div{
  border: var(--border);
  padding-bottom: 1rem;
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
