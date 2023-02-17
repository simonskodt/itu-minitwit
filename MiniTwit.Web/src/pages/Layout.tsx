import React, { Component } from 'react';
import './Layout.css';

class Layout extends Component {
    constructor(props: any) {
        super(props);
    }

    render() {
        return (
            <div>
                    <div className="navigation">
                        <a>timeline</a> | &nbsp;
                        <a>public timeline</a> | &nbsp;
                        <a>sign out</a>
                    </div>
                <div className="footer">
                    MiniTwit &mdash; A Twitter clone
                </div>
            </div>
        );
    }
}

export default Layout;