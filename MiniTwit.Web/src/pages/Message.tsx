import { Link } from "react-router-dom";

interface IMessageProps {
    username: string;
    text: string;
    date: string;
}

export function Message(props: IMessageProps) {
    return (
        <ul className='messages'>
            <li>
                <img src="https://www.google.com/url?sa=i&url=https%3A%2F%2Fitxdesign.com%2Fwhy-is-a-gravatar-something-you-should-start-using-immediately%2F&psig=AOvVaw0yw7lIV_6xksyBFbyzZMLn&ust=1677065371349000&source=images&cd=vfe&ved=0CA0QjRxqFwoTCMDjncHBpv0CFQAAAAAdAAAAABAY"/>
                <p>
                    <Link to={props.username}>{props.username}</Link>&nbsp;
                    { props.text }&nbsp;
                    <small>&mdash; &nbsp; { props.date }</small>
                </p>
            </li>
        </ul>
    )
}