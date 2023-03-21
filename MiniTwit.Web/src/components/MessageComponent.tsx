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
            const id = result.data.id
            appService.sendMessage(text, id)
        })
    }

    if (isLoggedIn) {
        return (
            <div className='twitbox'>
                <h3>What&apo;s on your mind, &{userName}?</h3>
                <input
                    type="text"
                    placeholder="Write here"
                    name="username"
                    size={70}
                    required
                    onChange={e => setMessage(e.target.value)}
                />&nbsp;&nbsp;
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

