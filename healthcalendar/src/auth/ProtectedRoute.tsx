import React from 'react'
import { Navigate } from 'react-router-dom'
import { useAuth } from './AuthContext'
import type { Role } from '../types/user'

interface Props {
  allowedRoles?: Role[]
  redirectTo?: string
  children: React.ReactNode
}

const ProtectedRoute: React.FC<Props> = ({ allowedRoles, redirectTo = '/login', children }) => {
  const { user, token, isLoading } = useAuth()

  if (isLoading) return null

  // Not authenticated
  if (!token || !user) return <Navigate to={redirectTo} replace />

  // Role check if provided
  if (allowedRoles && !allowedRoles.includes(user.role)) {
    return <Navigate to={redirectTo} replace />
  }

  return <>{children}</>
}

export default ProtectedRoute
