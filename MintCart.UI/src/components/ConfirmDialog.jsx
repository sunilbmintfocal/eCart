/**
 * Reusable confirmation dialog for destructive or important actions.
 * Replaces native window.confirm() with a styled modal matching the app's design system.
 *
 * @param {boolean}  open          - Whether the dialog is visible.
 * @param {string}   title         - Heading text.
 * @param {ReactNode}message       - Body text/description (supports JSX for inline emphasis).
 * @param {string}   note          - Short emphasized callout shown below the message (e.g. "This action cannot be undone").
 * @param {string}   confirmLabel  - Text for the confirm button.
 * @param {string}   cancelLabel   - Text for the cancel button.
 * @param {string}   confirmIcon   - Material symbol name shown on the confirm button.
 * @param {'danger'|'warning'|'info'} variant - Controls icon/accent styling.
 * @param {boolean}  loading       - Shows a spinner and disables buttons while an action is in progress.
 * @param {Function} onConfirm     - Called when the confirm button is clicked.
 * @param {Function} onCancel      - Called when the cancel button or backdrop is clicked.
 */
const VARIANTS = {
  danger: {
    iconBg: 'bg-error-container/20',
    iconColor: 'text-error',
    icon: 'warning',
    confirmClass: 'bg-error text-white shadow-error/20',
  },
  warning: {
    iconBg: 'bg-error-container/20',
    iconColor: 'text-error',
    icon: 'warning',
    confirmClass: 'btn-gradient',
  },
  info: {
    iconBg: 'bg-primary/10',
    iconColor: 'text-primary',
    icon: 'info',
    confirmClass: 'btn-gradient',
  },
};

export default function ConfirmDialog({
  open = false,
  title = 'Are you sure?',
  message = '',
  note = '',
  confirmLabel = 'Confirm',
  cancelLabel = 'Cancel',
  confirmIcon = null,
  variant = 'danger',
  loading = false,
  onConfirm,
  onCancel,
}) {
  if (!open) return null;

  const v = VARIANTS[variant] || VARIANTS.danger;

  return (
    <div className="fixed inset-0 z-[80] flex items-center justify-center p-4">
      <div className="absolute inset-0 bg-surface-container-highest/60 backdrop-blur-sm" onClick={onCancel}></div>
      <div className="bg-surface relative z-10 w-full max-w-lg rounded-none ambient-shadow border border-outline-variant/10 overflow-hidden animate-in fade-in zoom-in duration-200">
        <div className="px-10 py-8 flex flex-col items-center text-center gap-4">
          <div className={`w-14 h-14 flex items-center justify-center ${v.iconBg} ${v.iconColor} rounded-full shrink-0`}>
            <span className="material-symbols-outlined text-3xl">{v.icon}</span>
          </div>
          <div>
            <h3 className="text-lg font-headline font-bold text-on-surface tracking-tight">{title}</h3>
            {message && (
              <p className="text-[13px] font-medium text-on-surface-variant mt-2 leading-relaxed">
                {message}
              </p>
            )}
            {note && (
              <p className="text-[11px] font-bold text-error uppercase tracking-widest mt-3">{note}</p>
            )}
          </div>
        </div>
        <div className="px-8 py-5 border-t border-outline-variant/10 bg-surface-container-low/30 flex gap-3">
          <button
            onClick={onCancel}
            disabled={loading}
            className="flex-1 px-6 py-3 border border-outline-variant/20 text-on-surface rounded-none font-bold text-xs uppercase tracking-widest hover:bg-surface-container-low transition-all disabled:opacity-50"
          >
            {cancelLabel}
          </button>
          <button
            onClick={onConfirm}
            disabled={loading}
            className={`flex-1 px-6 py-3 rounded-none font-bold text-xs uppercase tracking-widest shadow-lg hover:opacity-90 transition-all flex items-center justify-center gap-2 disabled:opacity-50 ${v.confirmClass}`}
          >
            {loading ? (
              <>
                <span className="material-symbols-outlined text-base animate-spin">sync</span>
                Processing...
              </>
            ) : (
              <>
                {confirmIcon && <span className="material-symbols-outlined text-base">{confirmIcon}</span>}
                {confirmLabel}
              </>
            )}
          </button>
        </div>
      </div>
    </div>
  );
}
