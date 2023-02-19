import React, { Component } from 'react';
import './Layout.css';
import Menu from './Menu';

class Layout extends Component {
    constructor(props: any) {
        super(props);
    }

    render() {
        return (
            <div className="layout">
                <h1>MiniTwit</h1>
                <Menu />
            </div>
        );
    }
}

export default Layout;