<script setup>
import { inject, ref } from "vue";
import { Link, usePage } from "@inertiajs/vue3";
import LanguageName from "./Components/LanguageName.vue";

const auth = usePage().props.auth;
const language = inject('language');

const isMobileShowAside = ref(false);

</script>

<template>
  <UApp>
    <div class="relative flex flex-row items-stretch min-h-[100dvh]">
      <aside class="max-sm:absolute sticky max-sm:top-10 sm:top-0 bottom-0 flex flex-col gap-2  w-fit max-h-[100dvh] font-yordan shadow-sm shadow-neutral-400 dark:shadow-neutral-700 max-sm:not-[.show]:hidden backdrop-blur-lg z-100"
      :class="{ show: isMobileShowAside }">
        <Link href="/" class="w-full" prefetch>
          <UButton class="mb-2 px-10 font-yordan text-2xl shadow-md w-full" variant="ghost">Ëрдан</UButton>
        </Link>
        <Link href="/languages" class="w-full" prefetch>
          <UButton icon="i-lucide-languages" variant="ghost" class="mb-2 text-neutral-200 w-full">Все языки</UButton>
        </Link>
        <div v-if="language" class="flex flex-col gap-1 items-start">
          <USeparator />
          <Link :href="`/languages/${language.id}`" class="w-full" prefetch>
            <UButton icon="i-lucide-info" variant="ghost" class="mb-2 text-neutral-200 w-full justify-start">О языке</UButton>
          </Link>
          <Link :href="`/languages/${language.id}/dictionary`" class="w-full" prefetch>
            <UButton icon="i-lucide-book-a" variant="ghost" class="mb-2 text-neutral-200 w-full justify-start">Словарь</UButton>
          </Link>
          <Link :href="`/grammatic/${language.id}`" class="w-full" prefetch>
            <UButton icon="i-lucide-cpu" variant="ghost" class="mb-2 text-neutral-200 w-full justify-items-start justify-start">Грамматика</UButton>
          </Link>
        </div>

        
        <div class="pb-4 mt-auto flex flex-col gap-2">
          <Link v-if="auth" href="/profile" class="w-full flex flex-row gap-2 justify-center items-center">
            <UAvatar :src="`/uploaded-files/avatars/${auth.user?.id}`" :alt="auth.user?.displayName ?? auth.user?.userName" />
            {{ auth.user?.displayName ?? auth.user?.userName }}
          </Link>
          
          <Link v-if="auth" href="/logout" class="w-full">
            <UButton icon="i-lucide-log-out" variant="ghost" class="w-full">Выйти</UButton>
          </Link>
          <Link v-else href="/login" class="w-full" prefetch>
            <UButton icon="i-lucide-log-in" variant="ghost" class="w-full">Войти</UButton>
          </Link>
        </div>
      </aside>
      <div class="flex flex-col gap-2 grow">
        <header class="sticky top-0 flex flex-row justify-between items-center px-2 w-full h-[44px] bg-black/10 backdrop-blur-md">
          <div class="flex flex-row items-center gap-2">
            <UButton class="sm:hidden" variant="ghost" color="secondary" icon="i-lucide-menu" @click="isMobileShowAside = !isMobileShowAside" />           
            <slot name="top-left" />
            <LanguageName v-if="language" :language="language" class="max-sm:hidden" />
          </div>

          <div class="justify-self-end">
            <slot name="top-right"/>
          </div>
        </header>
        <div class="flex flex-col gap-2 p-4 max-w-4xl w-full mx-auto">
          <LanguageName v-if="language" :language="language" class="sm:hidden" />
          
          <slot/>
        </div>
      </div>
    </div>
  </UApp>
</template>

<style scoped>

</style>