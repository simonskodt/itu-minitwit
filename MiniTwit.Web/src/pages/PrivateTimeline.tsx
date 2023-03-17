import { useEffect, useState } from "react";
import { checkLogIn } from "../builders/functions";
import { MessageObjectWithName } from "../builders/interface";
import { FetchPrivateTimeLine } from "./fetch";
import Footer from "./Footer";
import Header from "../components/Header";
import { Message } from "./Message";
import MessageComponent from "../components/MessageComponent";
import FollowComponent from "../components/FollowComponent";

function PrivateTimeline()
{
    let url = window.location.href;
    var parts = url.split("/");
    var userName = parts[parts.length - 1]; 
    const [messages, setMessages] = useState<MessageObjectWithName[]>();

    useEffect(() => {
        FetchPrivateTimeLine(userName).then((messages) => {
          console.log(messages);
          setMessages(messages);
        });
      }, []);
    
  if (messages != undefined) {
    return (
      <div className="page">
        <Header isLoggedIn={checkLogIn()} />
        <MessageComponent isLoggedIn={checkLogIn()} />
        <FollowComponent isLoggedIn={checkLogIn()} userToFollow ={userName}/>
        <div className="body">
        <h2>{userName}'s TimeLine</h2>
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