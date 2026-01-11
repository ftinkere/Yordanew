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
    <template v-if="userId === language.author.id" #top-right>
      <Link :href="`/grammatic/${language.id}/edit`">
        <UButton
          icon="i-lucide-pencil"
          variant="soft"
          color="primary">
          Редактировать
        </UButton>
      </Link>
    </template>

    <div class="flex flex-col gap-6 w-full">
      <!-- Page Header -->
      <div class="bg-white/5 backdrop-blur-xl border border-white/10 rounded-3xl p-6 shadow-xl shadow-black/20">
        <div class="flex items-center gap-4">
          <div class="w-14 h-14 rounded-full bg-linear-to-br from-purple-500/20 to-violet-500/20 border border-purple-400/30 flex items-center justify-center">
            <UIcon name="i-lucide-cpu" class="size-7 text-purple-400" />
          </div>
          <div>
            <h1 class="text-3xl font-bold text-white mb-1">Грамматика</h1>
            <p class="text-white/60">
              Части речи и грамматические категории
            </p>
          </div>
        </div>
      </div>

      <!-- Parts of Speech -->
      <div v-if="partsOfSpeech && partsOfSpeech.length > 0" class="flex flex-col gap-4">
        <div
          v-for="pos in partsOfSpeech"
          :key="pos.id"
          class="bg-white/5 backdrop-blur-xl border border-white/10 rounded-3xl p-6 shadow-xl shadow-black/20">
          <!-- Part of Speech Header -->
          <div class="flex items-start gap-4 mb-4 pb-4 border-b border-white/10">
            <UIcon name="i-lucide-speech" class="size-6 text-purple-400 shrink-0 mt-1" />
            <div class="flex-1 min-w-0">
              <h2 class="text-2xl font-bold text-white mb-1">{{ pos.name }}</h2>
              <p class="text-purple-400/80 text-sm font-mono">{{ pos.code }}</p>
              <div v-if="pos.description" class="ProseMirror text-white/70 text-sm mt-2" v-html="pos.description"></div>
            </div>
          </div>

          <!-- Categories -->
          <div v-if="pos.categories && pos.categories.length > 0" class="flex flex-col gap-4">
            <div
              v-for="category in pos.categories"
              :key="category.id"
              class="bg-white/5 border border-white/10 rounded-2xl p-4">
              <!-- Category Header -->
              <div class="flex items-start gap-3 mb-3">
                <UIcon name="i-lucide-tag" class="size-5 text-blue-400 shrink-0 mt-0.5" />
                <div class="flex-1 min-w-0">
                  <h3 class="text-lg font-semibold text-white">{{ category.name }}</h3>
                  <p class="text-blue-400/80 text-xs font-mono">{{ category.code }}</p>
                  <div v-if="category.description" class="ProseMirror text-white/70 text-sm mt-1" v-html="category.description"></div>
                </div>
              </div>

              <!-- Features -->
              <div v-if="category.features && category.features.length > 0" class="flex flex-wrap gap-2 mt-3 pt-3 border-t border-white/10">
                <div
                  v-for="feature in category.features"
                  :key="feature.id"
                  class="group relative">
                  <div class="px-3 py-1.5 rounded-full bg-white/5 border border-white/10 hover:border-white/20 transition-all">
                    <div class="flex items-center gap-2">
                      <span class="text-white text-sm font-medium">{{ feature.name }}</span>
                      <span class="text-white/50 text-xs font-mono">{{ feature.code }}</span>
                    </div>
                  </div>
                  <div v-if="feature.description"
                       class="absolute left-0 top-full mt-2 z-10 w-64 p-3 rounded-2xl
                              bg-black/90 backdrop-blur-xl border border-white/20
                              opacity-0 invisible group-hover:opacity-100 group-hover:visible
                              transition-all duration-200 pointer-events-none">
                    <div class="ProseMirror text-white/90 text-xs" v-html="feature.description"></div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Empty State -->
      <div v-else class="bg-white/5 backdrop-blur-xl border border-white/10 rounded-3xl p-12 shadow-xl shadow-black/20 text-center">
        <div class="flex flex-col items-center gap-4">
          <div class="w-16 h-16 rounded-full bg-white/10 flex items-center justify-center">
            <UIcon name="i-lucide-cpu" class="size-8 text-white/50" />
          </div>
          <div>
            <h3 class="text-xl font-semibold text-white mb-2">
              Грамматика не определена
            </h3>
            <p class="text-white/60 mb-6">
              {{ userId === language.author.id
                ? 'Добавьте части речи и грамматические категории для вашего языка'
                : 'В этом языке ещё не определена грамматическая структура' }}
            </p>
            <Link v-if="userId === language.author.id" :href="`/grammatic/${language.id}/edit`">
              <UButton
                icon="i-lucide-plus"
                color="primary"
                size="lg">
                Добавить части речи
              </UButton>
            </Link>
          </div>
        </div>
      </div>
    </div>
  </Layout>
</template>

<style scoped>

</style>