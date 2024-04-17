<template>
  <nav class="#nav">
    <ul>       
      <li><a class="active" href="/">
        <svg width="2.5rem" height="2.5rem" xmlns="http://www.w3.org/2000/svg">
            <path d="M2.568,7.179H8.96c1.411,0,2.557-1.145,2.557-2.557c0-1.412-1.146-2.557-2.557-2.557H8.534c-0.235,0-0.426,0.19-0.426,0.426c0,0.236,0.191,0.426,0.426,0.426H8.96c0.941,0,1.704,0.763,1.704,1.705S9.901,6.327,8.96,6.327H2.568c-0.236,0-0.426,0.19-0.426,0.426C2.142,6.988,2.333,7.179,2.568,7.179 M15.778,7.179c0-0.941-0.763-1.704-1.704-1.704h-0.427c-0.235,0-0.426,0.19-0.426,0.426c0,0.235,0.19,0.426,0.426,0.426h0.427c0.47,0,0.852,0.382,0.852,0.852c0,0.471-0.382,0.853-0.852,0.853H0.864c-0.236,0-0.426,0.19-0.426,0.426c0,0.235,0.19,0.426,0.426,0.426h13.21C15.016,8.884,15.778,8.12,15.778,7.179 M16.631,9.736H2.568c-0.236,0-0.426,0.19-0.426,0.426c0,0.236,0.19,0.426,0.426,0.426h14.062c0.94,0,1.704,0.764,1.704,1.705s-0.764,1.704-1.704,1.704h-0.427c-0.235,0-0.426,0.19-0.426,0.427c0,0.235,0.19,0.426,0.426,0.426h0.427c1.411,0,2.557-1.145,2.557-2.557S18.042,9.736,16.631,9.736 M10.665,11.44H4.273c-0.236,0-0.426,0.19-0.426,0.426c0,0.236,0.19,0.427,0.426,0.427h6.392c1.412,0,2.557,1.145,2.557,2.557s-1.146,2.557-2.557,2.557h-0.426c-0.236,0-0.426,0.19-0.426,0.426s0.19,0.427,0.426,0.427h0.426c1.883,0,3.41-1.526,3.41-3.409S12.548,11.44,10.665,11.44"
            fill="white"
            stroke="white"
            stroke-width="0"
            transform="scale(2)"
            ></path>
        </svg>
      </a></li>
    </ul> 
  </nav>
  <router-view></router-view>
</template>


<style>
* {
  margin: 0;
  padding: 0;
}

body {
  font-family: 'Helvetica'
}

#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: left;
  color: #2c3e50;
}

.navbar {
  display: flex;
  align-items: center; 
}

.icon-container {
  display: flex;
  justify-content: center; 
  align-items: center; 
  margin-right: 1rem;
}

nav a {
  font-weight: bold;
  color: #752d2d;
}

nav a.router-link-exact-active {
  padding: 2rem;
  color: #42b983;
}

ul {
  list-style-type: none;
  margin: 0;
  padding: 0;
  overflow: hidden;
  background-color:#303030;
  box-shadow: 0px 2px 7px 1px #000000;
}

li {
  float: left;
  margin-left: 1rem;
  margin-right: 1rem;
}

li a {
  display: block;
  color: white;
  text-align: center;
  padding: 14px 16px;
  text-decoration: none;
}

li a:hover svg path {
  stroke: rgb(162, 177, 26);
  fill: rgb(162, 177, 26);
}

.home{
  padding: 1rem;
}
</style>
