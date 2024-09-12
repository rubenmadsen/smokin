<template>
    <div class="bar-chart">
      <div v-for="(value, index) in data" :key="index" class="bar-container">
        <div
          class="bar"
          :style="{ height: `${value}px` }"
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
    }
  }
  </script>
  
  <style scoped>
  .bar-chart {
    display: flex;
    justify-content: space-around;
    align-items: flex-end;
    height: 300px;
    border: 1px solid #ddd;
    padding: 10px;
  }
  
  .bar-container {
    display: flex;
    flex-direction: column;
    align-items: center;
    margin: 0 10px;
  }
  
  .bar {
    width: 30px;
    background-color: #3498db;
    color: white;
    text-align: center;
    transition: background-color 0.3s;
  }
  
  .bar:hover {
    background-color: #2980b9;
  }
  
  .label {
    margin-top: 5px;
    font-size: 12px;
  }
  </style>
  