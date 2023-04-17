import { Component } from 'react';
import { getCurrentUsername } from '../builders/functions';
import '../pages/Layout.css';

const username = sessionStorage.getItem('username')

function getMenu(isLoggedIn: boolean) {
    if (!isLoggedIn) {
        return (
            <>
                <a href="./public">public timeline</a>&nbsp;|&nbsp;
                <a href="./register">sign up</a>&nbsp;|&nbsp;
                <a href="./login">sign in</a>
            </>
        );
    } else {
        return (
            <>
                <a href={getCurrentUsername()}>my timeline</a>&nbsp;|&nbsp;
                <a href="./public">public timeline</a>&nbsp;|&nbsp;
                <a href="./" onClick={ () => sessionStorage.clear()}>sign out [{username}]</a>
            </>
        );
    }
}

interface Props {
    isLoggedIn: boolean;
}

class Header extends Component<Props> {
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