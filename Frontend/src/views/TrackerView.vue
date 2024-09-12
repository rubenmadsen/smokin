<template>
    <div class="sumContainer">
      <div class="child">
        <p>money</p>
        <p > {{numberOfCig}} </p>
        <p >{{ yearsOfSmoking }}</p>
      </div>
      <div class="child">
        <p>time</p>
      </div>
    </div>
    <div>
        <h2>Graph</h2>
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
     
      numberOfCig: this.data || "No data", // Default slider value
      yearsOfSmoking: this.years|| "No data" ,
      
    };
  },
  computed: {
    receivedData() {
      return this.$route.query.data || "No data received";
    }
  }
  }
  </script>
  
  <style scoped>
    .sumContainer{
        display: flex;
        flex-direction: row;
        justify-content: center;
    }
    .child{
        width: 150px;
        height: 150px;
        background-color: blueviolet;
    }
    .sumContainer last-child{
        background-color: aqua;
    }
  </style>
  