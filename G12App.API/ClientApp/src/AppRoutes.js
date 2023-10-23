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
    element: <Home key="home" />
  },
  {
    path: '/members',
    element: <Members key="members" />
  },
  {
    path: '/new-member',
    element: <MemberDetails key="new-member" />
  },
  {
    path: '/members/:id',
    element: <MemberDetails key="member-details" />
  },
  {
    path: '/ministries',
    element: <Home key="home" />
  },
  {
    path: '/cells',
    element: <Cells key="cells" />
  },
  {
    path: '/new-cell',
    element: <CellDetails key="new-cell" />
  },
  {
    path: '/cells/:id',
    element: <CellDetails key="cell-details" />
  },
  {
    path: '/log-report',
    element: <Home key="log-report" />
  }
];

export default AppRoutes;
