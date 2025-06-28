<script setup>
import { useEditor, EditorContent } from '@tiptap/vue-3'
import StarterKit from '@tiptap/starter-kit'
import TaskItem from '@tiptap/extension-task-item'
import TaskList from '@tiptap/extension-task-list'
import { watch } from "vue";

const model = defineModel()

const editor = useEditor({
    extensions: [StarterKit, TaskItem, TaskList],
    content: model.value ?? '',
    onUpdate: () => {
        model.value = editor.value?.getHTML()
    }
})

watch(model, (newVal) => {
    const isSame = editor.value?.getHTML() === newVal

    if (isSame) {
        return
    }

    editor.value?.commands.setContent(newVal ?? '')
})

function setLink() {
    const previousUrl = editor.value?.getAttributes('link').href
    const url = window.prompt('URL', previousUrl)

    // cancelled
    if (url === null) {
        return
    }

    // empty
    if (url === '') {
        editor.value?.chain().focus().extendMarkRange('link').unsetLink().run()

        return
    }

    // update link
    editor.value?.chain().focus().extendMarkRange('link').setLink({ href: url }).run()
}

const items = [
    {
        label: 'Заголовок 1',
        level: 1,
        icon: 'i-lucide-heading-1',
        onSelect() {
            editor.value?.chain().focus().toggleHeading({ level: 1 }).run()
        }
    }, {
        label: 'Заголовок 2',
        level: 2,
        icon: 'i-lucide-heading-2',
        onSelect() {
            editor.value?.chain().focus().toggleHeading({ level: 2 }).run()
        }
    }, {
        label: 'Заголовок 3',
        level: 3,
        icon: 'i-lucide-heading-3',
        onSelect() {
            editor.value?.chain().focus().toggleHeading({ level: 3 }).run()
        }
    }, {
        label: 'Заголовок 4',
        level: 4,
        icon: 'i-lucide-heading-4',
        onSelect() {
            editor.value?.chain().focus().toggleHeading({ level: 4 }).run()
        }
    }, {
        label: 'Заголовок 5',
        level: 5,
        icon: 'i-lucide-heading-5',
        onSelect() {
            editor.value?.chain().focus().toggleHeading({ level: 5 }).run()
        }
    },
]

</script>

<template>
  <div class="grid gap-1">
    <div v-if="editor" class="sticky top-0 py-1 px-2 flex flex-row gap-1 rounded-lg bg-black/20 *:size-8 z-50">

      <UDropdownMenu :items="items" :ui="{ content: 'w-48' }">
        <UButton label="Заголовок" color="neutral" :variant="editor.isActive('heading') ? 'soft' : 'ghost'">
          <UIcon name="i-lucide-heading" class="h-[2em]" />
        </UButton>
      </UDropdownMenu>

      <UButton
              :variant="editor.isActive('bold') ? 'soft' : 'ghost'" color="neutral"
              @click.prevent="editor.chain().focus().toggleBold().run()"
      >
        <UIcon name="i-lucide-bold" class="h-[2em]" />
      </UButton>
      <UButton
              :variant="editor.isActive('italic') ? 'soft' : 'ghost'" color="neutral"
              @click.prevent="editor.chain().focus().toggleItalic().run()"
      >
        <UIcon name="i-lucide-italic"/>
      </UButton>
      <UButton
              :variant="editor.isActive('strike') ? 'soft' : 'ghost'" color="neutral"
              @click.prevent="editor.chain().focus().toggleStrike().run()"
      >
        <UIcon name="i-lucide-strikethrough"/>
      </UButton>
      <UButton
              :variant="editor.isActive('link') ? 'soft' : 'ghost'" color="neutral"
              @click.prevent="setLink"
      >
        <UIcon name="i-lucide-link"/>
      </UButton>
      <UButton
              :variant="editor.isActive('code') ? 'soft' : 'ghost'" color="neutral"
              @click.prevent="editor.chain().focus().toggleCode().run()"
      >
        <UIcon name="i-lucide-code"/>
      </UButton>
      <UButton
              :variant="editor.isActive('codeBlock') ? 'soft' : 'ghost'" color="neutral"
              @click.prevent="editor.chain().focus().toggleCodeBlock().run()"
      >
        <UIcon name="i-lucide-code-2"/>
      </UButton>
      <UButton
              variant="ghost" color="neutral"
              @click.prevent="editor.chain().focus().unsetAllMarks().run()"
      >
        <UIcon name="i-lucide-brush-cleaning"/>
      </UButton>

      <USeparator orientation="vertical"/>

      <UButton
              :variant="editor.isActive('bulletList') ? 'soft' : 'ghost'" color="neutral"
              @click.prevent="editor.chain().focus().toggleBulletList().run()"
      >
        <UIcon name="i-lucide-list"/>
      </UButton>
      <UButton
              :variant="editor.isActive('orderedList') ? 'soft' : 'ghost'" color="neutral"
              @click.prevent="editor.chain().focus().toggleOrderedList().run()"
      >
        <UIcon name="i-lucide-list-ordered"/>
      </UButton>
      <UButton
              :variant="editor.isActive('taskList') ? 'soft' : 'ghost'" color="neutral"
              @click.prevent="editor.chain().focus().toggleTaskList().run()"
      >
        <UIcon name="i-lucide-list-checks"/>
      </UButton>

    </div>
    <EditorContent :editor="editor" class="p-4 border-2  bg-black/20 border-black/10 rounded-lg"/>
  </div>
</template>

<style scoped>
</style>