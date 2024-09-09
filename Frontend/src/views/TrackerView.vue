<template>
    <div class="sumContainer">
      <div class="child">
        <p>money</p>
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
    name: 'TrackerView'
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
  