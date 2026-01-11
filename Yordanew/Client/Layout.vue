<script setup>
import { inject, ref } from "vue";
import { Link, usePage } from "@inertiajs/vue3";
import { onClickOutside } from '@vueuse/core'
import LanguageName from "./Components/LanguageName.vue";

const auth = usePage().props.auth;
const language = inject('language');

const isMobileShowAside = ref(false);
const asideRef = ref(null);

// Close sidebar when clicking outside on mobile
onClickOutside(asideRef, () => {
  if (isMobileShowAside.value) {
    isMobileShowAside.value = false;
  }
});

</script>

<template>
  <UApp>
    <!-- Static gradient background -->
    <div class="fixed inset-0 -z-10 bg-linear-to-br from-neutral-950 via-amber-950/20 to-stone-950"></div>

    <div class="relative flex flex-row items-stretch min-h-dvh gap-4 p-4">
      <!-- Floating sidebar with glass morphism -->
      <aside
        ref="asideRef"
        class="max-sm:fixed sm:sticky
               max-sm:top-0 max-sm:left-0 max-sm:bottom-0
               sm:top-4 sm:left-4
               flex flex-col gap-3 w-72
               max-sm:h-screen sm:max-h-[calc(100dvh-2rem)]
               bg-white/10 dark:bg-black/20 backdrop-blur-xl
               border border-white/20 dark:border-white/10
               max-sm:rounded-r-3xl sm:rounded-3xl p-4 shadow-2xl shadow-black/50
               transition-all duration-300
               max-sm:z-50"
        :class="{
          'max-sm:translate-x-0': isMobileShowAside,
          'max-sm:-translate-x-full': !isMobileShowAside
        }">

        <!-- Logo -->
        <Link href="/" prefetch>
          <div class="group relative overflow-hidden rounded-full bg-linear-to-r from-amber-500/20 to-orange-500/20
                      hover:from-amber-500/30 hover:to-orange-500/30
                      border border-amber-400/30 hover:border-amber-400/50
                      transition-all duration-300 hover:scale-105 hover:shadow-lg hover:shadow-amber-500/20">
            <div class="px-6 py-4 text-center font-yordan text-3xl font-bold
                        bg-linear-to-r from-amber-400 via-orange-400 to-amber-400
                        bg-clip-text text-transparent
                        group-hover:from-amber-300 group-hover:via-orange-300 group-hover:to-amber-300">
              Ëрдан
            </div>
          </div>
        </Link>

        <!-- Main navigation -->
        <nav class="flex flex-col gap-2">
          <Link href="/languages" prefetch>
            <div class="group relative overflow-hidden rounded-full
                        bg-white/5 hover:bg-white/10
                        border border-white/10 hover:border-white/20
                        transition-all duration-300 hover:scale-105 hover:shadow-lg hover:shadow-amber-500/10">
              <div class="flex items-center gap-3 px-4 py-3">
                <UIcon name="i-lucide-languages" class="size-5 text-amber-400 group-hover:text-amber-300 transition-colors" />
                <span class="text-white/90 group-hover:text-white font-medium">Все языки</span>
              </div>
            </div>
          </Link>
        </nav>

        <!-- Language navigation -->
        <div v-if="language" class="flex flex-col gap-2 pt-2 border-t border-white/10">
          <Link :href="`/languages/${language.id}`" prefetch>
            <div class="group relative overflow-hidden rounded-full
                        bg-white/5 hover:bg-white/10
                        border border-white/10 hover:border-white/20
                        transition-all duration-300 hover:scale-105 hover:shadow-lg hover:shadow-blue-500/10">
              <div class="flex items-center gap-3 px-4 py-3">
                <UIcon name="i-lucide-info" class="size-5 text-blue-400 group-hover:text-blue-300 transition-colors" />
                <span class="text-white/90 group-hover:text-white font-medium">О языке</span>
              </div>
            </div>
          </Link>

          <Link :href="`/languages/${language.id}/dictionary`" prefetch>
            <div class="group relative overflow-hidden rounded-full
                        bg-white/5 hover:bg-white/10
                        border border-white/10 hover:border-white/20
                        transition-all duration-300 hover:scale-105 hover:shadow-lg hover:shadow-green-500/10">
              <div class="flex items-center gap-3 px-4 py-3">
                <UIcon name="i-lucide-book-a" class="size-5 text-green-400 group-hover:text-green-300 transition-colors" />
                <span class="text-white/90 group-hover:text-white font-medium">Словарь</span>
              </div>
            </div>
          </Link>

          <Link :href="`/grammatic/${language.id}`" prefetch>
            <div class="group relative overflow-hidden rounded-full
                        bg-white/5 hover:bg-white/10
                        border border-white/10 hover:border-white/20
                        transition-all duration-300 hover:scale-105 hover:shadow-lg hover:shadow-purple-500/10">
              <div class="flex items-center gap-3 px-4 py-3">
                <UIcon name="i-lucide-cpu" class="size-5 text-purple-400 group-hover:text-purple-300 transition-colors" />
                <span class="text-white/90 group-hover:text-white font-medium">Грамматика</span>
              </div>
            </div>
          </Link>
        </div>

        <!-- User section -->
        <div class="mt-auto pt-4 border-t border-white/10 flex flex-col gap-2">
          <Link v-if="auth" href="/profile" prefetch>
            <div class="flex items-center gap-3 px-4 py-3">
              <UAvatar
                :src="`/uploaded-files/avatars/${auth.user?.id}`"
                :alt="auth.user?.displayName ?? auth.user?.userName"
                size="sm"
                class="ring-2 ring-amber-400/30 group-hover:ring-amber-400/50 transition-all" />
              <span class="text-white/90 group-hover:text-white font-medium truncate">
                {{ auth.user?.displayName ?? auth.user?.userName }}
              </span>
            </div>
          </Link>

          <Link v-if="auth" href="/logout">
            <div class="group relative overflow-hidden rounded-full
                        bg-red-500/10 hover:bg-red-500/20
                        border border-red-400/30 hover:border-red-400/50
                        transition-all duration-300 hover:scale-105">
              <div class="flex items-center gap-3 px-4 py-3">
                <UIcon name="i-lucide-log-out" class="size-5 text-red-400 group-hover:text-red-300 transition-colors" />
                <span class="text-red-400 group-hover:text-red-300 font-medium">Выйти</span>
              </div>
            </div>
          </Link>

          <Link v-else href="/login" prefetch>
            <div class="group relative overflow-hidden rounded-full
                        bg-linear-to-r from-amber-500/20 to-orange-500/20
                        hover:from-amber-500/30 hover:to-orange-500/30
                        border border-amber-400/30 hover:border-amber-400/50
                        transition-all duration-300 hover:scale-105 hover:shadow-lg hover:shadow-amber-500/20">
              <div class="flex items-center gap-3 px-4 py-3">
                <UIcon name="i-lucide-log-in" class="size-5 text-amber-400 group-hover:text-amber-300 transition-colors" />
                <span class="text-amber-400 group-hover:text-amber-300 font-medium">Войти</span>
              </div>
            </div>
          </Link>
        </div>
      </aside>

      <!-- Main content -->
      <div class="flex flex-col gap-4 grow min-w-0">
        <!-- Floating header -->
        <header class="sticky top-4 z-40
                       flex flex-row justify-between items-center
                       px-6 py-3
                       bg-white/10 dark:bg-black/20 backdrop-blur-xl
                       border border-white/20 dark:border-white/10
                       rounded-full shadow-xl shadow-black/20
                       transition-all duration-300">
          <div class="flex flex-row items-center gap-3">
            <!-- Mobile menu button -->
            <UButton
              @click="isMobileShowAside = !isMobileShowAside"
              icon="i-lucide-menu"
              color="white"
              variant="ghost"
              class="sm:hidden rounded-full" />

            <slot name="top-left" />
            <LanguageName v-if="language" :language="language" class="max-sm:hidden" />
          </div>

          <div class="justify-self-end">
            <slot name="top-right"/>
          </div>
        </header>

        <!-- Content container with island design -->
        <main class="flex flex-col gap-6 px-2 pb-6 max-w-5xl w-full mx-auto">
          <LanguageName v-if="language" :language="language" class="sm:hidden" />

          <slot/>
        </main>
      </div>
    </div>
  </UApp>
</template>

<style scoped>
/* Стили для лейаута */
</style>