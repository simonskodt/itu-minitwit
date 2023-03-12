import { useEffect, useState } from "react";
import { MessageObject } from "../builders/interface";
import { FetchPrivateTimeLine } from "./fetch";
import Footer from "./Footer";
import Header from "./Header";
import { Message } from "./Message";

function PrivateTimeline()
{
    let url = window.location.href;
    var parts = url.split("/");
    var userName = parts[parts.length - 1]; 
    const [messages, setMessages] = useState<MessageObject[]>();

    useEffect(() => {
        FetchPrivateTimeLine(userName).then((messages) => {
          setMessages(messages);
        });
      }, []);

    if (messages!= undefined){
    return(
    <div className="page">
        <Header isLoggedIn={false} />
        <div className="body">
        <h2>{userName}'s TimeLine</h2>
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
        <view>This user does not have any messages</view>
    );
    }
}

export default PrivateTimeline