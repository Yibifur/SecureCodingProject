import React, { useEffect } from 'react'
import { useNavigate } from 'react-router-dom'
import { useAuth } from '../context/AuthContext'

export default function AuthMiddleware({ children }) {

    const navigate = useNavigate()

    const { userToken, doctorToken } = useAuth()

    useEffect(() => {

        if (!(userToken || doctorToken)) navigate("/")

    }, [userToken, doctorToken])

    return <>{children}</>
}
