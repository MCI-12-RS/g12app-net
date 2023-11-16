import React, { useContext, useEffect } from 'react';

const AuthenticationContext = React.createContext();
export const useAuthenticationContext = () => useContext(AuthenticationContext);

const { Provider, Consumer } = AuthenticationContext;

const AuthenticationContextProvider = ({ children }) => {

    const emptyAuth = {
        authenticated: false,
        user: {
            name: '',
            email: '',
            token: ''
        }
    };

    const [auth, setAuth] = React.useState(emptyAuth);

    const logOff = () => {
        setAuth(emptyAuth);
    };

    const logIn = (user) => {
        setAuth({
            authenticated: true,
            user: {
                memberId: user.memberId,                
                name: user.name,
                userName: user.userName,
                token: user.token
            }
        });
    };

    const getHeaders = () => {
        return {
            'Authorization': 'Bearer ' + auth.user.token
        };
    };

    useEffect(() => {
        const storedAuth = localStorage.getItem('auth');
        if (storedAuth) {
            setAuth(JSON.parse(storedAuth));
        }
    }, []);

    useEffect(() => {
        localStorage.setItem('auth', JSON.stringify(auth));
        localStorage.setItem('authDate', new Date());
        localStorage.setItem('jwtToken', auth?.user?.token);
    }, [auth]);

    return (
        <Provider value={{ auth, logOff, logIn, getHeaders }}>
            {children}
        </Provider>
    );
}

export { AuthenticationContextProvider, Consumer as ConsumerProvider };
export default AuthenticationContext;