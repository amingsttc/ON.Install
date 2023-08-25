import React, { useEffect, useState } from "react";
//import "./assets/App.css";
import "@styles/App.css";
import { buildSignalR } from "./signalR/signalR";
import { config } from "./config/config";
import { RootView } from "./views/RootView";
import { createBrowserRouter, RouterProvider } from "react-router-dom";
import { ChatChannelView } from "./views/ChatChannelView";
import Sidebar from "./components/sidebar/sidebar";
import { Message } from "./types/message";
import { ContextMenuProvider } from "./providers/ContextMenuProvider";
import { ModalProvider } from "./components/modal/ModalProvider";

globalThis.token = localStorage.getItem("jwt");

const router = createBrowserRouter([
  {
    path: "/",
    element: <RootView hubConnection={globalThis.hubConnection} />,
  },
  {
    path: "/channels/:id",
    element: <ChatChannelView />,
  },
]);

function App() {
  const [token, setToken] = useState(globalThis.token);
  const [isLoading, setIsLoading] = useState(false);
  const [showServerSettings, setShowServerSettings] = useState(false);

  useEffect(() => {
    if (token === undefined) {
      setIsLoading(true);
    } else {
      if (globalThis.hubConnection === undefined) {
        globalThis.hubConnection = buildSignalR(
          `${config.mercuryApi}/hub`,
          token as string,
        );

        globalThis.hubConnection.start();
      }
    }
  }, [token, setToken, globalThis.hubConnection]);

  return (
    <div className="App">
      <ModalProvider>
        <ContextMenuProvider>
          <RouterProvider router={router} />
        </ContextMenuProvider>
      </ModalProvider>
    </div>
  );
}

export default App;
