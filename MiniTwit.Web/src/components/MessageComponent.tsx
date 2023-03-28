import './MessageComponent.css'
import { AppService } from '../services/app.service'
import React, { Component, useState }  from 'react';


interface Props {
    isLoggedIn: boolean;
}

const MessageComponent: React.FC<Props> = ({ isLoggedIn }) => {
    const [message, setMessage] = useState('');

    const appService = new AppService();
    const userName = sessionStorage.getItem('username')

    function postMessage(text: string, username: any): void {
        appService.getUserId(username).then((result) => {
            alert('Message posted!');
            const id = result.data.id;
            appService.sendMessage(text, id);
            setMessage('');
        })
    }

    if (isLoggedIn) {
        return (
            <div className='twitbox'>
                <h3>What&apos;s on your mind, {userName}?</h3>
                <div className="message-container">
                    <input
                        className='message-input'
                        type="text"
                        placeholder="Write here"
                        name="username"
                        size={70}
                        required
                        onChange={e => setMessage(e.target.value)}
                    />&nbsp;&nbsp;
                    <button className='message-button' onClick={() => postMessage(message, userName)}>Share</button>
                </div>
            </div>
        );
    } else {
        return (
            <div></div>
        )
    }
}

export default MessageComponent;

