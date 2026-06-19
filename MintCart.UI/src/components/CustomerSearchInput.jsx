import { useState, useMemo, useEffect, useRef } from 'react';

function HighlightMatch({ text, query }) {
  if (!query || !text) return <>{text || ''}</>;
  const lowerText = text.toLowerCase();
  const lowerQuery = query.toLowerCase();
  const idx = lowerText.indexOf(lowerQuery);
  if (idx === -1) return <>{text}</>;
  return (
    <>
      {text.slice(0, idx)}
      <span className="text-primary font-bold">{text.slice(idx, idx + query.length)}</span>
      {text.slice(idx + query.length)}
    </>
  );
}

export default function CustomerSearchInput({
  customers = [],
  isLoadingCache = false,
  onSearch,
  placeholder = 'Search by name or phone...',
}) {
  const [inputValue, setInputValue] = useState('');
  const [isOpen, setIsOpen] = useState(false);
  const [activeIndex, setActiveIndex] = useState(-1);
  const inputRef = useRef(null);
  const containerRef = useRef(null);

  // Instant client-side filter — no debounce, no API call
  const suggestions = useMemo(() => {
    const q = inputValue.trim();
    if (q.length < 2 || customers.length === 0) return [];
    const lower = q.toLowerCase();
    const results = [];
    for (const c of customers) {
      if (results.length >= 10) break;
      const name = (c.CustomerName || c.customerName || '').toLowerCase();
      const phone = c.PhoneNo || c.phoneNo || '';
      if (name.includes(lower) || phone.includes(lower)) results.push(c);
    }
    return results;
  }, [inputValue, customers]);

  // Open dropdown whenever input has 2+ chars (show results or "no match")
  useEffect(() => {
    if (inputValue.trim().length >= 2) {
      setIsOpen(true);
      setActiveIndex(-1);
    } else {
      setIsOpen(false);
    }
  }, [inputValue]);

  // Close on outside click
  useEffect(() => {
    const handleClickOutside = (e) => {
      if (containerRef.current && !containerRef.current.contains(e.target)) {
        setIsOpen(false);
      }
    };
    document.addEventListener('mousedown', handleClickOutside);
    return () => document.removeEventListener('mousedown', handleClickOutside);
  }, []);

  const applySearch = (term) => {
    setIsOpen(false);
    onSearch(term);
  };

  const handleSelect = (suggestion) => {
    const name = suggestion.CustomerName || suggestion.customerName || '';
    setInputValue(name);
    applySearch(name);
  };

  const handleClear = () => {
    setInputValue('');
    setIsOpen(false);
    onSearch('');
    inputRef.current?.focus();
  };

  const handleKeyDown = (e) => {
    if (e.key === 'ArrowDown') {
      e.preventDefault();
      if (!isOpen || suggestions.length === 0) return;
      setActiveIndex(i => Math.min(i + 1, suggestions.length - 1));
    } else if (e.key === 'ArrowUp') {
      e.preventDefault();
      if (!isOpen) return;
      setActiveIndex(i => Math.max(i - 1, -1));
    } else if (e.key === 'Enter') {
      e.preventDefault();
      if (isOpen && activeIndex >= 0 && suggestions[activeIndex]) {
        handleSelect(suggestions[activeIndex]);
      } else {
        applySearch(inputValue.trim());
      }
    } else if (e.key === 'Escape') {
      setIsOpen(false);
      setActiveIndex(-1);
    }
  };

  return (
    <div ref={containerRef} className="flex items-center group">
      {/* Input + dropdown anchored together */}
      <div className="relative">
        {/* Icon — spinner while cache loads, search otherwise */}
        <span
          className={`material-symbols-outlined absolute left-3 top-1/2 -translate-y-1/2 text-base z-10 pointer-events-none transition-colors ${
            isLoadingCache
              ? 'text-primary animate-spin'
              : 'text-on-surface-variant group-focus-within:text-primary'
          }`}
        >
          {isLoadingCache ? 'sync' : 'search'}
        </span>

        <input
          ref={inputRef}
          type="text"
          placeholder={isLoadingCache ? 'Loading customers...' : placeholder}
          value={inputValue}
          onChange={(e) => setInputValue(e.target.value)}
          onKeyDown={handleKeyDown}
          onFocus={() => { if (inputValue.trim().length >= 2) setIsOpen(true); }}
          className={`pl-10 pr-8 py-2 bg-surface-container-lowest border text-[13px] font-medium text-on-surface transition-all outline-none min-w-[320px] h-11 rounded-none ${
            isOpen
              ? 'border-primary ring-1 ring-primary/10'
              : 'border-outline-variant/20 focus:border-primary focus:ring-1 focus:ring-primary/10'
          }`}
        />

        {inputValue && (
          <button
            type="button"
            onClick={handleClear}
            tabIndex={-1}
            className="absolute right-3 top-1/2 -translate-y-1/2 text-on-surface-variant hover:text-on-surface transition-colors z-10"
          >
            <span className="material-symbols-outlined text-[18px]">close</span>
          </button>
        )}

      {isOpen && (
        <div className="absolute top-full left-0 right-0 mt-px bg-surface border border-outline-variant/20 shadow-lg shadow-surface-container-highest/20 z-50 overflow-hidden animate-in fade-in slide-in-from-top-1 duration-100">
          {suggestions.length > 0 ? (
            <ul className="max-h-64 overflow-y-auto custom-scrollbar">
              {suggestions.map((s, i) => {
                const name = s.CustomerName || s.customerName || '';
                const phone = s.PhoneNo || s.phoneNo || '';
                return (
                  <li key={s.Id || s.id} className="border-b border-outline-variant/10 last:border-0">
                    <button
                      type="button"
                      onMouseDown={(e) => { e.preventDefault(); handleSelect(s); }}
                      onMouseEnter={() => setActiveIndex(i)}
                      className={`w-full flex items-center gap-3 px-4 py-2.5 text-left transition-colors ${
                        i === activeIndex ? 'bg-primary/10' : 'hover:bg-surface-container-low'
                      }`}
                    >
                      <span className="material-symbols-outlined text-[16px] text-on-surface-variant shrink-0">person</span>
                      <div className="flex-1 min-w-0">
                        <div className="text-[13px] font-semibold text-on-surface truncate leading-tight">
                          <HighlightMatch text={name} query={inputValue} />
                        </div>
                        <div className="text-[11px] text-on-surface-variant mt-0.5">
                          <HighlightMatch text={phone} query={inputValue} />
                        </div>
                      </div>
                      <span className="material-symbols-outlined text-[13px] text-on-surface-variant/30 shrink-0">north_west</span>
                    </button>
                  </li>
                );
              })}
            </ul>
          ) : (
            <div className="px-4 py-3 flex items-center gap-2 text-on-surface-variant">
              <span className="material-symbols-outlined text-[16px]">search_off</span>
              <span className="text-[12px] font-medium">No customers found for &ldquo;{inputValue}&rdquo;</span>
            </div>
          )}
        </div>
      )}
      </div>

      <button
        type="button"
        onClick={() => applySearch(inputValue.trim())}
        className="ml-2 flex items-center gap-2 px-6 h-11 border border-outline-variant/20 text-on-surface-variant hover:text-primary hover:bg-surface-container-low transition-all rounded-none font-bold text-xs uppercase tracking-widest whitespace-nowrap shrink-0"
      >
        <span className="material-symbols-outlined text-base">search</span>
        Search
      </button>
    </div>
  );
}
