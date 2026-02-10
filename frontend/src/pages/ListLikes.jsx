import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { fetchUserLikes, likeUser } from '../services/apiClient.js';
import { useAuth } from '../hooks/useAuth.jsx';
import './ListLikes.css';
import './UsersPage.css'; 

export default function ListLikes() {
  const [users, setUsers] = useState([]);
  const [predicate, setPredicate] = useState('Likers'); // Default: Who likes me
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const { userId } = useAuth();

  useEffect(() => {
    let cancelled = false;
    const load = async () => {
      setLoading(true);
      setError('');
      try {
        const data = await fetchUserLikes(predicate);
        if (!cancelled) {
          setUsers(Array.isArray(data) ? data : []);
        }
      } catch (err) {
        if (!cancelled) {
          setError(err.message || 'Failed to load lists');
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
  }, [predicate]);

  return (
    <div className="lists-container">
      <section className="lists-panel">
        <header className="lists-header">
          <h1>{predicate === 'Likers' ? 'Members who like me' : 'Members I like'}</h1>
        </header>

        <div className="lists-tab-group">
          <button 
            className={`lists-tab ${predicate === 'Likers' ? 'active' : ''}`}
            onClick={() => setPredicate('Likers')}
          >
            Members who like me
          </button>
          <button 
            className={`lists-tab ${predicate === 'Likees' ? 'active' : ''}`}
            onClick={() => setPredicate('Likees')}
          >
            Members I like
          </button>
        </div>

        {loading && <div className="users-status">Loading...</div>}
        {error && <div className="users-error">{error}</div>}

        {!loading && !error && (
          <div className="lists-grid">
            {users.map((u) => {
              // Safe access to properties
              const id = u.id ?? u.Id;
              const username = u.username ?? u.Username;
              const knownAs = u.knownAs ?? u.KnownAs;
              const gender = u.gender ?? u.Gender;
              const age = u.age ?? u.Age;
              const city = u.city ?? u.City;
              const photoUrl = u.photoUrl ?? u.PhotoUrl;

              return (
                <Link key={id} to={`/users/${id}`} className="user-card">
                   <div className="user-avatar-wrapper">
                    {photoUrl ? (
                      <img src={photoUrl} alt={knownAs} className="user-avatar" />
                    ) : (
                      <div className="user-avatar-fallback">
                        {(knownAs || username || '?').charAt(0).toUpperCase()}
                      </div>
                    )}
                  </div>
                  <div className="user-card-main">
                    <div className="user-card-header">
                      <span className="user-name">{knownAs || username}</span>
                      <span className="user-meta">{age}</span>
                    </div>
                    <div className="user-location">{city}</div>
                  </div>
                </Link>
              );
            })}
            {users.length === 0 && (
              <div className="lists-empty">
                {predicate === 'Likers' 
                  ? "No one has liked you yet. Keep your profile updated!" 
                  : "You haven't liked anyone yet."}
              </div>
            )}
          </div>
        )}
      </section>
    </div>
  );
}