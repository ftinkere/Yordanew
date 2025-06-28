<script setup>
import Layout from "../Layout.vue";
import { reactive, computed } from "vue";
import { router, usePage, Link } from "@inertiajs/vue3";

const errors = computed(() => usePage().props.errors)

const state = reactive({
    userName: '',
    displayName: '',
    email: '',
    password: '',
    passwordConfirm: '',
})

function register(event) {
    router.post('/register', event.data)
}
</script>

<template>
  <Layout>
    <div class="mx-auto max-w-md">
      <UForm :state @submit="register" class="flex flex-col gap-2">
        <span class="font-yordan text-3xl text-center w-full">Зарегистрироваться</span>
        
        <UFormField label="Имя пользователя" name="userName" :error="errors?.userName ?? false">
          <UInput placeholder="username" v-model="state.userName" class="w-full" />
        </UFormField>
        
        <UFormField label="Имя пользователя" name="displayName" :error="errors?.displayName ?? false">
          <UInput placeholder="User Name" v-model="state.displayName" class="w-full" />
        </UFormField>
  
        
        <UFormField label="Почта" name="email" :error="errors?.email ?? false">
          <UInput placeholder="username@yordan.ru" v-model="state.email" type="email" class="w-full" />
        </UFormField>
  
        <UFormField label="Пароль" name="password" :error="errors?.password ?? false">
          <UInput v-model="state.password" type="password" class="w-full" />
        </UFormField>
  
        <UFormField label="Пароль" name="password" :error="errors?.passwordConfirm ?? false">
          <UInput v-model="state.passwordConfirm" type="password" class="w-full" />
        </UFormField>
        
        <UButton type="submit" class="w-full">Зарегистрироваться</UButton>
      </UForm>

      <div class="mt-4 text-center text-sm">
        <span>Уже есть аккаунт?</span>
        <Link href="/login">
          <UButton variant="link">Войти</UButton>
        </Link>
      </div>
    </div>
  </Layout>
</template>

<style scoped>

</style>