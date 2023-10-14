import React, { useEffect } from 'react';
import { SearchMembers } from '../../services/Member.service';
import { Pagination } from 'react-bootstrap';
import Grid from '../../components/Grid';

export const Members = () => {

  const [members, setMembers] = 
    React.useState([]);
  const [pagination, setPagination] = 
    React.useState({page: 1, limit: 20});
  const [searchData, setSearchData] = 
    React.useState({name: '', leadershipName: '', email: '', neighborhood: '', city: '', state: ''});
  const [loading, setLoading] = React.useState(false);
  const [total, setTotal] = React.useState(0);

  const columns = [
    {
      name: '#',
      render: (member) => (member.id.toString() ?? '0').padStart(4, '0')
    },
    {
      name: 'Nome',
      render: (member) => member.name
    },
    {
      name: 'Ministerio / Liderança',
      render: (member) => member.leadership?.name ?? '-'
    },
    {
      name: 'Bairro',
      render: (member) => member.homeAddress.neighborhood
    },
    {
      name: 'Cidade',
      render: (member) => member.homeAddress.city
    }
  ];

  const search = async () => {
    setLoading(true);

    const result = await SearchMembers(searchData, pagination);
    if(result.isValid){
      setMembers(result.data);
      setTotal(result.totalRecords ?? result.data.length);
    }
    
    setLoading(false);
  };
  
  useEffect(() => {
    search();
  }, []);

  useEffect(() => {
    search();   
  }, [pagination]);

  return (
    <>
      <div>
        <h1>Pessoas</h1>
        <Grid 
          columns={columns} 
          data={members} 
          textTotal='pessoas cadastradas' 
          loading={loading} 
          pagination={pagination} 
          totalRecords={total}/>
      </div>
    </>
  );
  
}
