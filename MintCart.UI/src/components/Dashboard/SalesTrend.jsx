import { useState } from "react";

export default function SalesTrend({ trendData }) {
  const points = trendData?.points || [];
  const [hoveredIndex, setHoveredIndex] = useState(null);
  
  // Calculate SVG path points
  const width = 400;
  const height = 100;
  const padding = 15;
  
  let pathD = "";
  let areaD = "";
  const coords = [];

  if (points.length > 0) {
    const maxVal = Math.max(...points.map(p => p.value), 1);
    const getX = (index) => (index / (points.length - 1)) * width;
    const getY = (val) => height - ((val / maxVal) * (height - padding * 2)) - padding;

    // Collect coordinates for interactive points
    points.forEach((p, i) => coords.push({ x: getX(i), y: getY(p.value) }));

    // Build the trend line path
    pathD = `M ${coords[0].x} ${coords[0].y}`;
    for (let i = 1; i < coords.length; i++) {
      const prevX = coords[i-1].x;
      const prevY = coords[i-1].y;
      const currX = coords[i].x;
      const currY = coords[i].y;
      const cpX = prevX + (currX - prevX) / 2;
      pathD += ` C ${cpX} ${prevY}, ${cpX} ${currY}, ${currX} ${currY}`;
    }

    // Build the area path for the gradient
    areaD = `${pathD} V ${height} H 0 Z`;
  }

  return (
    <div className="lg:col-span-2 bg-surface-container-lowest rounded-3xl p-8 border border-outline-variant/5 relative group/chart">
      <div className="flex justify-between items-start mb-8">
        <div>
          <h3 className="text-xl font-bold text-on-surface tracking-tight">Sales Trend</h3>
          <p className="text-sm text-slate-500">Last 5 days performance tracking</p>
        </div>
        <div className="flex gap-2">
          <div className="flex items-center gap-2 px-3 py-1 bg-primary/10 rounded-full">
            <span className="w-2 h-2 bg-primary rounded-full"></span>
            <span className="text-[10px] font-bold text-primary uppercase">Active Sales</span>
          </div>
        </div>
      </div>
      
      <div className="relative h-64 w-full mb-4">
        {/* Tooltip */}
        {hoveredIndex !== null && coords[hoveredIndex] && (
          <div 
            className="absolute z-20 pointer-events-none bg-surface-container-highest border border-outline-variant/20 px-3 py-2 -translate-x-1/2 -translate-y-[calc(100%+12px)] shadow-xl animate-in fade-in zoom-in duration-150"
            style={{ 
              left: `${(coords[hoveredIndex].x / width) * 100}%`, 
              top: `${(coords[hoveredIndex].y / height) * 100}%` 
            }}
          >
            <p className="text-[10px] font-bold text-primary uppercase tracking-widest leading-none mb-1">
              {points[hoveredIndex].label}
            </p>
            <p className="text-sm font-bold text-on-surface leading-none">
              ₹{Number(points[hoveredIndex].value).toLocaleString('en-IN')}
            </p>
            <div className="absolute w-2 h-2 bg-surface-container-highest border-r border-b border-outline-variant/20 rotate-45 left-1/2 -translate-x-1/2 -bottom-1"></div>
          </div>
        )}

        <svg className="w-full h-full" preserveAspectRatio="none" viewBox={`0 0 ${width} ${height}`}>
          <defs>
            <linearGradient id="sales-gradient" x1="0%" x2="0%" y1="0%" y2="100%">
              <stop offset="0%" stopColor="#006a61" stopOpacity="0.2"></stop>
              <stop offset="100%" stopColor="#006a61" stopOpacity="0"></stop>
            </linearGradient>
          </defs>
          {points.length > 0 && (
            <>
              <path d={areaD} fill="url(#sales-gradient)"></path>
              <path className="trend-line" d={pathD} fill="none" stroke="#006a61" strokeLinecap="round" strokeWidth="1.5"></path>
              
              {/* Interaction Points */}
              {coords.map((c, i) => (
                <g key={i} onMouseEnter={() => setHoveredIndex(i)} onMouseLeave={() => setHoveredIndex(null)}>
                  {/* Invisible hit area */}
                  <rect x={c.x - 20} y={0} width={40} height={height} fill="transparent" className="cursor-pointer" />
                  
                  {/* Data point dot */}
                  <circle 
                    cx={c.x} cy={c.y} r={hoveredIndex === i ? 4 : 2} 
                    fill={hoveredIndex === i ? "#006a61" : "white"} 
                    stroke="#006a61" strokeWidth={hoveredIndex === i ? 2 : 1.5}
                    className="transition-all duration-200"
                  />

                  {/* Vertical guide line on hover */}
                  {hoveredIndex === i && (
                    <line x1={c.x} y1={c.y + 6} x2={c.x} y2={height} stroke="#006a61" strokeWidth="1" strokeDasharray="2 2" />
                  )}
                </g>
              ))}
            </>
          )}
        </svg>

        <div className="absolute inset-0 flex justify-between items-end text-[10px] text-slate-400 font-bold uppercase tracking-tighter pt-4 pointer-events-none">
          {points.map((p, i) => (
            <span key={i} className={hoveredIndex === i ? "text-primary transition-colors" : "transition-colors"}>
              {p.label}
            </span>
          ))}
          {!points.length && (
            <>
              <span>-</span><span>-</span><span>-</span><span>-</span><span>-</span>
            </>
          )}
        </div>
      </div>
    </div>
  );
}


