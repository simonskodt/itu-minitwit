import React, { useEffect, useState } from "react";
import { checkLogIn } from "../builders/functions";
import { MessageObject } from "../builders/interface";
import { fetchPrivateTimeLine } from "./fetch";
import Footer from "./Footer";
import Header from "../components/Header";
import { Message } from "./Message";
import MessageComponent from "../components/MessageComponent";


function PrivateTimeline() {
  const url = window.location.href;
  const parts = url.split("/");
  const userName = parts[parts.length - 1];
  const [messages, setMessages] = useState<MessageObject[]>();

  useEffect(() => {
    fetchPrivateTimeLine(userName).then((messages) => {

      setMessages(messages);
    });
  }, []);

  if (messages != undefined) {
    return (
      <div className="page">
        <Header isLoggedIn={checkLogIn()} />
        <div className="body">
          <MessageComponent isLoggedIn={checkLogIn()} />
          <h2>{userName}&apos;s TimeLine</h2>
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