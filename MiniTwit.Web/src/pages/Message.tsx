
interface IMessageProps {
    gravatar: string;
    username: string;
    text: string;
    date: string;
}

function Message(props: IMessageProps) {
    return (
        <ul className='messages'>
            <li>
                <img src="{ props.gravatar }"/>
                <p>
                    <strong><a href="{./ + props.username}">props.username</a></strong>
                    { props.text }
                    <small>&mdash; { props.date }</small>
                </p>
            </li>
        </ul>
    )
}