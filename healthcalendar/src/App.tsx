//import { useState } from 'react'
//import reactLogo from './assets/react.svg'
//import viteLogo from '/vite.svg'
import HomePage from './home/HomePage'
import EventCalendar from './patientComponents/EventCalendar'
import LoginPage from './auth/LoginPage'
import RegistrationPage from './auth/RegistrationPage.tsx'
import WorkerLoginPage from './auth/WorkerLoginPage.tsx'
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom'
import './App.css'

const App: React.FC = () => {
  return (
    <Router>
      <Routes>
        <Route path='/' element={<HomePage />} />
        <Route path='/patient/events' element={<EventCalendar />} />
        <Route path='/login' element={<LoginPage />} />
        <Route path='/register' element={<RegistrationPage />} />
        <Route path='/worker/login' element={<WorkerLoginPage />} />
        <Route path='*' element={<HomePage />} />
      </Routes>
    </Router>
  )
}

export default App
