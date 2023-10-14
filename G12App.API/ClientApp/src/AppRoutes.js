import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import { Home } from "./components/Home";
import { Members } from "./pages/Members"

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/counter',
    element: <Counter />
  },
  {
    path: '/fetch-data',
    element: <FetchData />
  },
  {
    path: '/members',
    element: <Members />
  },
  {
    path: '/new-member',
    element: <Home />
  },
  {
    path: '/cells',
    element: <Home />
  },
  {
    path: '/new-cell',
    element: <Home />
  },
  {
    path: '/log-report',
    element: <Home />
  }
];

export default AppRoutes;
