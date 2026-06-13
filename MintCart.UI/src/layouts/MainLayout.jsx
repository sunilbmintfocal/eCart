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
        <div
          className={`fixed top-24 left-1/2 -translate-x-1/2 z-[150] p-4 min-w-[340px] rounded-none shadow-lg backdrop-blur-md animate-in slide-in-from-top-12 duration-500 ${
            toast.type === 'error'
              ? 'bg-error-container/25 text-error'
              : toast.type === 'warning'
                ? 'bg-warning-container/25 text-on-warning-container'
                : 'bg-primary-container/20 text-primary'
          }`}
        >
          <div className="flex items-center gap-4">
            <div className={`w-10 h-10 rounded-full flex items-center justify-center ${
              toast.type === 'error' ? 'bg-error/10' : 'bg-primary/10'
            }`}>
              <span className={`material-symbols-outlined text-[20px] ${
                toast.type === 'error' ? 'text-error' : 'text-primary'
              }`}>
                {toast.type === 'error' ? 'report' : toast.type === 'warning' ? 'warning' : 'check_circle'}
              </span>
            </div>
            <div className="flex-1">
              <p className="text-sm font-bold tracking-tight">{toast.message}</p>
            </div>
            <button onClick={() => setToast(null)} className="opacity-40 hover:opacity-100 transition-opacity p-1">
              <span className="material-symbols-outlined text-sm">close</span>
            </button>
          </div>
          {/* Progress timer bar */}
          <div className={`absolute bottom-0 left-0 h-1 w-full origin-left animate-toast-progress ${
            toast.type === 'error' ? 'bg-error/30' : 'bg-primary/30'
          }`}></div>
        </div>
      )}
    </div>
  );
}
