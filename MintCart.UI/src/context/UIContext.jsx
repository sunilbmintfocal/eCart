import { createContext, useContext, useState, useLayoutEffect } from 'react';

const UIContext = createContext();

export function UIProvider({ children }) {
  const [rootFontSize, setRootFontSize] = useState(15);

  useLayoutEffect(() => {
    document.documentElement.style.setProperty('--root-font-size', `${rootFontSize}px`);
  }, [rootFontSize]);

  return (
    <UIContext.Provider value={{ rootFontSize, setRootFontSize }}>
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
