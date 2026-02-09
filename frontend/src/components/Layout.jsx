import { Link, useLocation, useNavigate } from 'react-router-dom';
import { useAuth } from '../hooks/useAuth.jsx';
import './Layout.css';

export default function Layout({ children }) {
  const { isAuthenticated, logout } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  return (
    <div className="app-shell">
      <header className="app-header">
        <div className="app-logo">DatApp</div>
        <nav className="app-nav">
          {isAuthenticated && (
            <>
              <button type="button" className="nav-link" onClick={() => navigate('/')}>
                Values
              </button>
              <button type="button" className="nav-link" onClick={() => navigate('/users')}>
                Users
              </button>
            </>
          )}
          {!isAuthenticated && (
            <>
              <Link
                to="/login"
                className={`nav-link ${location.pathname === '/login' ? 'nav-link-active' : ''}`}
              >
                Login
              </Link>
              <Link
                to="/register"
                className={`nav-link ${location.pathname === '/register' ? 'nav-link-active' : ''}`}
              >
                Register
              </Link>
            </>
          )}
          {isAuthenticated && (
            <button type="button" className="nav-link nav-link-logout" onClick={handleLogout}>
              Logout
            </button>
          )}
        </nav>
      </header>
      <main className="app-main">{children}</main>
    </div>
  );
}

