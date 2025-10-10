import React from 'react';

import baptized from '../../images/badges/baptized.png';
import leader from '../../images/badges/cell-leader.png';
import timothy from '../../images/badges/cell-timothy.png';
import host from '../../images/badges/cell-host.png';

export const Baptized = () => (
    <span title="Batizado" style={{ margin: '0 4px' }}>
        <img
            src={baptized}
            alt="Batizado"
            title="Batizado"
            style={{ width: '32px', height: '32px', verticalAlign: 'middle' }}
        />
    </span>
);

export const CellLeader = () => (
    <span title="Líder de Célula" style={{ margin: '0 4px' }}>
        <img
            src={leader}
            alt="Líder de Célula"
            title="Líder de Célula"
            style={{ width: '32px', height: '32px', verticalAlign: 'middle' }}
        />
    </span>
);

export const CellTimothy = () => (
    <span title="Timóteo" style={{ margin: '0 4px' }}>
        <img
            src={timothy}
            alt="Timóteo"
            title="Timóteo"
            style={{ width: '32px', height: '32px', verticalAlign: 'middle' }}
        />
    </span>
);

export const CellHost = () => (
    <span title="Anfitrião" style={{ margin: '0 4px' }}>
        <img
            src={host}
            alt="Anfitrião"
            title="Anfitrião"
            style={{ width: '32px', height: '32px', verticalAlign: 'middle' }}
        />
    </span>
);

export const Badges = ({ badges = [], children }) => {
    const items = badges?.length ? badges : React.Children.toArray(children);
    if (!items.length)
        return null;

    return (
        <>
            <hr className="badge-hr" />
            <div style={{ display: 'flex', alignItems: 'center' }}>
                {items.map((BadgeComponent, idx) => {
                    if (React.isValidElement(BadgeComponent))
                        return React.cloneElement(BadgeComponent, { key: idx });
                    if (typeof BadgeComponent === 'function')
                        return <BadgeComponent key={idx} />;
                    return null;
                })}
            </div>
        </>
    );
};

