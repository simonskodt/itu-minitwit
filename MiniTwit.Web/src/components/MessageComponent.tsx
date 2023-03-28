import './MessageComponent.css'
import { AppService } from '../services/app.service'
import React, { Component, useState }  from 'react';


interface Props {
    isLoggedIn: boolean;
    clickedUser : string
}

const MessageComponent: React.FC<Props> = ({ isLoggedIn, clickedUser }) => {
    const [message, setMessage] = useState('');
    const [placeholderText, setPlaceholderText] = useState('Write here');

    const appService = new AppService();
    const userName = sessionStorage.getItem('username')

    function postMessage(text: string, username: any): void {
        appService.getUserId(username).then((result) => {
            const id = result.data.id
            appService.sendMessage(text, id)
            setMessage('');
            setPlaceholderText('Write here');
        })
    }

    function handleKeyDown(event: React.KeyboardEvent<HTMLInputElement>) {
        if (event.key === 'Enter') {
            postMessage(message, userName);
        }
    }

    if (isLoggedIn && userName == clickedUser) {
        return (
            <div className='twitbox'>
                <h3>What&apo;s on your mind, &{userName}?</h3>
                <input
                    type="text"
                    placeholder={placeholderText}
                    name="username"
                    size={70}
                    required
                    value={message}
                    onChange={e => setMessage(e.target.value)}
                    onFocus={() => setPlaceholderText('')}
                    onBlur={() => setPlaceholderText('Write here')}
                    onKeyDown = {handleKeyDown}
                />
                &nbsp;&nbsp;
                <button onClick={() => postMessage(message, userName)}>Share</button>
            </div>
        );
    } else {
        return (
            <div></div>
        )
    }
}

export default MessageComponent;

