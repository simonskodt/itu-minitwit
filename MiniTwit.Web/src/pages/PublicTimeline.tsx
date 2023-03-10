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

  const [AllMessages, setMessages] = useState<MessageObject[]>();
  const MesWithUsername : MessageObjectWithName [] = [] 


  useEffect(() => {
    FetchPublicTimeline().then((messages) => {
      messages.forEach(element  => {
        FetchUserByid(element.authorId).then((u)=>{
          var user = buildUser(u);
          var userWithName = makeMessageObjectWithName(element, user.username);
          console.log(userWithName);
          MesWithUsername.push(userWithName);
          //console.log(MesWithUsername);

          });
      });
      setMessages(messages);
    });
  }, []);

 
 var slicedArray = AllMessages?.slice(0, 50)

  if (AllMessages != undefined )  {
    return (
      <div className="page">
        <Header isLoggedIn={false} />
        <div className="body">
          <h2>Public TimeLine</h2>
          {MesWithUsername.map((mes) => (
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
        <button onClick={() => console.log(AllMessages)}> TEST </button>
        <Footer />
      </div>
    );
  }
}

export default PublicTimeline;
