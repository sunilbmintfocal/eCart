import { useUI } from '../context/UIContext';

export default function Header() {
  const { rootFontSize, setRootFontSize } = useUI();

  const handleScale = (delta) => {
    setRootFontSize((prev) => Math.min(Math.max(prev + delta, 10), 22));
  };

  return (
<header className="w-full sticky top-0 z-40 glass bg-surface/80 ambient-shadow flex justify-between items-center px-8 py-4">
      <div className="flex items-center gap-10">
        <div className="text-xl font-bold text-on-surface font-headline tracking-tighter">eCart</div>
        <div className="hidden md:flex items-center bg-surface-container-low/50 border border-outline-variant/20 rounded-none transition-all duration-300 focus-within:border-primary focus-within:bg-surface focus-within:shadow-md w-96 h-12 group">
          <span className="material-symbols-outlined text-on-surface-variant/50 text-[18px] pl-5 transition-colors duration-300 group-focus-within:text-primary" style={{ fontVariationSettings: "'wght' 700" }}>search</span>
          <input 
            className="flex-1 bg-transparent border-none focus:ring-0 text-[15px] font-body font-bold text-on-surface placeholder:text-on-surface-variant/40 h-full w-full px-4 leading-none" 
            placeholder="Search inventory..." 
            type="text" 
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
            <p className="text-xs font-bold text-on-surface tracking-tight uppercase leading-none">Alexander V.</p>
            <p className="text-[10px] font-medium text-on-surface-variant mt-1.5 uppercase tracking-wider">Chief Curator</p>
          </div>
          <div className="h-9 w-9 rounded-none overflow-hidden bg-primary-container/10 border border-outline-variant/10">
            <img alt="User profile" src="https://lh3.googleusercontent.com/aida-public/AB6AXuBK2xTR1jss0lVyUlTE9LcpUzfe9fexevHdO-9N1OO3yuPVMiyh_HNsbjMuqyX4jSNYJkwy2NJavEjr7PvU0TRE0XTGCo_BMZAXwgBHqFeCrQoAE9_s0oq55GE6aVWFMLUSpIHo9fFEPOlFULmvSDjH3k2PpEazhZTJQewIzD-WmWYBOlABPKN7XdSNzUmUEZWUPYjikWo4SZu6NVChCsYfigWIpODmogJ0PzSmPtCU7--WUjWUwHlmnn59FyqlQHE2UTUW3n5o_Q" />
          </div>
        </div>
      </div>
    </header>
  );
}
