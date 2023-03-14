import React, { Component, useState } from 'react';
import './MessageComponent.css'
import { API_URL } from '../App';
import { AppService } from '../services/app.service'


interface Props {
    isLoggedIn: boolean;
}

const MessageComponent: React.FC<Props> = ({ isLoggedIn }) => {
    const [message, setMessage] = useState('');

    const appService = new AppService();
    const userName = sessionStorage.getItem('username')

    function PostMessage(text: string, username : any){
        appService.getUserId(username).then((result) => {
            const id = result.data.id
            appService.sendMessage(text, id)
        })
    }
    if(isLoggedIn){
        return (
            <div className='twitbox'>
                <h3>What's on your mind {userName}</h3>
                <input
                    type="text"
                    placeholder="Username"
                    name="username"
                    size={60}
                    required
                    onChange={e => setMessage(e.target.value)}
                />
                <button onClick={ () => PostMessage(message, userName)}> Share </button>
            </div>
        );
    }else{
        return(
        <div></div>
        )
    }
}

export default MessageComponent;

