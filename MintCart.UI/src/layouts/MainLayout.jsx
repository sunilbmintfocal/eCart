import { useState } from 'react';
import { Outlet } from 'react-router-dom';
import Sidebar from './Sidebar';
import Header from './Header';
import { useUI } from '../context/UIContext';

export default function MainLayout() {
  const [isCollapsed, setIsCollapsed] = useState(false);
  const { toast, setToast } = useUI();

  return (
    <div className="flex min-h-screen bg-surface font-body overflow-hidden">
      <Sidebar isCollapsed={isCollapsed} setIsCollapsed={setIsCollapsed} />
      
      <div className={`flex-1 flex flex-col min-h-screen transition-all duration-300 ${isCollapsed ? 'ml-20' : 'ml-64'} overflow-hidden`}>
        <Header />
        
        <main className="flex-1 overflow-auto bg-surface-container-lowest/30">
          <Outlet />
        </main>
      </div>

      {/* Global Toast Notification */}
      {toast && (
        <div className="fixed top-24 left-1/2 -translate-x-1/2 z-[150] bg-[#F0FDF4] border border-primary/15 p-4 min-w-[340px] shadow-[0_8px_30px_rgb(16,185,129,0.1)] backdrop-blur-sm animate-in slide-in-from-top-12 duration-500">
          <div className="flex items-center gap-4">
            <div className="w-10 h-10 rounded-none bg-primary/10 flex items-center justify-center">
              <span className="material-symbols-outlined text-primary text-[20px]">check_circle</span>
            </div>
            <div>
              <p className="text-sm font-bold text-[#064E3B] tracking-tight">{toast}</p>
            </div>
            <button onClick={() => setToast(null)} className="ml-auto text-primary/40 hover:text-primary transition-colors p-1">
              <span className="material-symbols-outlined text-sm">close</span>
            </button>
          </div>
          {/* Progress timer bar - 10s */}
          <div className="absolute bottom-0 left-0 h-0.5 bg-primary/20 w-full animate-out fade-out duration-[10000ms] origin-left scale-x-0 transition-transform"></div>
        </div>
      )}
    </div>
  );
}
