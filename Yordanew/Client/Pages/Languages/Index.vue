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
    <template v-if="userId" #top-left>
      <USwitch label="Только мои" v-model="isOwn" />
    </template>

    <template v-if="userId" #top-right>
      <Link href="/languages/create">
        <UButton variant="soft" color="success">Создать язык</UButton>
      </Link>
    </template>

    <div class="flex flex-col gap-2 items-stretch">
      <UCard variant="soft" v-for="language in languages" :key="language.id">
        <template #header>
          <Link :href="`/languages/${language.id}`">
            <UButton class="text-xl dark:text-neutral-200" variant="link" color="primary">
              {{ language.name.content }}
            </UButton>
          </Link>
        </template>
        <div class="line-clamp-5" v-html="language.description.content"></div>
      </UCard>
    </div>
  </Layout>
</template>

<style scoped>

</style>