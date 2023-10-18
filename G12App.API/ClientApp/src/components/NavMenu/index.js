import React, { Component } from 'react';
import { Navbar, Container, Nav, NavDropdown, Button } from 'react-bootstrap';
import { Link } from "react-router-dom";

import './style.css';
import logo from '../../images/logo.png';

export class NavMenu extends Component {
  static displayName = NavMenu.name;

  constructor (props) {
    super(props);

    this.toggleNavbar = this.toggleNavbar.bind(this);
    this.state = {
      collapsed: true
    };
  }

  toggleNavbar () {
    this.setState({
      collapsed: !this.state.collapsed
    });
  }

  render() {
    return (
      <header>        
        <Navbar expand="lg" className="bg-body-tertiary">
            <Container>
            <Navbar.Brand as={Link} to="/">
              <img className="logo" width="64" height="64" src={logo} />
            </Navbar.Brand>
            <Navbar.Toggle aria-controls="basic-navbar-nav" />
            <Navbar.Collapse id="basic-navbar-nav">
              <Nav className="me-auto">
                <Nav.Link as={Link} to="/">Dashboard</Nav.Link>
               
                <NavDropdown title="Ministérios" id="basic-nav-dropdown" renderMenuOnMount={true}>
                  <NavDropdown.Item as={Link} to="/members">Gerenciar pessoas</NavDropdown.Item>
                  <NavDropdown.Item as={Link} to="/new-member">Cadastrar pessoa</NavDropdown.Item>
                  <NavDropdown.Divider />                  
                  <NavDropdown.Item as={Link} to="/ministries">Ver ministérios</NavDropdown.Item>
                </NavDropdown>
                
                <NavDropdown title="Células" id="basic-nav-dropdown" renderMenuOnMount={true}>
                  <NavDropdown.Item as={Link} to="/cells">Gerenciar</NavDropdown.Item>
                  <NavDropdown.Item as={Link} to="/new-cell">Cadastrar</NavDropdown.Item>
                </NavDropdown> 
                
              </Nav>
            
              <div className="btn-wrapper">
                <Button className="register-report" as={Link} to="/log-report">Registrar relatório</Button>                
              </div>
            </Navbar.Collapse>
          </Container>
        </Navbar>
      </header>
    );
  }
}
