import React, { useEffect } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import { useAuthenticationContext } from '../../contexts/AuthenticationContext';

export const PrivateRoute = ({ children }) => {
    const authContext = useAuthenticationContext();
    const location = useLocation();
    const navigate = useNavigate();

    useEffect(() => {
        console.log('PrivateRoute - Checking if user is authenticated', JSON.stringify(authContext));
        if(!authContext.auth.authenticated) {
            navigate('/login', { replace: true, state: { from: location } });
        }
    }, [authContext.auth]);
    
    return children;
};