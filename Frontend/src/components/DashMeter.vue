<template>
    <div class="mainContainer">
        <p>{{ money + " kr" }}</p>
    </div>
    <div class="mainContainer">
        {{ life + " min" }}
    </div>
  </template>
  
  <script>
  import Backend from '@/services/backend';
  export default {
    name: 'DashMeter',
    data(){
        return{
            money:0,
            life:0,
            values:0,
            username:'ruben'
        }
    },
    setup() {
        const log = async () => {
              console.log("Dash meter" + this.values)
        };
        return {
            log,
        };
    },
    methods: {
        calculateDifferenceInDates(start, end){
            const difference = end - start;
            let dayDifference = Math.floor(difference / (1000 * 60 * 60 * 24));
            return dayDifference;
        },
        recalculate(){
            const firstDate = new Date(this.values[0].date);
            let dayDifference = this.calculateDifferenceInDates(firstDate, new Date());
            
            let totalOriginalPuffs = dayDifference * this.values[0].amount;
            let totalRealPuffs = 0;
            console.log("Days:" + dayDifference + " yields " + totalOriginalPuffs + " puffs");

            // Calculate real
            let previous = this.values[0];
            for (let i = 1 ; i<this.values.length-1; i++){
                const current = this.values[i];
                let diff = this.calculateDifferenceInDates(new Date(previous.date), new Date(current.date));
                totalRealPuffs += (diff * previous.amount);
                previous = current;

            }
            let diff = this.calculateDifferenceInDates(new Date(previous.date), new Date());
            totalRealPuffs += (diff * previous.amount);
            console.log("Actual puffs:" + totalRealPuffs);
        },
        async getData(){
            try{
                const trackedData = await Backend.getTrackedUserData(this.username);
                this.values = trackedData.Value;
                console.log('Tracked Data:', this.values);
                this.recalculate();
            } 
            catch (error) {
                console.error('Error fetching tracked data:', error);
            }
        },
        setUsername(newUsername){
            this.username = newUsername;
        }
    },
    mounted(){
        this.getData();
    }
  };
  </script>
  
  <style scoped>
    .mainContainer{
        position: relative;
        width: 150px;
        height: 150px;
        background-color: rgba(255, 255, 255, 0.158);
    }
  </style>
  