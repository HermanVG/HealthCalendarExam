import { createContext, useCallback, useContext, useMemo, useRef, useState } from 'react'
import '../styles/Toast.css'

type Toast = { id: number; type: 'success' | 'error' | 'info'; message: string }

type ToastContextType = {
  showSuccess: (msg: string) => void
  showError: (msg: string) => void
  showInfo: (msg: string) => void
}

const ToastContext = createContext<ToastContextType | null>(null)

export function ToastProvider({ children }: { children: React.ReactNode }) {
  const [toasts, setToasts] = useState<Toast[]>([])
  const idRef = useRef(1)

  const remove = useCallback((id: number) => {
    setToasts(ts => ts.filter(t => t.id !== id))
  }, [])

  const push = useCallback((type: Toast['type'], message: string) => {
    const id = idRef.current++
    setToasts(ts => [...ts, { id, type, message }])
    // auto-dismiss
    window.setTimeout(() => remove(id), 2500)
  }, [remove])

  const ctx = useMemo<ToastContextType>(() => ({
    showSuccess: (m) => push('success', m),
    showError: (m) => push('error', m),
    showInfo: (m) => push('info', m),
  }), [push])

  return (
    <ToastContext.Provider value={ctx}>
      {children}
      <div className="toast-region" role="status" aria-live="polite" aria-atomic="true">
        {toasts.map(t => (
          <div key={t.id} className={`toast toast--${t.type}`}>
            {t.message}
            <button className="toast__close" onClick={() => remove(t.id)} aria-label="Dismiss">×</button>
          </div>
        ))}
      </div>
    </ToastContext.Provider>
  )
}

export function useToast() {
  const ctx = useContext(ToastContext)
  if (!ctx) throw new Error('useToast must be used within a ToastProvider')
  return ctx
}
