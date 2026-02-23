import React, { useEffect, useState } from 'react';
import { getMessageThread, sendMessage } from '../services/apiClient';
import { useAuth } from '../hooks/useAuth.jsx';
import '../styles/messages.css';

const MessageThread = ({ recipientId }) => {
  const { username: currentUsername } = useAuth();
  const [messages, setMessages] = useState([]);
  const [newMessage, setNewMessage] = useState('');
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchMessageThread = async () => {
      try {
        const data = await getMessageThread(recipientId);
        setMessages(data);
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    };

    fetchMessageThread();
  }, [recipientId]);

  const handleSendMessage = async () => {
    try {
      const message = { recipientId, content: newMessage };
      const sentMessage = await sendMessage(message);
      setMessages([sentMessage, ...messages]);
      setNewMessage('');
    } catch (err) {
      setError(err.message);
    }
  };

  if (loading) return <p>Loading messages...</p>;
  if (error) return <p>Error: {error}</p>;

  return (
    <div className="message-thread-container">
      <h2>Message Thread</h2>
      <div>
        <input
          type="text"
          value={newMessage}
          onChange={(e) => setNewMessage(e.target.value)}
          placeholder="Type your message..."
        />
        <button onClick={handleSendMessage}>Send</button>
      </div>
      <ul className="thread-list">
        {messages.map((message) => {
          const isMe = message.senderUsername === currentUsername;
          return (
            <li key={message.id} className={`thread-message ${isMe ? 'me' : 'other'}`}>
              <div className="bubble">
                <span className="avatar">
                  {message.senderUsername ? message.senderUsername.charAt(0).toUpperCase() : '?'}
                </span>
                <span className="sender">{message.senderUsername}</span>
                <span className="content">{message.content}</span>
              </div>
              <span className="timestamp">{new Date(message.messageSent).toLocaleString()}</span>
            </li>
          );
        })}
      </ul>
    </div>
  );
};

export default MessageThread;