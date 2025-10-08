// src/i18n/i18n.ts

import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';
import LanguageDetector from 'i18next-browser-languagedetector';
import Backend from 'i18next-http-backend';

// Inicialização do i18next
i18n
  // Carrega traduções via http (geralmente em /public/locales)
  .use(Backend)
  // Detecta o idioma do usuário
  .use(LanguageDetector)
  // Passa o i18n para react-i18next
  .use(initReactI18next)
  // Inicializa o i18next
  .init({
    fallbackLng: 'pt-BR', // Idioma padrão/fallback
    supportedLngs: ['pt-BR', 'en-US'],
    load: 'currentOnly',
    debug: import.meta.env.DEV, // Debug apenas em desenvolvimento
    backend: {
      loadPath: `${import.meta.env.VITE_API_URL}/locales/{{lng}}/{{ns}}.json`,
      expirationTime: 7 * 24 * 60 * 60 * 1000,
      defaultVersion: 'v1',
      store: window.localStorage,
    },
    // Namespace padrão usado
    ns: ['common', 'auth'],
    defaultNS: 'common',
    
    interpolation: {
      escapeValue: false, // Não necessário para React
    },
    
    // Opção para não carregar traduções ausentes
    saveMissing: false,
    
    // Configuração de detecção de idioma
    detection: {
      order: ['querystring', 'localStorage', 'cookie', 'navigator', 'htmlTag'],
      caches: ['localStorage', 'cookie'],
      lookupQuerystring: 'lang',
      lookupLocalStorage: 'i18nextLng',
      lookupCookie: 'i18next',
    }
  });

export default i18n;