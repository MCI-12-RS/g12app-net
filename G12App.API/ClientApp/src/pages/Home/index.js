import React from 'react';
import { Card, Container } from 'react-bootstrap';

import { useToastContext } from '../../contexts/ToastContext';

import './style.css';

export const Home = () => {
  const [cards, setCards] = React.useState([]);
  let toastContext = useToastContext();

  const addToast = () => {
    let id = Math.random() * 1000;
    toastContext.addToast({
      id: id,
      title: 'Sucesso',
      hideDelay: 5000,
      description: 'Mensagem de sucesso ' + id
    });
  };

  React.useEffect(() => {
    setCards([
      { type: 'info', title: <>Membros</>, value: 34 },
      { type: 'info', title: <>Celulas Ativas</>, value: 34 },
      { type: 'warning', title: <>Relatorios Pendentes</>, value: 10 },
      { type: 'success', title: <>Consolidados <br />nesta semana</>, value: 8 },
      { type: 'success', title: <>Visitantes em <br />celulas nesta semana</>, value: 8 },
    ]);
  }, []);

  return (
    <>
      <Container fluid className="dashboard">
        <h1>Dashboard</h1>
        <hr />

        <Container fluid className="dashboard-cards">
          {cards.map((card, index) => (
            <Card bg={card.type} key={index} text="" className="mb-2">
              <Card.Body>
                <Card.Title>{card.value}</Card.Title>
                <Card.Text>
                  {card.title}
                </Card.Text>
              </Card.Body>
            </Card>
          ))}

        </Container>

      </Container>
    </>
  );
};
