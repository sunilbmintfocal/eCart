import { useEffect } from 'react';
import { useUI } from '../context/UIContext';
import { useAuth } from 'react-oidc-context';

export default function Header() {
  const { rootFontSize, setRootFontSize } = useUI();
  const auth = useAuth();

  const handleScale = (delta) => {
    setRootFontSize((prev) => Math.min(Math.max(prev + delta, 10), 22));
  };

  const handleAuth = () => {
    if (auth.isAuthenticated) {
      auth.removeUser();
    } else {
      auth.signinRedirect();
    }
  };

  useEffect(() => {
    if (auth.isAuthenticated && auth.user?.profile) {
      console.log('!!! HEADER AUTH DEBUG !!!', {
        profile: auth.user.profile,
        profileKeys: Object.keys(auth.user.profile),
        isAuthenticated: auth.isAuthenticated
      });
    }
  }, [auth.isAuthenticated, auth.user]);

  // Helper to find the name claim by scanning both ID Token profile and Access Token claims
  const getDisplayName = () => {
    if (!auth.user) return 'User';
    
    // 1. Try standard profile from ID Token
    const p = auth.user.profile;
    if (p) {
      const name = p.name || p.display_name || p.given_name || p.nickname || p.preferred_username;
      if (name) return name;
    }

    // 2. Secondary Scan: Extract from Access Token if ID token is missing the claim
    try {
      if (auth.user.access_token) {
        const base64Url = auth.user.access_token.split('.')[1];
        const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
        const payload = JSON.parse(window.atob(base64));
        
        const tokenName = payload.name || payload.display_name || payload.given_name || payload.unique_name;
        if (tokenName) return tokenName;
        
        if (payload.email) return payload.email.split('@')[0];
      }
    } catch (e) {
      console.error('[Auth] Failed to decode access token for name', e);
    }

    // 3. Last resort fallback
    return p?.email?.split('@')[0] || 'User';
  };

  return (
    <header className="w-full sticky top-0 z-40 glass bg-surface/80 ambient-shadow flex justify-between items-center px-8 py-4">
      <div className="flex items-center gap-10">
        <div className="hidden md:flex relative group">
          <span className="material-symbols-outlined absolute left-3 top-1/2 -translate-y-1/2 text-on-surface-variant/50 text-base group-focus-within:text-primary transition-colors">search</span>
          <input
            type="text"
            placeholder="Search everything..."
            className="pl-10 pr-4 py-2 bg-surface-container-low/50 border border-outline-variant/20 rounded-none text-[13px] font-medium text-on-surface focus:border-primary focus:ring-1 focus:ring-primary/10 transition-all outline-none min-w-[320px] h-11"
          />
        </div>
      </div>
      <div className="flex items-center gap-6">
        <div className="flex items-center bg-surface-container-low/40 rounded-none px-2 py-1 border border-outline-variant/10">
          <button
            onClick={() => handleScale(-1)}
            className="w-7 h-7 flex items-center justify-center rounded-none hover:bg-surface-container-lowest text-on-surface-variant hover:text-primary transition-all active:scale-95"
            title="Decrease scaling"
          >
            <span className="material-symbols-outlined text-[18px]">remove</span>
          </button>
          <div className="px-2 min-w-[32px] text-center">
            <span className="text-[10px] font-bold text-on-surface-variant tracking-tighter">{rootFontSize}</span>
          </div>
          <button
            onClick={() => handleScale(1)}
            className="w-7 h-7 flex items-center justify-center rounded-none hover:bg-surface-container-lowest text-on-surface-variant hover:text-primary transition-all active:scale-95"
            title="Increase scaling"
          >
            <span className="material-symbols-outlined text-[18px]">add</span>
          </button>
        </div>

        {auth.isAuthenticated ? (
          <>
            <div className="flex items-center gap-2">
              <button className="p-2 rounded-none hover:bg-surface-container-low transition-all text-on-surface-variant">
                <span className="material-symbols-outlined">notifications</span>
              </button>
              <button className="p-2 rounded-none hover:bg-surface-container-low transition-all text-on-surface-variant">
                <span className="material-symbols-outlined">settings</span>
              </button>
            </div>
            <div className="flex items-center gap-3 pl-4 border-l border-outline-variant/15">
              <div className="text-right hidden sm:block">
                <p className="text-xs font-bold text-on-surface tracking-tight uppercase leading-none">
                  {getDisplayName()}
                </p>
                <button
                  onClick={() => auth.signoutRedirect()}
                  className="text-[10px] font-medium text-primary mt-1.5 uppercase tracking-wider hover:underline"
                >
                  Sign Out
                </button>
              </div>
              <div className="h-9 w-9 rounded-none overflow-hidden bg-primary-container/10 border border-outline-variant/10">
                <img alt="User profile" src={auth.user?.profile?.picture || "https://lh3.googleusercontent.com/aida-public/AB6AXuBK2xTR1jss0lVyUlTE9LcpUzfe9fexevHdO-9N1OO3yuPVMiyh_HNsbjMuqyX4jSNYJkwy2NJavEjr7PvU0TRE0XTGCo_BMZAXwgBHqFeCrQoAE9_s0oq55GE6aVWFMLUSpIHo9fFEPOlFULmvSDjH3k2PpEazhZTJQewIzD-WmWYBOlABPKN7XdSNzUmUEZWUPYjikWo4SZu6NVChCsYfigWIpODmogJ0PzSmPtCU7--WUjWUwHlmnn59FyqlQHE2UTUW3n5o_Q"} />
              </div>
            </div>
          </>
        ) : (
          <button
            onClick={() => auth.signinRedirect()}
            className="px-6 py-2.5 bg-primary text-on-primary text-[12px] font-bold uppercase tracking-widest hover:bg-primary/90 transition-all flex items-center gap-2"
          >
            <span className="material-symbols-outlined text-[18px]">login</span>
            Sign In
          </button>
        )}
      </div>
    </header>
  );
}
