import React, { Md5 } from 'ts-md5';

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

// Formatting of datetime, so it is more readable
function formatDatetime(date: string): string {
    return new Date(date).toLocaleString();
}

export function Message(props: IMessageProps) {
    return (
        <ul className="messages">
            <li>
                <img src={generateHash(props.username)} />
                <p>
                    <a href={props.username}>{props.username}</a>&nbsp;
                    {props.text}&nbsp;
                    <small>&mdash; {formatDatetime(props.date)}</small>
                </p>
            </li>
        </ul>
    )
}