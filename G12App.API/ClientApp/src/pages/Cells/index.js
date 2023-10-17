import React, { useEffect} from 'react';
import { InputGroup, Form, Col, Row, Button } from 'react-bootstrap';
import { Link} from "react-router-dom";
import { SearchCells} from '../../services/Cell.service.js';
import { debounce} from 'lodash';
import Grid from '../../components/Grid';

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faMagnifyingGlass } from '@fortawesome/free-solid-svg-icons';

export const Cells = () => {

    const [cells, setCells] =
        React.useState([]);
    const [pagination, setPagination] =
        React.useState({page: 1, limit: 20});
    const [searchData, setSearchData] =
        React.useState({lead: '', host: '', timothy: '', weekDay: null, cellType: null});
    const [loading, setLoading] = React.useState(false);
    const [total, setTotal] = React.useState(0);

    const columns = [
      {
        name: '#',
        render: (cell) => (cell.id.toString() ?? '0').padStart(4, "0")
      },
      {
        name: 'Liderança',
        render: (cell) => <><span><b>Líder: </b>{cell.lead?.name}</span><br/>
                          <span><b>Líder G12: </b>{cell.lead?.leadership?.name}</span></>
      },
      {
        name: 'Rua',
        render: (cell) => cell.place?.street
      },
      {
        name: 'Bairro',
        render: (cell) => cell.place?.neighborhood
      },
      {
        name: 'Cidade',
        render: (cell) => cell.place?.city
      }
    ];

    const search = async () => {
      setLoading(true);

      const result = await SearchCells(searchData, pagination);
      if (result.isValid){
        setCells(result.data);
        setTotal(result.totalRecords ?? result.data.length);
      }

      setLoading(false);
    };

    const debounceEventTrigger = debounce(field => {
      var currentValue = searchData[field.name] || '';
      if(field.value != currentValue)
      setSearchData({...searchData, [field.name]: field.value});
    }, 500);

    const handleSearchEvent = (event) => {
      var field = event.target;
      debounceEventTrigger(field);
    };

    useEffect(() => {
      search();
    }, [pagination, searchData]);

    return (
      <>
        <div>
          <h1>Células</h1>
          <hr />
          <div className="row">
            <h6>&nbsp;</h6>
            <Grid 
            columns={columns}
            data={cells}
            textTotal='Células cadastradas'
            loading={loading}
            pagination={pagination}
            setPagination={setPagination}
            totalRecords={total}/>
          </div>
        </div>
      </>
    )
}