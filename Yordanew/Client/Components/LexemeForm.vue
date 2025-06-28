<script setup>
import Editor from "./Editor.vue";

const model = defineModel();

</script>

<template>
  <div class="flex flex-col gap-2">
    <UFormField
            :name="``"
            label="Номер"
            :error="errors?.groups?.[gIndex]?.children?.[lexeme.path]?.path"
    >
      <UInput v-model="lexeme.path">
        <template #trailing>
          <UButton
                  v-if="index !== 0"
                  variant="ghost"
                  color="error"
                  icon="i-lucide-trash"
                  @click="removeLexeme(gIndex, index)"
          />
        </template>
      </UInput>
    </UFormField>

    <UFormField
            :name="`groups[${gIndex}].children[${index}].article`"
            label="Статья"
            :error="errors?.groups?.[gIndex]?.children?.[lexeme.path]?.article"
    >
      <Editor v-model="lexeme.article"/>
    </UFormField>
  </div>
</template>

<style scoped>

</style>