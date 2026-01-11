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

// Инициализация состояния с сортировкой лексем
const initLexemes = () => {
    if (!article.value?.lexemes || article.value.lexemes.length === 0) {
        return [{ path: "1", description: '', tags: [] }]
    }

    // Сортируем загруженные лексемы
    return [...article.value.lexemes].sort((a, b) => {
        const partsA = parsePath(a.path)
        const partsB = parsePath(b.path)

        for (let i = 0; i < Math.max(partsA.length, partsB.length); i++) {
            const numA = partsA[i] || 0
            const numB = partsB[i] || 0
            if (numA !== numB) return numA - numB
        }
        return 0
    })
}

const state = reactive({
    vocabula: article.value?.lemma ?? "",
    transcription: article.value?.transcription ?? "",
    adaptation: article.value?.adaptation ?? "",
    lexemes: initLexemes(),
    addFiles: article.value?.files ?? [],
})

// Утилиты для работы с путями
function parsePath(path) {
    return path.split(".").map(u => parseInt(u, 10))
}

function makePath(parts) {
    return parts.join(".")
}

// Сортировка лексем по пути
function sortLexemes() {
    state.lexemes.sort((a, b) => {
        const partsA = parsePath(a.path)
        const partsB = parsePath(b.path)

        for (let i = 0; i < Math.max(partsA.length, partsB.length); i++) {
            const numA = partsA[i] || 0
            const numB = partsB[i] || 0
            if (numA !== numB) return numA - numB
        }
        return 0
    })
}

// Проверка есть ли дочерние лексемы
function hasChildren(lexeme) {
    const parentPath = lexeme.path
    const parentParts = parsePath(parentPath)
    return state.lexemes.some(l => {
        if (l.path === parentPath) return false
        const parts = parsePath(l.path)
        // Дочерняя если начинается с родительского пути и на 1 уровень глубже
        if (parts.length !== parentParts.length + 1) return false
        return parentParts.every((p, i) => p === parts[i])
    })
}

// Проверка есть ли следующая соседняя лексема
function hasNextSibling(lexeme) {
    const parts = parsePath(lexeme.path)
    const nextParts = [...parts]
    nextParts[nextParts.length - 1]++
    const nextPath = makePath(nextParts)
    return state.lexemes.some(l => l.path === nextPath)
}

// Добавить дочернюю лексему (path.1)
function addChildLexeme(parentLexeme) {
    const parentParts = parsePath(parentLexeme.path)
    const childPath = makePath([...parentParts, 1])
    state.lexemes.push({
        path: childPath,
        description: "",
        tags: [],
    })
    sortLexemes()
}

// Добавить соседнюю лексему (path с последним номером +1)
function addSiblingLexeme(lexeme) {
    const parts = parsePath(lexeme.path)
    parts[parts.length - 1]++
    const newPath = makePath(parts)
    state.lexemes.push({
        path: newPath,
        description: "",
        tags: [],
    })
    sortLexemes()
}

// Удалить лексему и перестроить пути
function deleteLexeme(lexeme) {
    if (lexeme.path === "1") return // Нельзя удалить первую
    if (hasChildren(lexeme)) return // Нельзя удалить если есть дочерние

    const parts = parsePath(lexeme.path)
    const indexToDelete = state.lexemes.findIndex(l => l.path === lexeme.path)
    state.lexemes.splice(indexToDelete, 1)

    // Перенумеровать соседние лексемы того же уровня
    const parentParts = parts.slice(0, -1)
    const deletedNumber = parts[parts.length - 1]

    state.lexemes.forEach(l => {
        const lParts = parsePath(l.path)
        // Проверяем что это сосед того же уровня и идёт после удалённой
        if (lParts.length === parts.length) {
            const lParentParts = lParts.slice(0, -1)
            const isSameLevelSibling = parentParts.every((p, i) => p === lParentParts[i])
            if (isSameLevelSibling && lParts[lParts.length - 1] > deletedNumber) {
                lParts[lParts.length - 1]--
                l.path = makePath(lParts)
            }
        }
    })

    sortLexemes()
}

// Можно ли удалить лексему
function canDelete(lexeme) {
    return lexeme.path !== "1" && !hasChildren(lexeme)
}

const toast = useToast()

