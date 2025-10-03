import React, { useEffect, isValidElement, cloneElement } from 'react';
import { Route, Routes, useLocation } from 'react-router-dom';
import AppRoutes from './AppRoutes';
import { Layout } from './components/Layout';
import './custom.css';
import { ToastContextProvider } from './contexts/ToastContext';
import { AuthenticationContextProvider } from './contexts/AuthenticationContext';
import { PrivateRoute } from './components/PrivateRoute';

const App = () => {
  const location = useLocation();

    return (
      <ToastContextProvider>
        <AuthenticationContextProvider>
          <Layout>
            <Routes>
              {AppRoutes.map((route, index) => {
                const { element: CompoElement, ...rest } = route;
                const elementWithKey = isValidElement(CompoElement)
                  ? cloneElement(CompoElement, { key: location.key })
                  : CompoElement;
                if (route.public)
                  return <Route key={index} {...rest} element={elementWithKey} />;
                return <Route key={index} {...rest} element={<PrivateRoute key={location.key}>{elementWithKey}</PrivateRoute>} />;
              })}
            </Routes>
          </Layout>
        </AuthenticationContextProvider>
      </ToastContextProvider>
    );
};

export default App;
