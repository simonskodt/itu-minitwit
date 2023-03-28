import { useEffect, useState } from "react";
import { checkLogIn } from "../builders/functions";
import { MessageObject } from "../builders/interface";
import { fetchPrivateTimeLine } from "./fetch";
import Footer from "./Footer";
import Header from "../components/Header";
import { Message } from "./Message";
import MessageComponent from "../components/MessageComponent";
import React, { Component }  from 'react';

function replaceSpaces(str: string): string {
  return str.replace(/%20/g, " ");
}

function PrivateTimeline() {
  const url = window.location.href;
  const parts = url.split("/");
  const tempUserName = parts[parts.length - 1];
  const userName = replaceSpaces(tempUserName)
  const [messages, setMessages] = useState<MessageObject[]>();
  const sessionUser = sessionStorage.getItem('username')

  const displayName = () => {
    if(userName == sessionUser){
      return "My Timeline"
    }else {
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

  if (messages != undefined) {
    return (
      <div className="page">
        <Header isLoggedIn={checkLogIn()} />
        <div className="body">
          <MessageComponent isLoggedIn={checkLogIn()} clickedUser = {userName} />
          <h2>{displayName()}</h2>
          {messages.map((mes) => (
            <view key={mes.messageId}>
              <view>
                <Message
                  username={userName}
                  text={mes.text}
                  date={mes.pubDate}
                />
              </view>
            </view>
          ))}
        </div>
        <Footer />
      </div>

    );
  }
  else {
    return (
      <view></view>
    );
  }
}

export default PrivateTimeline