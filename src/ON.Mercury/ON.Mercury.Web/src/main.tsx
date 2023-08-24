import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App";
import "./index.css";
import "./assets/app.css";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { Provider } from "react-redux";
import { store } from "./app/store";

globalThis.token =
  "Bearer eyJhbGciOiJFUzI1NiIsImtpZCI6ImY2MGIyM2Q3LWE3YmMtNDJjYy1hNGI0LWU3YmYyNDg2YmM4OSIsInR5cCI6IkpXVCJ9.eyJJZCI6ImVmYjY2MWNjLTEwYTEtNDNmNi05NTU4LTY3ZmUwMzZlYzg4MCIsInN1YiI6ImFtaW5nc3QiLCJEaXNwbGF5IjoiYV9taW5nc3QiLCJNZXJjdXJ5UHJvZmlsZSI6IntcIklkXCI6XCJlZmI2NjFjYy0xMGExLTQzZjYtOTU1OC02N2ZlMDM2ZWM4ODBcIixcIlVzZXJuYW1lXCI6XCJhX21pbmdzdFwiLFwiUm9sZXNcIjpbe1wiSWRcIjpcIjliYjhkNjUyLWNjMjMtNDk3ZC1hMjIyLWVhNGM2ODFjMmE2NFwiLFwiTmFtZVwiOlwiZXZlcnlvbmVcIixcIkhpZXJhcmNoeVwiOjIsXCJQZXJtaXNzaW9uc1wiOntcImFubm91bmNlXCI6ZmFsc2UsXCJnb2RfbW9kZVwiOmZhbHNlLFwidmlld19sb2dzXCI6ZmFsc2UsXCJiYW5fbWVtYmVyXCI6ZmFsc2UsXCJlbWJlZF9saW5rXCI6dHJ1ZSxcImtpY2tfbWVtYmVyXCI6ZmFsc2UsXCJtZW50aW9uX2FsbFwiOmZhbHNlLFwibW92ZV9tZW1iZXJcIjpmYWxzZSxcInBpbl9tZXNzYWdlXCI6dHJ1ZSxcInJlYWRfaGlzdG9yeVwiOnRydWUsXCJzZW5kX21lc3NhZ2VcIjp0cnVlLFwidmlld19jaGFubmVsXCI6dHJ1ZSxcInRpbWVvdXRfbWVtYmVyXCI6ZmFsc2UsXCJtYW5hZ2VfbWVzc2FnZXNcIjpmYWxzZSxcImRpc2Nvbm5lY3RfbWVtYmVyXCI6ZmFsc2UsXCJzZXJ2ZXJfbXV0ZV9tZW1iZXJcIjpmYWxzZSxcInNlcnZlcl9kZWFmZW5fbWVtYmVyXCI6ZmFsc2V9LFwiQ3JlYXRlZE9uXCI6XCIyMDIzLTA4LTE0VDIwOjEzOjA2LjIxNjE5NS0wNDowMFwifV19IiwiTWVyY3VyeVByb2ZpbGVFeHAiOiIyNTM0MDIzMDA3OTkiLCJuYmYiOjE2OTI2NTYzMzMsImV4cCI6MTY5MzI2MTEzMywiaWF0IjoxNjkyNjU2MzMzfQ._n9BW4h4bvTdaZmUsQX17g1EHBeqkfPckKqNvS4K0LMQpv21FgBmvF19ROZuXjO5Z09J05JSRFzHF1wV3CVPNQ";

const queryClient = new QueryClient();
ReactDOM.createRoot(document.getElementById("root")!).render(
  <React.StrictMode>
    <Provider store={store}>
      <QueryClientProvider client={queryClient}>
        <App />
      </QueryClientProvider>
    </Provider>
  </React.StrictMode>,
);
