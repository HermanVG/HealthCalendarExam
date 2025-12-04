import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { registerWorker } from '../services/authService'
import { useToast } from '../shared/toastContext'
import '../styles/PatientRegistrationPage.css'
import '../styles/UserManagement.css'
import '../styles/EventCalendarPage.css'
import LogoutConfirmationModal from '../shared/LogoutConfirmationModal'

// Admin page for registering new healthcare workers

const WorkerRegistrationPage: React.FC = () => {
  const navigate = useNavigate()
  const { showSuccess } = useToast()

  const [name, setName] = useState('')
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')

  // Validation error state for each field
  const [nameError, setNameError] = useState<string | null>(null)
  const [emailError, setEmailError] = useState<string | null>(null)
  const [passwordError, setPasswordError] = useState<string | null>(null)

  // UI state
  const [loading, setLoading] = useState(false)
  const [showLogoutConfirm, setShowLogoutConfirm] = useState(false)

  // Handle form submission and worker registration
  const onSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    setNameError(null)
    setEmailError(null)
    setPasswordError(null)
    let hasError = false

    // Client-side validation
    if (!name) { setNameError('Name is required.'); hasError = true }
    else if (name.length > 30) {
      setNameError('Name must have 30 characters or less.'); hasError = true 
    }
    if (!email) { setEmailError('Email is required.'); hasError = true }
    else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) {
      setEmailError('Enter a valid email address (e.g., name@example.com).');
      hasError = true
    }
    if (!password) { setPasswordError('Password is required.'); hasError = true }
    else if (password.length < 6) {
      setPasswordError('Password must be at least 6 characters long.');
      hasError = true
    }
    else if (!/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).+$/.test(password)) {
      setPasswordError('Password must have at least one small letter, one big letter, one number and one special character.');
      hasError = true
    }

    // Attempt registration if name, email and password format is valid (backend will check for duplicates)
    const nameIsValid = name && name.length <= 30
    const emailIsValid = email && /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)
    const passwordIsValid = password && password.length >= 6 && 
      /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).+$/.test(password)
    if (nameIsValid && emailIsValid && passwordIsValid) {
      try {
        setLoading(true)
        await registerWorker({ Name: name, Email: email, Password: password })
        if (!hasError) {
          showSuccess('Healthcare worker registered successfully!')
          setName('')
          setEmail('')
          setPassword('')
        }
      } catch (err: any) {
        console.debug('Worker registration failed', err)
        const errorMessage = err?.message || ''
        if (errorMessage.includes('DuplicateUserName') || errorMessage.includes('already taken')) {
          setEmailError('This email is already in use.')
        }
      } finally {
        setLoading(false)
      }
    }
  }

  return (
    <div className="auth-page">
      <main className="admin-form-container">
        <section className="admin-form-content">
          <h1 className="auth-title">Register Healthcare Worker</h1>
          <form className="auth-form" onSubmit={onSubmit} noValidate>
            {/* Name input field */}
            <label>
              Name
              <input
                type="text"
                placeholder="Worker name here…"
                value={name}
                onChange={e => {
                  const v = e.target.value
                  setName(v)
                  const lengthOk = v.length <= 30
                  // Clear error when user starts typing a valid value
                  if (nameError && v.trim() && lengthOk) setNameError(null)
                }}
                className="auth-input"
                aria-invalid={!!nameError}
                required
              />
              {nameError && <small className="field-error">{nameError}</small>}
            </label>

            {/* Email input field with format validation */}
            <label>
              Email
              <input
                type="email"
                placeholder="Worker email here…"
                value={email}
                onChange={e => {
                  const v = e.target.value
                  setEmail(v)
                  const patternOk = /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(v)
                  // Clear error when user enters a valid email format
                  if (emailError) {
                    if (v.trim() && patternOk) setEmailError(null)
                  }
                }}
                className="auth-input"
                aria-invalid={!!emailError}
                required
              />
              {emailError && <small className="field-error">{emailError}</small>}
            </label>

            {/* Password input field with minimum length requirement */}
            <label>
              Password
              <input
                type="password"
                placeholder="Worker password here…"
                value={password}
                onChange={e => {
                  const v = e.target.value
                  setPassword(v)
                  const patternOk = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).+$/.test(v)
                  // Clear error when password meets minimum length and pattern is matched
                  if (passwordError) {
                    if (v.length >= 6 && patternOk) setPasswordError(null)
                  }
                }}
                className="auth-input"
                aria-invalid={!!passwordError}
                required
                minLength={6}
              />
              {passwordError && <small className="field-error">{passwordError}</small>}
            </label>
            <button className="auth-btn" type="submit" disabled={loading}>
              Register Worker
            </button>
          </form>
          <p className="auth-alt">
            <button
              type="button"
              onClick={() => navigate('/admin/manage')}
              className="admin-link-button"
            >
              Go back to Manage Workers & Patients
            </button>
          </p>
        </section>
      </main>

      {/* Logout confirmation modal */}
      <LogoutConfirmationModal
        isOpen={showLogoutConfirm}
        onClose={() => setShowLogoutConfirm(false)}
      />
    </div>
  )
}

export default WorkerRegistrationPage
