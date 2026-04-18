import { createContext, useContext, useState, useLayoutEffect, useEffect } from 'react';
import { subscribeLoading } from '../api/loadingService';
import { subscribeToast } from '../api/toastService';

const UIContext = createContext();

export function UIProvider({ children }) {
  const [rootFontSize, setRootFontSize] = useState(15);
  const [globalLoading, setGlobalLoading] = useState(false);
  const [toast, setToast] = useState(null); // { message, type }

  useLayoutEffect(() => {
    document.documentElement.style.setProperty('--root-font-size', `${rootFontSize}px`);
  }, [rootFontSize]);

  useEffect(() => {
    subscribeLoading(setGlobalLoading);
    subscribeToast((t) => showToast(t.message, t.type));
  }, []);

  const showToast = (message, type = 'success') => {
    setToast({ message, type });
    setTimeout(() => setToast(null), 6000);
  };

  return (
    <UIContext.Provider value={{ rootFontSize, setRootFontSize, globalLoading, toast, setToast, showToast }}>
      {/* Global API Loading Indicator */}
      {globalLoading && (
        <div className="fixed top-0 left-0 right-0 z-[100] h-1.5 bg-primary/20 overflow-hidden animate-in fade-in duration-300">
          <div className="h-full bg-primary animate-progress origin-left"></div>
          <div className="absolute top-1.5 left-1/2 -translate-x-1/2 px-4 py-1.5 bg-primary text-on-primary text-[10px] font-bold uppercase tracking-[0.2em] shadow-lg animate-in slide-in-from-top-full duration-300 pointer-events-none">
            Syncing Data...
          </div>
        </div>
      )}
      {children}
    </UIContext.Provider>
  );
}

export function useUI() {
  const context = useContext(UIContext);
  if (!context) {
    throw new Error('useUI must be used within a UIProvider');
  }
  return context;
}
