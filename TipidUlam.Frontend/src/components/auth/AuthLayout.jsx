import React from 'react';
import { Link } from 'react-router-dom';
import './Auth.css';

const AuthLayout = ({ title, subtitle, children, footer }) => (
  <div className="auth-page">
    <aside className="auth-aside">
      <div className="auth-aside-inner">
        <Link to="/" className="auth-brand">
          TipidUlam
        </Link>
        <p className="auth-aside-lead">
          Plan meals that fit your budget—priced for Filipino households, serving by serving.
        </p>
        <ul className="auth-aside-points">
          <li>See true cost per ulam for your household size</li>
          <li>Subtract what you already have in the pantry</li>
          <li>Recipes sorted from lowest to highest spend</li>
        </ul>
      </div>
    </aside>

    <main className="auth-main">
      <div className="auth-card">
        <header className="auth-card-header">
          <h1>{title}</h1>
          {subtitle && <p>{subtitle}</p>}
        </header>
        {children}
        {footer && <footer className="auth-card-footer">{footer}</footer>}
      </div>
    </main>
  </div>
);

export default AuthLayout;
