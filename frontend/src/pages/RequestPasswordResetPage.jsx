import { useState } from 'react';
import './AuthPages.css';

export default function RequestPasswordResetPage() {
  const [email, setEmail] = useState('');
  const [token, setToken] = useState('');
  const [expiry, setExpiry] = useState('');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    setToken('');
    setExpiry('');
    setLoading(true);
    try {
      const response = await fetch('/api/auth/request-password-reset', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(email)
      });
      if (!response.ok) throw new Error(await response.text());
      const data = await response.json();
      setToken(data.token);
      setExpiry(data.expiry);
    } catch (err) {
      setError(err.message || 'Request failed');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="auth-card">
      <h1 className="auth-title">Request Password Reset</h1>
      <form className="auth-form" onSubmit={handleSubmit}>
        <label className="auth-label">
          <span>Email</span>
          <input
            type="email"
            value={email}
            onChange={e => setEmail(e.target.value)}
            className="auth-input"
            required
          />
        </label>
        <button className="auth-button" type="submit" disabled={loading}>
          {loading ? 'Requesting...' : 'Request Reset'}
        </button>
        {error && <div className="auth-error">{error}</div>}
      </form>
      {token && (
        <div className="auth-success">
          <div><b>Token:</b> {token}</div>
          <div><b>Expires:</b> {expiry}</div>
          <a
            href={`/reset-password?token=${encodeURIComponent(token)}`}
            className="auth-link"
            style={{ display: 'inline-block', marginTop: '1rem' }}
          >
            Go to Reset Password
          </a>
        </div>
      )}
    </div>
  );
}
