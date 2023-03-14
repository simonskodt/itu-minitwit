import {Md5} from 'ts-md5';

interface IMessageProps {
    username: string;
    text: string;
    date: string;
}

// Generation of MD5 hash to use as Gravatar image
function generateHash(name: any): string {
    var hash:string = Md5.hashStr(name);
    console.log('https://www.gravatar.com/avatar/' + hash);
    return 'https://www.gravatar.com/avatar/' + hash + '?s=50&d=identicon';
}

export function Message(props: IMessageProps) {
    return (
        <ul className='messages'>
            <li>
                <img src={generateHash(props.username)}/>
                <p>
                    <a href={props.username}>{props.username}</a>&nbsp;
                    { props.text }&nbsp;
                    <small>&mdash; { props.date }</small>
                </p>
            </li>
        </ul>
    )
}