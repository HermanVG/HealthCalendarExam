// Modal component for confirming when a removed availability will delete an event
import '../styles/EventFormsBase.css';

interface ConfirmationModalProps {
    isOpen: boolean;
    title: string;
    message: string;
    onConfirm: () => void;
    onCancel: () => void;
}

export default function ConfirmationModal({
    isOpen,
    title,
    message,
    onConfirm,
    onCancel
}: ConfirmationModalProps) {
    if (!isOpen) return null;

    return (
        <div className="overlay" role="dialog" aria-modal="true" aria-labelledby="confirm-modal-title" aria-describedby="confirm-modal-desc">
            <div className="modal confirm-modal">
                <header className="modal__header">
                    <h2 id="confirm-modal-title">{title}</h2>
                    <button className="icon-btn" onClick={onCancel} aria-label="Close confirmation">
                        <img src="/images/exit.png" alt="Close" />
                    </button>
                </header>
                <div id="confirm-modal-desc" className="confirm-body">
                    {message}
                </div>
                <div className="confirm-actions" style={{ display: 'flex', justifyContent: 'flex-end', gap: '1rem', marginTop: '1.5rem' }}>
                    <button type="button" className="btn" onClick={onCancel}>Cancel</button>
                    <button
                        type="button"
                        className="btn btn--primary"
                        onClick={onConfirm}
                    >
                        Confirm
                    </button>
                </div>
            </div>
        </div>
    );
}
