import { useState, useEffect } from "react";
import { getReportsList, getReportData } from "../../api/reports";
import DataTable from "../../components/DataTable";

export default function Reports() {
  const [reports, setReports] = useState([]);
  const [selectedReportId, setSelectedReportId] = useState('daily-revenue');
  const [reportData, setReportData] = useState(null);
  const [loadingList, setLoadingList] = useState(true);
  const [loadingData, setLoadingData] = useState(false);
  const [searchTerm, setSearchTerm] = useState('');

  const [isCatalogCollapsed, setIsCatalogCollapsed] = useState(false);

  useEffect(() => {
    const fetchList = async () => {
      try {
        const list = await getReportsList();
        setReports(list);
      } catch (err) {
        console.error("Failed to fetch reports list", err);
      } finally {
        setLoadingList(false);
      }
    };
    fetchList();
  }, []);

  useEffect(() => {
    if (selectedReportId) {
      const fetchData = async () => {
        setLoadingData(true);
        try {
          const data = await getReportData(selectedReportId);
          setReportData(data);
        } catch (err) {
          console.error("Failed to fetch report data", err);
        } finally {
          setLoadingData(false);
        }
      };
      fetchData();
    }
  }, [selectedReportId]);

  const categories = ['Financials', 'Inventory', 'Analytics'];

  const filteredTransactions = reportData?.transactions?.filter(t => 
    t.name.toLowerCase().includes(searchTerm.toLowerCase()) || 
    t.sku.toLowerCase().includes(searchTerm.toLowerCase())
  ) || [];

  const columns = [
    {
      header: 'Product Name',
      render: (item) => <p className="text-xs font-bold text-on-surface">{item.name}</p>
    },
    {
      header: 'SKU',
      render: (item) => (
        <span className="px-2 py-1 bg-surface-container text-[9px] font-mono font-bold rounded text-on-surface-variant tracking-tighter">
          {item.sku}
        </span>
      )
    },
    {
      header: 'Amount',
      align: 'right',
      render: (item) => <p className="text-xs font-black text-on-surface font-headline">{item.amount}</p>
    },
    {
      header: 'Status',
      render: (item) => (
        <span className={`px-2 py-1 rounded-none text-[9px] font-black uppercase tracking-widest ${item.status === 'Reconciled' ? 'bg-primary-container/10 text-primary' : 'bg-warning-container/10 text-warning'}`}>
          {item.status}
        </span>
      )
    },
    {
      header: 'Timestamp',
      render: (item) => <p className="text-[10px] font-bold text-on-surface-variant">{item.date}</p>
    }
  ];

  return (
    <div className="flex-1 flex flex-col min-h-screen bg-surface">
      <header className="px-8 py-6 border-b border-outline-variant/10 flex justify-between items-end">
        <div className="flex items-center gap-4">
          <button 
            onClick={() => setIsCatalogCollapsed(!isCatalogCollapsed)}
            className="w-10 h-10 flex items-center justify-center rounded-full bg-surface-container-high hover:bg-surface-container-highest transition-all active:scale-90 border border-outline-variant/20"
            title={isCatalogCollapsed ? "Show Catalog" : "Hide Catalog"}
          >
            <span className={`material-symbols-outlined text-primary transition-transform duration-300 ${isCatalogCollapsed ? '' : 'rotate-180'}`}>
              {isCatalogCollapsed ? 'menu' : 'menu_open'}
            </span>
          </button>
          <div>
            <h1 className="text-3xl font-headline font-extrabold text-on-surface tracking-tight">Reports & Analytics</h1>
            <p className="text-on-surface-variant text-sm mt-1">Monitor operational excellence and financial accuracy.</p>
          </div>
        </div>
        <div className="text-right">
          <p className="text-sm font-bold text-on-surface">Alex Mercer</p>
          <p className="text-[10px] font-bold text-primary uppercase tracking-widest">Ops Manager</p>
        </div>
      </header>

      <div className="flex flex-1 overflow-hidden">
        {/* Sidebar Catalog */}
        <aside className={`${isCatalogCollapsed ? 'w-0 opacity-0 -translate-x-full' : 'w-80 opacity-100 translate-x-0'} border-r border-outline-variant/10 flex flex-col bg-surface-container-lowest overflow-hidden transition-all duration-500 ease-in-out`}>
          <div className="p-6 min-w-[20rem] h-full overflow-y-auto">
            <div className="flex items-center gap-2 mb-6">
              <span className="material-symbols-outlined text-primary">folder_open</span>
              <h2 className="text-xs font-black uppercase tracking-[0.2em] text-on-surface-variant">Report Catalog</h2>
            </div>

            {categories.map(category => (
              <div key={category} className="mb-8 last:mb-0">
                <h3 className="text-[11px] font-black text-primary/60 uppercase tracking-widest mb-4 px-2">{category}</h3>
                <div className="space-y-1">
                  {reports.filter(r => r.category === category).map(report => (
                    <button
                      key={report.id}
                      onClick={() => setSelectedReportId(report.id)}
                      className={`w-full text-left p-3 transition-all duration-300 group ${selectedReportId === report.id ? 'bg-primary-container/10 border-l-4 border-primary' : 'hover:bg-surface-container-low border-l-4 border-transparent'}`}
                    >
                      <p className={`text-xs font-bold leading-tight ${selectedReportId === report.id ? 'text-primary' : 'text-on-surface'}`}>
                        {report.title}
                      </p>
                      <p className="text-[10px] text-on-surface-variant mt-1 line-clamp-1 group-hover:line-clamp-none transition-all">
                        {report.description}
                      </p>
                    </button>
                  ))}
                </div>
              </div>
            ))}
          </div>
        </aside>

        {/* Main Content Detail */}
        <main className="flex-1 overflow-y-auto bg-surface p-8 transition-all duration-500 ease-in-out">
          {loadingData ? (
            <div className="h-full flex flex-col items-center justify-center opacity-50">
              <span className="material-symbols-outlined animate-spin text-4xl text-primary">progress_activity</span>
              <p className="text-[10px] font-black uppercase tracking-widest mt-4">Generating Report...</p>
            </div>
          ) : reportData ? (
            <div className="w-full space-y-8 animate-in fade-in slide-in-from-bottom-4 duration-700">
              <div className="flex justify-between items-start">
                <div>
                  <h2 className="text-2xl font-headline font-black text-on-surface">{reportData.title}</h2>
                  <p className="text-on-surface-variant mt-1">{reportData.description}</p>
                </div>
                <button className="flex items-center gap-2 px-4 py-2 bg-surface-container-high border border-outline-variant/30 text-xs font-bold uppercase tracking-widest hover:bg-surface-container-highest transition-colors active:scale-95">
                  <span className="material-symbols-outlined text-lg">download</span>
                  Export CSV
                </button>
              </div>

              {/* Stats Grid */}
              <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
                {reportData.stats.map((stat, i) => (
                  <div key={i} className="p-6 bg-surface-container-lowest border border-outline-variant/10 relative overflow-hidden group">
                    <div className="absolute top-0 right-0 p-4 opacity-5 group-hover:opacity-10 transition-opacity">
                      <span className="material-symbols-outlined text-4xl">trending_up</span>
                    </div>
                    <p className="text-[10px] font-black text-on-surface-variant uppercase tracking-widest mb-1">{stat.label}</p>
                    <p className="text-2xl font-headline font-black text-on-surface tracking-tight">{stat.value}</p>
                    <div className={`mt-2 inline-flex items-center gap-1 text-[10px] font-bold px-2 py-0.5 rounded-full ${stat.trend === 'up' ? 'bg-primary-container/20 text-primary' : 'bg-error-container/20 text-error'}`}>
                      <span className="material-symbols-outlined text-xs">{stat.trend === 'up' ? 'arrow_upward' : 'arrow_downward'}</span>
                      {stat.change}
                    </div>
                  </div>
                ))}
              </div>

              {/* Data Table */}
              <div className="bg-surface-container-lowest border border-outline-variant/10 overflow-hidden shadow-sm">
                <div className="px-6 py-4 border-b border-outline-variant/10 flex justify-between items-center">
                  <h3 className="text-xs font-black uppercase tracking-widest text-on-surface-variant">Transactional Audit</h3>
                  <div className="flex gap-2">
                    <div className="relative">
                      <span className="material-symbols-outlined absolute left-3 top-1/2 -translate-y-1/2 text-sm text-outline">search</span>
                      <input 
                        type="text" 
                        placeholder="SEARCH SKU OR NAME..." 
                        value={searchTerm}
                        onChange={(e) => setSearchTerm(e.target.value)}
                        className="pl-9 pr-4 py-1.5 bg-surface-container text-[10px] font-bold border-none focus:ring-1 ring-primary w-48 placeholder:text-outline-variant"
                      />
                    </div>
                  </div>
                </div>
                
                <DataTable
                  data={filteredTransactions}
                  columns={columns}
                  defaultPageSize={5}
                  selectable={false}
                  emptyMessage="No transactions found matching your criteria."
                />
              </div>
            </div>
          ) : (
            <div className="h-full flex flex-col items-center justify-center text-center">
              <span className="material-symbols-outlined text-6xl text-outline-variant mb-4">analytics</span>
              <h3 className="text-xl font-headline font-black text-on-surface">Select a report to begin</h3>
              <p className="text-on-surface-variant mt-2 max-w-xs">Detailed analytics and reconciliation data will appear here.</p>
            </div>
          )}
        </main>
      </div>
    </div>
  );
}
