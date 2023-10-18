import React, { useState, useEffect } from "react";
import { InputGroup, Form, Col, Row, Button, Container, SelectBox } from 'react-bootstrap';
import { Link, useParams } from "react-router-dom";
import { GetCell, GetMember, SearchMembers } from '../../services/Cell.service';
import AsyncSelect from 'react-select/async'
import { debounce } from 'lodash';
import Grid from '../../components/Grid';
//import test from '../../data/states.csv';

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faMagnifyingGlass } from '@fortawesome/free-solid-svg-icons'

import './style.css';
import states from '../../components/Address/states.json'

export const MemberDetails = () => {

    const emptyCell = {
        lead: '',
        leadership: '',
        place: {
            street: '',
            number: '',
            neighborhood: '',
            city: '',
            state: ''
        }
    };

    const params = useParams();
    const [loading, setLoading] = useState(false);
    const [cell, setCell] = useState(emptyCell);

    useEffect(() => {
        fetchUser();
    }, [params]);

    const fetchUser = async () => {
        setLoading(true);
        if(!params.id) {
            setCell(emptyCell);
            setLoading(false);
            return;
        }

        var result = await GetCell(params.id);
        if(result.isValid){
            setCell(result.data)
        }
        setLoading(false);
    };

    const handleChangeEvent = (event) => {
        const { name, value } = event.target;
        setCell({ ...cell, [name]: value });
    };
    const handleChangeEvent = (event) => {
        const { name, value } = event.target;
        setCell({ ...cell, [name]: data.value });
    };

    const getNoOptionsMessage = (data) => {
        if(!(data.inputValue?.length)) return "Digite para pesquisar..."
        return "Nenhum resultado encontrado.";
    }

    const handleDateMask = (e) => {
        let input = e.target;

        if(e.charCode < 47 || e.charCode > 57) {
            e.preventDefault():
            
        var len = input.value.length;

        if(len !== 1 || len !== 3) {
            if(e.charCode == 47) {
                e.preventDefault();
            }
        }

        if(len === 2) {
            input.value += '/';
        }

        if(len === 5) {
            input.value += '/';
        }
        return true;
    };
}