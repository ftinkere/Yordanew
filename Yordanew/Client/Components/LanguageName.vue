<script setup>
  const { language } = defineProps({ language: Object });
</script>

<template>
  <div class="inline-flex items-center gap-2 px-4 py-2 rounded-full
              bg-white/5 border border-white/10
              backdrop-blur-sm">
    <UIcon name="i-lucide-languages" class="size-4 text-amber-400 shrink-0" />
    <div class="flex items-baseline gap-2 text-sm">
      <span v-if="language.autoName" class="text-white/90 font-medium">{{ language.autoName }}</span>
      <span v-if="language.autoNameTranscription" class="text-amber-400/80 text-xs">/{{ language.autoNameTranscription }}/</span>
      <span v-if="language.autoName || language.autoNameTranscription" class="text-white/50">—</span>
      <span class="text-white/70">{{ language.name }}</span>
    </div>
  </div>
</template>

<style scoped>

</style>