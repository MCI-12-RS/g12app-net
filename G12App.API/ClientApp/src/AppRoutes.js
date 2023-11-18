import { Home } from "./pages/Home";
import { Cells } from "./pages/Cells";
import { CellDetails } from "./pages/Cells/details";
import { Members } from "./pages/Members";
import { MemberDetails } from "./pages/Members/details";
import { Report } from "./pages/Report";
import { LoginPage } from "./pages/Login";
import { ReportDetails } from "./pages/Report/details";

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
    element: <ReportDetails key="log-report" />
  },
  {
    path: '/login',
    element: <LoginPage key="login" />,
    public: true
  }
];

export default AppRoutes;
