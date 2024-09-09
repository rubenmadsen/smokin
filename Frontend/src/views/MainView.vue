<template>
    <div class="mainContainer">
        <div id="child1">
            <router-link to="/info">
                <h1>Det är svindåligt att röka</h1>            
            </router-link>
        </div>
        <div id="child2">

        </div>
    </div>
    <div id="goBtn">
        <router-link to="/tracker">
            <button>Link</button>
        </router-link>
    </div>

  </template>
  
  <script>

  export default {
    name: 'MainView'
  }
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
  </style>
  