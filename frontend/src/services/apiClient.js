const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || 'http://localhost:5000';

function getAuthHeaders() {
  const token = localStorage.getItem('datapp_token');
  return token ? { Authorization: `Bearer ${token}` } : {};
}

async function request(path, options = {}) {
  const headers = {
    ...(options.body instanceof FormData ? {} : { 'Content-Type': 'application/json' }),
    ...(options.headers || {}),
    ...getAuthHeaders()
  };

  const response = await fetch(`${API_BASE_URL}${path}`, {
    ...options,
    headers
  });

  const text = await response.text();
  let data;
  try {
    data = text ? JSON.parse(text) : null;
  } catch {
    data = text;
  }

  if (!response.ok) {
    const message = data?.message || data || 'Request failed';
    throw new Error(message);
  }

  return data;
}

export function login(username, password) {
  return request('/api/auth/login', {
    method: 'POST',
    body: JSON.stringify({ username, password })
  });
}

export function register(username, password) {
  return request('/api/auth/register', {
    method: 'POST',
    body: JSON.stringify({ username, password })
  });
}

export function fetchValues() {
  return request('/api/values', {
    method: 'GET'
  });
}

export function fetchUsers({ gender, minAge, maxAge } = {}) {
  const params = new URLSearchParams();
  if (gender) params.append('gender', gender);
  if (minAge != null) params.append('minAge', minAge);
  if (maxAge != null) params.append('maxAge', maxAge);
  const query = params.toString();

  return request(`/api/users${query ? `?${query}` : ''}`, {
    method: 'GET'
  });
}

export function fetchUser(id) {
  return request(`/api/users/${id}`, {
    method: 'GET'
  });
}

export function likeUser(userId, recipientId) {
  return request(`/api/users/${userId}/like/${recipientId}`, {
    method: 'POST'
  });
}export function updateUserProfile(id, payload) {
  return request(`/api/users/${id}`, {
    method: 'PUT',
    body: JSON.stringify(payload)
  });
}

export function fetchUserLikes(predicate) {
  return request(`/api/users/likes?predicate=${predicate}`, {
    method: 'GET'
  });
}