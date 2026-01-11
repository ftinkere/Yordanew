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
    <template v-if="userId === language.author.id" #top-right>
      <Link :href="`/languages/${language.id}/dictionary/create`">
        <UButton
          icon="i-lucide-plus"
          variant="soft"
          color="primary">
          Добавить слово
        </UButton>
      </Link>
    </template>

    <div class="flex flex-col gap-6 w-full">
      <!-- Page Header -->
      <div class="bg-white/5 backdrop-blur-xl border border-white/10 rounded-3xl p-6 shadow-xl shadow-black/20">
        <div class="flex items-center gap-4">
          <div class="w-14 h-14 rounded-full bg-linear-to-br from-green-500/20 to-emerald-500/20 border border-green-400/30 flex items-center justify-center">
            <UIcon name="i-lucide-book-a" class="size-7 text-green-400" />
          </div>
          <div>
            <h1 class="text-3xl font-bold text-white mb-1">Словарь</h1>
            <p class="text-white/60">
              {{ language.totalCount }} {{ language.totalCount === 1 ? 'статья' : language.totalCount < 5 ? 'статьи' : 'статей' }}
            </p>
          </div>
        </div>
      </div>

      <!-- Articles List -->
      <div v-if="language.articles && language.articles.length > 0" class="flex flex-col gap-4">
        <Link
          v-for="article in language.articles"
          :key="article.id"
          :href="`/dictionary/${article.id}`"
          class="group">
          <div class="bg-white/5 hover:bg-white/10 backdrop-blur-xl
                      border border-white/10 hover:border-white/20
                      rounded-3xl p-6 shadow-lg shadow-black/20
                      transition-all duration-300 hover:scale-[1.01] hover:shadow-xl hover:shadow-green-500/10">
            <!-- Article Header -->
            <div class="flex flex-col gap-2 mb-4">
              <div class="flex items-baseline gap-2">
                <h2 class="text-2xl font-bold text-white group-hover:text-green-400 transition-colors">
                  {{ article.lemma }}
                </h2>
                <span v-if="article.transcription" class="text-green-400/80 text-lg">
                  /{{ article.transcription }}/
                </span>
              </div>
              <p v-if="article.adaptation && article.adaptation !== article.lemma"
                 class="text-white/50 text-sm">
                {{ article.adaptation }}
              </p>
            </div>

            <!-- Lexemes Preview -->
            <div class="flex flex-col gap-2">
              <div
                v-for="lexeme in article.lexemes"
                :key="lexeme.id"
                class="flex gap-3 text-white/80">
                <span class="font-mono text-green-400 shrink-0">{{ lexeme.path }}</span>
                <div class="ProseMirror line-clamp-1 first-line-fixed flex-1" v-html="lexeme.description"></div>
              </div>
            </div>

            <!-- Hover Arrow -->
            <div class="mt-4 flex items-center gap-2 text-green-400 opacity-0 group-hover:opacity-100 transition-opacity">
              <span class="text-sm font-medium">Подробнее</span>
              <UIcon name="i-lucide-arrow-right" class="size-4 group-hover:translate-x-1 transition-transform" />
            </div>
          </div>
        </Link>
      </div>

      <!-- Empty State -->
      <div v-else class="bg-white/5 backdrop-blur-xl border border-white/10 rounded-3xl p-12 shadow-xl shadow-black/20 text-center">
        <div class="flex flex-col items-center gap-4">
          <div class="w-16 h-16 rounded-full bg-white/10 flex items-center justify-center">
            <UIcon name="i-lucide-book-open" class="size-8 text-white/50" />
          </div>
          <div>
            <h3 class="text-xl font-semibold text-white mb-2">
              Словарь пуст
            </h3>
            <p class="text-white/60 mb-6">
              {{ userId === language.author.id
                ? 'Добавьте первое слово в словарь вашего языка'
                : 'В этом словаре пока нет статей' }}
            </p>
            <Link v-if="userId === language.author.id" :href="`/languages/${language.id}/dictionary/create`">
              <UButton
                icon="i-lucide-plus"
                color="primary"
                size="lg">
                Добавить первое слово
              </UButton>
            </Link>
          </div>
        </div>
      </div>

      <!-- Pagination -->
      <div v-if="language.totalCount > language.pageSize" class="flex justify-center">
        <UPagination
          :page="language.page"
          :total="language.totalCount"
          :items-per-page="language.pageSize"
          show-edges
          @update:page="(newPage) => router.get(to(newPage))"
        />
      </div>
    </div>
  </Layout>
</template>

<style scoped>
.first-line-fixed {
    overflow: hidden;
    line-height: 1.4em;
    max-height: 1.4em;
    white-space: pre-line;
}
</style>