import { useEffect, useState } from "react";
import { checkLogIn } from "../builders/functions";
import { fetchPrivateTimeLine } from "./fetch";
import Footer from "./Footer";
import Header from "../components/Header";
import { Message } from "./Message";
import MessageComponent from "../components/MessageComponent";
import FollowComponent from "../components/FollowComponent";
import './Layout.css';
import { MessageDTO } from "../models/MessageDTO";


function replaceSpaces(str: string): string {
  return str.replace(/%20/g, " ");
}

function PrivateTimeline() {
  const url = window.location.href;
  const parts = url.split("/");
  const tempUserName = parts[parts.length - 1];
  const username = replaceSpaces(tempUserName)
  const [messages, setMessages] = useState<MessageDTO[]>();
  const sessionUser = sessionStorage.getItem('username')

  const displayName = () => {
    if (username === sessionUser) {
      return "My Timeline"
    } else {
      return username + "'s TimeLine"
    }
  }

  useEffect(() => {
    const fetchMessages = async () => {
      const messages = await fetchPrivateTimeLine(username);
      setMessages(messages);
    };
    fetchMessages();

    const intervalId = setInterval(() => {
      fetchMessages();
    }, 2000);

    return () => clearInterval(intervalId);
  }, [username]);

  function ShowFittedMessages(messages: MessageDTO[]) {
    return (
      <div>
        {messages.map((mes) => (
          <view key={mes.messageId}>
            <view>
              <Message
                username={mes.username}
                text={mes.text}
                date={mes.pubDate}
              />
            </view>
          </view>
        ))}
      </div>
    )
  }

  function showMessages() {
    if (messages !== undefined && messages.length >= 15) {
      return (
        <>
          <div className="scrollable-container">
            {ShowFittedMessages(messages)}
          </div>
        </>
      );
    } else if (messages !== undefined && messages.length < 15) {
      return (
        <>
          <div>
            {ShowFittedMessages(messages)}
          </div>
        </>
      );
    }
    else {
      return (
        <div>
          <ul className="messages">
            <li>
              <p></p>
            </li>
          </ul>
        </div>
      );
    }
  }

  return (
    <div className="page">
      <Header isLoggedIn={checkLogIn()} />
      <div className="body">
        <MessageComponent isLoggedIn={checkLogIn()} clickedUser={username} />
        <div className="headers">
          <div className="left">
            <h2>{displayName()}</h2>
          </div>
          <div className="right">
            <FollowComponent isLoggedIn={checkLogIn()} userToFollow={username} />
          </div>
        </div>
        {showMessages()}
      </div>
      <Footer />
    </div>
  );
}

export default PrivateTimeline