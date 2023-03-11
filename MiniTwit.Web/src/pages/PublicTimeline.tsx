import Footer from "./Footer";
import Header from "./Header";
import "./Layout.css";
import { useContext } from "react";
import { buildUser, getMessageArray, makeMessageObjectWithName } from "../builders/functions";
import { FetchPublicTimeline, FetchUserByid } from "./fetch";
import { useState } from "react";
import { MessageObject, MessageObjectWithName } from "../builders/interface";
import { useEffect } from "react";
import { Message } from "./Message";

//function connectIdToName (){}

function PublicTimeline() {
  //const {messages} = useContext(MessagesContext)
  //const { setMessages } = useContext(MessagesContext);

  const [AllMessages, setMessages] = useState<MessageObjectWithName[]>();


  useEffect(() => {
    FetchPublicTimeline().then((messages) => {
      setMessages(messages);
    });
  }, []);


if (AllMessages!= undefined){
  console.log(AllMessages);
  return (
    <div className="page">
      <Header isLoggedIn={false} />
      <div className="body">
        <h2>Public TimeLine</h2>
        {AllMessages.map((mes) => (
              <view>
                {mes.userName}
              </view>
          ))}
      </div>
      <Footer />
    </div>
  );
  }
  else {
    return (
      <view>ELSE STATEMENT</view>
    );
  }
}

export default PublicTimeline;
