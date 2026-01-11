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

  <UModal v-model:open="openedModals.pos" class="w-full max-w-2xl">
    <template #header>
      <div class="flex items-center gap-3">
        <div class="w-10 h-10 rounded-full bg-purple-500/20 flex items-center justify-center">
          <UIcon name="i-lucide-speech" class="size-5 text-purple-400" />
        </div>
        <div>
          <h3 class="text-xl font-bold text-white">Создать часть речи</h3>
          <p class="text-white/60 text-sm">Добавление новой части речи в грамматику</p>
        </div>
      </div>
    </template>
    <template #body="{ close }">
      <div class="p-6">
        <UForm :state @submit="e => { submitInsert(e); close() }" class="flex flex-col gap-6">
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

          <div class="flex gap-3 pt-4">
            <UButton
              type="submit"
              size="lg"
              :loading="disabled"
              class="flex-1 bg-linear-to-r from-purple-500/80 to-violet-500/80 hover:from-purple-500 hover:to-violet-500 border-purple-400/50">
              <div class="flex items-center gap-2">
                <UIcon name="i-lucide-plus" class="size-5" />
                <span class="font-semibold">Создать</span>
              </div>
            </UButton>
            <UButton
              size="lg"
              variant="ghost"
              color="white"
              @click="close">
              <div class="flex items-center gap-2">
                <UIcon name="i-lucide-x" class="size-5" />
                <span>Отменить</span>
              </div>
            </UButton>
          </div>
        </UForm>
      </div>
    </template>
  </UModal>

  <UModal v-model:open="openedModals.category" class="w-full max-w-2xl">
    <template #header>
      <div class="flex items-center gap-3">
        <div class="w-10 h-10 rounded-full bg-violet-500/20 flex items-center justify-center">
          <UIcon name="i-lucide-folder" class="size-5 text-violet-400" />
        </div>
        <div>
          <h3 class="text-xl font-bold text-white">Создать категорию</h3>
          <p class="text-white/60 text-sm">Добавление новой грамматической категории</p>
        </div>
      </div>
    </template>
    <template #body="{ close }">
      <div class="p-6">
        <UForm :state @submit="e => { submitInsert(e); close() }" class="flex flex-col gap-6">
          <UFormField label="Название" name="name" required class="w-full">
            <UInput
              v-model="state.name"
              icon="i-lucide-text"
              size="lg"
              placeholder="Например: Число"
              class="w-full"/>
          </UFormField>

          <UFormField label="Код" name="code" required class="w-full">
            <UInput
              v-model="state.code"
              icon="i-lucide-code"
              size="lg"
              placeholder="Например: NUM"
              class="w-full font-mono"/>
          </UFormField>

          <UFormField label="Описание" name="description" class="w-full">
            <div class="bg-white/5 border border-white/10 rounded-2xl overflow-hidden">
              <Editor v-model="state.description" class="w-full"/>
            </div>
          </UFormField>

          <div class="flex gap-3 pt-4">
            <UButton
              type="submit"
              size="lg"
              :loading="disabled"
              class="flex-1 bg-linear-to-r from-violet-500/80 to-purple-500/80 hover:from-violet-500 hover:to-purple-500 border-violet-400/50">
              <div class="flex items-center gap-2">
                <UIcon name="i-lucide-plus" class="size-5" />
                <span class="font-semibold">Создать</span>
              </div>
            </UButton>
            <UButton
              size="lg"
              variant="ghost"
              color="white"
              @click="close">
              <div class="flex items-center gap-2">
                <UIcon name="i-lucide-x" class="size-5" />
                <span>Отменить</span>
              </div>
            </UButton>
          </div>
        </UForm>
      </div>
    </template>
  </UModal>

  <UModal v-model:open="openedModals.feature" class="w-full max-w-2xl">
    <template #header>
      <div class="flex items-center gap-3">
        <div class="w-10 h-10 rounded-full bg-blue-500/20 flex items-center justify-center">
          <UIcon name="i-lucide-tag" class="size-5 text-blue-400" />
        </div>
        <div>
          <h3 class="text-xl font-bold text-white">Создать признак</h3>
          <p class="text-white/60 text-sm">Добавление нового грамматического признака</p>
        </div>
      </div>
    </template>
    <template #body="{ close }">
      <div class="p-6">
        <UForm :state @submit="e => { submitInsert(e); close() }" class="flex flex-col gap-6">
          <UFormField label="Название" name="name" required class="w-full">
            <UInput
              v-model="state.name"
              icon="i-lucide-text"
              size="lg"
              placeholder="Например: Единственное"
              class="w-full"/>
          </UFormField>

          <UFormField label="Код" name="code" required class="w-full">
            <UInput
              v-model="state.code"
              icon="i-lucide-code"
              size="lg"
              placeholder="Например: SG"
              class="w-full font-mono"/>
          </UFormField>

          <UFormField label="Описание" name="description" class="w-full">
            <div class="bg-white/5 border border-white/10 rounded-2xl overflow-hidden">
              <Editor v-model="state.description" class="w-full"/>
            </div>
          </UFormField>

          <div class="flex gap-3 pt-4">
            <UButton
              type="submit"
              size="lg"
              :loading="disabled"
              class="flex-1 bg-linear-to-r from-blue-500/80 to-cyan-500/80 hover:from-blue-500 hover:to-cyan-500 border-blue-400/50">
              <div class="flex items-center gap-2">
                <UIcon name="i-lucide-plus" class="size-5" />
                <span class="font-semibold">Создать</span>
              </div>
            </UButton>
            <UButton
              size="lg"
              variant="ghost"
              color="white"
              @click="close">
              <div class="flex items-center gap-2">
                <UIcon name="i-lucide-x" class="size-5" />
                <span>Отменить</span>
              </div>
            </UButton>
          </div>
        </UForm>
      </div>
    </template>
  </UModal>
</template>

<style scoped>

</style>