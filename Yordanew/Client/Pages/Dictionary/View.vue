<script setup>
import Layout from "../../Layout.vue";
import { Link, usePage } from "@inertiajs/vue3";
import { provide, ref, onMounted, onUnmounted } from "vue";

const language = usePage().props.language
const article = usePage().props.article
provide('language', language)
const userId = usePage().props.auth?.user?.id

// Реф на последний элемент списка и флаг его видимости
const lastElement = ref(null);
const lastVisible = ref(true);

let observer = null;

// Функция, которую Vue вызовет для каждого рендера v-for
function setLastRef(el, index) {
    // сохраняем в lastElement только DOM-узел последнего по индексу
    if (index === article.lexemes.length - 1) {
        lastElement.value = el;
    }
}

// Настраиваем наблюдателя при монтировании
onMounted(() => {
    observer = new IntersectionObserver(
        ([entry]) => {
            // Когда последний элемент пересекается с viewport, entry.isIntersecting = true
            lastVisible.value = entry.isIntersecting;
        },
        {
            root: null,      // viewport
            threshold: 0.01  // срабатываем при хоть небольшом появлении на экране
        }
    );
    // Если lastElement уже в DOM — начать наблюдение
    if (lastElement.value) {
        observer.observe(lastElement.value);
    }
});

// И не забываем чистить
onUnmounted(() => {
    if (observer) observer.disconnect();
});
</script>

<template>
  <Layout>
    <template v-if="userId === language.authorId" #top-right>
      <Link :href="`/dictionary/${article.id}/edit`">
        <UButton variant="soft" color="primary">Редактировать</UButton>
      </Link>
    </template>
    
    <div class="flex flex-col gap-2 divide-y divide-neutral-700">
      <div class="flex flex-col">
        <span class="text-2xl">{{ article.lemma.content }} <span v-if="article.lemma.transcription">/{{ article.lemma.transcription }}/</span></span>
        <span v-if="article.lemma.adaptation && article.lemma.adaptation !== article.lemma.content" class="text-neutral-400 dark:text-neutral-700 text-sm">{{ article.lemma.adaptation }}</span>
      </div>

      <div v-if="!lastVisible" class="p-4 flex flex-col gap-1">
        <div v-for="lexeme in article.lexemes" :key="lexeme.id">
          <div class="font-bold float-left me-2">
            {{ lexeme.path?.join('.') }}
          </div>
          <div class="ProseMirror line-clamp-1 first-line-fixed" v-html="lexeme.description.content"></div>
        </div>
      </div>
      
      <div class="p-4 flex flex-col gap-2 divide-y divide-neutral-700">
        <div v-for="(lexeme, index) in article.lexemes" :key="lexeme.id" :ref="el => setLastRef(el, index)">
          <div class="font-bold float-left me-2">
            {{ lexeme.path?.join('.') }}
          </div>
          <div class="ProseMirror" v-html="lexeme.description.content"></div>
        </div>
      </div>
    </div>
  </Layout>
</template>

<style scoped>
  .first-line-fixed {
      overflow: hidden;              /* скрываем всё, что выходит за рамки блока */
      line-height: 1.4em;            /* высота одной строки */
      max-height: 1.4em;             /* ровно одна строка */
      white-space: pre-line;         /* сохраняем <br> и \n */
  }
</style>