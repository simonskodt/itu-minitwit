import "../pages/Layout.css";
import { useState } from "react";
import { useEffect } from "react";
import { Message } from "../pages/Message";
import React from 'react';
import { MessageDTO } from "../models/MessageDTO";
import { MesssageService } from "../services/MessageService";


interface Props {
  pageNumber: number;
}

const MessageSliceComponent: React.FC<Props> = ({ pageNumber }) => {

  const [AllMessages, setMessages] = useState<MessageDTO[]>();
  const messageService = new MesssageService()

  useEffect(() => {
    const fetchMessages = async () => {
      const messages = await messageService.getPublicTimeline(pageNumber);
      setMessages(messages);
    };
    fetchMessages();

    const intervalId = setInterval(() => {
      fetchMessages();
    }, 5000);

    return () => clearInterval(intervalId);
  }, [pageNumber]);

  if (AllMessages !== undefined) {
    return (
      <>
        {AllMessages?.map((mes) => (
          <view key={mes.id}>
            <view>
              <Message
                username={mes.authorName}
                text={mes.text}
                date={mes.pubDate}
              />
            </view>
          </view>
        ))}
      </>
    );
  } else {
    return (
      <div>
        <ul className="messages">
          <li>
            <p></p>
          </li>
        </ul>
      </div>
    );
  }
}

export default MessageSliceComponent;