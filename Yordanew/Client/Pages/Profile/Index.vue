<script setup>
import Layout from "../../Layout.vue";
import { Link, usePage } from "@inertiajs/vue3";

const user = usePage().props.user
const languages = usePage().props.languages
</script>

<template>
  <Layout>
    <template #top-right>
      <Link href="/profile/edit">
        <UButton variant="soft" color="primary">Редактировать</UButton>
      </Link>
    </template>

    <div class="flex flex-col items-center gap-2">
      <UAvatar size="xl" :src="`/uploaded-files/avatars/${user.id}`" />
      <span class="text-2xl">{{ user.displayName ?? user.userName }}</span>
    </div>

    <div class="mt-4 flex flex-col gap-2 items-stretch w-full">
      <UCard variant="soft" v-for="language in languages" :key="language.id">
        <template #header>
          <Link :href="`/languages/${language.id}`">
            <UButton variant="link" color="primary" class="text-xl">{{ language.name }}</UButton>
          </Link>
        </template>
        <div class="line-clamp-5" v-html="language.description"></div>
      </UCard>
    </div>
  </Layout>
</template>

<style scoped>
</style>
