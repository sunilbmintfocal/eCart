export default function SalesTrend() {
  return (
    <div className="lg:col-span-2 bg-surface-container-lowest rounded-3xl p-8 border border-outline-variant/5">
      <div className="flex justify-between items-start mb-8">
        <div>
          <h3 className="text-xl font-bold text-on-surface tracking-tight">Sales Trend</h3>
          <p className="text-sm text-slate-500">Weekly sales performance tracking</p>
        </div>
        <div className="flex gap-2">
          <div className="flex items-center gap-2 px-3 py-1 bg-primary/10 rounded-full">
            <span className="w-2 h-2 bg-primary rounded-full"></span>
            <span className="text-[10px] font-bold text-primary uppercase">Active Sales</span>
          </div>
        </div>
      </div>
      <div className="relative h-64 w-full mb-4">
        <svg className="w-full h-full" preserveAspectRatio="none" viewBox="0 0 400 100">
          <defs>
            <linearGradient id="sales-gradient" x1="0%" x2="0%" y1="0%" y2="100%">
              <stop offset="0%" stopColor="#006a61" stopOpacity="0.2"></stop>
              <stop offset="100%" stopColor="#006a61" stopOpacity="0"></stop>
            </linearGradient>
          </defs>
          <path d="M 0 90 Q 50 60 100 70 T 200 30 T 300 40 T 400 10 V 100 H 0 Z" fill="url(#sales-gradient)"></path>
          <path className="trend-line" d="M 0 90 Q 50 60 100 70 T 200 30 T 300 40 T 400 10" fill="none" stroke="#006a61" strokeLinecap="round" strokeWidth="1.5"></path>
        </svg>
        <div className="absolute inset-0 flex justify-between items-end text-[10px] text-slate-400 font-bold uppercase tracking-tighter pt-4">
          <span>Mon</span>
          <span>Tue</span>
          <span>Wed</span>
          <span>Thu</span>
          <span>Fri</span>
          <span>Sat</span>
          <span>Sun</span>
        </div>
      </div>
    </div>
  );
}
