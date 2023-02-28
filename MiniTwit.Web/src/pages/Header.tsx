import React, { Component } from 'react';
import { isPropertySignature } from 'typescript';
import './Layout.css';


function getMenu(isLoggedIn: boolean) {
    if (!isLoggedIn) {
        return (
            <>
                <a href="./public">public timeline</a>&nbsp;|&nbsp;
                <a href="./register">sign up</a>&nbsp;|&nbsp;
                <a href="./login">sign in</a>&nbsp;|&nbsp;
            </>
        );
    } else {
        return (
            <>
                <a href="./">my timeline</a>&nbsp;|&nbsp;
                <a href="./public">public timeline</a>&nbsp;|&nbsp;
                <a href="./logout">sign out</a>&nbsp;|&nbsp;
            </>
        );
    }
}

interface Props {
    isLoggedIn: boolean;
}

class Header extends React.Component<Props> {
    render() {
        return (
            <div className="layout">
                <h1>MiniTwit</h1>
                <div className="navigation">
                    {getMenu(this.props.isLoggedIn)}
                </div>
            </div>
        );
    }
}

export default Header;