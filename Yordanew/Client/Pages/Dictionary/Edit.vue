<script setup>
import Layout from "../../Layout.vue";
import { Link, router, usePage } from "@inertiajs/vue3";
import { computed, provide, reactive, ref, onMounted } from "vue";
import XsampaToIpaButton from "../../Components/XsampaToIpaButton.vue";
import Editor from "../../Components/Editor.vue";
import 'filepond/dist/filepond.min.css';
import * as FilePond from 'filepond';

const language = usePage().props.language
const article = usePage().props.article
const userId = usePage().props.auth?.user?.id
const errors = computed(() => usePage().props.errors)

provide('language', language)

article.lexemes = article.lexemes.map(lexeme => {
    lexeme.article = lexeme.description
    lexeme.path = lexeme.path.join('.')
    return lexeme
})

function back() {
    window.history.back()
}

const state = reactive({
    vocabula: article.lemma,
    transcription: article.transcription,
    adaptation: article.adaptation,
    lexemes: article.lexemes,
    addFiles: article.files ?? [],
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

function edit(event) {
    console.log(event.data);
    router.post(`/dictionary/${article.id}/edit`, event.data, {
        preserveState: true,
        preserveScroll: true,
    })
}

const disableFormSubmit = ref(false)

const fileElement = ref(null)
onMounted(() => {
    const pond = FilePond.create(fileElement.value, {
        allowMultiple: true,
        storeAsFile: true,
        server: "/dictionary/files",
        onprocessfile: (error, file) => {
            if (!error) {
                state.addFiles.push(file.serverId)
            }
        },
        onprocessfiles: () => {
            disableFormSubmit.value = false
        },
        onaddfile: () => {
            disableFormSubmit.value = true
        }
    })
})
</script>

<template>
  <Layout>
    <template #top-left>
      <span class="font-yordan">Словарь</span>
    </template>

    <template v-if="userId" #top-right>
      <Link :href="`/dictionary/${article.id}`">
        <UButton variant="soft" color="error">Отменить</UButton>
      </Link>
    </template>

    <UForm :state @submit="edit" class="flex flex-col gap-2 max-w-lg mx-auto">
      <span class="font-yordan text-3xl text-center w-full">Изменить слово</span>

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

      <div class="my-4">
        <input ref="fileElement" class="filepond" name="file" type="file" multiple accept="image/*"/>
      </div>

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

      <UButton type="submit" class="w-full" variant="soft" color="success" :disabled="disableFormSubmit">Обновить</UButton>

    </UForm>
  </Layout>
</template>

<style scoped>

</style>