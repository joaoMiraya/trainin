import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import tailwindcss from '@tailwindcss/vite';
import path from 'path';

const root = path.resolve(__dirname, 'src')

// Mapeamento de aliases
const aliases = {
  '@': root,
  '@pages': `${root}/pages`,
  '@customApi': `${root}/api`,
  '@i18n': `${root}/i18n`,
  '@features': `${root}/features`,
  '@routes': `${root}/routes`,
  '@components': `${root}/components`,
  '@services': `${root}/services`,
  '@assets': `${root}/assets`,
  '@utils': `${root}/components/utils`,
  '@patterns': `${root}/components/patterns`,
  '@stylesheets': `${root}/stylesheets`,
  '@customTypes': `${root}/types`,
  '@constants': `${root}/constants`,
  '@middlewares': `${root}/middlewares`,
  '@scripts': `${root}/scripts`,
  '@store': `${root}/store`,
}


// https://vite.dev/config/
export default defineConfig({
  plugins: [
    react(),
    tailwindcss(),
  ],
  resolve: {
    alias: Object.fromEntries(
      Object.entries(aliases).map(([key, value]) => [
        key,
        path.resolve(__dirname, value),
      ])
    ),
  },
})
