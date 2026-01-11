<script setup>
import Layout from "../../Layout.vue";
import { Link, usePage } from "@inertiajs/vue3";
import { provide, ref, computed, onMounted, onUnmounted, watch } from "vue";
import * as CRC from "crc-32";

const language = usePage().props.language
const article = usePage().props.article
const flash = usePage().props.flash
provide('language', language)
const userId = usePage().props.auth?.user?.id
const justCreated = computed(() => {
    const flashValue = flash?.justCreated
    return flashValue === article.id || flashValue === article.id.toLowerCase()
})

// Флаг показа навигации
const showQuickNav = ref(false);
const lastElement = ref(null);

function setLastLexemeRef(el, index) {
  if (index === sortedLexemes.value.length - 1) {
    lastElement.value = el;
  }
}

// Проверяем один раз при монтировании, видна ли последняя лексема
onMounted(async () => {
  const checkVisibility = () => {
    console.log(lastElement.value);

    if (showQuickNav.value) return;

    const lastLexeme = lastElement.value;
    if (!lastLexeme) return;

    const rect = lastLexeme.getBoundingClientRect();
    const viewportHeight = window.innerHeight;

    // Если последняя лексема не видна (ниже viewport), показываем навигацию
    // rect.bottom может быть чуть-чуть больше viewportHeight из-за отступов или дробных значений, 
    // но если он значительно больше, значит элемент точно не влез.
    // Также проверим document.documentElement.scrollHeight > viewportHeight как более надежный признак наличия скролла
    const hasScroll = document.documentElement.scrollHeight > viewportHeight + 5;
    
    if (rect.bottom > viewportHeight || hasScroll) {
      showQuickNav.value = true;
    }
  };

  // Проверяем несколько раз, так как контент (картинки) может подгружаться
  checkVisibility();
  
  // Дополнительно следим за установкой рефа
  watch(lastElement, () => {
    checkVisibility();
  });

  const observer = new ResizeObserver(() => {
    checkVisibility();
  });
  
  if (document.body) {
    observer.observe(document.body);
  }

  // На всякий случай проверяем через некоторое время
  setTimeout(checkVisibility, 500);
  setTimeout(checkVisibility, 2000);

  onUnmounted(() => {
    observer.disconnect();
  });
});

// Сортировка лексем по пути
function parsePath(path) {
    return path.split(".").map(u => parseInt(u, 10))
}

const sortedLexemes = computed(() => {
    return [...article.lexemes].sort((a, b) => {
        const partsA = parsePath(a.path)
        const partsB = parsePath(b.path)

        for (let i = 0; i < Math.max(partsA.length, partsB.length); i++) {
            const numA = partsA[i] || 0
            const numB = partsB[i] || 0
            if (numA !== numB) return numA - numB
        }
        return 0
    })
})

const colors = ['red', 'orange', 'amber', 'yellow', 'lime', 'green', 'emerald', 'teal', 'cyan', 'sky', 'blue', 'indigo', 'violet', 'purple', 'fuchsia', 'pink', 'rose']
function tagColor(tag) {
    const hash = CRC.str(tag)
    const index = (hash % colors.length + colors.length) % colors.length
    return colors[index];
}
</script>

