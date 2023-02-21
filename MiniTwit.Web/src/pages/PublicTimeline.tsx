import Footer from './Footer';
import Layout from './Layout';
import './Layout.css';
import { useContext } from 'react';
//import { MessagesContext } from '../contexts/messageContext';
import { getMessageArray } from '../builders/functions';
import { FetchPublicTimeline } from './fetch';
import { useState } from 'react';
import { MessageObject } from '../builders/interface';
import { useEffect } from 'react';


function PublicTimeline() {
  //const {messages} = useContext(MessagesContext) 
  //const { setMessages } = useContext(MessagesContext);
  
  const [messages, setMessages] = useState<MessageObject[]>()

  const fetchAllUsers = () => {
    FetchPublicTimeline()
      .then(messages => {
        console.log(messages)
        let buildingMessage = getMessageArray(messages);
        setMessages(buildingMessage);
      });
  }

  useEffect(() => {
    FetchPublicTimeline()
      .then(messages => {
        console.log(messages)
        setMessages(messages);
      });
  }, [])
  


  if (messages != undefined){
    return (
      <div>
       {messages.map((mes) => (
        <view key={mes.messageId}>
          <view>
              <ul>
              {mes.messageId} {'\n'}
              {mes.authorId} {'\n'}
              {mes.text } {'\n'}
              {mes.pubDate} {'\n'}
              {mes.flagged} {'\n'}

              </ul>

          </view>
        </view>
      ))} 
    </div>
    );

  } else {
    return (
      <div>
      <button onClick={()=>console.log(messages)}> TEST </button>
    </div>
    );

  }

}

export default PublicTimeline