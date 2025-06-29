<script setup>
import { Link, usePage } from "@inertiajs/vue3";
import { computed, provide, reactive } from "vue";
import Layout from "../../Layout.vue";
import { ref } from 'vue'
import Editor from "../../Components/Editor.vue";
import XsampaToIpaButton from "../../Components/XsampaToIpaButton.vue";
import { router } from "@inertiajs/core";

const language = usePage().props.language;
provide('language', language);

const errors = computed(() => usePage().props.errors)

const state = reactive({
    name: language.name,
    autoname: language.autoName,
    autonameTranscription: language.autoNameTranscription,
    isPublished: language.isPublished,
    description: language.description,
})

function create(event) {
    router.post(`/languages/${language.id}/edit`, event.data)
}

</script>

<template>
  <Layout>
    <template #top-right>
      <Link :href="`/languages/${language.id}/`">
        <UButton variant="soft" color="error">Отменить</UButton>
      </Link>
    </template>
    
    <div class="flex flex-col gap-2 items-center w-full">
      <UForm :state @submit="create" class="flex flex-col gap-2 max-w-lg mx-auto">
        <span class="font-yordan text-3xl text-center w-full">Создать язык</span>

        <UFormField name="name" label="Название" required :error="errors?.name" class="w-full">
          <UInput icon="i-lucide-pencil-line" v-model="state.name" class="w-full" />
        </UFormField>
        <div class="grid grid-cols-2 gap-2">
          <UFormField name="autoname" label="Самоназвание" :error="errors?.autoname" class="w-full">
            <UInput icon="i-lucide-user" v-model="state.autoname" class="w-full" />
          </UFormField>

          <UFormField name="autonameTranscription" label="Транскрипция" :error="errors?.autonameTranscription" class="w-full">
            <UInput icon="i-lucide-speech" v-model="state.autonameTranscription" class="w-full">
              <template #trailing>
                <XsampaToIpaButton v-model="state.autonameTranscription" />
              </template>
            </UInput>
          </UFormField>
        </div>
        <UFormField name="isPublished" label="Транскрипция" :error="errors?.isPublished" class="w-full">
          <USwitch v-model="state.isPublished" label="Опубликовать" />
        </UFormField>

        <UFormField name="description" label="Описание" :error="errors?.description" class="w-full">
          <Editor v-model="state.description" class="w-full" />
        </UFormField>
        
        <UButton type="submit" class="w-full" variant="soft" color="success">Сохранить</UButton>

      </UForm>
    </div>
  </Layout>
</template>

<style scoped>

</style>