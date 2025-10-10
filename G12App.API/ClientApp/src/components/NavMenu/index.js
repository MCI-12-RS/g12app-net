import React, { useState } from 'react';
import { Navbar, Container, Nav, NavDropdown, Badge } from 'react-bootstrap';
import { Link, useNavigate } from "react-router-dom";

import { useToastContext } from '../../contexts/ToastContext';
import { useAuthenticationContext } from '../../contexts/AuthenticationContext';


import './style.css';
import logo from '../../images/logo.png';
import { ToBRDate } from '../../shared/utils';
import moment from 'moment';

export const NavMenu = () => {

  const toastContext = useToastContext();
  const authContext = useAuthenticationContext();
  const navigate = useNavigate();
  const [expanded, setExpanded] = useState(false);

  const doLogoff = () => {
    authContext.logOff();
    window.history.replaceState({}, '')
    toastContext.addToast({
      title: 'Ate a proxima!', hideDelay: 5000,
      description: 'Logout efetuado com sucesso.'
    });
  }

  const startOfCurrentWeek = ToBRDate(moment().startOf('week'));
  const startOfPreviosWeek = ToBRDate(moment().subtract(1, 'week').startOf('week'));
  const startOfWeekBefore = ToBRDate(moment().subtract(2, 'week').startOf('week'));

  const endOfCurrentWeek = ToBRDate(moment().endOf('week'));
  const endOfPreviosWeek = ToBRDate(moment().subtract(1, 'week').endOf('week'));
  const endOfWeekBefore = ToBRDate(moment().subtract(2, 'week').endOf('week'));

  const gotoReports = (start, end) => {
    
    navigate('/reports', { state: { startDate: start, endDate: end } });
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

                  {authContext.auth.user.role === 'AppAdmin' && (
                  // <Nav.Link as={Link} to="/ministries" onClick={() => setExpanded(false)}>Meu Ministério</Nav.Link>
                  <></>
                  )}

                  {authContext.auth.user.role === 'AppSuperAdmin' && (
                  // <Nav.Link as={Link} to="/ministries" onClick={() => setExpanded(false)}>Ministérios</Nav.Link>
                  <></>
                  )}

                  <NavDropdown title="Pessoas" id="basic-nav-dropdown" renderMenuOnMount={true}>
                    <NavDropdown.Item as={Link} to="/members" onClick={() => setExpanded(false)}>Gerenciar pessoas</NavDropdown.Item>
                    <NavDropdown.Item as={Link} to="/new-member" onClick={() => setExpanded(false)}>Cadastrar pessoa</NavDropdown.Item>
                  </NavDropdown>

                  <NavDropdown title="Células" id="basic-nav-dropdown" renderMenuOnMount={true}>
                    <NavDropdown.Item as={Link} to="/cells" onClick={() => setExpanded(false)}>Gerenciar células</NavDropdown.Item>
                    <NavDropdown.Item as={Link} to="/new-cell" onClick={() => setExpanded(false)}>Cadastrar célula</NavDropdown.Item>
                    <NavDropdown.Divider />
                    <NavDropdown.Item as={Link} to="/log-report" onClick={() => setExpanded(false)}>Registrar relatório</NavDropdown.Item>
                    <NavDropdown.Divider />
                    <NavDropdown.Item onClick={() => { setExpanded(false); gotoReports(startOfCurrentWeek, endOfCurrentWeek); }}>Relatórios desta semana</NavDropdown.Item>
                    <NavDropdown.Item onClick={() => { setExpanded(false); gotoReports(startOfPreviosWeek, endOfPreviosWeek); }}>Relatórios de semana passada</NavDropdown.Item>
                    <NavDropdown.Item onClick={() => { setExpanded(false); gotoReports(startOfWeekBefore, endOfWeekBefore); }}>Relatórios de semana retrasada</NavDropdown.Item>
                  </NavDropdown>
                </>
              )}

              {authContext.auth.user.role === 'AppUser' && (
                <>
                  {/* <Nav.Link as={Link} to="/ministries" onClick={() => setExpanded(false)}>Meu Ministério</Nav.Link> */}
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
