import Footer from "./Footer";
import Header from "./Header";
import "./Layout.css";
import { Component, useContext } from "react";
import { buildUser, getMessageArray, makeMessageObjectWithName } from "../builders/functions";
import { FetchPublicTimeline, FetchUserByid } from "./fetch";
import { useState } from "react";
import { MessageObject, MessageObjectWithName } from "../builders/interface";
import { useEffect } from "react";
import { Message } from "./Message";


function PublicTimeline() {
  const [AllMessages, setMessages] = useState<MessageObjectWithName[]>();

  useEffect(() => {
    FetchPublicTimeline().then((messages) => {
      console.log(messages);
      setMessages(messages);
    });
  }, []);

  if (AllMessages !=undefined && AllMessages.length>0){
    console.log(AllMessages);
    return (
      <div className="page">
        <Header isLoggedIn={false} />
        <div className="body">
          <h2>Public TimeLine</h2>
          {AllMessages.map((mes) => (
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
        </div>
        <Footer />
      </div>
    );
    }
    else {
      return (
        <view>Loading</view>
      );
    }
}

export default PublicTimeline;
