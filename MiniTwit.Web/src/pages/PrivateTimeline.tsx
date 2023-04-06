import { useEffect, useState } from "react";
import { checkLogIn } from "../builders/functions";
import { MessageObjectWithName } from "../builders/interface";
import { fetchPrivateTimeLine } from "./fetch";
import Footer from "./Footer";
import Header from "../components/Header";
import { Message } from "./Message";
import MessageComponent from "../components/MessageComponent";
import FollowComponent from "../components/FollowComponent";
import React from 'react';

function replaceSpaces(str: string): string {
  return str.replace(/%20/g, " ");
}

function PrivateTimeline() {
  const url = window.location.href;
  const parts = url.split("/");
  const tempUserName = parts[parts.length - 1];
  const userName = replaceSpaces(tempUserName)
  const [messages, setMessages] = useState<MessageObjectWithName[]>();
  const sessionUser = sessionStorage.getItem('username')

  const displayName = () => {
    if (userName == sessionUser) {
      return "My Timeline"
    } else {
      return userName + "'s TimeLine"
    }
  }

  useEffect(() => {
    const fetchMessages = async () => {
      const messages = await fetchPrivateTimeLine(userName);
      setMessages(messages);
    };
    fetchMessages();

    const intervalId = setInterval(() => {
      fetchMessages();
    }, 2000);

    return () => clearInterval(intervalId);
  }, []);

  function showMessages() {
    if (messages != undefined) {
      return (
        <>
          {messages.map((mes) => (
            <view key={mes.messageId}>
              <view>
                <Message
                  username={mes.userName}
                  text={mes.text}
                  date={mes.pubDate}
                />
              </view>
            </view>
          ))}
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
        <MessageComponent isLoggedIn={checkLogIn()} clickedUser={userName} />
        <div className="headers">
          <div className="left">
            <h2>{displayName()}</h2>
          </div>
          <div className="right">
            <FollowComponent isLoggedIn={checkLogIn()} userToFollow={userName} />
          </div>
        </div>
        {showMessages()}
      </div>
      <Footer />
    </div>
  );
}

export default PrivateTimeline