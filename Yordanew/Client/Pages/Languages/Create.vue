<script setup>
import { reactive, computed } from "vue";
import Layout from "../../Layout.vue";
import { router } from "@inertiajs/core"
import { usePage } from "@inertiajs/vue3";
import XsampaToIpaButton from "../../Components/XsampaToIpaButton.vue";

const errors = computed(() => usePage().props.errors)

const state = reactive({
    name: "",
    autoname: "",
    autonameTranscription: "",
    isPublished: false,
})

function create(event) {
    router.post("/languages/create", event.data)
}
</script>

<template>
  <Layout>
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
      
      <UButton type="submit" class="w-full" variant="soft" color="success">Создать</UButton>
      
    </UForm>
  </Layout>
</template>

<style scoped>

</style>