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
    <template #top-left>
      <span class="font-yordan">Грамматика</span>
    </template>

    <template #top-right>
      <Link :href="`/grammatic/${language.id}/`">
        <UButton variant="soft" color="error">Отменить</UButton>
      </Link>
    </template>

    <div class="flex flex-row">
      <GrammaticTree :parts-of-speech="partsOfSpeech" :disabled :submit-insert />
      <div class="grow p-2">
        <UForm v-if="partOfSpeech" :state @submit="submitUpdate">
          <UFormField label="Название" name="name" required class="w-full">
            <UInput v-model="state.name" class="w-full"/>
          </UFormField>
          <UFormField label="Код" name="code" required class="w-full">
            <UInput v-model="state.code" class="w-full"/>
          </UFormField>
          <UFormField label="Описание" name="description" class="w-full">
            <Editor v-model="state.description" class="w-full"/>
          </UFormField>

          <UButton type="submit" class="mt-4 w-full" variant="soft" color="info" :loading="disabled">Изменить</UButton>
        </UForm>
        <div v-else>
          Выберите сущность для редактирования слева.
        </div>
      </div>
    </div>
  </Layout>
</template>

<style scoped>

</style>