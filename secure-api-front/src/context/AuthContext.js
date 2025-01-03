import React, { createContext, useContext, useEffect, useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {

    const apiUrl = "https://api.guls4h.com/api/Auth"

    const [called, setCalled] = useState(false)

    const [userToken, setUserToken] = useState(() => {
        return localStorage.getItem('user') || undefined;
    });

    const [doctorToken, setDoctorToken] = useState(() => {
        return localStorage.getItem('doctor') || undefined;
    });


    useEffect(() => {
        const user = localStorage.getItem('user')
        const doctor = localStorage.getItem('doctor')
        setUserToken(user)
        setDoctorToken(doctor)
    }, [called])

    const userLogin = async (credentials) => {
        try {
            const response = await axios.post(apiUrl + "/LoginUser", credentials);
            const userData = response.data;
            const cleanToken = userData.replace(/^"|"$/g, '');
            console.log(cleanToken);
            localStorage.setItem('user', JSON.stringify(cleanToken));
            localStorage.removeItem('doctor')
            setCalled(!called)
        } catch (error) {
            console.error('Login failed:', error);
            throw error;
        }
    };

    const doctorLogin = async (credentials) => {
        try {
            const response = await axios.post(apiUrl + "/LoginDoctor", credentials);
            const userData = response.data;
            const cleanToken = userData.replace(/^"|"$/g, '');
            console.log(cleanToken);
            localStorage.setItem('doctor', JSON.stringify(cleanToken));
            localStorage.removeItem('user')
            setCalled(!called)

        } catch (error) {
            console.error('Login failed:', error);
            throw error;
        }
    };


    const logout = async (jwt) => {
        try {

            let userToken = localStorage.getItem("user")
            let doctorToken = localStorage.getItem("doctor")

            userToken = userToken?.replace(/^"|"$/g, '');
            doctorToken = doctorToken?.replace(/^"|"$/g, '');


            if (userToken) {

                await axios.post(
                    apiUrl + "/LogoutDoctor",
                    null,
                    {
                        params: {
                            key: userToken,
                        },

                    }
                );
                localStorage.removeItem('user')

            }


            else if (doctorToken) {

                await axios.post(
                    apiUrl + "/LogoutDoctor",
                    null,
                    {
                        params: {
                            key: doctorToken,
                        },

                    }
                );
                localStorage.removeItem('doctor')

            }
            setCalled(!called)

        } catch (error) {
            console.error('Login failed:', error);

        }
    };



    const userRegister = async (credentials) => {
        try {
            console.log(credentials)
            const response = await axios.post(apiUrl + "/RegisterUser", credentials);
            const userData = response.data;
            localStorage.setItem('user', JSON.stringify(userData));
        } catch (error) {
            console.error('Login failed:', error);
            throw error;
        }
    };

    const doctorRegister = async (credentials) => {
        try {
            const response = await axios.post(apiUrl + "/RegisterDoctor", credentials);
            const userData = response.data;
            localStorage.setItem('user', JSON.stringify(userData));
        } catch (error) {
            console.error('Login failed:', error);
            throw error;
        }
    }



    return (
        <AuthContext.Provider value={{ userLogin, doctorLogin, logout, userRegister, doctorRegister, userToken, doctorToken, called }}>
            {children}
        </AuthContext.Provider>
    );
};

export const useAuth = () => {
    return useContext(AuthContext);
};
