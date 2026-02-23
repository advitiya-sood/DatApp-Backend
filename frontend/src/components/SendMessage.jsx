import React, { useState } from 'react';
import { sendMessage } from '../services/apiClient';

const SendMessage = ({ recipientId, onMessageSent }) => {
  const [content, setContent] = useState('');
  const [error, setError] = useState(null);

  const handleSend = async () => {
    try {
      const message = { recipientId, content };
      const sentMessage = await sendMessage(message);
      setContent('');
      if (onMessageSent) onMessageSent(sentMessage);
    } catch (err) {
      setError(err.message);
    }
  };

  return (
    <div>
      <h3>Send a Message</h3>
      {error && <p style={{ color: 'red' }}>Error: {error}</p>}
      <textarea
        value={content}
        onChange={(e) => setContent(e.target.value)}
        placeholder="Write your message here..."
      />
      <button onClick={handleSend}>Send</button>
    </div>
  );
};

export default SendMessage;