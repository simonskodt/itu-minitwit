import './MessageComponent.css';
import { AppService } from '../services/app.service';
import React, { useState }  from 'react';


interface Props {
    isLoggedIn: boolean;
    clickedUser : string
}

const MessageComponent: React.FC<Props> = ({ isLoggedIn, clickedUser }) => {
    const [message, setMessage] = useState('');
    const [placeholderText, setPlaceholderText] = useState('Write here');

    const appService = new AppService();
    const username = sessionStorage.getItem('username')

    function postMessage(text: string, username: any): void {
        appService.getUserId(username).then((result) => {
            const id = result.data.id
            appService.sendMessage(text, id)
            setMessage('');
            setPlaceholderText('Write here');
            alert('Message posted!');
        })
    }

    function handleKeyDown(event: React.KeyboardEvent<HTMLInputElement>) {
        if (event.key === 'Enter') {
            postMessage(message, username);
        }
    }

    if (isLoggedIn && username === clickedUser) {
        return (
            <div className='twitbox'>
                <h3>What&apos;s on your mind, {username}?</h3>
                <div className="message-container">
                    <input
                        className='message-input'
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
                    <button className='message-button' onClick={() => postMessage(message, username)}>Share</button>
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

