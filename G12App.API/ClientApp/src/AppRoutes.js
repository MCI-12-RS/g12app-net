import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import { Home } from "./components/Home";
import { Cells } from "./pages/Cells";
import { CellDetails } from "./pages/Cells/details";
import { Members } from "./pages/Members"
import { MemberDetails } from "./pages/Members/details";

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
    element: <MemberDetails />
  },
  {
    path: '/members/:id',
    element: <MemberDetails />
  },
  {
    path: '/ministries',
    element: <Home />
  },
  {
    path: '/cells',
    element: <Cells />
  },
  {
    path: '/new-cell',
    element: <CellDetails />
  },
  {
    path: '/cells/:id',
    element: <CellDetails />
  },
  {
    path: '/log-report',
    element: <Home />
  }
];

export default AppRoutes;
