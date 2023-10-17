import React, { useEffect} from 'react';
import { InputGroup, Form, Col, Row, Button } from 'react-bootstrap';
import { Link} from "react-router-dom";
import { SearchCells} from '../../services/Cell.service';
import { debounce} from 'lodash';
import Grid from '../../components/Grid';
