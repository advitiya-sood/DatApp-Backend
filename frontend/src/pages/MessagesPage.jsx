import React, { useState } from 'react';
import MessageList from '../components/MessageList';
import '../styles/messages.css';

const MessagesPage = () => {
  const [container, setContainer] = useState('Inbox');

  return (
    <div>
      <h1>Messages</h1>
      <div>
        <button onClick={() => setContainer('Inbox')}>Inbox</button>
        <button onClick={() => setContainer('Outbox')}>Outbox</button>
        <button onClick={() => setContainer('Unread')}>Unread</button>
      </div>
      <MessageList container={container} />
    </div>
  );
};

export default MessagesPage;