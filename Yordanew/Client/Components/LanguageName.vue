<script setup>
  const { language } = defineProps({ language: Object });
</script>

<template>
<span>
  <span>{{ language.autoName }}&nbsp;</span> 
  <span class="break-after-all">/{{language.autoNameTranscription }}/</span> 
  — 
  <span>{{ language.name }}</span>
</span>
</template>

<style scoped>

</style>