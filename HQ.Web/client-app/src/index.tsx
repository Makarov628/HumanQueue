import React from 'react';
import ReactDOM from 'react-dom/client';
import { createTheme, ThemeProvider, responsiveFontSizes, Theme } from '@mui/material/styles';
import { BrowserRouter as Router } from 'react-router-dom';
import App from './App';
import { I18nextProvider } from 'react-i18next';
import i18n from './components/localization/i18n';
import { SnackbarProvider } from 'notistack';


let theme: Theme = createTheme({
  palette: {
    primary: {
      main: '#009c7a',
    },
    background: {
      default: '#FFFFFF',
    },
    secondary: {
      main: '#005758'
    }
  }
});

theme = responsiveFontSizes(theme);

const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);
root.render(
  <SnackbarProvider>
    <I18nextProvider i18n={i18n}>
      <ThemeProvider theme={theme}>
        <Router>
          <App />
        </Router>
      </ThemeProvider>
    </I18nextProvider>
  </SnackbarProvider>
);
