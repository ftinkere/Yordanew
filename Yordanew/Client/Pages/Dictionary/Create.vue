<script setup>
import Layout from "../../Layout.vue";
import { Link, router, usePage } from "@inertiajs/vue3";
import { computed, provide, reactive } from "vue";
import XsampaToIpaButton from "../../Components/XsampaToIpaButton.vue";
import Editor from "../../Components/Editor.vue";

const language = usePage().props.language
const userId = usePage().props.auth?.user?.id
const errors = computed(() => usePage().props.errors)

provide('language', language)

function back() {
    window.history.back()
}

const state = reactive({
    vocabula: "",
    transcription: "",
    adaptation: "",
    lexemes: [
        {
            path: '1.1',
            article: '',
        }
    ]
})

function parsePath(path) {
    return path.split(".").map(u => parseInt(u, 10))
}

function makePath(parts) {
    return parts.join(".")
}

const lastLexeme = computed(() => state.lexemes[state.lexemes.length - 1])

function addLexeme() {
    const parts = parsePath(lastLexeme.value.path)
    if (parts.length > 0) {
        parts[parts.length - 1]++
    }
    const path = makePath(parts)
    state.lexemes.push({
        path: path,
        article: "",
    })
}

function create(event) {
    router.post(`/languages/${language.id}/dictionary/create`, event.data, {
        preserveState: true,
        preserveScroll: true,
    })
}
</script>

<template>
  <Layout>
    <template #top-left>
      <span v-if="language">
        <span class="font-yordan">Словарь</span> {{ language.autoName }} /{{ language.autoNameTranscription }}/ — {{
          language.name
        }}
      </span>
    </template>

    <template v-if="userId" #top-right>
      <UButton variant="soft" color="error" @click="back">Отменить</UButton>
    </template>

    <UForm :state @submit="create" class="flex flex-col gap-2 max-w-lg mx-auto">
      <span class="font-yordan text-3xl text-center w-full">Добавить слово</span>

      <div class="grid grid-cols-2 gap-2">
        <UFormField name="vocabula" label="Написание" required :error="errors?.vocabula" class="w-full">
          <UInput icon="i-lucide-languages" v-model="state.vocabula" class="w-full"/>
        </UFormField>

        <UFormField name="transcription" label="Транскрипция" :error="errors?.transcription" class="w-full">
          <UInput icon="i-lucide-speech" v-model="state.transcription" class="w-full">
            <template #trailing>
              <XsampaToIpaButton v-model="state.transcription"/>
            </template>
          </UInput>
        </UFormField>
      </div>
      <UFormField name="adaptation" label="Адаптация (латиница или кириллица)" :error="errors?.adaptation"
                  class="w-full">
        <UInput icon="i-lucide-pencil-line" v-model="state.adaptation" class="w-full"/>
      </UFormField>

      <div class="-mx-4 p-4 flex flex-col gap-4 divide-y dark:divide-neutral-700 bg-black/20 rounded-lg">
        <span class="font-yordan text-center w-full">Лексемы</span>

        <template v-for="lexeme in state.lexemes" :key="state.lexemes.indexOf(lexeme)">
          <div class="flex flex-col gap-2">
            <UFormField name="path" label="Номер" :error="errors?.lexemes ? errors?.lexemes[lexeme.path]?.path : null"
                        class="w-full">
              <UInput v-model="lexeme.path" class="w-full">
                <template #trailing>
                  <UButton v-if="state.lexemes.indexOf(lexeme) !== 0" variant="ghost" color="error"
                           icon="i-lucide-trash" @click="state.lexemes.splice(state.lexemes.indexOf(lexeme), 1)"/>
                </template>
              </UInput>
            </UFormField>

            <UFormField name="article" label="Статья"
                        :error="errors?.lexemes ? errors?.lexemes[lexeme.path]?.article : null" class="w-full">
              <Editor v-model="lexeme.article" class="w-full"/>
            </UFormField>
          </div>
        </template>

        <div class="flex flex-row justify-end">
          <UButton variant="ghost" icon="i-lucide-plus" @click="addLexeme">Добавить лексему</UButton>
        </div>
      </div>

      <UButton type="submit" class="w-full" variant="soft" color="success">Создать</UButton>

    </UForm>
  </Layout>
</template>

<style scoped>

</style>