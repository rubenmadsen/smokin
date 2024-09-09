<template>
  <div>
      <button @click="handleClick">Test</button>
  </div>
</template>

<script>
import Backend from '@/services/backend';
export default {
  name: 'TestComponent',
  setup() {
      const handleClick = async () => {
            try {
              const toxinData = await Backend.getToxinData();
              toxinData.forEach(element => {
                console.log("Toxin:" + element.Name)
                console.log("Description:" + element.Description);
                console.log("");
              });
              //console.log('Toxin Data:', toxinData);
            } catch (error) {
              console.error('Error fetching toxin data:', error);
            }
      };

      return {
          handleClick,
      };
  },
};
</script>

<style lang="scss" scoped>

</style>
