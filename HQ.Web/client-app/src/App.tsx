import React from 'react';
import { Route, Routes } from 'react-router-dom';
import Operator from './components/operator/Operator';
import Terminal from './components/terminal/Terminal';
import Window from './components/window/Window';
import Admin from './components/admin/Admin';
import NotFound from './components/NotFound';
import useToken from './components/common/Token';
import Login from './components/auth/Login';
import { SnackbarProvider, useSnackbar } from 'notistack';
import Request from './components/request/Request';
import { SignalRContext } from './signalr/signalr'





function App() {
  const { token, isAdmin } = useToken();
  const { enqueueSnackbar } = useSnackbar();

  const isAuth = () => {
    const auth = !token && token !== "" && token !== undefined;
    return !auth;
  }

  const websocketAlert = (text: string, variant: "default" | "error" | "success" | "warning" | "info") => {
    enqueueSnackbar(text, { variant })
  }

  return (
      <SignalRContext.Provider
        url={`${window.location.origin}/hub`}
        connectEnabled={true}
        onOpen={() => { websocketAlert("Соединение с сервером установлено",  "info" ) }}
        onClosed={() => websocketAlert("Соединение с сервером закрыто", "info" )}
        onReconnect={() => websocketAlert("Соединение с сервером восстановлено", "success" )}
        onError={(error) => { websocketAlert(`Соединение с сервером потеряно: ${error?.message ?? error}`, "error" ); return Promise.resolve() }}
      >

        <Routes>
          {/* <Route path="/" element={
          isAuth() ? <Operator/> : <Login />
        } 
      /> */}
          {/* <Route path="/admin" element={
          isAuth() && isAdmin() ? <Admin/> : <Login />
        } /> */}
          <Route path="/terminal/:terminalId" element={<Terminal />} />
          <Route path="/tablo/:queueId" element={<Window />} />
          <Route path="/window/:windowId" element={<Operator />} />
          <Route path="/request/:requestId" element={<Request />} />
          <Route element={<NotFound />} />
        </Routes>
      </SignalRContext.Provider>
  );
}

export default App;
