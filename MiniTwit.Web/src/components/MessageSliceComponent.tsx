import "../pages/Layout.css";
import { fetchPublicTimeline } from "../pages/fetch";
import { useState } from "react";
import { MessageObjectWithName } from "../builders/interface";
import React, { useEffect } from "react";
import { Message } from "../pages/Message";

interface Props {
  pageNumber: number;
}

const MessageSliceComponent: React.FC<Props> = ({ pageNumber }) => {

  const [AllMessages, setMessages] = useState<MessageObjectWithName[]>();

  useEffect(() => {
    fetchPublicTimeline(pageNumber).then((messages) => {
      setMessages(messages);
    });
  }, []);

  if (AllMessages != undefined) {
    return (
      <>
        {AllMessages?.map((mes) => (
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