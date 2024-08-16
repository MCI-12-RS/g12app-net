import React, { Component } from 'react';
import { Container } from 'reactstrap';
import { NavMenu } from './NavMenu';
import { useAuthenticationContext } from '../contexts/AuthenticationContext';
import './style.css';

export const Layout = ({ children }) => {

  const authContext = useAuthenticationContext();

  return (
    <>
      <div>
        {authContext.auth.authenticated ? <NavMenu /> : <></>}
        <Container tag="main" >
          {children}
        </Container >
      </div >
      <br />
      <br />
      <br />
    </>
  );
}
