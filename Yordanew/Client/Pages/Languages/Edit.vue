<script setup>
import { Link, usePage } from "@inertiajs/vue3";
import { computed, provide, reactive } from "vue";
import Layout from "../../Layout.vue";
import { ref } from 'vue'
import Editor from "../../Components/Editor.vue";
import XsampaToIpaButton from "../../Components/XsampaToIpaButton.vue";
import { router } from "@inertiajs/core";

import { Emoji } from '@tiptap/extension-emoji'
import { TextAlign } from '@tiptap/extension-text-align'

const language = usePage().props.language;
provide('language', language);

const errors = computed(() => usePage().props.errors)

const state = reactive({
    name: language?.name ?? "",
    autoname: language?.autoName ?? "",
    autonameTranscription: language?.autoNameTranscription ?? "",
    isPublished: language?.isPublished ?? false,
    description: language?.description ?? "",
})

const isSaving = ref(false);

function save(event) {
    isSaving.value = true;
    const data = { ...event.data, id: language?.id }
    router.post('/languages/edit', data, {
        onFinish: () => {
            isSaving.value = false;
        }
    })
}
</script>

<template>
  <Layout>
    <template v-if="language" #top-right>
      <Link :href="`/languages/${language.id}/`">
        <UButton
          icon="i-lucide-x"
          variant="ghost"
          color="white"
          class="rounded-full">
          Отменить
        </UButton>
      </Link>
    </template>

    <!-- Form Container -->
    <div class="flex flex-col gap-6 w-full">
      <!-- Page Header -->
      <div class="bg-white/5 backdrop-blur-xl border border-white/10 rounded-3xl p-6 shadow-xl shadow-black/20">
        <div class="flex items-center gap-4">
          <div class="w-14 h-14 rounded-full bg-linear-to-br from-amber-500/20 to-orange-500/20 border border-amber-400/30 flex items-center justify-center">
            <UIcon :name="language ? 'i-lucide-pencil' : 'i-lucide-plus'" class="size-7 text-amber-400" />
          </div>
          <div>
            <h1 class="text-3xl font-bold text-white mb-1">
              {{ language ? 'Редактировать язык' : 'Создать новый язык' }}
            </h1>
            <p class="text-white/60">
              {{ language ? 'Измените данные вашего языка' : 'Заполните информацию о новом языке' }}
            </p>
          </div>
        </div>
      </div>

      <!-- Form Card -->
      <div class="bg-white/5 backdrop-blur-xl border border-white/10 rounded-3xl p-8 shadow-xl shadow-black/20">
        <UForm :state @submit="save" class="flex flex-col gap-6">
          <!-- Basic Info Section -->
          <div class="space-y-4">
            <h2 class="text-xl font-semibold text-white flex items-center gap-2">
              <UIcon name="i-lucide-info" class="size-5 text-amber-400" />
              Основная информация
            </h2>

            <!-- Name Field -->
            <UFormField name="name" label="Название языка" required :error="errors?.name">
              <UInput
                v-model="state.name"
                icon="i-lucide-languages"
                size="lg"
                placeholder="Например: Русский"
                class="w-full"
              />
            </UFormField>

            <!-- Autoname and Transcription -->
            <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
              <UFormField name="autoname" label="Самоназвание" :error="errors?.autoname">
                <UInput
                  v-model="state.autoname"
                  icon="i-lucide-globe"
                  size="lg"
                  placeholder="Например: Русский"
                  class="w-full"
                />
              </UFormField>

              <UFormField name="autonameTranscription" label="Транскрипция" :error="errors?.autonameTranscription">
                <UInput
                  v-model="state.autonameTranscription"
                  icon="i-lucide-speech"
                  size="lg"
                  placeholder="ˈruskʲɪj"
                  class="w-full"
                >
                  <template #trailing>
                    <XsampaToIpaButton v-model="state.autonameTranscription" />
                  </template>
                </UInput>
              </UFormField>
            </div>
          </div>

          <!-- Divider -->
          <div class="border-t border-white/10"></div>

          <!-- Description Section -->
          <div class="space-y-4">
            <h2 class="text-xl font-semibold text-white flex items-center gap-2">
              <UIcon name="i-lucide-file-text" class="size-5 text-blue-400" />
              Описание
            </h2>

            <UFormField name="description" label="Подробное описание языка" :error="errors?.description">
              <div class="bg-white/5 border border-white/10 rounded-2xl overflow-hidden">
                <Editor v-model="state.description" />
              </div>
            </UFormField>
          </div>

          <!-- Divider -->
          <div class="border-t border-white/10"></div>

          <!-- Settings Section -->
          <div class="space-y-4">
            <h2 class="text-xl font-semibold text-white flex items-center gap-2">
              <UIcon name="i-lucide-settings" class="size-5 text-purple-400" />
              Настройки публикации
            </h2>

            <UFormField name="isPublished" :error="errors?.isPublished">
              <div class="flex items-center gap-3 p-4 bg-white/5 border border-white/10 rounded-2xl">
                <USwitch v-model="state.isPublished" size="lg" />
                <div>
                  <div class="text-white font-medium">Опубликовать язык</div>
                  <div class="text-sm text-white/60">
                    {{ state.isPublished
                      ? 'Язык будет виден всем пользователям'
                      : 'Язык будет доступен только вам' }}
                  </div>
                </div>
              </div>
            </UFormField>
          </div>

          <!-- Action Buttons -->
          <div class="flex flex-col sm:flex-row gap-3 pt-4">
            <UButton
              type="submit"
              size="lg"
              :loading="isSaving"
              :disabled="isSaving"
              class="flex-1 rounded-full bg-linear-to-r from-amber-500/80 to-orange-500/80 hover:from-amber-500 hover:to-orange-500 border-amber-400/50">
              <div class="flex items-center gap-2">
                <div class="i-lucide-save w-5 h-5" />
                <span class="font-semibold">{{ isSaving ? 'Сохранение...' : 'Сохранить язык' }}</span>
              </div>
            </UButton>

            <Link v-if="language" :href="`/languages/${language.id}/`">
              <UButton
                size="lg"
                variant="ghost"
                color="white"
                class="rounded-full w-full sm:w-auto">
                <div class="flex items-center gap-2">
                  <div class="i-lucide-x w-5 h-5" />
                  <span>Отменить</span>
                </div>
              </UButton>
            </Link>

            <Link v-else href="/languages">
              <UButton
                size="lg"
                variant="ghost"
                color="white"
                class="rounded-full w-full sm:w-auto"
              >
                <div class="flex items-center gap-2">
                  <UIcon name="i-lucide-x" class="size-5" />
                  <span>Отменить</span>
                </div>
              </UButton>
            </Link>
          </div>
        </UForm>
      </div>

      <!-- Help Card -->
      <div class="bg-blue-500/10 backdrop-blur-xl border border-blue-400/20 rounded-3xl p-6">
        <div class="flex gap-4">
          <div class="w-10 h-10 rounded-full bg-blue-500/20 flex items-center justify-center shrink-0">
            <UIcon name="i-lucide-lightbulb" class="size-5 text-blue-400" />
          </div>
          <div>
            <h3 class="text-lg font-semibold text-blue-400 mb-2">Совет</h3>
            <p class="text-white/70 text-sm">
              Начните с создания базовой структуры языка. После сохранения вы сможете добавить словарные статьи и грамматические правила.
            </p>
          </div>
        </div>
      </div>
    </div>
  </Layout>
</template>

<style scoped>

</style>