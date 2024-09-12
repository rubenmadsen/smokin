<template>
  <div class="sumContainer">
    <div class="child">
      <p>money</p>
      <p>{{ numberOfCig }}</p>
      <p>{{ yearsOfSmoking }}</p>
    </div>
    <div class="child">
      <p>time</p>
    </div>
  </div>
  <div>
    <h2>Graph</h2>
  </div>

  <div class="sliderContainer">
    <p>How many cigarettes/e-cigarettes did you smoke today?</p>
    <input v-model="sliderValue" type="range" min="0" max="100" class="slider" />
    <p>{{ sliderValue }} cigarettes/e-cigarettes</p>

    <button @click="handleSubmit" class="submitButton">Submit</button>
  </div>
</template>

<script>
export default {
  name: 'TrackerView',
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
    handleSubmit() {
      alert(`Submitted value: ${this.sliderValue}`);
    }
  }
};
</script>

<style scoped>
.sumContainer {
  display: flex;
  flex-direction: row;
  justify-content: center;
}
.child {
  width: 150px;
  height: 150px;
  background-color: blueviolet;
}
.sumContainer last-child {
  background-color: aqua;
}

.sliderContainer {
  margin-top: 20px;
  text-align: center;
  display: flex;
  align-items: center;
}

.slider {
  width: 80%;
  margin-right: 20px;
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
</style>
