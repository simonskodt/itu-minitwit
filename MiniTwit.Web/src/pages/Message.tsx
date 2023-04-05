import { Md5 } from 'ts-md5';
import React, { Component }  from 'react';


interface IMessageProps {
    username: string;
    text: string;
    date: string;
}

// Generation of MD5 hash to use as Gravatar image
function generateHash(name: string): string {
    const hash: string = Md5.hashStr(name);
    return 'https://www.gravatar.com/avatar/' + hash + '?s=50&d=identicon';
}

// Transparant background for identicon
function generateHash2(name: string): string {
    const hash: string = Md5.hashStr(name);
    return 'https://api.dicebear.com/5.x/identicon/svg?seed=' + hash;
}

// Formatting of datetime, so it is more readable
function formatDatetime(date: string): string {
    return new Date(date).toLocaleString();
}

export function Message(props: IMessageProps) {
    return (
        <ul className="messages">
            <li>
                <img src={generateHash2(props.username)} />
                <p>
                    <b><a href={props.username}>{props.username}</a></b>&nbsp;
                    {props.text}&nbsp;
                    <small>&mdash; {formatDatetime(props.date)}</small>
                </p>
            </li>
        </ul>
    )
}