function save(event) {
    // Валидация: лемма обязательна
    if (!state.vocabula || state.vocabula.trim() === '') {
        toast.add({
            color: 'error',
            title: 'Ошибка',
            description: 'Заполните поле "Написание"',
            duration: 3000
        })
        return
    }

    // Валидация: должна быть хотя бы одна лексема
    if (state.lexemes.length === 0) {
        toast.add({
            color: 'error',
            title: 'Ошибка',
            description: 'Добавьте хотя бы одно определение (лексему)',
            duration: 3000
        })
        return
    }

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
    <template v-if="userId === language.author.id" #top-right>
      <Link v-if="article" :href="`/dictionary/${article.id}`">
        <UButton
          icon="i-lucide-x"
          variant="ghost"
          color="white">
          Отменить
        </UButton>
      </Link>
      <Link v-else :href="`/languages/${language.id}/dictionary`">
        <UButton
          icon="i-lucide-x"
          variant="ghost"
          color="white">
          Отменить
        </UButton>
      </Link>
    </template>

    <div class="flex flex-col gap-6 w-full max-w-3xl mx-auto">
      <!-- Page Header -->
      <div class="bg-white/5 backdrop-blur-xl border border-white/10 rounded-3xl p-6 shadow-xl shadow-black/20">
        <div class="flex items-center gap-4">
          <div class="w-14 h-14 rounded-full bg-linear-to-br from-green-500/20 to-emerald-500/20 border border-green-400/30 flex items-center justify-center">
            <UIcon :name="article ? 'i-lucide-pencil' : 'i-lucide-plus'" class="size-7 text-green-400" />
          </div>
          <div>
            <h1 class="text-3xl font-bold text-white mb-1">
              {{ article ? 'Изменить слово' : 'Добавить слово' }}
            </h1>
            <p class="text-white/60">
              {{ article ? 'Редактирование словарной статьи' : 'Создание новой словарной статьи' }}
            </p>
          </div>
        </div>
      </div>

      <!-- Form Card -->
      <UForm :state @submit="save" class="flex flex-col gap-6">
        <div class="bg-white/5 backdrop-blur-xl border border-white/10 rounded-3xl p-2 shadow-xl shadow-black/20">
          <!-- Basic Info Section -->
          <div class="space-y-4">
            <h2 class="text-xl font-semibold text-white flex items-center gap-2 mb-4">
              <UIcon name="i-lucide-info" class="size-5 text-green-400" />
              Основная информация
            </h2>

            <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
              <UFormField name="vocabula" label="Написание" required :error="errors?.vocabula" class="w-full">
                <UInput
                  icon="i-lucide-languages"
                  v-model="state.vocabula"
                  size="lg"
                  placeholder="Например: слово"
                  class="w-full"/>
              </UFormField>

              <UFormField name="transcription" label="Транскрипция" :error="errors?.transcription" class="w-full">
                <UInput
                  icon="i-lucide-speech"
                  v-model="state.transcription"
                  size="lg"
                  placeholder="ˈsɫovə"
                  class="w-full">
                  <template #trailing>
                    <XsampaToIpaButton v-model="state.transcription"/>
                  </template>
                </UInput>
              </UFormField>
            </div>

            <UFormField name="adaptation" label="Адаптация (латиница или кириллица)" :error="errors?.adaptation" class="w-full">
              <UInput
                icon="i-lucide-pencil-line"
                v-model="state.adaptation"
                size="lg"
                placeholder="slovo"
                class="w-full"/>
            </UFormField>
          </div>

          <!-- Images Section -->
          <div v-if="(article?.files && article.files.length > 0) || true" class="mt-6 pt-6 border-t border-white/10">
            <h2 class="text-xl font-semibold text-white flex items-center gap-2 mb-4">
              <UIcon name="i-lucide-image" class="size-5 text-blue-400" />
              Изображения
            </h2>

            <div v-if="article?.files && article.files.length > 0" class="flex flex-wrap gap-4 mb-4">
              <div v-for="fileId in article.files" :key="fileId">
                <div class="relative group">
                  <UButton
                    variant="solid"
                    color="error"
                    icon="i-lucide-trash"
                    size="xs"
                    class="absolute -end-2 -top-2 z-10 opacity-0 group-hover:opacity-100 transition-opacity"
                    @click="fileDelete(fileId)"/>
                  <img
                    :src="`/dictionary/files/${fileId}`"
                    alt="Изображение статьи"
                    class="h-32 rounded-2xl border border-white/10" />
                </div>
              </div>
            </div>

            <div class="bg-white/5 border border-white/10 rounded-2xl p-4">
              <input ref="fileElement" class="filepond" name="file" type="file" multiple accept="image/*"/>
            </div>
          </div>
        </div>

        <!-- Lexemes Section -->
        <div class="bg-white/5 backdrop-blur-xl border border-white/10 rounded-3xl p-2 shadow-xl shadow-black/20">
          <h2 class="text-xl font-semibold text-white flex items-center gap-2 mb-6">
            <UIcon name="i-lucide-book-text" class="size-5 text-purple-400" />
            Лексемы
          </h2>

          <div class="flex flex-col gap-6 divide-y divide-white/10">
            <template v-for="lexeme in state.lexemes" :key="lexeme.path">
              <div class="flex flex-col gap-4 pt-6 first:pt-0">
                <!-- Path Display (Read-only) -->
                <div class="flex items-center gap-3">
                  <div class="flex items-center gap-2 px-4 py-2 rounded-full bg-purple-500/10 border border-purple-400/30">
                    <UIcon name="i-lucide-hash" class="size-4 text-purple-400" />
                    <span class="font-mono text-lg font-bold text-purple-400">{{ lexeme.path }}</span>
                  </div>
                  <UButton
                    v-if="canDelete(lexeme)"
                    variant="ghost"
                    color="error"
                    icon="i-lucide-trash"
                    size="sm"
                    @click="deleteLexeme(lexeme)">
                    Удалить
                  </UButton>
                </div>

                <UFormField name="article" label="Определение" :error="errors?.lexemes ? errors?.lexemes[lexeme.path]?.article : null" class="w-full">
                  <div class="bg-white/5 border border-white/10 rounded-2xl overflow-hidden">
                    <Editor v-model="lexeme.description" class="w-full"/>
                  </div>
                </UFormField>

                <UFormField name="tags" label="Теги" :error="errors?.lexemes ? errors?.lexemes[lexeme.path]?.tags : null" class="w-full">
                  <UInputTags
                    v-model="lexeme.tags"
                    variant="soft"
                    size="lg"
                    placeholder="Добавьте теги..."
                    class="w-full" />
                </UFormField>

                <!-- Action Buttons for Lexeme -->
                <div class="flex gap-2 pt-2">
                  <UButton
                    v-if="!hasChildren(lexeme)"
                    variant="soft"
                    color="primary"
                    icon="i-lucide-corner-down-right"
                    size="sm"
                    @click="addChildLexeme(lexeme)">
                    Добавить дочернюю
                  </UButton>
                  <UButton
                    v-if="!hasNextSibling(lexeme)"
                    variant="soft"
                    color="secondary"
                    icon="i-lucide-arrow-down"
                    size="sm"
                    @click="addSiblingLexeme(lexeme)">
                    Добавить соседнюю
                  </UButton>
                </div>
              </div>
            </template>
          </div>
        </div>

        <!-- Action Buttons -->
        <div class="flex flex-col sm:flex-row gap-3">
          <UButton
            type="submit"
            size="lg"
            :disabled="disableFormSubmit"
            class="flex-1 bg-linear-to-r from-green-500/80 to-emerald-500/80 hover:from-green-500 hover:to-emerald-500 border-green-400/50">
            <div class="flex items-center gap-2">
              <UIcon :name="article ? 'i-lucide-save' : 'i-lucide-plus'" class="size-5" />
              <span class="font-semibold">{{ article ? 'Обновить' : 'Создать' }}</span>
            </div>
          </UButton>

          <Link v-if="article" :href="`/dictionary/${article.id}`">
            <UButton
              size="lg"
              variant="ghost"
              color="white"
              class="w-full sm:w-auto">
              <div class="flex items-center gap-2">
                <UIcon name="i-lucide-x" class="size-5" />
                <span>Отменить</span>
              </div>
            </UButton>
          </Link>
          <Link v-else :href="`/languages/${language.id}/dictionary`">
            <UButton
              size="lg"
              variant="ghost"
              color="white"
              class="w-full sm:w-auto">
              <div class="flex items-center gap-2">
                <UIcon name="i-lucide-x" class="size-5" />
                <span>Отменить</span>
              </div>
            </UButton>
          </Link>
        </div>
      </UForm>
    </div>
  </Layout>
</template>

<style scoped>

</style>