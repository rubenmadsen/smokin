<template>
  <div>
    <h1>Info View</h1>
    <p>This is the info view.</p>
    <p>Received Data: {{ receivedData }}</p>
  </div>
</template>

<script>
export default {
  name: 'InfoView',
  computed: {
    receivedData() {
      return this.$route.query.data || 'No data received';
    }
  }
};
</script>
