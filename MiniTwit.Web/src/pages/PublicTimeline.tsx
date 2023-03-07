import Footer from "./Footer";
import Header from "./Header";
import "./Layout.css";
import { useContext } from "react";
import { getMessageArray } from "../builders/functions";
import { FetchPublicTimeline } from "./fetch";
import { useState } from "react";
import { MessageObject } from "../builders/interface";
import { useEffect } from "react";
import { Message } from "./Message";


function PublicTimeline() {
  //const {messages} = useContext(MessagesContext)
  //const { setMessages } = useContext(MessagesContext);

  const [messages, setMessages] = useState<MessageObject[]>();

  

  const fetchAllUsers = () => {
    FetchPublicTimeline().then((messages) => {
      console.log(messages);
      let buildingMessage = getMessageArray(messages);
      setMessages(buildingMessage);
    });
  };

  useEffect(() => {
    FetchPublicTimeline().then((messages) => {
      console.log(messages);
      setMessages(messages);
    });
  }, []);

 var slicedArray = messages?.slice(0, 50)

  if (messages != undefined && slicedArray!= undefined) {
    return (
      <div className="page">
        <Header isLoggedIn={false} />
        <div className="body">
          <h2>Public TimeLine</h2>
          {slicedArray.map((mes) => (
            <view key={mes.messageId}>
              <view>
                <Message
                  username = {mes.authorId}
                  text = {mes.text}
                  date = {mes.pubDate}
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
        <button onClick={() => console.log(messages)}> TEST </button>
        <Footer />
      </div>
    );
  }
}

export default PublicTimeline;
