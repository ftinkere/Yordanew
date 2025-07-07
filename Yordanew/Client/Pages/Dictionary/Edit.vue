<script setup>
import Layout from "../../Layout.vue";
import { Link, router, usePage } from "@inertiajs/vue3";
import { computed, provide, reactive, ref, onMounted } from "vue";
import XsampaToIpaButton from "../../Components/XsampaToIpaButton.vue";
import Editor from "../../Components/Editor.vue";
import 'filepond/dist/filepond.min.css';
import * as FilePond from 'filepond';

const language = computed(() => usePage().props.language)
const article = computed(() => usePage().props.article)
const userId = computed(() => usePage().props.auth?.user?.id)
const errors = computed(() => usePage().props.errors)

provide('language', language)

function back() {
    window.history.back()
}

const state = reactive({
    vocabula: article.value?.lemma ?? "",
    transcription: article.value?.transcription ?? "",
    adaptation: article.value?.adaptation ?? "",
    lexemes: article.value?.lexemes ?? [{ path: "1.1", description: '', tags: [] }],
    addFiles: article.value?.files ?? [],
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
        description: "",
        tags: [],
    })
}

function save(event) {
    const data = { ...event.data, id: article.value?.id, languageId: language.value.id }
    router.post('/dictionary/edit', data, {
        preserveState: true,
        preserveScroll: true,
    })
}

function fileDelete(fileId) {
    router.delete(`/dictionary/${article.value.id}/files/${fileId}`, {
        preserveState: true,
        preserveScroll: true,
        onSuccess: () => {
            state.addFiles = state.addFiles.filter(id => id !== fileId)
        }
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
      <Link v-if="article" :href="`/dictionary/${article.id}`">
        <UButton variant="soft" color="error">Отменить</UButton>
      </Link>
      <Link v-else :href="`/languages/${language.id}/dictionary`">
        <UButton variant="soft" color="error">Отменить</UButton>
      </Link>
    </template>

    <UForm :state @submit="save" class="flex flex-col gap-2 max-w-lg mx-auto">
      <span class="font-yordan text-3xl text-center w-full">{{ article ? 'Изменить слово' : 'Добавить слово' }}</span>

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

      <div class="py-4 flex flex-row flex-wrap gap-2">
        <div v-for="fileId in article?.files ?? []" :key="fileId">
          <div class="relative">
            <UButton variant="solid" color="error" icon="i-lucide-trash" class="absolute -end-4 -top-4 rounded-full" @click="fileDelete(fileId)"/>
            <img :src="`/dictionary/files/${fileId}`" alt="Изображение статьи" class="w-32 rounded-lg" />
          </div>
        </div>
      </div>
      
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
              <Editor v-model="lexeme.description" class="w-full"/>
            </UFormField>
            
            <UFormField name="tags" label="Теги" :error="errors?.lexemes ? errors?.lexemes[lexeme.path]?.tags : null" class="w-full">
              <UInputTags v-model="lexeme.tags" variant="soft" placeholder="Добавьте теги..." class="w-full" />
            </UFormField>
          </div>
        </template>

        <div class="flex flex-row justify-end">
          <UButton variant="ghost" icon="i-lucide-plus" @click="addLexeme">Добавить лексему</UButton>
        </div>
      </div>

      <UButton type="submit" class="w-full" variant="soft" color="success" :disabled="disableFormSubmit">{{ article ? 'Обновить' : 'Создать' }}</UButton>

    </UForm>
  </Layout>
</template>

<style scoped>

</style>