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

    useEffect(() => {
        const storedAuth = localStorage.getItem('auth');
        if (storedAuth) {
            setAuth(JSON.parse(storedAuth));
        }
    }, []);

    useEffect(() => {
        localStorage.setItem('auth', JSON.stringify(auth));
    }, [auth]);

    return (
        <Provider value={{ auth, logOff, logIn }}>
            {children}
        </Provider>
    );
}

export { AuthenticationContextProvider, Consumer as ConsumerProvider };
export default AuthenticationContext;