import React from "react";
import { QueryClient, QueryClientProvider } from "react-query";
import "./App.css";
import HeaderOptions from "./components/HeaderOptions";
import Routing from "./routes/Routing";

const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      refetchOnWindowFocus: false,
      retry: 3,
      retryDelay: 3000,
    },
  },
});

export const MyContext = React.createContext<{
  refreshStatus: boolean;
  setRefreshStatus: React.Dispatch<React.SetStateAction<boolean>>;
  authenticated: boolean;
  setAuthenticated: React.Dispatch<React.SetStateAction<boolean>>;
  shouldRefresh: boolean;
  setShouldRefresh: React.Dispatch<React.SetStateAction<boolean>>;
}>({
  refreshStatus: false,
  setRefreshStatus: () => {},
  authenticated: false,
  setAuthenticated: () => {},
  shouldRefresh: true,
  setShouldRefresh: () => {},
});

function App() {
  return (
    <QueryClientProvider client={queryClient}>
        <div className="App">
          <HeaderOptions />
          <Routing />
        </div>
    </QueryClientProvider>
  );
}

export default App;
