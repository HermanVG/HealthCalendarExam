import { createContext, useContext } from 'react'

export type ToastContextType = {
  showSuccess: (msg: string) => void
  showError: (msg: string) => void
  showInfo: (msg: string) => void
}

export const ToastContext = createContext<ToastContextType | null>(null)

export function useToast() {
  const ctx = useContext(ToastContext)
  if (!ctx) throw new Error('useToast must be used within a ToastProvider')
  return ctx
}
