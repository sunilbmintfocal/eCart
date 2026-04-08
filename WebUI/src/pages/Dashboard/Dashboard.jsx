export default function Dashboard() {
  return (
    <section className="p-8 bg-surface min-h-screen">
      <div className="flex justify-between items-end mb-10">
        <div>
          <span className="text-primary text-[10px] font-medium uppercase tracking-[0.2em] mb-1.5 block">Inventory Performance</span>
          <h1 className="text-4xl font-headline font-bold text-on-surface tracking-tight">Dashboard Overview</h1>
        </div>
        <div className="flex gap-3">
          <button className="flex items-center gap-2 px-6 py-2.5 border border-outline-variant/20 text-on-surface rounded-none font-headline text-sm font-bold hover:bg-surface-container-low transition-all">
            <span className="material-symbols-outlined text-lg">download</span>
            Export Data
          </button>
        </div>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 mb-8">
        <div className="bg-surface-container-lowest p-6 rounded-none ambient-shadow border border-outline-variant/10 group hover:translate-y-[-2px] transition-all duration-300">
          <div className="flex justify-between items-start mb-4">
            <div className="p-2.5 bg-primary-container/10 rounded-none text-primary">
              <span className="material-symbols-outlined">warehouse</span>
            </div>
            <span className="text-primary text-[10px] font-bold bg-primary-container/20 px-2.5 py-1 rounded-none uppercase tracking-tighter">+4.2%</span>
          </div>
          <p className="text-on-surface-variant text-[10px] font-medium uppercase tracking-widest mb-1">Total Stock</p>
          <h3 className="text-2xl font-headline font-bold text-on-surface tracking-tight">12,482</h3>
          <p className="text-[10px] text-on-surface-variant/60 font-medium mt-2">Active SKUs across 4 zones</p>
        </div>

        <div className="bg-surface-container-lowest p-6 rounded-none ambient-shadow border border-outline-variant/10 group hover:translate-y-[-2px] transition-all duration-300">
          <div className="flex justify-between items-start mb-4">
            <div className="p-2.5 bg-error-container/10 text-error rounded-none">
              <span className="material-symbols-outlined">warning</span>
            </div>
            <span className="text-error text-[10px] font-bold bg-error-container px-2.5 py-1 rounded-none uppercase tracking-tighter">Critical</span>
          </div>
          <p className="text-on-surface-variant text-[10px] font-medium uppercase tracking-widest mb-1">Low Stock Alerts</p>
          <h3 className="text-2xl font-headline font-bold text-on-surface tracking-tight">24</h3>
          <p className="text-[10px] text-on-surface-variant/60 font-medium mt-2">12 items require immediate restock</p>
        </div>

        <div className="bg-surface-container-lowest p-6 rounded-none ambient-shadow border border-outline-variant/10 group hover:translate-y-[-2px] transition-all duration-300">
          <div className="flex justify-between items-start mb-4">
            <div className="p-2.5 bg-secondary-container/10 text-secondary rounded-none">
              <span className="material-symbols-outlined">pending_actions</span>
            </div>
          </div>
          <p className="text-on-surface-variant text-[10px] font-medium uppercase tracking-widest mb-1">Pending Orders</p>
          <h3 className="text-2xl font-headline font-bold text-on-surface tracking-tight">156</h3>
          <p className="text-[10px] text-on-surface-variant/60 font-medium mt-2">Est. fulfillment: 4.2 hours</p>
        </div>

        <div className="bg-surface-container-lowest p-6 rounded-none ambient-shadow border border-outline-variant/10 group hover:translate-y-[-2px] transition-all duration-300">
          <div className="flex justify-between items-start mb-4">
            <div className="p-2.5 bg-tertiary-container/10 text-tertiary rounded-none">
              <span className="material-symbols-outlined">payments</span>
            </div>
            <span className="text-primary text-[10px] font-bold bg-primary-container/20 px-2.5 py-1 rounded-none uppercase tracking-tighter">+12%</span>
          </div>
          <p className="text-on-surface-variant text-[10px] font-medium uppercase tracking-widest mb-1">Total Revenue</p>
          <h3 className="text-2xl font-headline font-bold text-on-surface tracking-tight">$2.4M</h3>
          <p className="text-[10px] text-on-surface-variant/60 font-medium mt-2">Quarterly target: 82% reached</p>
        </div>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-3 gap-8 mb-8">
        <div className="lg:col-span-2 bg-surface-container-lowest p-8 rounded-none ambient-shadow border border-outline-variant/5">
          <div className="flex justify-between items-center mb-8">
            <h3 className="text-xl font-headline font-bold text-on-surface tracking-tight">Inventory Value</h3>
            <div className="flex gap-2 p-1 bg-surface-container-low rounded-none">
              <button className="px-4 py-1.5 text-[10px] font-bold rounded-none transition-all text-on-surface-variant hover:text-on-surface uppercase tracking-wider">7D</button>
              <button className="px-4 py-1.5 text-[10px] font-bold rounded-none transition-all bg-surface-container-lowest text-on-surface shadow-sm uppercase tracking-wider">1M</button>
              <button className="px-4 py-1.5 text-[10px] font-bold rounded-none transition-all text-on-surface-variant hover:text-on-surface uppercase tracking-wider">1Y</button>
            </div>
          </div>
          <div className="h-64 flex items-end justify-between gap-4 relative pt-10 px-2">
            <div className="absolute inset-0 flex flex-col justify-between pointer-events-none">
              <div className="border-b border-outline-variant/30 w-full h-0"></div>
              <div className="border-b border-outline-variant/30 w-full h-0"></div>
              <div className="border-b border-outline-variant/30 w-full h-0"></div>
              <div className="border-b border-outline-variant/30 w-full h-0"></div>
            </div>
            <div className="flex-1 bg-primary/20 hover:bg-primary/40 h-[40%] rounded-none transition-all cursor-pointer"></div>
            <div className="flex-1 bg-primary/20 hover:bg-primary/40 h-[60%] rounded-none transition-all cursor-pointer"></div>
            <div className="flex-1 bg-primary/20 hover:bg-primary/40 h-[45%] rounded-none transition-all cursor-pointer"></div>
            <div className="flex-1 bg-primary/20 hover:bg-primary/40 h-[80%] rounded-none transition-all cursor-pointer"></div>
            <div className="flex-1 bg-primary/20 hover:bg-primary/40 h-[70%] rounded-none transition-all cursor-pointer"></div>
            <div className="flex-1 bg-primary/20 hover:bg-primary/40 h-[95%] rounded-none transition-all cursor-pointer"></div>
            <div className="flex-1 bg-primary/20 hover:bg-primary/40 h-[85%] rounded-none transition-all cursor-pointer"></div>
          </div>
          <div className="flex justify-between mt-6 text-[10px] font-bold text-on-surface-variant/40 uppercase tracking-[0.2em] px-1">
            <span>WK 01</span><span>WK 02</span><span>WK 03</span><span>WK 04</span>
          </div>
        </div>

        <div className="bg-surface-container-lowest p-8 rounded-none ambient-shadow border border-outline-variant/5">
          <h3 className="text-xl font-headline font-bold text-on-surface mb-8 tracking-tight">Sales Categories</h3>
          <div className="space-y-7">
            <div>
              <div className="flex justify-between mb-2.5">
                <span className="text-[10px] font-bold uppercase tracking-wider text-on-surface-variant">Electronics</span>
                <span className="text-sm font-bold text-primary">42%</span>
              </div>
              <div className="w-full h-1.5 bg-surface-container-low rounded-none">
                <div className="h-full bg-primary rounded-none w-[42%]"></div>
              </div>
            </div>
            <div>
              <div className="flex justify-between mb-2.5">
                <span className="text-[10px] font-bold uppercase tracking-wider text-on-surface-variant">Furniture</span>
                <span className="text-sm font-bold text-on-surface">28%</span>
              </div>
              <div className="w-full h-1.5 bg-surface-container-low rounded-none">
                <div className="h-full bg-secondary rounded-none w-[28%]"></div>
              </div>
            </div>
            <div>
              <div className="flex justify-between mb-2.5">
                <span className="text-[10px] font-bold uppercase tracking-wider text-on-surface-variant">Lifestyle</span>
                <span className="text-sm font-bold text-on-surface">18%</span>
              </div>
              <div className="w-full h-1.5 bg-surface-container-low rounded-none">
                <div className="h-full bg-tertiary rounded-none w-[18%]"></div>
              </div>
            </div>
          </div>
          <div className="mt-12 pt-6">
            <div className="flex items-center gap-4 p-4 border border-outline-variant/10 rounded-none">
              <div className="h-10 w-10 rounded-none bg-primary-container/10 flex items-center justify-center text-primary">
                <span className="material-symbols-outlined">trending_up</span>
              </div>
              <div>
                <p className="text-[10px] font-bold text-on-surface-variant uppercase tracking-widest">Top Performer</p>
                <p className="text-sm font-bold text-on-surface">UltraHD Displays</p>
              </div>
            </div>
          </div>
        </div>
      </div>

      <div className="bg-surface-container-lowest rounded-none ambient-shadow border border-outline-variant/10 overflow-hidden">
        <div className="px-8 py-5 flex justify-between items-center border-b border-outline-variant/5">
          <h3 className="text-xl font-headline font-bold text-on-surface tracking-tight">Recent Activity</h3>
          <button className="text-primary text-[10px] font-bold uppercase tracking-[0.15em] hover:text-primary-container transition-colors">View Catalog</button>
        </div>
        <div className="overflow-x-auto">
          <table className="w-full text-left">
            <thead className="bg-primary/20 text-primary relative z-10">
              <tr>
                <th className="px-8 py-5 text-xs font-bold uppercase tracking-[0.2em] border-r border-primary/20">Transaction ID</th>
                <th className="px-8 py-5 text-xs font-bold uppercase tracking-[0.2em] border-r border-primary/20">Product Info</th>
                <th className="px-8 py-5 text-xs font-bold uppercase tracking-[0.2em] border-r border-primary/20">Quantity</th>
                <th className="px-8 py-5 text-xs font-bold uppercase tracking-[0.2em] border-r border-primary/20">Timeline</th>
                <th className="px-8 py-5 text-xs font-bold uppercase tracking-[0.2em] text-right">Status</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-outline-variant/10 text-on-surface">
              {/* Note: Zebra striping removed for Option 1 Ledger style to prioritize vertical alignment focus */}
              <tr className="hover:bg-primary/5 transition-all duration-150 group">
                <td className="px-8 py-5 text-sm font-mono font-bold text-primary border-r border-outline-variant/10">#TR-88210</td>
                <td className="px-8 py-5 border-r border-outline-variant/10">
                  <div className="flex items-center gap-4">
                    <div className="h-10 w-10 rounded-none bg-surface-container-low overflow-hidden">
                      <img alt="Modern watch" src="https://lh3.googleusercontent.com/aida-public/AB6AXuAHiy0RjDXg4SUHMTvfxNoiUKYS2EskKhjeHrZzXHM2FA67KqV3-3d5SbSVlceRtZrWc7oWsUGBMQsjQgazHD0dYZiJs7reWYYAQoH6Ji5yG2Zje05IMLoXWPCIIGE97z0cJhc87NFFVkwm2jFC8nqR66HylGPAAbMFTZWcp02tpnTH_Yz-ed-qfZwRNLm_ZM5Jf_t3yFKdGdRlR52p7IgdBgVEL2kxJJluPjIU2elbcvfEvOO_Xg1W68KUKgyJ-q4EeN5JkTopjw" />
                    </div>
                    <div>
                      <p className="text-sm font-bold text-on-surface">Zenith Leather Watch</p>
                      <p className="text-[10px] font-medium text-primary uppercase tracking-widest">Lifestyle</p>
                    </div>
                  </div>
                </td>
                <td className="px-8 py-5 text-sm font-bold text-on-surface border-r border-outline-variant/10">12 Units</td>
                <td className="px-8 py-5 border-r border-outline-variant/10">
                  <p className="text-sm font-medium text-on-surface">Oct 24, 2023</p>
                  <p className="text-[10px] text-on-surface-variant font-medium uppercase mt-0.5">14:20 PM</p>
                </td>
                <td className="px-8 py-5 text-right">
                  <span className="inline-block px-3 py-1 rounded-sm text-[10px] font-bold uppercase tracking-wider bg-primary-container/20 text-primary">Completed</span>
                </td>
              </tr>
              <tr className="hover:bg-primary/5 transition-all duration-150 group">
                <td className="px-8 py-5 text-sm font-mono font-bold text-primary border-r border-outline-variant/10">#TR-88209</td>
                <td className="px-8 py-5 border-r border-outline-variant/10">
                  <div className="flex items-center gap-4">
                    <div className="h-10 w-10 rounded-none bg-surface-container-low overflow-hidden">
                      <img alt="Silver laptop" src="https://lh3.googleusercontent.com/aida-public/AB6AXuBjh63yO75O3iCm17Xm94s2KTFWy3OZ8eA5ssToKlEHWYX2NMMYTrvdI_44ykml8XWJwup6C5W4Iaw8h76cmWg78sBvB4_VOWAyMeY4VO7xB2Ip2QpABKYkBqgq3L8BDGROyMn8zYMNhB9lYwxQZLGaH4I0EWvstTTW-g-7ebWm4USyxo_wPIu8RTU2PtBATXUPvAbdq1UaSiJa5aezWzTAlWcYZZ8ueLPYqgo6iI16mk8WWath1o1B_YAZmeYu5fLHwGe-uNFb2w" />
                    </div>
                    <div>
                      <p className="text-sm font-bold text-on-surface">Quantum Pro Laptop</p>
                      <p className="text-[10px] font-medium text-primary uppercase tracking-widest">Electronics</p>
                    </div>
                  </div>
                </td>
                <td className="px-8 py-5 text-sm font-bold text-on-surface border-r border-outline-variant/10">05 Units</td>
                <td className="px-8 py-5 border-r border-outline-variant/10">
                  <p className="text-sm font-medium text-on-surface">Oct 24, 2023</p>
                  <p className="text-[10px] text-on-surface-variant font-medium uppercase mt-0.5">12:45 PM</p>
                </td>
                <td className="px-8 py-5 text-right">
                  <span className="inline-block px-3 py-1 rounded-sm text-[10px] font-bold uppercase tracking-wider bg-secondary-container/30 text-secondary">Processing</span>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </section>
  );
}
