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
  

  useEffect(() => {
    FetchPublicTimeline().then((messages) => {
      //fetchUserByID("63f8b294082250d882e1860d");
      console.log(messages);
      setMessages(messages);
    });
  }, []);

  const fetchUser = (message: MessageObject, id: string) => {
    FetchUserByid(id).then((u)=>{
      var user = buildUser(u);
      var userWithName = makeMessageObjectWithName(message, user.username)
      console.log(userWithName);
    });
  }

  if (messages!= undefined)
  {
    messages.forEach(element => {
      fetchUser(element, element.authorId);
    });

  }


/*   if (messages != undefined){
    for(let i = 0; i<10; i++){
      var user = fetchUser(messages[i].authorId);
      var fullUserObject = makeMessageObjectWithName(user, messages[i].authorId);
      MessageAndName.push(fullUserObject);
      console.log(MessageAndName);
    }
  } else {
    console.log("error in loop");
  }   */


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
