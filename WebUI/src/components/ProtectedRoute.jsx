import { useAuth } from "react-oidc-context";
import { Navigate, useLocation } from "react-router-dom";

export default function ProtectedRoute({ children }) {
  const auth = useAuth();
  const location = useLocation();

  if (auth.isLoading) {
    return (
      <div className="flex h-screen w-full items-center justify-center bg-surface">
        <div className="flex flex-col items-center gap-4">
          <div className="h-10 w-10 animate-spin border-4 border-primary border-t-transparent"></div>
          <p className="font-body text-[12px] font-bold uppercase tracking-widest text-on-surface-variant">Authenticating...</p>
        </div>
      </div>
    );
  }

  if (auth.error) {
    return (
      <div className="flex h-screen w-full items-center justify-center bg-surface">
        <div className="max-w-md p-8 bg-surface-container border border-error/20 text-center">
          <span className="material-symbols-outlined text-error text-4xl mb-4">error</span>
          <h2 className="text-xl font-headline font-bold text-on-surface mb-2">Authentication Error</h2>
          <p className="text-sm font-body text-on-surface-variant mb-6">{auth.error.message}</p>
          <button 
            onClick={() => auth.signinRedirect()}
            className="px-6 py-2 bg-primary text-on-primary font-bold text-xs uppercase tracking-widest"
          >
            Try Again
          </button>
        </div>
      </div>
    );
  }

  if (!auth.isAuthenticated) {
    auth.signinRedirect();
    return null;
  }

  return children;
}
