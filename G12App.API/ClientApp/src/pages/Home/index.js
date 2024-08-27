import React from 'react';
import { Card, Container } from 'react-bootstrap';
import { GetDashboardData } from '../../services/Dashboard.service';
import Spinner from 'react-bootstrap/Spinner';

import './style.css';
import { useNavigate } from 'react-router-dom';

export const Home = () => {
  const [cards, setCards] = React.useState([]);
  const [loading, setLoading] = React.useState(false);
  const navigate = useNavigate();

  const cardClick = (card) => {
    if (card.link) navigate(card.link);
  }

  const SetCardsData = (data) => {

    var arrCards = [];

    if (data.totalMembers)
      arrCards.push({ type: 'info', title: <>Membros</>, value: data.totalMembers, link: '/members/' });

    if (data.totalCells) {
      arrCards.push({ type: 'info', title: <>Celulas<br />Ativas</>, value: data.totalCells, link: '/cells/' });


      var pendingReports = Math.max(0, data.totalCells - data.weekReports);
      var pendingReportsType = pendingReports > 0 ? 'warning' : 'success';
      arrCards.push({ type: pendingReportsType, title: <>Relatorios<br />Pendentes</>, value: pendingReports });
    }

    setCards(arrCards);
  }

  React.useEffect(() => {
    setLoading(true);
    GetDashboardData().then((data) => {
      SetCardsData(data);
    })
      .catch((error) => {
        console.log('Error while loading dashboard data.', error);
      })
      .finally(() => {
        setLoading(false);
      });
  }, []);

  return (
    loading ?

      <Spinner animation="border" /> :

      <>
        <Container fluid className="dashboard">
          <h1>Dashboard</h1>
          <hr />

          <Container fluid className="dashboard-cards">
            {cards.map((card, index) => (
              <Card bg={card.type} key={index} text="" className={"mb-2" + (card.link && ' clickable')} onClick={() => cardClick(card)}>
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
