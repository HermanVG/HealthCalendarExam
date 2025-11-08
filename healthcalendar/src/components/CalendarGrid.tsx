import type { Event } from '../types/event'
import '../styles/CalendarGrid.css'

export type CalendarGridProps = {
  events: Event[]
  weekStartISO: string // Monday of the week in YYYY-MM-DD
  startHour?: number // default 8
  endHour?: number // default 20
  slotMinutes?: number // default 30
  onEdit?: (e: Event) => void
}

// Helper to convert time strings to minutes from midnight
const toMinutes = (t: string) => {
  const [h, m] = t.split(':').map(Number)
  return h * 60 + m
}

const formatTimeLabel = (mins: number) => {
  const h = Math.floor(mins / 60)
  const m = mins % 60
  return `${String(h).padStart(2, '0')}:${String(m).padStart(2, '0')}`
}

const addDays = (iso: string, days: number) => {
  const d = new Date(iso)
  d.setDate(d.getDate() + days)
  return d.toISOString().slice(0, 10)
}

const dayNames = ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun']

export default function CalendarGrid({
  events,
  weekStartISO,
  startHour = 8,
  endHour = 20,
  slotMinutes = 30,
  onEdit
}: CalendarGridProps) {
  const startMins = startHour * 60
  const endMins = endHour * 60
  const totalSlots = Math.floor((endMins - startMins) / slotMinutes)

  const timeLabels = Array.from({ length: totalSlots + 1 }, (_, i) => startMins + i * slotMinutes)

  const days = Array.from({ length: 7 }, (_, i) => addDays(weekStartISO, i))
  const todayISO = new Date().toISOString().slice(0, 10)

  const eventsByDay = days.map(d => events.filter(e => e.date === d))

  return (
    <div className="cal-grid">
      <div className="cal-grid__header">
        <div className="cal-grid__corner" />
        {days.map((d, idx) => (
          <div className={`cal-grid__day${d === todayISO ? ' cal-grid__day--today' : ''}`} key={d}>
            <div className="cal-grid__dayname">{dayNames[idx]}</div>
            <div className="cal-grid__daydate">{d}</div>
          </div>
        ))}
      </div>
      <div className="cal-grid__body">
        <div className="cal-grid__times">
          {timeLabels.map((m) => (
            <div className="cal-grid__time" key={m}>
              {formatTimeLabel(m)}
            </div>
          ))}
        </div>
        <div className="cal-grid__days">
          {days.map((d, idx) => {
            const evs = eventsByDay[idx]
            return (
              <div className={`cal-grid__col${d === todayISO ? ' cal-grid__col--today' : ''}`} key={d}>
                {/* slots background */}
                {timeLabels.map((m) => (
                  <div className="cal-grid__slot" key={m + d} />
                ))}
                {/* events */}
                {evs.map(e => {
                  const top = ((toMinutes(e.startTime) - startMins) / (endMins - startMins)) * 100
                  const height = ((toMinutes(e.endTime) - toMinutes(e.startTime)) / (endMins - startMins)) * 100
                  return (
                    <div
                      key={e.eventId}
                      className="cal-grid__event"
                      style={{ top: `${top}%`, height: `${height}%` }}
                      onClick={() => onEdit?.(e)}
                      title={`${e.title} @ ${e.location}\n${e.startTime} - ${e.endTime}`}
                    >
                      <div className="cal-grid__event-title">{e.title}</div>
                      <div className="cal-grid__event-meta">{e.startTime} - {e.endTime}</div>
                      <button className="cal-grid__event-edit" aria-label="Edit event" onClick={(ev) => { ev.stopPropagation(); onEdit?.(e) }}>
                        <img src="/images/edit.png" alt="Edit" />
                      </button>
                    </div>
                  )
                })}
              </div>
            )
          })}
        </div>
      </div>
    </div>
  )
}
