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
        React.useState({leadId: '', hostId: '', timothyId: '', weekDay: '', placeId: '', cellType: ''});
    const [loading, setLoading] = React.useState(false);
    const [total, setTotal] = React.useState(0);

    const columns = [
      {
        name: '#',
        render: (cell) => (cell.id.toString() ?? '0').padStart(4, "0")
      },
    ]
}