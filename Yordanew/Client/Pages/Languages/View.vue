<script setup>
import { Link, usePage } from "@inertiajs/vue3";
import { provide } from "vue";
import Layout from "../../Layout.vue";

const language = usePage().props.language;
const userId = usePage().props.auth?.user?.id;

provide('language', language);

const date = (new Date(language.createdAt)).toLocaleDateString(undefined, {
    day: 'numeric',
    month: 'long',
    year: 'numeric'
});
</script>

<template>
  <Layout>
    <template v-if="language.author.id === userId" #top-right>
      <Link :href="`/languages/${language.id}/edit`">
        <UButton variant="soft" color="primary">Редактировать</UButton>
      </Link>
    </template>
    
    <div class="flex flex-col gap-2 items-center w-full">
      <span v-if="language" class="text-2xl text-center">
        {{ language.autoName }} /{{ language.autoNameTranscription }}/ — {{ language.name }}
      </span>
      
      <div class="flex flex-row items-center justify-end gap-4 h-fit w-full">
        <div class="flex flex-row items-center px-4 h-8 rounded-full bg-black/20">
          С нами с {{ date }}
        </div>
        
        <Link :href="`/profile/${language.author.id}`" class="cursor-pointer">
          <div class="flex flex-row gap-2 items-center h-8 ps-4 rounded-full bg-black/20 hover:bg-black/40">
            {{ language.author.displayName ?? language.author.userName }}
            <UAvatar :src="`/uploaded-files/avatars/${language.author.id}`" />
          </div>
        </Link>
      </div>
      
      <div class="ProseMirror self-start" v-html="language.description" />
    </div>
  </Layout>
</template>

<style scoped>

</style>