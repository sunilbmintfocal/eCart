let loadingCount = 0;
let onLoadingChange = null;

/**
 * Global service to track active API requests.
 */
export const startLoading = () => {
  loadingCount++;
  if (loadingCount === 1) onLoadingChange?.(true);
};

export const stopLoading = () => {
  loadingCount = Math.max(0, loadingCount - 1);
  if (loadingCount === 0) onLoadingChange?.(false);
};

export const subscribeLoading = (callback) => {
  onLoadingChange = callback;
};
