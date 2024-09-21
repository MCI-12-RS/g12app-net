import React from 'react';
import { Card, Container } from 'react-bootstrap';
import { GetDashboardData } from '../../services/Dashboard.service';
import Spinner from 'react-bootstrap/Spinner';

import './style.css';
import { useNavigate } from 'react-router-dom';
import { PieChart } from '@mui/x-charts';
import { ToBRDate } from '../../shared/utils';
import moment from 'moment';

export const Home = () => {
  const [cards, setCards] = React.useState([]);
  const [graphs, setGraphs] = React.useState([]);
  const [loading, setLoading] = React.useState(false);
  const navigate = useNavigate();

  const clickCellPendingReports = () => {
    navigate('/cells', { state: { reportStatus: 'overDueThisWeek' } });
  }

  const clickCellCreatedThisWeek = () => {
    navigate('/cells', { state: { createdDateFrom: ToBRDate(moment().startOf('week')) } });
  }

  const clickMemberCreatedThisWeek = () => {
    navigate('/members', { state: { createdDateFrom: ToBRDate(moment().startOf('week')) } });
  }

  const clickReportsLoggedThisWeek = () => {
    navigate('/reports', { state: { startDate: ToBRDate(moment().startOf('week')), endDate: ToBRDate(moment().endOf('week')) } });
  }

  const clickActiveMembers = () => {
    navigate('/members', { state: { idle: false } });
  }

  const clickIdleMembers = () => {
    navigate('/members', { state: { idle: true } });
  }

  const clickNonBaptizedMembers = () => {
    navigate('/members', { state: { baptized: false } });
  }

  const clickMembersWithoutLeadership = () => {
    navigate('/members', { state: { hasNoLeadership: true } });
  }

  const clickMembersWithoutCellLink = () => {
    navigate('/members', { state: { cellLink: 'none' } });
  }

  const clickTotalEvangelisticCells = () => {
    navigate('/cells', { state: { cellType: 'Evangelistic' } });
  }

  const clickTotalDiscipleshipCells = () => {
    navigate('/cells', { state: { cellType: 'Discipleship' } });
  }
  
  const cardClick = (card) => {
    if (card.link)
      navigate(card.link);
  }

  const SetGraphsData = (data) => {
    var arrGraphs = [];

    if (data.totalCellReportedSkipped || data.totalCellReportedCompleted || data.cellsPendingReports)
      arrGraphs.push({
        element: (
          <PieChart className='graph-element' style={{ margin: '0 auto' }}
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
            margin={{ top: 0, bottom: 80, left: 0, right: 0 }}
            width={350}
            height={270}
            series={[
              {
                data: [
                  { id: 0, value: data.totalCellReportedSkipped, label: data.totalCellReportedSkipped + ' Células canceladas', color: 'rgb(220, 53, 69)' },
                  { id: 1, value: data.totalCellReportedCompleted, label: data.totalCellReportedCompleted + ' Células realizadas', color: 'rgba(0, 105, 92, 0.9)' },
                  { id: 2, value: data.cellsPendingReports, label: data.cellsPendingReports + ' Células sem relatório', color: 'rgba(255, 187, 51, 0.9)' },
                ]
              }
            ]}
          />
        ),
        group: 'weekly',
        title: 'Relatórios de Células'
      });

    setGraphs(arrGraphs);
  }

  const SetCardsData = (data) => {
    var arrCards = [];

    arrCards.push({ type: 'info', title: <>Relatórios<br />Enviados</>, value: data.weekReports, click: clickReportsLoggedThisWeek, group: 'weekly' });

    var pendingReportsType = data.cellsPendingReports > 0 ? 'danger' : 'success';
    arrCards.push({
      type: pendingReportsType, title: <>Células com<br /> Relatórios Pendentes</>, value: data.cellsPendingReports,
      click: clickCellPendingReports, group: 'weekly'
    });

    arrCards.push({ type: !data.weekVisitors ? 'warning' : 'info', title: <>Visitantes em<br />Células</>, value: data.weekVisitors, link: '/reports/', group: 'weekly' });

    if (data.weekNewMembers)
      arrCards.push({ type: 'info', title: <>Novos membros<br />cadastrados</>, value: data.weekNewMembers, click: clickMemberCreatedThisWeek, group: 'weekly' });

    if (data.weekNewCells)
      arrCards.push({ type: 'info', title: <>Novas células<br />cadastradas</>, value: data.weekNewCells, click: clickCellCreatedThisWeek, group: 'weekly' });

    let activeMembers = data.totalMembers - data.totalIdleMembers;
    arrCards.push({ type: 'info', title: <>Membros<br />Ativos</>, value: activeMembers, click: clickActiveMembers, group: 'general' });

    if (data.totalEvangelisticCells)
      arrCards.push({ type: 'info', title: <>Celulas<br />Evangelísticas</>, value: data.totalEvangelisticCells, click: clickTotalEvangelisticCells, group: 'general' });

    if (data.totalDiscipleshipCells)
      arrCards.push({ type: 'info', title: <>Celulas<br />Discipuladoras</>, value: data.totalDiscipleshipCells, click: clickTotalDiscipleshipCells, group: 'general' });

    if (data.totalIdleMembers)
      arrCards.push({ type: 'info', title: <>Membros<br />Inativos</>, value: data.totalIdleMembers, click: clickIdleMembers, group: 'general' });

    if (data.totalNonBaptizedMembers)
      arrCards.push({ type: 'info', title: <>Membros<br />Não Batizados</>, value: data.totalNonBaptizedMembers, click: clickNonBaptizedMembers, group: 'general' });

    if (data.totalMembersWithoutLeadership)
      arrCards.push({ type: 'warning', title: <>Membros<br />Sem Liderança</>, value: data.totalMembersWithoutLeadership, click: clickMembersWithoutLeadership, group: 'general' });

    if (data.totalMembersWithNoCellLink)
      arrCards.push({ type: 'warning', title: <>Membros Sem<br />Vínculo Celular</>, value: data.totalMembersWithNoCellLink, click: clickMembersWithoutCellLink, group: 'general' });

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
            <div key={index}>
              <h3>{group[0] == 'weekly' ? 'Esta Semana' : 'Geral'}</h3>
              <Container fluid className="dashboard-cards">
                {group[1].map((card, index) => (
                  <Card bg={card.type} key={index} text="" className={"mb-2 " + ((card.link || card.click) && ' clickable')}
                    onClick={() => card.link ? cardClick(card) : card.click ? card.click() : () => { }}>
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
                    .map((graph, index) => <div className='graph-wrapper' key={index}><h5>{graph.title}</h5>{graph.element}</div>)}
                </div>
              )}
            </div>
          ))}


        </div>
      </>
  );
};
