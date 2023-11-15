import React from 'react';
import { Route, Navigate } from 'react-router-dom';
import { useAuthenticationContext } from '../../contexts/AuthenticationContext';

export const PrivateRoute = ({ children }) => {
    const authContext = useAuthenticationContext();
    
    return authContext.auth.authenticated ?
                <>{ children }</> : 
                <Navigate to="/login" replace />;
};