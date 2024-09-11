<template>
    <div class="mainContainer">
      <div id="child1">
        <input v-model="inputData" placeholder="Enter something" />
        <router-link :to="{ path: '/info', query: { data: inputData } }">
          <h1>Det är svindåligt att röka</h1>
        </router-link>
        <p v-if="selectedButton">{{ selectedButton === 'first' ? 'You have selected Cigarette' : 'You have selected E-Cigarette' }}</p>
      </div>
      <div id="child2">
        <button class="button-custom-1"
                :class="{ 'button-pressed-1': selectedButton === 'first' }"
                @click="toggleButton('first')">Cigarette</button>
        <button class="button-custom-2"
                :class="{ 'button-pressed-2': selectedButton === 'second' }"
                @click="toggleButton('second')">E-Cigarette</button>
      </div>
      <div id="goBtn">
        <router-link :to="{ path: '/tracker', query: { data: inputData } }">
          <button>Link</button>
        </router-link>
      </div>
    </div>
  </template>
  
  <script>
  export default {
    name: 'MainView',
    data() {
      return {
        inputData: '',  // This is your original input data
        selectedButton: 'first' // First button by default, then it changes to the selected by user
        };
    },
    methods: {
      toggleButton(button) {
        this.selectedButton = button; // Set the button as pressed
      }
    }
  };
  </script>
  
  <style scoped>
    .mainContainer{
        padding: 1rem;
        display: flex;
    }
    #child1{
        background-color: burlywood;
        flex: 1.2;
    }
    #child2{
        flex: 1;
        background-color: chartreuse;
    }
    #goBtn{
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
  