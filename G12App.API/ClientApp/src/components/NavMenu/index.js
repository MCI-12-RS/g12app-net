import React from 'react';
import { Navbar, Container, Nav, NavDropdown, Badge } from 'react-bootstrap';
import { Link } from "react-router-dom";

import { useToastContext } from '../../contexts/ToastContext';
import { useAuthenticationContext } from '../../contexts/AuthenticationContext';

import './style.css';
import logo from '../../images/logo.png';

export const NavMenu = () => {

  const toastContext = useToastContext();
  const authContext = useAuthenticationContext();

  const doLogoff = () => {
    authContext.logOff();
    window.history.replaceState({}, '')
    toastContext.addToast({
      title: 'Ate a proxima!', hideDelay: 5000,
      description: 'Logout efetuado com sucesso.'
    });
  }

  return (
    <header>
      <Navbar expand="lg" className="bg-body-tertiary navbar-dark" bg="dark" data-bs-theme="dark">
        <Container>
          <Navbar.Brand as={Link} to="/">
            <img className="logo" width="64" height="64" src={logo} />
          </Navbar.Brand>
          <Navbar.Toggle aria-controls="basic-navbar-nav" />
          <Navbar.Collapse id="basic-navbar-nav">
            <Nav className="me-auto">
              <Nav.Link as={Link} to="/">Dashboard</Nav.Link>

              {authContext.auth.user.role !== 'AppUser' && (
                <>
                  <NavDropdown title="Pessoas" id="basic-nav-dropdown" renderMenuOnMount={true}>
                    <NavDropdown.Item as={Link} to="/members">Gerenciar pessoas</NavDropdown.Item>
                    <NavDropdown.Item as={Link} to="/new-member">Cadastrar pessoa</NavDropdown.Item>
                  </NavDropdown>

                  <NavDropdown title="Células" id="basic-nav-dropdown" renderMenuOnMount={true}>
                    <NavDropdown.Item as={Link} to="/cells">Gerenciar células</NavDropdown.Item>
                    <NavDropdown.Item as={Link} to="/new-cell">Cadastrar célula</NavDropdown.Item>
                    <NavDropdown.Divider />
                    <NavDropdown.Item as={Link} to="/reports">Visualizar relatórios</NavDropdown.Item>
                    <NavDropdown.Item as={Link} to="/log-report">Registrar relatório</NavDropdown.Item>
                  </NavDropdown>
                </>
              )}

              {authContext.auth.user.role === 'AppUser' && (
                <>
                  
                  <Nav.Link as={Link} to="/members">Pessoas</Nav.Link>
                  <Nav.Link as={Link} to="/cells">Células</Nav.Link>
                  <Nav.Link as={Link} to="/log-report">Registrar Relatório</Nav.Link>
                </>
              )}

            </Nav>

            <Nav className="ml-auto">

              <Nav.Link className="greeting-text">
                <Badge pill bg="info" text="dark">Logado como: {authContext.auth.user.name}</Badge>
              </Nav.Link>
              <Nav.Link onClick={doLogoff}>Sair</Nav.Link>
            </Nav>

          </Navbar.Collapse>
        </Container>
      </Navbar>
    </header>
  );
};
