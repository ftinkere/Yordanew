<script setup>
import Layout from "../../Layout.vue";
import { computed, provide, ref, reactive, watch } from "vue";
import { Link, router, usePage } from "@inertiajs/vue3";
import GrammaticTree from "../../Components/GrammaticTree.vue";
import Editor from "../../Components/Editor.vue";

const language = computed(() => usePage().props.language)
const partsOfSpeech = computed(() => language.value.partsOfSpeech)
const partOfSpeech = computed(() => usePage().props.partOfSpeech)
const category = computed(() => usePage().props.category)
const feature = computed(() => usePage().props.feature)

provide('language', language)

function reset() {
    const a = {
        name: feature.value?.name ?? category.value?.name ?? partOfSpeech.value?.name ?? '',
        code: feature.value?.code ?? category.value?.code ?? partOfSpeech.value?.code ?? '',
        description: feature.value?.description ?? category.value?.description ?? partOfSpeech.value?.description ?? '',
    };
    return a;
}

const state = reactive(reset())

watch(feature, () => {
    const a = reset()
    state.name = a.name
    state.code = a.code
    state.description = a.description
})
watch(category, () => {
    const a = reset()
    state.name = a.name
    state.code = a.code
    state.description = a.description
})
watch(partOfSpeech, () => {
    const a = reset()
    state.name = a.name
    state.code = a.code
    state.description = a.description
})

const disabled = ref(false)

function onStart() {
    disabled.value = true
}
const toast = useToast()
function onFinish() {
    disabled.value = false
}

function onSuccess() {
    toast.add({ color: 'success', title: 'Сохранено', duration: 2000 })
}

function onError() {
    toast.add({ color: 'error', title: 'Ошибка сохранения', duration: 2000 })
}

function submitUpdate(event) {
    if (feature.value) {
        let data = { categoryId: category.value?.id, posId: partOfSpeech.value?.id, ...event.data }
        router.post(`/grammatic/features/${feature.value.id}`, data, {
            preserveScroll: true,
            preserveState: true,
            preserveUrl: true,
            onStart,
            onFinish,
            onSuccess,
            onError,
        })
    } else if (category.value) {
        let data = { posId: partOfSpeech.value?.id, ...event.data }
        router.post(`/grammatic/categories/${category.value.id}`, data, {
            preserveScroll: true,
            preserveState: true,
            preserveUrl: true,
            onStart,
            onFinish,
            onSuccess,
            onError,
        })
    } else if (partOfSpeech.value) {
        let data = { languageId: language.value?.id, ...event.data }
        router.post(`/grammatic/parts-of-speech/${partOfSpeech.value.id}`, data, {
            preserveScroll: true,
            preserveState: true,
            preserveUrl: true,
            onStart,
            onFinish,
            onSuccess,
            onError,
        })
    }
}

function submitInsert(event) {
    if (event.data.categoryId) {
        let data = { categoryId: category.value?.id, ...event.data }
        router.post(`/grammatic/features`, data, {
            preserveScroll: true,
            preserveState: true,
            preserveUrl: true,
            onStart,
            onFinish,
            onSuccess,
            onError,
        })
    } else if (event.data.posId) {
        let data = { posId: partOfSpeech.value?.id, ...event.data }
        router.post(`/grammatic/categories`, data, {
            preserveScroll: true,
            preserveState: true,
            preserveUrl: true,
            onStart,
            onFinish,
            onSuccess,
            onError,
        })
    } else {
        let data = { ...event.data, languageId: language.value?.id }
        router.post(`/grammatic/parts-of-speech`, data, {
            preserveScroll: true,
            preserveState: true,
            preserveUrl: true,
            onStart,
            onFinish,
            onSuccess,
            onError,
        })
    }
}

</script>

