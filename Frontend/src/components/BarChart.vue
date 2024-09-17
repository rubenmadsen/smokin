<template>
  <div class="bar-chart">
    <div v-for="(value, index) in data" :key="index" class="bar-container">
      <div
        class="bar"
        :style="{ height: getBarHeight(value) }"
        :title="categories[index] + ': ' + value"
      ></div>
      <div class="label">{{ categories[index] }}</div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'BarChart',
  props: {
    data: {
      type: Array,
      required: true
    },
    categories: {
      type: Array,
      required: true
    }
  },
  computed: {
    maxValue() {
      return Math.max(...this.data);
    }
  },
  methods: {
    getBarHeight(value) {
      const maxHeight = 500; // Maximum height of the bar in pixels
      if (this.maxValue === 0) return '0px'; // Handle case where all values are zero
      return `${(value / this.maxValue) * maxHeight}px`;
    }
  }
}
</script>

<style scoped>
.bar-chart {
  display: flex;
  justify-content: space-between; /* Evenly space bars */
  align-items: flex-end;
  height: 600px;
  border: 1px solid #ddd;
  padding: 10px;
  overflow-x: auto; /* Add horizontal scroll if needed */
}

.bar-container {
  display: flex;
  flex-direction: column;
  align-items: center;
  margin: 0 5px; /* Consistent space between bars */
  position: relative;
}

.bar {
  width: 20px; /* Increased width for better visibility */
  background-color: #3498db;
  color: white;
  text-align: center;
  transition: background-color 0.3s;
  border-radius: 3px; /* Optional: rounded corners */
}

.bar:hover {
  background-color: #2980b9;
}

.label {
  font-size: 12px; /* Adjusted font size */
  transform: rotate(-90deg); /* Rotate labels to vertical */
  transform-origin: left bottom; /* Set origin for rotation */
  white-space: nowrap; /* Prevent wrapping */
  position: absolute; /* Position labels absolutely if needed */
  top: 100%; /* Position label below the bar */
  left: 50%; /* Center label horizontally */
  margin-left: -10px; /* Adjust margin to fit rotated label */
}
</style>
