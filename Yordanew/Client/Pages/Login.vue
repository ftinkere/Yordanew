<script setup>
import Layout from "../Layout.vue";
import { reactive, computed } from "vue";
import { router, usePage, Link } from "@inertiajs/vue3";

const errors = computed(() => usePage().props.errors)

const state = reactive({
    username: '',
    password: '',
})

function login(event) {
    router.post('/login', event.data)
}
</script>

<template>
  <Layout>
    <div class="mx-auto max-w-md">
      <UForm :state @submit="login" class="flex flex-col gap-2">
        <span class="font-yordan text-3xl text-center w-full">Войти</span>
        <UFormField label="Имя пользователя" name="username" :error="errors?.username ?? false">
          <UInput v-model="state.username" class="w-full" />
        </UFormField>
  
        <UFormField label="Пароль" name="password">
          <UInput v-model="state.password" type="password" class="w-full" />
        </UFormField>
        
        <UButton type="submit" class="w-full">Войти</UButton>
      </UForm>
      
      <div class="mt-4 text-center text-sm">
        <span>Нет аккаунта?</span> 
        <Link href="/register">
          <UButton variant="link">Регистрация</UButton>
        </Link>
      </div>
    </div>
  </Layout>
</template>

<style scoped>

</style>