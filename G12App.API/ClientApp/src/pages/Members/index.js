import React, { useEffect } from 'react';
import { SearchMembers } from '../../services/Member.service';
import { Pagination } from 'react-bootstrap';

export const Members = () => {

  const [members, setMembers] = 
    React.useState([]);
  const [pagination, setPagination] = 
    React.useState({page: 1, limit: 20});
  const [searchData, setSearchData] = 
    React.useState({name: '', leadershipName: '', email: '', neighborhood: '', city: '', state: ''});

  const search = async () => {
    const result = await SearchMembers(searchData, pagination);
    if(result.isValid)
      setMembers(result.data); 
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
        
        <table className="table table-striped" aria-labelledby="tableLabel">
          <thead>
            <tr>
              <th>#</th>
              <th>Nome</th>
              <th>Ministerio / Lideranca</th>
              <th>Bairro</th>
              <th>Cidade</th>
            </tr>
          </thead>
          <tbody>
            {members.map(member =>
              <tr key={member.id}>
                <td>{member.id}</td>
                <td>{member.name}</td>
                <td>{member.leadership?.name}</td>
                <td>{member.homeAddress.neighborhood}</td>
                <td>{member.homeAddress.city}</td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
    </>
  );
  
}
