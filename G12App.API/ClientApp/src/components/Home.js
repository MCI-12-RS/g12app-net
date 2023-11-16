import React, { Component } from 'react';
import { Alert, Button, Card } from 'react-bootstrap';

import { useToastContext } from '../contexts/ToastContext';

export const Home = () => {
  const displayName = Home.name;

  let toastContext = useToastContext();

  const addToast = () => {
    let id = Math.random()*1000;
    toastContext.addToast({
      id: id,
      title: 'Sucesso',
      hideDelay: 5000,
      description: 'Mensagem de sucesso ' + id
    });
  };

  return (
    <>
      <div>
        <h1>Dashboard</h1>
        <hr />
      </div>
    </>
  );
};
