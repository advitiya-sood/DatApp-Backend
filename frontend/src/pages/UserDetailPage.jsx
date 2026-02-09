import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { fetchUser, likeUser, updateUserProfile } from '../services/apiClient.js';
import { useAuth } from '../hooks/useAuth.jsx';
import './UserDetailPage.css';

export default function UserDetailPage() {
  const { id } = useParams();
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
   const [statusMessage, setStatusMessage] = useState('');
  const [isEditing, setIsEditing] = useState(false);
  const [editForm, setEditForm] = useState({
    knownAs: '',
    gender: '',
    lookingFor: '',
    aboutMe: '',
    intrests: '',
    city: '',
    country: ''
  });

  const { userId } = useAuth();

  useEffect(() => {
    let cancelled = false;
    const load = async () => {
      try {
        const data = await fetchUser(id);
        if (!cancelled) {
          setUser(data);
          const knownAs = data.knownAs ?? data.KnownAs ?? '';
          const gender = data.gender ?? data.Gender ?? '';
          const lookingFor = data.lookingFor ?? data.LookingFor ?? '';
          const aboutMe = data.aboutMe ?? data.AboutMe ?? '';
          const intrests = data.intrests ?? data.Intrests ?? '';
          const city = data.city ?? data.City ?? '';
          const country = data.country ?? data.Country ?? '';
          setEditForm({
            knownAs,
            gender,
            lookingFor,
            aboutMe,
            intrests,
            city,
            country
          });
        }
      } catch (err) {
        if (!cancelled) {
          setError(err.message || 'Failed to load user');
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
  }, [id]);

  const handleLike = async () => {
    if (!user || !userId) {
      setStatusMessage('You must be logged in to like users.');
      return;
    }
    try {
      await likeUser(userId, user.id ?? user.Id);
      setStatusMessage('You liked this user.');
    } catch (err) {
      setStatusMessage(err.message || 'Failed to like user.');
    }
  };

  const handleEditChange = (e) => {
    const { name, value } = e.target;
    setEditForm((prev) => ({ ...prev, [name]: value }));
  };

  const handleEditSubmit = async (e) => {
    e.preventDefault();
    if (!userId || Number(id) !== userId) {
      setStatusMessage('You can only edit your own profile.');
      return;
    }

    try {
      await updateUserProfile(userId, editForm);
      setStatusMessage('Profile updated.');
      setIsEditing(false);
      // refresh displayed data
      const refreshed = await fetchUser(id);
      setUser(refreshed);
    } catch (err) {
      setStatusMessage(err.message || 'Failed to update profile.');
    }
  };

  if (loading) {
    return (
      <div className="user-detail-container">
        <section className="user-detail-panel">
          <div className="user-detail-status">Loading profile…</div>
        </section>
      </div>
    );
  }

  if (error) {
    return (
      <div className="user-detail-container">
        <section className="user-detail-panel">
          <div className="user-detail-error">{error}</div>
        </section>
      </div>
    );
  }

  if (!user) {
    return (
      <div className="user-detail-container">
        <section className="user-detail-panel">
          <div className="user-detail-status">User not found.</div>
        </section>
      </div>
    );
  }

  const knownAs = user.knownAs ?? user.KnownAs;
  const username = user.username ?? user.Username;
  const gender = user.gender ?? user.Gender;
  const age = user.age ?? user.Age;
  const city = user.city ?? user.City;
  const country = user.country ?? user.Country;
  const aboutMe = user.aboutMe ?? user.AboutMe;
  const lookingFor = user.lookingFor ?? user.LookingFor;
  const intrests = user.intrests ?? user.Intrests;
  const photoUrl = user.photoUrl ?? user.PhotoUrl;
  const photos = user.photos ?? user.Photos ?? [];
  const isCurrentUser = userId && userId === (user.id ?? user.Id);

  return (
    <div className="user-detail-container">
      <section className="user-detail-panel">
        {statusMessage && <div className="user-detail-status-message">{statusMessage}</div>}
        <div className="user-detail-header">
          <div className="user-detail-avatar-wrapper">
            {photoUrl ? (
              <img src={photoUrl} alt={knownAs || username} className="user-detail-avatar" />
            ) : (
              <div className="user-detail-avatar-fallback">
                {(knownAs || username || '?').charAt(0).toUpperCase()}
              </div>
            )}
          </div>
          <div className="user-detail-heading">
            <h1>{knownAs || username}</h1>
            <div className="user-detail-meta">
              {gender && <span>{gender}</span>}
              {age && <span>{age} years old</span>}
              {(city || country) && (
                <span>
                  {city}
                  {city && country && ', '}
                  {country}
                </span>
              )}
            </div>
          </div>
          {!isCurrentUser && (
            <div className="user-detail-actions">
              <button type="button" className="like-button" onClick={handleLike}>
                ❤️ Like
              </button>
            </div>
          )}
        </div>

        <div className="user-detail-body">
          {isCurrentUser && (
            <div className="user-detail-section">
              <div className="edit-header">
                <h2>Your profile</h2>
                <button type="button" onClick={() => setIsEditing((v) => !v)}>
                  {isEditing ? 'Cancel' : 'Edit profile'}
                </button>
              </div>
              {isEditing && (
                <form className="profile-edit-form" onSubmit={handleEditSubmit}>
                  <div className="form-row">
                    <label>
                      Display name
                      <input
                        name="knownAs"
                        value={editForm.knownAs}
                        onChange={handleEditChange}
                      />
                    </label>
                  </div>
                  <div className="form-row">
                    <label>
                      Gender
                      <select
                        name="gender"
                        value={editForm.gender}
                        onChange={handleEditChange}
                      >
                        <option value="">Select</option>
                        <option value="male">Male</option>
                        <option value="female">Female</option>
                      </select>
                    </label>
                  </div>
                  <div className="form-row">
                    <label>
                      About me
                      <textarea
                        name="aboutMe"
                        value={editForm.aboutMe}
                        onChange={handleEditChange}
                      />
                    </label>
                  </div>
                  <div className="form-row">
                    <label>
                      Looking for
                      <textarea
                        name="lookingFor"
                        value={editForm.lookingFor}
                        onChange={handleEditChange}
                      />
                    </label>
                  </div>
                  <div className="form-row">
                    <label>
                      Interests
                      <textarea
                        name="intrests"
                        value={editForm.intrests}
                        onChange={handleEditChange}
                      />
                    </label>
                  </div>
                  <div className="form-row inline">
                    <label>
                      City
                      <input
                        name="city"
                        value={editForm.city}
                        onChange={handleEditChange}
                      />
                    </label>
                    <label>
                      Country
                      <input
                        name="country"
                        value={editForm.country}
                        onChange={handleEditChange}
                      />
                    </label>
                  </div>
                  <div className="form-actions">
                    <button type="submit">Save changes</button>
                  </div>
                </form>
              )}
            </div>
          )}
          {(aboutMe || lookingFor || intrests) && (
            <div className="user-detail-section">
              <h2>About</h2>
              {aboutMe && <p>{aboutMe}</p>}
              {lookingFor && (
                <p>
                  <strong>Looking for:</strong> {lookingFor}
                </p>
              )}
              {intrests && (
                <p>
                  <strong>Interests:</strong> {intrests}
                </p>
              )}
            </div>
          )}

          <div className="user-detail-section">
            <h2>Photos</h2>
            {photos.length === 0 && <div className="user-detail-empty">No photos uploaded.</div>}
            {photos.length > 0 && (
              <div className="user-detail-gallery">
                {photos.map((p) => {
                  const url = p.url ?? p.Url;
                  const caption = p.caption ?? p.Caption;
                  const isMain = p.isMain ?? p.IsMain;
                  return (
                    <figure key={p.id ?? p.Id} className={`user-detail-photo ${isMain ? 'photo-main' : ''}`}>
                      <img src={url} alt={caption || 'User photo'} />
                      {caption && <figcaption>{caption}</figcaption>}
                      {isMain && <span className="photo-badge">Main</span>}
                    </figure>
                  );
                })}
              </div>
            )}
          </div>
        </div>
      </section>
    </div>
  );
}

