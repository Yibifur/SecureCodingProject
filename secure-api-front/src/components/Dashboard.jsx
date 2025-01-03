// Dashboard.jsx
import React, { useEffect, useState } from 'react';
import './styles.css';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

const Dashboard = () => {
    const navigate = useNavigate()

    const { logout } = useAuth()

    const handleLogout = async () => {

        logout()



    }

    return (
        <div className="dashboard">
            <header className="dashboard-header">
                <h1>Dashboard</h1>
                <button className="logout-button" onClick={handleLogout}>Logout</button>
            </header>
            <div className="dashboard-content">
                <div className="dashboard-card">
                    <h2>Doctors</h2>
                    <p>....</p>
                </div>
                <div className="dashboard-card">
                    <h2>Users</h2>
                    <p>.....</p>
                </div>

            </div>
            <footer className="dashboard-footer">
                <p>© 2024 SecureCoding. All rights reserved.</p>
            </footer>
        </div>
    );
};

export default Dashboard;
