import React from 'react';
import { Card, Container } from 'react-bootstrap';
import { GetDashboardData } from '../../services/Dashboard.service';
import Spinner from 'react-bootstrap/Spinner';

import './style.css';
import { useNavigate } from 'react-router-dom';
import { PieChart } from '@mui/x-charts';

export const Home = () => {
  const [cards, setCards] = React.useState([]);
  const [graphs, setGraphs] = React.useState([]);
  const [loading, setLoading] = React.useState(false);
  const navigate = useNavigate();

  const cardClick = (card) => {
    if (card.link) navigate(card.link);
  }

  const SetGraphsData = (data) => {
    var arrGraphs = [];

    if (data.totalCellReportedSkipped || data.totalCellReportedCompleted)
      arrGraphs.push({
        element: <PieChart className='graph-element' style={{ margin: '0 auto' }}

          slotProps={{
            legend: {
              direction: 'column',
              position: { vertical: 'bottom', horizontal: 'middle' },
              padding: 0,
              labelStyle: {
                fontSize: 14
              },
              itemMarkWidth: 11,
              itemMarkHeight: 10,
            },
          }}
          margin={{ top: 0, bottom: 60, left: 0, right: 0 }}
          width={350}
          height={270}
          series={[
            {
              data: [
                { id: 0, value: data.totalCellReportedSkipped, label: 'Células Canceladas', color: 'rgb(220, 53, 69)' },
                { id: 1, value: data.totalCellReportedCompleted, label: 'Células Cumpridas', color: 'rgba(0, 105, 92, 0.9)' },
              ]
            }
          ]}
        />,
        group: 'weekly',
        title: 'Relatórios Entregues'
      });

    setGraphs(arrGraphs);
  }

  const SetCardsData = (data) => {
    var arrCards = [];

    arrCards.push({ type: 'info', title: <>Relatórios<br />Enviados</>, value: data.weekReports, link: '/reports/', group: 'weekly' });

    var pendingReportsType = data.cellsPendingReports > 0 ? 'danger' : 'success';
    arrCards.push({
      type: pendingReportsType, title: <>Células com<br /> Relatórios Pendentes</>, value: data.cellsPendingReports,
      link: '/cells/pending-reports', group: 'weekly'
    });

    arrCards.push({ type: !data.weekVisitors ? 'warning' : 'info', title: <>Visitantes em<br />Células</>, value: data.weekVisitors, link: '/reports/', group: 'weekly' });

    if (data.weekNewMembers)
      arrCards.push({ type: 'info', title: <>Novos membros<br />cadastrados</>, value: data.weekNewMembers, link: '/reports/', group: 'weekly' });

    if (data.weekNewCells)
      arrCards.push({ type: 'info', title: <>Novas células<br />cadastradss</>, value: data.weekNewCells, link: '/reports/', group: 'weekly' });

    let activeMembers = data.totalMembers - data.totalIdleMembers;
    arrCards.push({ type: 'info', title: <>Membros<br />Ativos</>, value: activeMembers, link: '/members/', group: 'general' });

    if (data.totalCells)
      arrCards.push({ type: 'info', title: <>Celulas<br />Ativas</>, value: data.totalCells, link: '/cells/', group: 'general' });

    if (data.totalIdleMembers)
      arrCards.push({ type: 'info', title: <>Membros<br />Inativos</>, value: data.totalIdleMembers, link: '/members/', group: 'general' });

    if (data.totalNonBaptizedMembers)
      arrCards.push({ type: 'info', title: <>Membros<br />Não Batizados</>, value: data.totalNonBaptizedMembers, link: '/members/', group: 'general' });

    setCards(Object.entries(Object.groupBy(arrCards, ({ group }) => group)));
  }

  React.useEffect(() => {
    setLoading(true);
    GetDashboardData().then((data) => {
      SetCardsData(data);
      SetGraphsData(data);
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
              <h3>{group[0] == 'weekly' ? 'Esta Semana' : 'Geral'}</h3>
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
              {graphs && graphs.length > 0 && (
                <div className='graphs'>
                  {graphs
                    .filter(x => x.group === group[0])
                    .map((graph, index) => <div className='graph-wrapper'><h5>{graph.title}</h5>{graph.element}</div>)}
                </div>
              )}
            </>
          ))}


        </div>
      </>
  );
};
