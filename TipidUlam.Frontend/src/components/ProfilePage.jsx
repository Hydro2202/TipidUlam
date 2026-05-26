import React, { useState } from 'react';
import { useAuth } from '../context/AuthContext';
import './ProfilePage.css';

const ProfilePage = () => {
  const { user, lastPassword } = useAuth();
  const [reveal, setReveal] = useState(false);

  if (!user) return null;

  return (
    <div className="profile-container">
      <div className="profile-hero">
        <div className="profile-avatar-large">
          {user?.username?.charAt(0).toUpperCase()}
        </div>
        <div className="profile-hero-text">
          <h1>{user.username}</h1>
          <p className="profile-subtitle">Account Credentials</p>
        </div>
      </div>

      <div className="profile-card">
        <div className="profile-section">
          <div className="credential-item">
            <div className="credential-header">
              <svg className="credential-icon" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                <path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2" />
                <circle cx="12" cy="7" r="4" />
              </svg>
              <span className="credential-label">Username</span>
            </div>
            <div className="credential-value">{user.username}</div>
          </div>

          <div className="credential-item">
            <div className="credential-header">
              <svg className="credential-icon" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                <rect x="2" y="4" width="20" height="16" rx="2" />
                <path d="M22 6l-10 5L2 6" />
              </svg>
              <span className="credential-label">Email Address</span>
            </div>
            <div className="credential-value email-value">{user.email}</div>
          </div>

          <div className="credential-item">
            <div className="credential-header">
              <svg className="credential-icon" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                <rect x="3" y="11" width="18" height="11" rx="2" ry="2" />
                <path d="M7 11V7a5 5 0 0 1 10 0v4" />
              </svg>
              <span className="credential-label">Password</span>
            </div>
            <div className="password-container">
              <input
                readOnly
                type={reveal ? 'text' : 'password'}
                value={lastPassword || ''}
                placeholder={lastPassword ? '' : 'Not available'}
                className="credential-value password-input"
              />
              <button
                type="button"
                className="reveal-toggle"
                onClick={() => setReveal((s) => !s)}
                title={reveal ? 'Hide password' : 'Show password'}
              >
                {reveal ? (
                  <svg viewBox="0 0 24 24" fill="currentColor">
                    <path d="M3.98 8.223A10.477 10.477 0 001.934 12c1.226 4.338 5.557 7.5 10.734 7.5.847 0 1.666-.105 2.457-.318m5.387-5.387C23.922 10.331 24 9.681 24 9c0-4.338-4.226-8-9.5-8-3.463 0-6.461 1.613-8.48 4.069M5.084 9c1.965-2.288 5.141-3.75 8.916-3.75 5.22 0 9.5 3.134 9.5 7 0 1.042-.208 2.053-.586 3.019" />
                  </svg>
                ) : (
                  <svg viewBox="0 0 24 24" fill="currentColor">
                    <path d="M12 4.5C7 4.5 2.73 7.61 1 12c1.73 4.39 6 7.5 11 7.5s9.27-3.11 11-7.5c-1.73-4.39-6-7.5-11-7.5zM12 17c-2.76 0-5-2.24-5-5s2.24-5 5-5 5 2.24 5 5-2.24 5-5 5zm0-8c-1.66 0-3 1.34-3 3s1.34 3 3 3 3-1.34 3-3-1.34-3-3-3z" />
                  </svg>
                )}
              </button>
            </div>
          </div>
        </div>

        <div className="profile-footer">
          <p className="profile-security-note">
            <svg className="info-icon" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
              <circle cx="12" cy="12" r="10" />
              <path d="M12 16v-4M12 8h.01" />
            </svg>
            Your password is stored securely in your session and will be cleared when you sign out.
          </p>
        </div>
      </div>
    </div>
  );
};

export default ProfilePage;
