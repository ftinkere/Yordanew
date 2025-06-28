import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import laravel from "laravel-vite-plugin";
import tailwindcss from '@tailwindcss/vite'
import ui from '@nuxt/ui/vite'

// https://vite.dev/config/
export default defineConfig({
    plugins: [
        laravel({
            input: ["./Client/app.js"],
            publicDirectory: "./wwwroot/build",
            refresh: true,
        }),
        vue(),
        tailwindcss(),
        ui({
            inertia: true,
            ui: {
                button: {
                    slots: {
                        base: 'justify-center cursor-pointer',
                    }
                },
                switch: {
                    slots: {
                        label: 'cursor-pointer',
                        base: 'cursor-pointer',
                    }
                },
                colors: {
                    primary: 'amber',
                    secondary: 'zinc',
                    success: 'teal',
                    warning: 'yellow',
                    error: 'rose',
                    neutral: 'zinc'
                }
            }
        }),
    ],
    server: {
        host: 'localhost',
        port: 5173,
        cors: true,
    },
    build: {
        manifest: true,
        outDir: './wwwroot/build',
        emptyOutDir: true,
    },
})
