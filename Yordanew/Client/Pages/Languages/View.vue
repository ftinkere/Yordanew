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

const isAuthor = language.author.id === userId;
</script>

<template>
  <Layout>
    <template v-if="isAuthor" #top-right>
      <Link :href="`/languages/${language.id}/edit`">
        <UButton
          icon="i-lucide-pencil"
          variant="soft"
          color="primary">
          Редактировать
        </UButton>
      </Link>
    </template>

    <div class="flex flex-col gap-6 w-full">
      <!-- Language Header Card -->
      <div class="bg-white/5 backdrop-blur-xl border border-white/10 rounded-3xl p-8 shadow-xl shadow-black/20">
        <div class="flex flex-col gap-4">
          <!-- Language Title -->
          <div class="text-center space-y-2">
            <h1 class="text-4xl font-bold text-white">
              {{ language.name }}
            </h1>
            <div v-if="language.autoName || language.autoNameTranscription" class="flex items-center justify-center gap-2 text-xl text-white/70">
              <span v-if="language.autoName">{{ language.autoName }}</span>
              <span v-if="language.autoNameTranscription" class="text-amber-400/80">
                /{{ language.autoNameTranscription }}/
              </span>
            </div>
          </div>

          <!-- Language Meta Info -->
          <div class="flex flex-wrap items-center justify-center gap-3 pt-4 border-t border-white/10">
            <!-- Author Info -->
            <Link :href="`/profile/${language.author.id}`">
              <div class="group flex items-center gap-2 px-4 py-2 rounded-full
                          bg-white/5 hover:bg-white/10
                          border border-white/10 hover:border-white/20
                          transition-all duration-300 hover:scale-105">
                <UAvatar
                  :src="`/uploaded-files/avatars/${language.author.id}`"
                  :alt="language.author.displayName ?? language.author.userName"
                  size="xs"
                  class="ring-2 ring-amber-400/30" />
                <UIcon name="i-lucide-user" class="size-4 text-amber-400" />
                <span class="text-white/90 group-hover:text-white font-medium">
                  {{ language.author.displayName ?? language.author.userName }}
                </span>
              </div>
            </Link>

            <!-- Created Date -->
            <div class="flex items-center gap-2 px-4 py-2 rounded-full
                        bg-white/5 border border-white/10">
              <UIcon name="i-lucide-calendar" class="size-4 text-blue-400" />
              <span class="text-white/70 text-sm">
                С нами с {{ date }}
              </span>
            </div>

            <!-- Published Status -->
            <div v-if="language.isPublished" class="flex items-center gap-2 px-4 py-2 rounded-full
                        bg-green-500/20 border border-green-400/30">
              <UIcon name="i-lucide-check-circle" class="size-4 text-green-400" />
              <span class="text-green-400 text-sm font-medium">
                Опубликован
              </span>
            </div>
            <div v-else class="flex items-center gap-2 px-4 py-2 rounded-full
                        bg-gray-500/20 border border-gray-400/30">
              <UIcon name="i-lucide-lock" class="size-4 text-gray-400" />
              <span class="text-gray-400 text-sm font-medium">
                Приватный
              </span>
            </div>
          </div>
        </div>
      </div>

      <!-- Description Card -->
      <div v-if="language.description"
           class="bg-white/5 backdrop-blur-xl border border-white/10 rounded-3xl p-8 shadow-xl shadow-black/20">
        <div class="flex items-center gap-2 mb-4 pb-4 border-b border-white/10">
          <UIcon name="i-lucide-file-text" class="size-5 text-blue-400" />
          <h2 class="text-xl font-semibold text-white">Описание</h2>
        </div>
        <div class="ProseMirror text-white/90" v-html="language.description" />
      </div>

      <!-- Quick Actions Card (for author) -->
      <div v-if="isAuthor"
           class="bg-white/5 backdrop-blur-xl border border-white/10 rounded-3xl p-6 shadow-xl shadow-black/20">
        <div class="flex items-center gap-2 mb-4 pb-4 border-b border-white/10">
          <UIcon name="i-lucide-zap" class="size-5 text-amber-400" />
          <h2 class="text-xl font-semibold text-white">Быстрые действия</h2>
        </div>

        <div class="grid grid-cols-1 sm:grid-cols-3 gap-3">
          <Link :href="`/languages/${language.id}/dictionary`">
            <div class="group relative overflow-hidden rounded-2xl
                        bg-white/5 hover:bg-white/10
                        border border-white/10 hover:border-white/20
                        p-4 transition-all duration-300 hover:scale-105 hover:shadow-lg hover:shadow-green-500/10">
              <div class="flex flex-col gap-2">
                <UIcon name="i-lucide-book-a" class="size-6 text-green-400" />
                <span class="text-white font-medium">Словарь</span>
                <span class="text-white/60 text-sm">Управление статьями</span>
              </div>
            </div>
          </Link>

          <Link :href="`/grammatic/${language.id}`">
            <div class="group relative overflow-hidden rounded-2xl
                        bg-white/5 hover:bg-white/10
                        border border-white/10 hover:border-white/20
                        p-4 transition-all duration-300 hover:scale-105 hover:shadow-lg hover:shadow-purple-500/10">
              <div class="flex flex-col gap-2">
                <UIcon name="i-lucide-cpu" class="size-6 text-purple-400" />
                <span class="text-white font-medium">Грамматика</span>
                <span class="text-white/60 text-sm">Части речи и категории</span>
              </div>
            </div>
          </Link>

          <Link :href="`/languages/${language.id}/edit`">
            <div class="group relative overflow-hidden rounded-2xl
                        bg-white/5 hover:bg-white/10
                        border border-white/10 hover:border-white/20
                        p-4 transition-all duration-300 hover:scale-105 hover:shadow-lg hover:shadow-amber-500/10">
              <div class="flex flex-col gap-2">
                <UIcon name="i-lucide-settings" class="size-6 text-amber-400" />
                <span class="text-white font-medium">Настройки</span>
                <span class="text-white/60 text-sm">Редактировать язык</span>
              </div>
            </div>
          </Link>
        </div>
      </div>

      <!-- Info Card (for non-authors) -->
      <div v-else
           class="bg-blue-500/10 backdrop-blur-xl border border-blue-400/20 rounded-3xl p-6">
        <div class="flex gap-4">
          <div class="w-10 h-10 rounded-full bg-blue-500/20 flex items-center justify-center shrink-0">
            <UIcon name="i-lucide-info" class="size-5 text-blue-400" />
          </div>
          <div>
            <h3 class="text-lg font-semibold text-blue-400 mb-2">О языке</h3>
            <p class="text-white/70 text-sm">
              Этот язык создан пользователем {{ language.author.displayName ?? language.author.userName }}.
              Вы можете просмотреть словарь и грамматику этого языка.
            </p>
            <div class="flex gap-2 mt-4">
              <Link :href="`/languages/${language.id}/dictionary`">
                <UButton
                  icon="i-lucide-book-a"
                  variant="soft"
                  color="primary"
                  size="sm">
                  Словарь
                </UButton>
              </Link>
              <Link :href="`/grammatic/${language.id}`">
                <UButton
                  icon="i-lucide-cpu"
                  variant="soft"
                  color="primary"
                  size="sm">
                  Грамматика
                </UButton>
              </Link>
            </div>
          </div>
        </div>
      </div>
    </div>
  </Layout>
</template>

<style scoped>

</style>