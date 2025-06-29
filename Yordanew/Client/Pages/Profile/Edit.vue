<script setup>
import Layout from "../../Layout.vue";
import { router, usePage, Link } from "@inertiajs/vue3";
import { reactive, computed } from "vue";

const user = usePage().props.user
const errors = computed(() => usePage().props.errors)

const state = reactive({
    displayName: user.displayName ?? "",
    avatar: null,
})

function handleFileChange(event) {
    state.avatar = event.target.files?.[0] ?? null
}

function submit(event) {
    router.post('/profile/edit', event.data, { forceFormData: true })
}
</script>

<template>
  <Layout>
    <template #top-right>
      <Link href="/profile">
        <UButton variant="soft" color="error">Отменить</UButton>
      </Link>
    </template>

    <UForm :state @submit="submit" class="flex flex-col gap-2 max-w-md mx-auto">
      <span class="font-yordan text-3xl text-center w-full">Редактировать профиль</span>

      <UFormField name="displayName" label="Отображаемое имя" :error="errors?.displayName">
        <UInput v-model="state.displayName" class="w-full"/>
      </UFormField>

      <UFormField name="avatar" label="Аватар">
        <UInput type="file" name="avatar" @change="handleFileChange" />
      </UFormField>

      <UButton type="submit" class="w-full" variant="soft" color="success">Сохранить</UButton>
    </UForm>
  </Layout>
</template>

<style scoped>
</style>
