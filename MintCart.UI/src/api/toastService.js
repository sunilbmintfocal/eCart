let onToast = null;

/**
 * Global service to trigger toasts from outside React components (e.g., API client).
 */
export const triggerToast = (message, type = 'success') => {
  onToast?.({ message, type });
};

export const subscribeToast = (callback) => {
  onToast = callback;
};
