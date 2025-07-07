<script setup>
import { computed, reactive, ref, inject } from "vue";
import { router } from "@inertiajs/vue3";
import Editor from "./Editor.vue";

const { partsOfSpeech, submitInsert, disabled } = defineProps({ partsOfSpeech: Object, submitInsert: Function, disabled: Boolean })

const language = inject("language");

const model = defineModel()

function whatIs(item) {
    if (item.feature) {
        return 'features';
    }
    if (item.category) {
        return 'categories';
    }
    if (item.partOfSpeech) {
        return 'parts-of-speech';
    }
    return null;
}

function whatIsId(item) {
    if (item.feature) {
        return item.feature;
    }
    if (item.category) {
        return item.category;
    }
    if (item.partOfSpeech) {
        return item.partOfSpeech;
    }
    return null;
}

function getUrlFor(item) {
    return `/grammatic/${whatIs(item)}/${whatIsId(item)}/`;
}

function onSelect(e) {
    router.get(getUrlFor(e.detail.value), undefined, {
        preserveScroll: true,
        preserveState: true,
    })
}

const items = computed(() => {
    return partsOfSpeech.map(partOfSpeech => {
        return {
            label: partOfSpeech.name,
            defaultExpanded: true,
            // onToggle: (e) => {
            //     e.preventDefault()
            // }, 
            partOfSpeech: partOfSpeech.id,
            onSelect,
            children: partOfSpeech.categories.map(category => {
                return {
                    label: category.name,
                    category: category.id,
                    defaultExpanded: true,
                    // onToggle: (e) => {
                    //     e.preventDefault()
                    // },
                    onSelect,
                    children: category.features.map(feature => {
                        return {
                            label: feature.name,
                            feature: feature.id,
                            onSelect,
                        }
                    })
                }
            })
        }
    });
})

const openedModals = reactive({
    pos: false,
    category: false,
    feature: false,
})

const state = reactive({
    name: '',
    code: '',
    description: '',
    posId: null,
    categoryId: null,
    languageId: null,
})

function addElement(item = null) {
    state.name = ''
    state.code = ''
    state.description = ''
    
    if (!item) {
        openedModals.pos = true
        state.posId = null
        state.categoryId = null
    }
    if (item?.partOfSpeech) {
        openedModals.category = true
        state.languageId = null
        state.posId = item.partOfSpeech
        state.categoryId = null
    }
    if (item?.category) {
        openedModals.feature = true
        state.languageId = null
        state.posId = null
        state.categoryId = item.category
    }

}
</script>

<template>
  <div class="flex flex-col">
    <div class="flex flex-row justify-between items-center">
      <span class="text-nowrap">Части речи</span>
      <UButton icon="i-lucide-plus" variant="soft" color="secondary" class="my-2 mx-[10px]" size="xs"
               @click.stop="addElement()"/>
    </div>
    <UTree :items="items" v-model="model">
      <template #item-trailing="{ item }">
        <UButton v-if="!item.feature" icon="i-lucide-plus" variant="soft" color="secondary" class="ms-2" size="xs"
                 @click.stop="addElement(item)"/>
      </template>
    </UTree>
  </div>

  <UModal title="Создать Часть речи" v-model:open="openedModals.pos" class="min-w-xl">
    <template #body="{ close }">
      <div class="p-4">
        <UForm :state @submit="e => { submitInsert(e); close() }" >
          <UFormField label="Название" name="name" required class="w-full">
            <UInput v-model="state.name" class="w-full"/>
          </UFormField>
          <UFormField label="Код" name="code" required class="w-full">
            <UInput v-model="state.code" class="w-full"/>
          </UFormField>
          <UFormField label="Описание" name="description" class="w-full">
            <Editor v-model="state.description" class="w-full"/>
          </UFormField>

          <UButton type="submit" class="mt-4 w-full" variant="soft" color="success" :loading="disabled">Создать часть речи</UButton>
        </UForm>
      </div>
    </template>
  </UModal>

  <UModal title="Создать Категорию" v-model:open="openedModals.category" class="w-lg">
    <template #body="{ close }">
      <div class="p-4">
        <UForm :state @submit="e => { submitInsert(e); close() }">
          <UFormField label="Название" name="name" required class="w-full">
            <UInput v-model="state.name" class="w-full"/>
          </UFormField>
          <UFormField label="Код" name="code" required class="w-full">
            <UInput v-model="state.code" class="w-full"/>
          </UFormField>
          <UFormField label="Описание" name="description" class="w-full">
            <Editor v-model="state.description" class="w-full"/>
          </UFormField>

          <UButton type="submit" class="mt-4 w-full" variant="soft" color="success" :loading="disabled">Создать категорию</UButton>
        </UForm>
      </div>
    </template>
  </UModal>

  <UModal title="Создать Значение" v-model:open="openedModals.feature" class="w-lg">
    <template #body="{ close }">
      <div class="p-4">
        <UForm :state @submit="e => { submitInsert(e); close() }">
          <UFormField label="Название" name="name" required class="w-full">
            <UInput v-model="state.name" class="w-full"/>
          </UFormField>
          <UFormField label="Код" name="code" required class="w-full">
            <UInput v-model="state.code" class="w-full"/>
          </UFormField>
          <UFormField label="Описание" name="description" class="w-full">
            <Editor v-model="state.description" class="w-full"/>
          </UFormField>

          <UButton type="submit" class="mt-4 w-full" variant="soft" color="success" :loading="disabled">Создать значение</UButton>
        </UForm>
      </div>
    </template>
  </UModal>
</template>

<style scoped>

</style>