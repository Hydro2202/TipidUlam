import React from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext';
import './AppShell.css';

const AppShell = ({ children }) => {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate('/login', { replace: true });
  };

  return (
    <div className="shell">
      <header className="shell-header">
        <div className="shell-header-inner">
          <Link to="/" className="shell-logo">
            TipidUlam
          </Link>
          <nav className="shell-nav" aria-label="Account">
            <span className="shell-user">{user?.username}</span>
            <button type="button" className="btn btn-ghost btn-sm" onClick={handleLogout}>
              Sign out
            </button>
          </nav>
        </div>
      </header>
      <main className="shell-main">{children}</main>
    </div>
  );
};

export default AppShell;