<template>
  <Layout>
    <template #top-right>
      <Link :href="`/grammatic/${language.id}/`">
        <UButton
          icon="i-lucide-x"
          variant="ghost"
          color="white">
          Отменить
        </UButton>
      </Link>
    </template>

    <div class="flex flex-col gap-6 w-full">
      <!-- Page Header -->
      <div class="bg-white/5 backdrop-blur-xl border border-white/10 rounded-3xl p-6 shadow-xl shadow-black/20">
        <div class="flex items-center gap-4">
          <div class="w-14 h-14 rounded-full bg-linear-to-br from-purple-500/20 to-violet-500/20 border border-purple-400/30 flex items-center justify-center">
            <UIcon name="i-lucide-pencil" class="size-7 text-purple-400" />
          </div>
          <div>
            <h1 class="text-3xl font-bold text-white mb-1">Редактор грамматики</h1>
            <p class="text-white/60">
              Изменение структуры частей речи, категорий и признаков
            </p>
          </div>
        </div>
      </div>

      <!-- Editor Layout -->
      <div class="flex flex-col lg:flex-row gap-6">
        <!-- Tree Sidebar -->
        <div class="lg:w-80 shrink-0">
          <div class="lg:sticky lg:top-24">
            <div class="bg-white/5 backdrop-blur-xl border border-white/10 rounded-3xl p-6 shadow-xl shadow-black/20">
              <div class="flex items-center gap-2 mb-4 pb-4 border-b border-white/10">
                <UIcon name="i-lucide-list-tree" class="size-5 text-purple-400" />
                <h2 class="text-xl font-semibold text-white">Структура</h2>
              </div>
              <GrammaticTree :parts-of-speech="partsOfSpeech" :disabled :submit-insert />
            </div>
          </div>
        </div>

        <!-- Editor Panel -->
        <div class="flex-1 min-w-0">
          <!-- Edit Form -->
          <div v-if="partOfSpeech || category || feature"
               class="bg-white/5 backdrop-blur-xl border border-white/10 rounded-3xl p-8 shadow-xl shadow-black/20">
            <!-- Current Item Header -->
            <div class="flex items-center gap-3 mb-6 pb-4 border-b border-white/10">
              <UIcon
                :name="feature ? 'i-lucide-tag' : category ? 'i-lucide-folder' : 'i-lucide-speech'"
                :class="feature ? 'text-blue-400' : category ? 'text-violet-400' : 'text-purple-400'"
                class="size-6" />
              <div>
                <h2 class="text-2xl font-bold text-white">
                  {{ feature ? 'Редактирование признака' : category ? 'Редактирование категории' : 'Редактирование части речи' }}
                </h2>
                <p class="text-white/60 text-sm mt-1">
                  {{ state.name || 'Новая сущность' }}
                  <span v-if="state.code" class="font-mono text-white/50">{{ state.code }}</span>
                </p>
              </div>
            </div>

            <!-- Form Fields -->
            <UForm :state @submit="submitUpdate" class="flex flex-col gap-6">
              <UFormField label="Название" name="name" required class="w-full">
                <UInput
                  v-model="state.name"
                  icon="i-lucide-text"
                  size="lg"
                  placeholder="Например: Существительное"
                  class="w-full"/>
              </UFormField>

              <UFormField label="Код" name="code" required class="w-full">
                <UInput
                  v-model="state.code"
                  icon="i-lucide-code"
                  size="lg"
                  placeholder="Например: NOUN"
                  class="w-full font-mono"/>
              </UFormField>

              <UFormField label="Описание" name="description" class="w-full">
                <div class="bg-white/5 border border-white/10 rounded-2xl overflow-hidden">
                  <Editor v-model="state.description" class="w-full"/>
                </div>
              </UFormField>

              <!-- Action Buttons -->
              <div class="flex gap-3 pt-4">
                <UButton
                  type="submit"
                  size="lg"
                  :loading="disabled"
                  class="flex-1 bg-linear-to-r from-purple-500/80 to-violet-500/80 hover:from-purple-500 hover:to-violet-500 border-purple-400/50">
                  <div class="flex items-center gap-2">
                    <UIcon name="i-lucide-save" class="size-5" />
                    <span class="font-semibold">Сохранить</span>
                  </div>
                </UButton>

                <Link :href="`/grammatic/${language.id}/`">
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

          <!-- Empty State -->
          <div v-else class="bg-white/5 backdrop-blur-xl border border-white/10 rounded-3xl p-12 shadow-xl shadow-black/20 text-center">
            <div class="flex flex-col items-center gap-4">
              <div class="w-16 h-16 rounded-full bg-white/10 flex items-center justify-center">
                <UIcon name="i-lucide-mouse-pointer-click" class="size-8 text-white/50" />
              </div>
              <div>
                <h3 class="text-xl font-semibold text-white mb-2">
                  Выберите элемент для редактирования
                </h3>
                <p class="text-white/60">
                  Используйте дерево структуры слева, чтобы выбрать<br>
                  часть речи, категорию или признак для редактирования
                </p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </Layout>
</template>

<style scoped>

</style>