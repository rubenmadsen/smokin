<template>
    <div class="dashboard">
      <DashMeter></DashMeter>
    </div>
    <div>
        <h2>Graph</h2>
    </div>
  </template>
  
  <script>
  import DashMeter from '@/components/DashMeter.vue';
  export default {
    name: 'TrackerView',
    components: {
      DashMeter, 
    },
  }
  </script>
  
  <style scoped>
    .dashboard{
      border: solid whitesmoke 1px;
      padding: 1rem;
    }
  
  </style>
  