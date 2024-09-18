<template>
    <div class="mainContainer non-selectable">
        <div class="itemContainer">
            <p class="value">{{ money + " "}} </p>
            <p class="unit">{{ moneyUnit }}</p>
        </div>
        <div class="itemContainer non-selectable">
            <p class="value">{{ life + " "}} </p>
            <p class="unit">{{ lifeUnit }}</p>
        </div>
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
            moneyUnit:"kr",
            lifeUnit:"min",
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
            let smokedCigarettes = (totalOriginalPuffs - totalRealPuffs) / 20;
            let pacPrice = 67;
            this.money = (pacPrice*(smokedCigarettes / 20));
            this.life = smokedCigarettes * 11;
            if (this.money > 1000)
                this.moneyUnit = "tkr";
            if (this.life > 60 * 24)
                this.lifeUnit = "days";
            else if (this.life > 60)
                this.lifeUnit = "hours";
            this.money = Math.round(this.money);
            this.life = Math.round(this.life);

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
        margin: 0px;
        text-align: center;
        height: 150px;
    }
    .itemContainer{
        position: relative;
        display: inline-block;
        border: 5px solid var(--primary-color);
        align-items: center;
        align-content: center;
        justify-content: center;
        margin:auto;
        aspect-ratio: 1;
        height: 100%;
        background-color: var(--text-color);
        color: var(--background-color);
        border-radius: 999px;
        margin: 0.5rem;
    }
    .value{
        font-family: var(--font-family-serif);
        font-size: 4em;
        color: var(--background-color);
    }
    .unit{
        font-family: var(--font-family-sans);
        color: var(--background-color);

    }
  </style>
  