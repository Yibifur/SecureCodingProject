import React from 'react'
import Login from '../components/Login';
import DoctorLogin from "../components/DoctorLogin"
import DoctorRegister from "../components/DoctorRegister"
import UserRegister from "../components/UserRegister"
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import { AuthProvider, useAuth } from "../context/AuthContext"
import Dashboard from '../components/Dashboard';
import { useEffect, useState } from 'react';
import AuthMiddleware from './AuthMiddleware';

export default function Root() {


    return (

        <BrowserRouter>

            <Routes>
                <Route path="/dashboard" element={<AuthMiddleware>
                    <Dashboard />
                </AuthMiddleware>} />
                <Route path="/" element={<Login />} />
                <Route path="/doctor-login" element={<DoctorLogin />} />
                <Route path="/doctor-register" element={<DoctorRegister />} />
                <Route path="/user-register" element={<UserRegister />} />
            </Routes>

        </BrowserRouter>

    )
}
