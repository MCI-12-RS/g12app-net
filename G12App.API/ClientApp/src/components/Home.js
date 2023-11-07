import React, { Component } from 'react';
import { Alert, Card } from 'react-bootstrap';

export class Home extends Component {
  static displayName = Home.name;

  render() {
    return (
      <>
        <div>
          <h1>Dashboard</h1>
          <hr />
          
          <Alert variant="info">Bem-vindo usuário!</Alert>
        </div>
      </>
    );
  }
}
