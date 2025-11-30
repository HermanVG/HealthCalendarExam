import React from 'react'
import { Navigate } from 'react-router-dom'
import { useAuth } from './AuthContext'
import type { Role } from '../types/user'

// Protected route component that checks authentication and role-based authorization

// Props for configuring route protection
interface Props {
  allowedRoles?: Role[]        // Optional array of roles that can access this route
  redirectPrefix: string       // Base path for redirect (e.g., '/patient', '/worker')
  redirectSuffix?: string      // Suffix for redirect path (defaults to '/login')
  children: React.ReactNode    // The protected content to render if authorized
}

const ProtectedRoute: React.FC<Props> = ({ allowedRoles, redirectPrefix, redirectSuffix = '/login', children }) => {
  // Get authentication state from context
  const { user, token, isLoading } = useAuth()

  if (isLoading) return null

  // Redirect to login if user is not authenticated (no token or user data)
  if (!token || !user) return <Navigate to={redirectPrefix + redirectSuffix} replace />

  // If specific roles are required, check if user's role is allowed
  if (allowedRoles && !allowedRoles.includes(user.role)) {
    return <Navigate to={redirectPrefix + redirectSuffix} replace />
  }

  // User is authenticated and authorized - render the protected content
  return <>{children}</>
}

export default ProtectedRoute
