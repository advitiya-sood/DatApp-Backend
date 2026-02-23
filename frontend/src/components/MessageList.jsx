import React, { useEffect, useState } from 'react';
import { getMessages } from '../services/apiClient';
import '../styles/messages.css';

const MessageList = ({ container }) => {
  const [messages, setMessages] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchMessages = async () => {
      try {
        const data = await getMessages(container);
        setMessages(data);
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    };

    fetchMessages();
  }, [container]);

  if (loading) return <p>Loading messages...</p>;
  if (error) return <p>Error: {error}</p>;

  return (
    <div className="messages-list">
      <h2>{container} Messages</h2>
      {messages.length === 0 ? (
        <p>No messages found.</p>
      ) : (
        <ul>
          {messages.map((message) => (
            <li key={message.id}>
              <div className={`message-card ${message.isRead ? '' : 'unread'}`}>
                <div className="message-header">
                  <span className="avatar">
                    {(container === 'Inbox' || container === 'Unread')
                      ? (message.senderUsername ? message.senderUsername.charAt(0).toUpperCase() : '?')
                      : (message.recipientUsername ? message.recipientUsername.charAt(0).toUpperCase() : '?')}
                  </span>
                  {(container === 'Inbox' || container === 'Unread') ? (
                    <span><strong>From:</strong> {message.senderUsername || 'Unknown'}</span>
                  ) : (
                    <span><strong>To:</strong> {message.recipientUsername || 'Unknown'}</span>
                  )}
                  <span className="timestamp">{new Date(message.messageSent).toLocaleString()}</span>
                </div>
                <div className="message-content">
                  <p>{message.content}</p>
                </div>
                <div className="message-actions">
                  {/* Future: Reply, Delete, Mark as Read buttons */}
                </div>
              </div>
            </li>
          ))}
        </ul>
      )}
    </div>
  );
};

export default MessageList;