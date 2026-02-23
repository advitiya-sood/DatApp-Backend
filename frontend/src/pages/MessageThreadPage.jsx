import React from 'react';
import { useParams } from 'react-router-dom';
import MessageThread from '../components/MessageThread';
import '../styles/messages.css';

const MessageThreadPage = () => {
  const { recipientId } = useParams();

  return (
    <div>
      <h1>Message Thread</h1>
      <MessageThread recipientId={recipientId} />
    </div>
  );
};

export default MessageThreadPage;