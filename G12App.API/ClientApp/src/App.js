import { Component } from 'react';
import { Route, Routes } from 'react-router-dom';
import AppRoutes from './AppRoutes';
import { Layout } from './components/Layout';
import './custom.css';
import { ToastContextProvider } from './contexts/ToastContext';
import { AuthenticationContextProvider } from './contexts/AuthenticationContext';
import { PrivateRoute } from './components/PrivateRoute';

export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
      <ToastContextProvider>
        <AuthenticationContextProvider>
          <Layout authenticated={this.state?.authenticated}>
            <Routes>
              {AppRoutes.map((route, index) => {
                const { element: CompoElement, ...rest } = route;
                if (route.public)
                  return <Route key={index} {...rest} element={CompoElement} />;
                return <Route key={index} {...rest} element={<PrivateRoute>{CompoElement}</PrivateRoute>} />;
              })}
            </Routes>
          </Layout>
        </AuthenticationContextProvider>
      </ToastContextProvider>
    );
  }
}