<template>
  <Layout>
    <template v-if="userId === language.author.id" #top-right>
      <Link :href="`/dictionary/${article.id}/edit`">
        <UButton
          icon="i-lucide-pencil"
          variant="soft"
          color="primary">
          Редактировать
        </UButton>
      </Link>
    </template>

    <div class="flex flex-col gap-6 w-full">
      <!-- Article Header Card -->
      <div class="bg-white/5 backdrop-blur-xl border border-white/10 rounded-3xl p-8 shadow-xl shadow-black/20">
        <div class="flex flex-col gap-3">
          <div class="flex items-baseline gap-3">
            <h1 class="text-4xl font-bold text-white">{{ article.lemma }}</h1>
            <span v-if="article.transcription" class="text-2xl text-green-400/80">
              /{{ article.transcription }}/
            </span>
          </div>
          <p v-if="article.adaptation && article.adaptation !== article.lemma"
             class="text-white/50 text-lg">
            {{ article.adaptation }}
          </p>
        </div>
      </div>

      <!-- Images Card -->
      <div v-if="article.files && article.files.length > 0"
           class="bg-white/5 backdrop-blur-xl border border-white/10 rounded-3xl p-6 shadow-xl shadow-black/20">
        <div class="flex items-center gap-2 mb-4 pb-4 border-b border-white/10">
          <UIcon name="i-lucide-image" class="size-5 text-blue-400" />
          <h2 class="text-xl font-semibold text-white">Изображения</h2>
        </div>
        <div class="flex flex-wrap gap-4">
          <div v-for="fileId in article.files" :key="fileId">
            <img
              :src="`/dictionary/files/${fileId}`"
              alt="Изображение статьи"
              class="h-40 rounded-2xl border border-white/10 hover:border-white/20 transition-all" />
          </div>
        </div>
      </div>

      <!-- Quick Navigation -->
      <div v-if="showQuickNav"
           class="z-30 bg-white/5 backdrop-blur-xl border border-white/10 rounded-3xl p-4 shadow-xl shadow-black/20">
        <div class="flex items-center gap-2 mb-3">
          <UIcon name="i-lucide-list" class="size-4 text-amber-400" />
          <span class="text-sm font-semibold text-white">Быстрая навигация</span>
        </div>
        <div class="flex flex-col gap-1 max-h-48 overflow-y-auto">
          <div v-for="lexeme in sortedLexemes" :key="lexeme.id" class="flex gap-2 text-sm">
            <span class="font-mono text-green-400 shrink-0">{{ lexeme.path }}</span>
            <div class="ProseMirror line-clamp-1 first-line-fixed text-white/70" v-html="lexeme.description"></div>
          </div>
        </div>
      </div>

      <!-- Lexemes Card -->
      <div class="bg-white/5 backdrop-blur-xl border border-white/10 rounded-3xl p-8 shadow-xl shadow-black/20">
        <div class="flex items-center gap-2 mb-6 pb-4 border-b border-white/10">
          <UIcon name="i-lucide-book-text" class="size-5 text-green-400" />
          <h2 class="text-xl font-semibold text-white">Определения</h2>
        </div>

        <div class="flex flex-col gap-6 divide-y divide-white/10">
          <div
            v-for="(lexeme, index) in sortedLexemes"
            :key="lexeme.id"
            :ref="el => setLastLexemeRef(el, index)"
            class="pt-6 first:pt-0">
            <!-- Lexeme Number -->
            <div class="flex items-start gap-4 mb-3">
              <span class="font-mono text-xl font-bold text-green-400 shrink-0">
                {{ lexeme.path }}
              </span>
              <div class="flex-1">
                <!-- Description -->
                <div class="ProseMirror text-white/90 mb-3" v-html="lexeme.description"></div>

                <!-- Tags -->
                <div v-if="lexeme.tags && lexeme.tags.length > 0" class="flex flex-wrap gap-2">
                  <div
                    v-for="tag in lexeme.tags"
                    :key="tag"
                    class="tag"
                    :data-color="tagColor(tag)">
                    {{ tag }}
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Add Another Word Card (only if just created) -->
      <div v-if="justCreated && userId === language.author.id"
           class="bg-linear-to-r from-green-500/10 to-emerald-500/10 backdrop-blur-xl border border-green-400/20 rounded-3xl p-6 shadow-xl shadow-black/20">
        <div class="flex flex-col sm:flex-row items-center gap-4">
          <div class="w-12 h-12 rounded-full bg-green-500/20 flex items-center justify-center shrink-0">
            <UIcon name="i-lucide-check-circle" class="size-6 text-green-400" />
          </div>
          <div class="flex-1 text-center sm:text-left">
            <h3 class="text-lg font-semibold text-green-400 mb-1">Слово успешно добавлено!</h3>
            <p class="text-white/70 text-sm">
              Хотите добавить ещё одно слово в словарь?
            </p>
          </div>
          <Link :href="`/languages/${language.id}/dictionary/create`">
            <UButton
              icon="i-lucide-plus"
              size="lg"
              class="bg-linear-to-r from-green-500/80 to-emerald-500/80 hover:from-green-500 hover:to-emerald-500 border-green-400/50">
              <div class="flex items-center gap-2">
                <span class="font-semibold">Добавить ещё</span>
              </div>
            </UButton>
          </Link>
        </div>
      </div>
    </div>
  </Layout>
</template>

<style scoped>
.first-line-fixed {
    overflow: hidden;
    line-height: 1.4em;
    max-height: 1.4em;
    white-space: pre-line;
}
</style>