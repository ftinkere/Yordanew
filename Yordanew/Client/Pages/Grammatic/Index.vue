<script setup>
import Layout from "../../Layout.vue";
import { computed, provide } from "vue";
import { Link, usePage } from "@inertiajs/vue3";

const language = computed(() => usePage().props.language)
const partsOfSpeech = computed(() => language.value.partsOfSpeech)
const userId = usePage().props.auth?.user?.id

provide('language', language)

</script>

<template>
  <Layout>
    <template #top-left>
      <span class="font-yordan">Грамматика</span>
    </template>

    <template v-if="userId === language.author.id" #top-right>
      <Link :href="`/grammatic/${language.id}/edit`">
        <UButton variant="soft" color="primary">Редактировать</UButton>
      </Link>
    </template>
    
    {{ partsOfSpeech }}
  </Layout>
</template>

<style scoped>

</style>