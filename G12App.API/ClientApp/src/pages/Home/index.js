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

    arrCards.push({ type: 'info', title: <>Relatórios<br />Enviados</>, value: data.weekReports, link: '/reports/', group: 'weekly' });

    var pendingReportsType = data.cellsPendingReports > 0 ? 'danger' : 'success';
    arrCards.push({ type: pendingReportsType, title: <>Células com<br /> Relatórios Pendentes</>, value: data.cellsPendingReports, link: '/cells/pending-reports', group: 'weekly' });
    
    arrCards.push({ type: 'info', title: <>Visitantes em<br />Células</>, value: data.weekVisitors, link: '/reports/', group: 'weekly' });

    arrCards.push({ type: 'info', title: <>Membros</>, value: data.totalMembers, link: '/members/', group: 'general' });

    arrCards.push({ type: 'info', title: <>Celulas<br />Ativas</>, value: data.totalCells, link: '/cells/', group: 'general' });

    setCards(Object.entries(Object.groupBy(arrCards, ({ group }) => group)));
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
        <div className="dashboard">
          {cards.map((group, index) => (
            <>
              <h3>{group[0] == 'weekly' ? 'Semanal' : 'Geral'}</h3>
              <Container fluid className="dashboard-cards">
                {group[1].map((card, index) => (
                  <Card bg={card.type} key={index} text="" className={"mb-2 " + (card.link && ' clickable')} onClick={() => cardClick(card)}>
                    <Card.Body>
                      <Card.Title>{card.value}</Card.Title>
                      <Card.Text>
                        {card.title}
                      </Card.Text>
                    </Card.Body>
                  </Card>
                ))}
              </Container>
            </>
          ))}


        </div>
      </>
  );
};
