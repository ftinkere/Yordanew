<script setup>
import Layout from "../../Layout.vue";
import { Link, usePage } from "@inertiajs/vue3";
import { ref, computed } from "vue";

const userId = usePage().props.auth?.user?.id

const publishedLanguages = usePage().props.publishedLanguages;
const ownLanguages = usePage().props.ownLanguages;

const isOwn = ref(false);

const languages = computed(() => isOwn.value ? ownLanguages : publishedLanguages);
</script>

<template>
  <Layout>
    <template v-if="userId" #top-right>
      <Link href="/languages/create">
        <UButton
          icon="i-lucide-plus"
          color="primary"
          variant="soft"
          class="rounded-full">
          Создать язык
        </UButton>
      </Link>
    </template>

    <!-- Page Header with controls -->
    <div class="flex flex-col gap-6">
      <!-- Title and Filter Section -->
      <div class="bg-white/5 backdrop-blur-xl border border-white/10 rounded-3xl p-6 shadow-xl shadow-black/20">
        <div class="flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4">
          <div>
            <h1 class="text-4xl font-bold bg-linear-to-r from-amber-400 to-orange-400 bg-clip-text text-transparent mb-2">
              Языки
            </h1>
            <p class="text-white/70">
              {{ isOwn ? 'Мои языки' : 'Все опубликованные языки' }}
            </p>
          </div>

          <!-- Filter Toggle -->
          <div v-if="userId" class="flex items-center gap-3">
            <UButton
              :variant="!isOwn ? 'solid' : 'ghost'"
              :color="!isOwn ? 'primary' : 'white'"
              @click="isOwn = false"
              class="rounded-full transition-all duration-300"
              :class="{ 'scale-105': !isOwn }">
              <div class="flex items-center gap-2">
                <UIcon name="i-lucide-globe" class="size-4" />
                <span>Все языки</span>
              </div>
            </UButton>

            <UButton
              :variant="isOwn ? 'solid' : 'ghost'"
              :color="isOwn ? 'primary' : 'white'"
              @click="isOwn = true"
              class="rounded-full transition-all duration-300"
              :class="{ 'scale-105': isOwn }">
              <div class="flex items-center gap-2">
                <UIcon name="i-lucide-user" class="size-4" />
                <span>Мои языки</span>
              </div>
            </UButton>
          </div>
        </div>
      </div>

      <!-- Languages Grid -->
      <div v-if="languages.length > 0" class="grid grid-cols-1 md:grid-cols-2 gap-4">
        <Link
          v-for="language in languages"
          :key="language.id"
          :href="`/languages/${language.id}`"
          class="group">
          <div class="h-full bg-white/5 hover:bg-white/10 backdrop-blur-xl
                      border border-white/10 hover:border-white/20
                      rounded-3xl p-6 shadow-lg shadow-black/20
                      transition-all duration-300 hover:scale-[1.02] hover:shadow-xl hover:shadow-amber-500/10">
            <!-- Language Header -->
            <div class="flex items-start justify-between gap-3 mb-3">
              <div class="flex-1 min-w-0">
                <h2 class="text-2xl font-bold text-white group-hover:text-amber-400 transition-colors truncate">
                  {{ language.name }}
                </h2>
                <p v-if="language.autoName" class="text-sm text-white/60 truncate">
                  {{ language.autoName }}
                </p>
              </div>

              <!-- Published Badge -->
              <div v-if="language.isPublished" class="flex-shrink-0">
                <div class="px-3 py-1 rounded-full bg-green-500/20 border border-green-400/30">
                  <div class="flex items-center gap-1.5">
                    <div class="i-lucide-check w-3 h-3 text-green-400" />
                    <span class="text-xs text-green-400 font-medium">Опубликован</span>
                  </div>
                </div>
              </div>
              <div v-else class="flex-shrink-0">
                <div class="px-3 py-1 rounded-full bg-gray-500/20 border border-gray-400/30">
                  <div class="flex items-center gap-1.5">
                    <div class="i-lucide-lock w-3 h-3 text-gray-400" />
                    <span class="text-xs text-gray-400 font-medium">Приватный</span>
                  </div>
                </div>
              </div>
            </div>

            <!-- Language Description -->
            <div v-if="language.description" class="text-white/70 text-sm line-clamp-3 mb-4"
                 v-html="language.description">
            </div>

            <!-- Language Meta Info -->
            <div class="flex items-center gap-4 text-xs text-white/50">
              <div class="flex items-center gap-1.5">
                <div class="i-lucide-user w-3 h-3" />
                <span>{{ language.authorName }}</span>
              </div>
              <div v-if="language.articlesCount" class="flex items-center gap-1.5">
                <div class="i-lucide-book-a w-3 h-3" />
                <span>{{ language.articlesCount }} статей</span>
              </div>
            </div>

            <!-- Hover Arrow -->
            <div class="mt-4 flex items-center gap-2 text-amber-400 opacity-0 group-hover:opacity-100 transition-opacity">
              <span class="text-sm font-medium">Открыть язык</span>
              <div class="i-lucide-arrow-right w-4 h-4 group-hover:translate-x-1 transition-transform" />
            </div>
          </div>
        </Link>
      </div>

      <!-- Empty State -->
      <div v-else class="bg-white/5 backdrop-blur-xl border border-white/10 rounded-3xl p-12 shadow-xl shadow-black/20 text-center">
        <div class="flex flex-col items-center gap-4">
          <div class="w-16 h-16 rounded-full bg-white/10 flex items-center justify-center">
            <div class="i-lucide-inbox w-8 h-8 text-white/50" />
          </div>
          <div>
            <h3 class="text-xl font-semibold text-white mb-2">
              {{ isOwn ? 'У вас пока нет языков' : 'Языки не найдены' }}
            </h3>
            <p class="text-white/60 mb-6">
              {{ isOwn ? 'Создайте свой первый язык и начните работу над словарем' : 'Пока никто не опубликовал ни одного языка' }}
            </p>
            <Link v-if="isOwn && userId" href="/languages/create">
              <UButton
                icon="i-lucide-plus"
                color="primary"
                size="lg"
                class="rounded-full">
                Создать первый язык
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