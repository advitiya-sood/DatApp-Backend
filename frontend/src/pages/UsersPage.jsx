import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { fetchUsers, likeUser } from '../services/apiClient.js';
import { useAuth } from '../hooks/useAuth.jsx';
import './UsersPage.css';

export default function UsersPage() {
  const [users, setUsers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [gender, setGender] = useState('');
  const [minAge, setMinAge] = useState(18);
  const [maxAge, setMaxAge] = useState(99);
  const [statusMessage, setStatusMessage] = useState('');

  const { userId, isAuthenticated } = useAuth();

  useEffect(() => {
    let cancelled = false;
    const load = async () => {
      try {
        const data = await fetchUsers({ gender, minAge, maxAge });
        if (!cancelled) {
          setUsers(Array.isArray(data) ? data : []);
        }
      } catch (err) {
        if (!cancelled) {
          setError(err.message || 'Failed to load users');
        }
      } finally {
        if (!cancelled) {
          setLoading(false);
        }
      }
    };
    load();
    return () => {
      cancelled = true;
    };
  }, [gender, minAge, maxAge]);

  const handleLike = async (event, targetUserId) => {
    event.preventDefault();
    event.stopPropagation();

    if (!isAuthenticated || !userId) {
      setStatusMessage('You must be logged in to like users.');
      return;
    }

    try {
      await likeUser(userId, targetUserId);
      setStatusMessage('You liked this user.');
    } catch (err) {
      setStatusMessage(err.message || 'Failed to like user.');
    }
  };

  return (
    <div className="users-container">
      <section className="users-panel">
        <header className="users-header">
          <h1>Members</h1>
          <p>Browse users coming from the secured `api/users` endpoint.</p>
        </header>
        <div className="users-filters">
          <div className="filter-group">
            <label htmlFor="gender-select">Gender</label>
            <select
              id="gender-select"
              value={gender}
              onChange={(e) => setGender(e.target.value)}
            >
              <option value="">All</option>
              <option value="male">Male</option>
              <option value="female">Female</option>
            </select>
          </div>
          <div className="filter-group">
            <label>Age range</label>
            <div className="age-range-inputs">
              <input
                type="number"
                min="18"
                max={maxAge}
                value={minAge}
                onChange={(e) => setMinAge(Number(e.target.value) || 18)}
              />
              <span>to</span>
              <input
                type="number"
                min={minAge}
                max="99"
                value={maxAge}
                onChange={(e) => setMaxAge(Number(e.target.value) || 99)}
              />
            </div>
          </div>
        </div>
        {statusMessage && <div className="users-status-message">{statusMessage}</div>}
        {loading && <div className="users-status">Loading members…</div>}
        {error && <div className="users-error">{error}</div>}
        {!loading && !error && (
          <div className="users-grid">
            {users.map((u) => {
              const id = u.id ?? u.Id;
              const username = u.username ?? u.Username;
              const knownAs = u.knownAs ?? u.KnownAs;
              const gender = u.gender ?? u.Gender;
              const age = u.age ?? u.Age;
              const city = u.city ?? u.City;
              const country = u.country ?? u.Country;
              const photoUrl = u.photoUrl ?? u.PhotoUrl;

              return (
                <Link key={id} to={`/users/${id}`} className="user-card">
                  <div className="user-avatar-wrapper">
                    {photoUrl ? (
                      <img src={photoUrl} alt={knownAs || username} className="user-avatar" />
                    ) : (
                      <div className="user-avatar-fallback">
                        {(knownAs || username || '?').charAt(0).toUpperCase()}
                      </div>
                    )}
                  </div>
                  <div className="user-card-main">
                    <div className="user-card-header">
                      <span className="user-name">{knownAs || username}</span>
                      {gender && age && (
                        <span className="user-meta">
                          {gender} • {age}
                        </span>
                      )}
                    </div>
                    {(city || country) && (
                      <div className="user-location">
                        {city}
                        {city && country && ', '}
                        {country}
                      </div>
                    )}
                    {id !== userId && (
                      <div className="user-actions">
                        <button
                          type="button"
                          className="like-button"
                          onClick={(e) => handleLike(e, id)}
                        >
                          ❤️ Like
                        </button>
                      </div>
                    )}
                  </div>
                </Link>
              );
            })}
            {users.length === 0 && <div className="users-empty">No members found.</div>}
          </div>
        )}
      </section>
    </div>
  );
}

