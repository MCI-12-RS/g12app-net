import React, { useState } from 'react';
import { Navbar, Container, Nav, NavDropdown, Badge } from 'react-bootstrap';
import { Link } from "react-router-dom";

import { useToastContext } from '../../contexts/ToastContext';
import { useAuthenticationContext } from '../../contexts/AuthenticationContext';

import './style.css';
import logo from '../../images/logo.png';

export const NavMenu = () => {

  const toastContext = useToastContext();
  const authContext = useAuthenticationContext();
  const [expanded, setExpanded] = useState(false);

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
      <Navbar expand="lg" className="bg-body-tertiary navbar-dark" bg="dark" data-bs-theme="dark" expanded={expanded}>
        <Container>
          <Navbar.Brand as={Link} to="/">
            <img className="logo" width="64" height="64" src={logo} />
          </Navbar.Brand>
          <Navbar.Toggle aria-controls="basic-navbar-nav" onClick={() => setExpanded(!expanded)} />
          <Navbar.Collapse id="basic-navbar-nav">
            <Nav className="me-auto">
              <Nav.Link as={Link} to="/" onClick={() => setExpanded(false)}>Dashboard</Nav.Link>

              {authContext.auth.user.role !== 'AppUser' && (
                <>
                  <NavDropdown title="Pessoas" id="basic-nav-dropdown" renderMenuOnMount={true}>
                    <NavDropdown.Item as={Link} to="/members" onClick={() => setExpanded(false)}>Gerenciar pessoas</NavDropdown.Item>
                    <NavDropdown.Item as={Link} to="/new-member" onClick={() => setExpanded(false)}>Cadastrar pessoa</NavDropdown.Item>
                  </NavDropdown>

                  <NavDropdown title="Células" id="basic-nav-dropdown" renderMenuOnMount={true}>
                    <NavDropdown.Item as={Link} to="/cells" onClick={() => setExpanded(false)}>Gerenciar células</NavDropdown.Item>
                    <NavDropdown.Item as={Link} to="/new-cell" onClick={() => setExpanded(false)}>Cadastrar célula</NavDropdown.Item>
                    <NavDropdown.Divider />
                    <NavDropdown.Item as={Link} to="/reports" onClick={() => setExpanded(false)}>Visualizar relatórios</NavDropdown.Item>
                    <NavDropdown.Item as={Link} to="/log-report" onClick={() => setExpanded(false)}>Registrar relatório</NavDropdown.Item>
                  </NavDropdown>
                </>
              )}

              {authContext.auth.user.role === 'AppUser' && (
                <>

                  <Nav.Link as={Link} to="/members" onClick={() => setExpanded(false)}>Pessoas</Nav.Link>
                  <Nav.Link as={Link} to="/cells" onClick={() => setExpanded(false)}>Células</Nav.Link>
                  <Nav.Link as={Link} to="/log-report" onClick={() => setExpanded(false)}>Registrar Relatório</Nav.Link>
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
