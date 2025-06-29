<script setup>
import Layout from "../../Layout.vue";
import { Link, usePage } from "@inertiajs/vue3";
import { provide } from "vue";

const language = usePage().props.language
const userId = usePage().props.auth?.user?.id

provide('language', language)

</script>

<template>
  <Layout>
    <template #top-left>
      <span v-if="language">
        <span class="font-yordan">Словарь</span> {{ language.autoName }} /{{ language.autoNameTranscription }}/ — {{ language.name }}
      </span>
    </template>

    <template v-if="userId" #top-right>
      <Link :href="`/languages/${language.id}/dictionary/create`">
        <UButton variant="soft" color="success">Добавить слово</UButton>
      </Link>
    </template>
    
    <div>Словарь</div>
  </Layout>
</template>

<style scoped>

</style>