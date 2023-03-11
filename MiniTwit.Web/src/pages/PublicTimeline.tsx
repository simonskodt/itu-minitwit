import Footer from "./Footer";
import Header from "./Header";
import "./Layout.css";
import { FetchPublicTimeline } from "./fetch";
import { useState } from "react";
import {MessageObjectWithName } from "../builders/interface";
import { useEffect } from "react";
import { Message } from "./Message";


function PublicTimeline() {
  const [AllMessages, setMessages] = useState<MessageObjectWithName[]>();

  useEffect(() => {
    FetchPublicTimeline().then((messages) => {
      setMessages(messages);
    });
  }, []);

  if (AllMessages !=undefined && AllMessages.length>0){
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
    } else {
    return (
      <div className="page">
        <Header isLoggedIn={false} />
        <div className="body">
          <h2>Public TimeLine</h2>
          <ul className="messages">
            <li>
              <p>There's no messages so far.</p>
            </li>
          </ul>
        </div>
        <Footer />
      </div>
    );
  }
}

export default PublicTimeline;
