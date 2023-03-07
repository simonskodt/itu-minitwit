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


function PublicTimeline() {
  //const {messages} = useContext(MessagesContext)
  //const { setMessages } = useContext(MessagesContext);

  const [messages, setMessages] = useState<MessageObject[]>();
  const MesWithUsername : MessageObjectWithName [] = [] 

  useEffect(() => {
    FetchPublicTimeline().then((messages) => {
      console.log(messages);
      setMessages(messages);
    });
  }, []);


  useEffect(() => {
    if (messages != undefined){
      console.log(":::::A:SDFASDF")
    messages.forEach(element => {
      FetchUserByid(element.authorId).then((u)=>{
      var user = buildUser(u);
      var userWithName = makeMessageObjectWithName(element, user.username);
      MesWithUsername.push(userWithName);
      console.log(MesWithUsername);
        }); 
      });
      
    }
  }, []);
/*   if (messages != undefined)
  {
    messages.forEach(element => {
      //console.log(element.text);
      FetchUserByid(element.authorId).then((u)=>{
      var user = buildUser(u);
      var userWithName = makeMessageObjectWithName(element, user.username);
      MesWithUsername.push(userWithName);
      console.log(MesWithUsername);
      }); 
    });
  } */
 

 var slicedArray = messages?.slice(0, 50)

  if (messages != undefined )  {
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
        <button onClick={() => console.log(messages)}> TEST </button>
        <Footer />
      </div>
    );
  }
}

export default PublicTimeline;
