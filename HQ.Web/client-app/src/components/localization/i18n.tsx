import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';
import enTranslation from './en.json';
import ruTranslation from './ru.json';
import kzTranslation from './kz.json';

i18n.use(initReactI18next).init({
    lng: 'kk', 
    fallbackLng: 'kk', 
    resources: {
      en: {
        translation: enTranslation
      },
      ru: {
        translation: ruTranslation
      },
      kk: {
        translation: kzTranslation
      }
    },
    interpolation: {
      escapeValue: false
    }
  });
  
  export default i18n;