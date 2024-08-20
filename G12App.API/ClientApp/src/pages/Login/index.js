import React, { useEffect, useState } from "react";
import { Button, Col, Container, InputGroup, Row, Form, Spinner } from "react-bootstrap";
import { useToastContext } from "../../contexts/ToastContext";
import { useAuthenticationContext } from "../../contexts/AuthenticationContext";
import './style.css';
import { Login } from "../../services/Auth.service";
import { useNavigate, useLocation } from "react-router-dom";
import md5 from 'md5';

import logo from '../../images/logo-login.png';

export const LoginPage = () => {

    const toastContext = useToastContext();
    const authContext = useAuthenticationContext();
    const navigate = useNavigate();
    const location = useLocation();

    const emptyLogin = {
        userName: '',
        password: ''
    };

    const [login, setLogin] = useState(emptyLogin);
    const [errorMessage, setErrorMessage] = useState(<>&nbsp;</>);
    const [loading, setLoading] = useState(false);

    const handleChangeEvent = (event) => {
        const { name, value } = event.target;
        setLogin({ ...login, [name]: value });
    }

    const doLogin = async (event) => {
        event.preventDefault();

        try {
            setLoading(true);
            var response = await Login({ 
                userName: (login.userName || '').trim(),
                password: md5(login.password) 
            });

            if (response.isValid) {

                authContext.logIn(response.data);

                toastContext.addToast({
                    title: 'Bem-vindo ' + response.data?.name + '!', hideDelay: 5000,
                    description: 'Login efetuado com sucesso.'
                });

                setErrorMessage(<>&nbsp;</>);
            }
            else {
                setLoading(false);
                setErrorMessage(response.errors.map(x => <>{x.errorMessage}<br /></>));
            }

        } catch (error) {
            setLoading(false);
            toastContext.addToast({
                title: 'Erro', hideDelay: 5000,
                description: 'Erro de sistema ao fazer login.'
            });
            console.error(error);
        }
    }

    const checkRedirect = () => {
        if (authContext.auth.authenticated) {
            setLoading(true);
            navigate(location.state?.from?.pathname ?? '/', { replace: true });
            //navigate('/', { replace: true });
        }
    };

    useEffect(() => {
        checkRedirect();
    }, [authContext.auth]);

    return (authContext.auth.authenticated ? <></> : (
        <Container fluid className="login">

            <img className="logo" width="200" height="200" src={logo} />

            {loading ? <Spinner animation="border" /> : <></>}

            <Form className={loading ? 'invisible' : ''}>
                <h3>Login</h3>
                <hr />

                <Row className="mb-3 pr-0 justify-content-md-center">
                    <Col>
                        <InputGroup className="mb-3">
                            <InputGroup.Text id="basic-addon1">Usuario</InputGroup.Text>
                            <Form.Control name="userName" aria-describedby="basic-addon1" placeholder="Celular ou email" onChange={handleChangeEvent} value={login['email']} />
                        </InputGroup>
                    </Col>
                </Row>

                <Row className="mb-3 pr-0 justify-content-md-center">
                    <Col>
                        <InputGroup className="mb-3">
                            <InputGroup.Text id="basic-addon1">Senha</InputGroup.Text>
                            <Form.Control name="password" type="password" aria-describedby="basic-addon1" onChange={handleChangeEvent} value={login['password']} />
                        </InputGroup>
                    </Col>
                </Row>

                <Row>
                    <small className="login-error">{errorMessage}</small>
                    <small className="login-error">&nbsp;</small>
                </Row>

                <Row className="mb-3 pr-0">
                    <div className="d-flex float-end">
                        <Button variant="primary" className="ms-3" type="submit" onClick={doLogin}>Entrar</Button>
                    </div>
                </Row>
            </Form>
        </Container>
    ));
}