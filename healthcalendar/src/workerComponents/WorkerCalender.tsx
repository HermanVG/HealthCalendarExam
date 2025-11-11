import React from 'react'
import { useNavigate } from 'react-router-dom'
import { useAuth } from '../auth/AuthContext'

const WorkerCalendar: React.FC = () => {
	const { logout } = useAuth()
	const navigate = useNavigate()
	return (
		<div style={{ padding: 24 }}>
			<div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
				<h2>Worker Calendar</h2>
				<button
					onClick={() => { logout(); navigate('/worker/login', { replace: true }); }}
					style={{ padding: '8px 12px', cursor: 'pointer' }}
				>
					Log Out
				</button>
			</div>
			<p>Scheduling interface coming soon.</p>
		</div>
	)
}

export default WorkerCalendar
