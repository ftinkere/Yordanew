<script setup>
import Layout from "../../Layout.vue";
import { Link, router, usePage } from "@inertiajs/vue3";
import { provide } from "vue";

const language = usePage().props.language
const userId = usePage().props.auth?.user?.id

provide('language', language)

function to(page) {
    const url = new URL(window.location.href);
    url.searchParams.set('page', page);
    return url.toString();
}

</script>

<template>
  <Layout>
    <template #top-left>
      <span class="font-yordan">Словарь&nbsp;</span>
    </template>

    <template v-if="userId" #top-right>
      <Link :href="`/languages/${language.id}/dictionary/create`">
        <UButton variant="soft" color="success">Добавить слово</UButton>
      </Link>
    </template>

    <div class="flex flex-col gap-2">
      <UCard v-for="article in language.articles" :key="article.id">
        <template #header>
          <div class="flex flex-col">
            <Link :href="`/dictionary/${article.id}`">
              <UButton variant="link" class="text-neutral-200">
                <span>
                  <span>{{ article.lemma }}</span>&nbsp;
                  <span v-if="article.transcription">/{{article.transcription }}/</span>
                </span>
              </UButton>
            </Link>
            <span v-if="article.adaptation && article.adaptation !== article.lemma"
                  class="ps-[10px] text-neutral-500">{{ article.adaptation }}</span>
          </div>
        </template>
        <div class="ps-[10px] flex flex-col gap-1">
          <div class="flex flex-row gap-2" v-for="lexeme in article.lexemes" :key="lexeme.id">
            <span>{{ lexeme.path.join('.') }}</span>
            <div class="ProseMirror line-clamp-1 first-line-fixed" v-html="lexeme.description"></div>
          </div>
        </div>
      </UCard>
      
      <UPagination
          variant="soft"
          active-variant="soft"
          :page="language.page"
          :defaultPage="language.page"
          :total="language.totalCount"
          :items-per-page="language.pageSize"
          show-edges
          @update:page="(newPage) => router.get(to(newPage))"
      />
    </div>
  </Layout>
</template>

<style scoped>
.first-line-fixed {
    overflow: hidden; /* скрываем всё, что выходит за рамки блока */
    line-height: 1.4em; /* высота одной строки */
    max-height: 1.4em; /* ровно одна строка */
    white-space: pre-line; /* сохраняем <br> и \n */
}
</style>