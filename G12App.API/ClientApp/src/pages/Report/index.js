import React from "react";
import Grid from "../../components/Grid";

export const Reports = () => {


    const columns = [
        {
            name: 'Dados da célula',
            render: (report) => <>
            <span><b>Líder:</b> {report.lead?.name} </span> <br/>
            <span><b>Líder G12:</b> {report.lead?.computedG12Leader} </span> <br/>
            <span> <b> Endereço:</b> Rua {report.place?.street}"," {report.place?.neighborhood}</span>
            </>
        },
        {
            name: 'Frequentantes',
            render: (report) => <>
            
            </>
        },
        {
            name: 'Visitantes',
            render: (report) => <>
            
            </>
        },
        {
            name: 'Horário',
            render: (report) => <>
                <span><b>Célula:</b> {report.dateOnly} + {report.timeOnly} </span> <br/>
            </>
        }
    ]
}