import { useState } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { registerPatient } from '../services/authService'
import '../styles/PatientRegistrationPage.css'
import NavBar from '../shared/NavBar'

// Patient registration page for creating new patient accounts

const PatientRegistrationPage: React.FC = () => {
  const navigate = useNavigate()
  
  const [name, setName] = useState('')
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  
  // Validation error state for each field
  const [nameError, setNameError] = useState<string | null>(null)
  const [emailError, setEmailError] = useState<string | null>(null)
  const [passwordError, setPasswordError] = useState<string | null>(null)
  const [formError, setFormError] = useState<string | null>(null)

  
  const [loading, setLoading] = useState(false)

  // Handle form submission and patient registration
  const onSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    
    // Clear previous validation errors
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
    else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) { setEmailError('Enter a valid email address (e.g., name@example.com).'); hasError = true }
    if (!password) { setPasswordError('Password is required.'); hasError = true }
    else if (password.length < 6) { setPasswordError('Password must be at least 6 characters long.'); hasError = true }
    else if (!/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).+$/.test(password)) {
      setPasswordError('Password must have at least one small letter, one big letter, one number and one special character.');
      hasError = true
    }
    
    // Stop submission if validation errors exist
    if (hasError) return
    
    try {
      setLoading(true)
      await registerPatient({ Name: name, Email: email, Password: password })
      // Redirect to login page after successful registration
      navigate('/patient/login')
    } catch (err: any) {
      console.debug('Registration failed', err)
      setFormError(err?.message || 'Registration failed')
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="auth-page">
      <NavBar />
      <main className="auth-main">
        {/* Left side with decorative image */}
        <section className="auth-left">
          <img src="/images/register_login.png" alt="Register" className="auth-image" />
        </section>
        {/* Right side with registration form */}
        <section className="auth-right">
          <h1 className="auth-title auth-title--nowrap">Create your account</h1>
          {/* Display form-level errors (authentication failures) */}
					{formError && <div role="alert" className="form-error-banner">{formError}</div>}
          <form className="auth-form" onSubmit={onSubmit} noValidate>
            {/* Name input field */}
            <label>
              Name
              <input
                type="text"
                placeholder="Your name here…"
                value={name}
                onChange={e => {
                  const v = e.target.value
                  setName(v)
                  // Clear error when user starts typing valid input
                  if (nameError && v.length <= 30) setNameError(null)
                }}
                className="auth-input"
                aria-invalid={!!nameError}
                required
              />
              {nameError && <small className="field-error">{nameError}</small>}
            </label>
            {/* Email input field */}
            <label>
              Email
              <input
                type="email"
                placeholder="Your email here…"
                value={email}
                onChange={e => {
                  const v = e.target.value
                  setEmail(v)
                  const patternOk = /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(v)
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
            {/* Password input field */}
            <label>
              Password
              <input
                type="password"
                placeholder="Your password here…"
                value={password}
                onChange={e => {
                  const v = e.target.value
                  setPassword(v)
                  const patternOk = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).+$/.test(v)
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
            {/* Submit button for registration */}
            <button className="auth-btn" type="submit" disabled={loading}>Sign Up</button>
          </form>
          {/* Link to login page for existing users */}
          <p className="auth-alt">
            Have an account? <Link to="/patient/login">Log in here</Link>
          </p>
        </section>
      </main>
    </div>
  )
}

export default PatientRegistrationPage