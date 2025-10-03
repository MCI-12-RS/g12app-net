import { Home } from "./pages/Home";
import { Cells } from "./pages/Cells";
import { CellDetails } from "./pages/Cells/details";
import { Members } from "./pages/Members";
import { MemberDetails } from "./pages/Members/details";
import { LoginPage } from "./pages/Login";
import { ReportDetails } from "./pages/Report/details";
import { Reports } from "./pages/Report";
import { AppChanges } from "./pages/AppChanges";
import { Ministries } from "./pages/Ministries";

const AppRoutes = [
  {
    path: '/',
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
    element: <Ministries key="ministries" />
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
    path: '/cells/:cellId/reports',
    element: <Reports key="cell-reports" />
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
    path: '/reports',
    element: <Reports key="reports" />
  },
  {
    path: '/login',
    element: <LoginPage key="login" />,
    public: true
  },
  {
    path: '/last-changes',
    element: <AppChanges key="updates" />,
    public: true
  }
];

export default AppRoutes;
