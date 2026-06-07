import { useState, useMemo, useEffect } from "react";
import DataTable from "../../components/DataTable";
import { getDailySalesReport } from "../../api/reports";

// ─── Static mock data (reports not yet API-backed) ────────────────────────────

const STOCK_DATA = [
  { id: 1,  Company: "SAMSUNG",   ItemCode: "SAMSUNG AR18CY3ZAWKNNNA", ItemName: "SAMSUNG AC AR18CY3ZAWKNNNA 1.5T",          Category: "Air Conditioner", OpeningStock: 5,  ReceivedQty: 10, SoldQty: 8,  ClosingStock: 7,  Unit: "Nos" },
  { id: 2,  Company: "LG",        ItemCode: "LG WM1455HWA",            ItemName: "LG Front Load Washing Machine 7KG",          Category: "Washing Machine", OpeningStock: 3,  ReceivedQty:  6, SoldQty: 4,  ClosingStock: 5,  Unit: "Nos" },
  { id: 3,  Company: "SONY",      ItemCode: "SONY KD55X75L",           ItemName: "SONY BRAVIA 55 INCH 4K LED TV",              Category: "Television",      OpeningStock: 8,  ReceivedQty:  4, SoldQty: 5,  ClosingStock: 7,  Unit: "Nos" },
  { id: 4,  Company: "BOSCH",     ItemCode: "BOSCH WAJ2846SIN",        ItemName: "BOSCH 7KG Fully Automatic Front Load WM",    Category: "Washing Machine", OpeningStock: 2,  ReceivedQty:  8, SoldQty: 3,  ClosingStock: 7,  Unit: "Nos" },
  { id: 5,  Company: "SAMSUNG",   ItemCode: "SAMSUNG RT42M553EBU",     ItemName: "SAMSUNG 415L Double Door Refrigerator",      Category: "Refrigerator",    OpeningStock: 6,  ReceivedQty: 12, SoldQty: 9,  ClosingStock: 9,  Unit: "Nos" },
  { id: 6,  Company: "WHIRLPOOL", ItemCode: "WHIRLPOOL IF375ELPB",     ItemName: "WHIRLPOOL 375L Frost Free Refrigerator",     Category: "Refrigerator",    OpeningStock: 4,  ReceivedQty:  5, SoldQty: 6,  ClosingStock: 3,  Unit: "Nos" },
  { id: 7,  Company: "VOLTAS",    ItemCode: "VOLTAS 185V DZA",         ItemName: "VOLTAS 1.5T 5 Star Inverter Split AC",       Category: "Air Conditioner", OpeningStock: 10, ReceivedQty: 15, SoldQty: 14, ClosingStock: 11, Unit: "Nos" },
  { id: 8,  Company: "HAIER",     ItemCode: "HAIER HRB-2764PMG-E",     ItemName: "HAIER 256L 3 Star Direct Cool Refrigerator", Category: "Refrigerator",    OpeningStock: 7,  ReceivedQty:  3, SoldQty: 4,  ClosingStock: 6,  Unit: "Nos" },
  { id: 9,  Company: "LG",        ItemCode: "LG GL-B201APZX",          ItemName: "LG 190L 5 Star Direct Cool Refrigerator",   Category: "Refrigerator",    OpeningStock: 9,  ReceivedQty:  6, SoldQty: 7,  ClosingStock: 8,  Unit: "Nos" },
  { id: 10, Company: "CARRIER",   ItemCode: "CARRIER CAI18EK3C8F0",    ItemName: "CARRIER 1.5T 3 Star Fixed Speed Split AC",   Category: "Air Conditioner", OpeningStock: 3,  ReceivedQty:  9, SoldQty: 5,  ClosingStock: 7,  Unit: "Nos" },
];

const PURCHASE_DATA = [
  { id: 1,  Supplier: "SAMSUNG INDIA",  ItemCode: "SAMSUNG AR18CY3ZAWKNNNA", ItemName: "SAMSUNG AC AR18CY3ZAWKNNNA 1.5T",          InvoiceNo: "PI-001", PurchaseDateString: "10-Apr-2026", Rate: 52000.00, Quantity: 5,  TaxableAmount: 203125.00, GSTRate: 28, TotalGSTAmount:  56875.00, TotalAmount: 260000.00 },
  { id: 2,  Supplier: "LG ELECTRONICS", ItemCode: "LG WM1455HWA",            ItemName: "LG Front Load Washing Machine 7KG",          InvoiceNo: "PI-002", PurchaseDateString: "10-Apr-2026", Rate: 28000.00, Quantity: 6,  TaxableAmount: 131250.00, GSTRate: 28, TotalGSTAmount:  36750.00, TotalAmount: 168000.00 },
  { id: 3,  Supplier: "SONY INDIA",     ItemCode: "SONY KD55X75L",           ItemName: "SONY BRAVIA 55 INCH 4K LED TV",              InvoiceNo: "PI-003", PurchaseDateString: "11-Apr-2026", Rate: 60000.00, Quantity: 4,  TaxableAmount: 203389.83, GSTRate: 18, TotalGSTAmount:  36610.17, TotalAmount: 240000.00 },
  { id: 4,  Supplier: "BOSCH LTD",      ItemCode: "BOSCH WAJ2846SIN",        ItemName: "BOSCH 7KG Fully Automatic Front Load WM",    InvoiceNo: "PI-004", PurchaseDateString: "11-Apr-2026", Rate: 36000.00, Quantity: 8,  TaxableAmount: 244067.80, GSTRate: 18, TotalGSTAmount:  43932.20, TotalAmount: 288000.00 },
  { id: 5,  Supplier: "SAMSUNG INDIA",  ItemCode: "SAMSUNG RT42M553EBU",     ItemName: "SAMSUNG 415L Double Door Refrigerator",      InvoiceNo: "PI-005", PurchaseDateString: "12-Apr-2026", Rate: 32000.00, Quantity: 12, TaxableAmount: 300000.00, GSTRate: 28, TotalGSTAmount:  84000.00, TotalAmount: 384000.00 },
  { id: 6,  Supplier: "WHIRLPOOL CORP", ItemCode: "WHIRLPOOL IF375ELPB",     ItemName: "WHIRLPOOL 375L Frost Free Refrigerator",     InvoiceNo: "PI-006", PurchaseDateString: "12-Apr-2026", Rate: 24000.00, Quantity: 5,  TaxableAmount: 101694.92, GSTRate: 18, TotalGSTAmount:  18305.08, TotalAmount: 120000.00 },
  { id: 7,  Supplier: "VOLTAS LTD",     ItemCode: "VOLTAS 185V DZA",         ItemName: "VOLTAS 1.5T 5 Star Inverter Split AC",       InvoiceNo: "PI-007", PurchaseDateString: "13-Apr-2026", Rate: 34000.00, Quantity: 15, TaxableAmount: 398437.50, GSTRate: 28, TotalGSTAmount: 111562.50, TotalAmount: 510000.00 },
  { id: 8,  Supplier: "HAIER INDIA",    ItemCode: "HAIER HRB-2764PMG-E",     ItemName: "HAIER 256L 3 Star Direct Cool Refrigerator", InvoiceNo: "PI-008", PurchaseDateString: "13-Apr-2026", Rate: 18500.00, Quantity: 3,  TaxableAmount:  47033.90, GSTRate: 18, TotalGSTAmount:   8466.10, TotalAmount:  55500.00 },
  { id: 9,  Supplier: "LG ELECTRONICS", ItemCode: "LG GL-B201APZX",          ItemName: "LG 190L 5 Star Direct Cool Refrigerator",   InvoiceNo: "PI-009", PurchaseDateString: "14-Apr-2026", Rate: 14500.00, Quantity: 6,  TaxableAmount:  73728.81, GSTRate: 18, TotalGSTAmount:  13271.19, TotalAmount:  87000.00 },
  { id: 10, Supplier: "CARRIER AIRCON", ItemCode: "CARRIER CAI18EK3C8F0",    ItemName: "CARRIER 1.5T 3 Star Fixed Speed Split AC",   InvoiceNo: "PI-010", PurchaseDateString: "14-Apr-2026", Rate: 28000.00, Quantity: 9,  TaxableAmount: 196875.00, GSTRate: 28, TotalGSTAmount:  55125.00, TotalAmount: 252000.00 },
];

// ─── Helpers ──────────────────────────────────────────────────────────────────

function fmt(n) {
  return n.toLocaleString("en-IN", { minimumFractionDigits: 2, maximumFractionDigits: 2 });
}

function parseDMYDate(str) {
  const months = { Jan:0,Feb:1,Mar:2,Apr:3,May:4,Jun:5,Jul:6,Aug:7,Sep:8,Oct:9,Nov:10,Dec:11 };
  const [day, mon, year] = str.split("-");
  return new Date(+year, months[mon], +day);
}

// ─── Report definitions ───────────────────────────────────────────────────────

const REPORTS = [
  {
    id: "daily-sales",
    label: "Daily Sales",
    icon: "point_of_sale",
    fetchFn: getDailySalesReport,
    staticData: null,
    dateKey: "SaleDateString",
    searchPlaceholder: "Search all columns...",
    hasDateFilter: true,
    columns: [
      { header: "Sl.No",         align: "center", className: "w-14",  render: (_, i) => i + 1 },
      { header: "Company",       className: "whitespace-nowrap",        render: r => r.Company },
      { header: "Item Code",                                            render: r => r.ItemCode },
      { header: "Item Name",                                            render: r => r.ItemName },
      { header: "Invoice No",                                           render: r => r.InvoiceNo },
      { header: "Sale Date",     className: "whitespace-nowrap",        render: r => r.SaleDateString },
      { header: "Rate",          align: "right",                        render: r => fmt(r.Rate) },
      { header: "Quantity",      align: "right", sumKey: "Quantity",       render: r => r.Quantity.toFixed(2) },
      { header: "Taxable Amt",   align: "right", sumKey: "TaxableAmount",  render: r => fmt(r.TaxableAmount) },
      { header: "GST Rate",      align: "right",                           render: r => r.GSTRate.toFixed(2) },
      { header: "Total GST Amt", align: "right", sumKey: "TotalGSTAmount", render: r => fmt(r.TotalGSTAmount) },
      { header: "Total Amount",  align: "right", sumKey: "TotalAmount",    render: r => fmt(r.TotalAmount) },
    ],
  },
  {
    id: "stock",
    label: "Stock Report",
    icon: "inventory_2",
    fetchFn: null,
    staticData: STOCK_DATA,
    dateKey: null,
    searchPlaceholder: "Search all columns...",
    hasDateFilter: false,
    columns: [
      { header: "Sl.No",         align: "center", className: "w-14", render: (_, i) => i + 1 },
      { header: "Company",       className: "whitespace-nowrap",       render: r => r.Company },
      { header: "Item Code",                                           render: r => r.ItemCode },
      { header: "Item Name",                                           render: r => r.ItemName },
      { header: "Category",                                            render: r => r.Category },
      { header: "Opening Stock", align: "right", sumKey: "OpeningStock", render: r => r.OpeningStock },
      { header: "Received Qty",  align: "right", sumKey: "ReceivedQty",  render: r => r.ReceivedQty },
      { header: "Sold Qty",      align: "right", sumKey: "SoldQty",      render: r => r.SoldQty },
      { header: "Closing Stock", align: "right", sumKey: "ClosingStock", render: r => r.ClosingStock },
      { header: "Unit",          align: "center",                        render: r => r.Unit },
    ],
  },
  {
    id: "purchase",
    label: "Purchase Report",
    icon: "shopping_cart",
    fetchFn: null,
    staticData: PURCHASE_DATA,
    dateKey: "PurchaseDateString",
    searchPlaceholder: "Search all columns...",
    hasDateFilter: true,
    columns: [
      { header: "Sl.No",         align: "center", className: "w-14",  render: (_, i) => i + 1 },
      { header: "Supplier",      className: "whitespace-nowrap",        render: r => r.Supplier },
      { header: "Item Code",                                            render: r => r.ItemCode },
      { header: "Item Name",                                            render: r => r.ItemName },
      { header: "Invoice No",                                           render: r => r.InvoiceNo },
      { header: "Purchase Date", className: "whitespace-nowrap",        render: r => r.PurchaseDateString },
      { header: "Rate",          align: "right",                        render: r => fmt(r.Rate) },
      { header: "Quantity",      align: "right", sumKey: "Quantity",       render: r => r.Quantity.toFixed(2) },
      { header: "Taxable Amt",   align: "right", sumKey: "TaxableAmount",  render: r => fmt(r.TaxableAmount) },
      { header: "GST Rate",      align: "right",                           render: r => r.GSTRate.toFixed(2) },
      { header: "Total GST Amt", align: "right", sumKey: "TotalGSTAmount", render: r => fmt(r.TotalGSTAmount) },
      { header: "Total Amount",  align: "right", sumKey: "TotalAmount",    render: r => fmt(r.TotalAmount) },
    ],
  },
];

// ─── Component ────────────────────────────────────────────────────────────────

export default function Reports() {
  const today = new Date().toISOString().slice(0, 10);

  const [activeId, setActiveId]     = useState("daily-sales");
  const [fromDate, setFromDate]     = useState(today);
  const [toDate, setToDate]         = useState(today);
  const [searchTerm, setSearchTerm] = useState("");
  const [applied, setApplied]       = useState({ fromDate: today, toDate: today, searchTerm: "" });
  const [reportData, setReportData] = useState([]);
  const [loading, setLoading]       = useState(false);
  const [hasSearched, setHasSearched] = useState(false);

  const report = REPORTS.find(r => r.id === activeId);

  // On tab switch: static reports load immediately; API reports wait for Apply
  useEffect(() => {
    setReportData([]);
    setHasSearched(false);
    if (!report.fetchFn) {
      setReportData(report.staticData);
      setHasSearched(true);
    }
  }, [report.id]); // eslint-disable-line react-hooks/exhaustive-deps

  const filtered = useMemo(() => {
    const from = applied.fromDate ? new Date(applied.fromDate) : null;
    const to   = applied.toDate   ? new Date(applied.toDate)   : null;
    const term = applied.searchTerm.toLowerCase();

    return reportData.filter(row => {
      if (report.dateKey && !report.fetchFn) {
        const d = parseDMYDate(row[report.dateKey]);
        if (from && d < from) return false;
        if (to   && d > to)   return false;
      }
      if (term && !Object.values(row).some(v => String(v ?? "").toLowerCase().includes(term))) return false;
      return true;
    });
  }, [reportData, report, applied]);


  const handleApply = async () => {
    setApplied({ fromDate, toDate, searchTerm });
    if (report.fetchFn) {
      setLoading(true);
      try {
        const data = await report.fetchFn({ fromDate, toDate });
        setReportData(data);
        setHasSearched(true);
      } finally {
        setLoading(false);
      }
    }
  };

  const resetFilters = () => {
    const today = new Date().toISOString().slice(0, 10);
    setFromDate(today); setToDate(today); setSearchTerm("");
    setApplied({ fromDate: today, toDate: today, searchTerm: "" });
    setReportData([]); setHasSearched(false);
  };

  const switchReport = (id) => { setActiveId(id); resetFilters(); };

  return (
    <section className="p-8 bg-surface min-h-screen flex flex-col">

      {/* Page Header */}
      <div className="flex justify-between items-center mb-6">
        <div>
          <h1 className="text-2xl font-headline font-bold text-on-surface tracking-tight">Reports</h1>
          <p className="text-sm text-on-surface-variant mt-1">Select a report and apply filters to view data.</p>
        </div>
        <button className="flex items-center gap-2 px-6 h-11 border border-outline-variant/20 text-on-surface-variant hover:text-primary hover:bg-surface-container-low transition-all rounded-none font-bold text-xs uppercase tracking-widest whitespace-nowrap">
          <span className="material-symbols-outlined text-[20px]">download</span>
          Export CSV
        </button>
      </div>

      {/* Report Tabs */}
      <div className="flex gap-0 border-b border-outline-variant/20 mb-6">
        {REPORTS.map(r => (
          <button
            key={r.id}
            onClick={() => switchReport(r.id)}
            className={`flex items-center gap-2 px-5 py-3 text-xs font-bold uppercase tracking-widest border-b-2 transition-all whitespace-nowrap ${
              activeId === r.id
                ? "border-primary text-primary"
                : "border-transparent text-on-surface-variant hover:text-on-surface hover:border-outline-variant/40"
            }`}
          >
            <span className="material-symbols-outlined text-base">{r.icon}</span>
            {r.label}
          </button>
        ))}
      </div>

      {/* Filters */}
      <div className="bg-surface-container-lowest border border-outline-variant/10 rounded-none ambient-shadow p-5 mb-5">
        <div className="flex flex-wrap items-end gap-4">

          {/* Date range */}
          {report.hasDateFilter && (
            <>
              <div>
                <label className="text-[10px] font-bold text-on-surface-variant uppercase tracking-widest mb-2 block">From Date</label>
                <input
                  type="date"
                  value={fromDate}
                  onChange={e => setFromDate(e.target.value)}
                  className="bg-surface-container-lowest border border-outline-variant/20 rounded-none px-4 py-2.5 text-sm font-medium text-on-surface focus:border-primary transition-all outline-none w-44 h-11"
                />
              </div>
              <div>
                <label className="text-[10px] font-bold text-on-surface-variant uppercase tracking-widest mb-2 block">To Date</label>
                <input
                  type="date"
                  value={toDate}
                  onChange={e => setToDate(e.target.value)}
                  className="bg-surface-container-lowest border border-outline-variant/20 rounded-none px-4 py-2.5 text-sm font-medium text-on-surface focus:border-primary transition-all outline-none w-44 h-11"
                />
              </div>
            </>
          )}

          {/* Apply / Clear — immediately after dates */}
          <div className="flex gap-3 items-end">
            <button
              onClick={handleApply}
              disabled={loading}
              className="flex items-center gap-2 px-6 h-11 btn-gradient rounded-none font-bold text-xs uppercase tracking-widest whitespace-nowrap disabled:opacity-50"
            >
              <span className={`material-symbols-outlined text-base ${loading ? "animate-spin" : ""}`}>
                {loading ? "progress_activity" : "filter_list"}
              </span>
              Apply
            </button>
            <button
              onClick={resetFilters}
              disabled={loading}
              className="flex items-center gap-2 px-4 h-11 border border-outline-variant/20 text-on-surface-variant hover:text-error hover:border-error/30 hover:bg-surface-container-low transition-all rounded-none font-bold text-[10px] uppercase tracking-widest disabled:opacity-50"
            >
              <span className="material-symbols-outlined text-base">backspace</span>
              Clear
            </button>
          </div>

          {/* Live client-side search */}
          <div className="flex-1 min-w-[200px]">
            <label className="text-[10px] font-bold text-on-surface-variant uppercase tracking-widest mb-2 block">Search</label>
            <div className="relative group">
              <span className="material-symbols-outlined absolute left-3 top-1/2 -translate-y-1/2 text-on-surface-variant text-base group-focus-within:text-primary transition-colors">search</span>
              <input
                type="text"
                placeholder={report.searchPlaceholder}
                value={searchTerm}
                onChange={e => {
                  setSearchTerm(e.target.value);
                  setApplied(prev => ({ ...prev, searchTerm: e.target.value }));
                }}
                className="pl-10 pr-4 py-2 bg-surface-container-lowest border border-outline-variant/20 rounded-none text-[13px] font-medium text-on-surface focus:border-primary focus:ring-1 focus:ring-primary/10 transition-all outline-none w-full h-11"
              />
            </div>
          </div>

        </div>
      </div>

      {/* Table */}
      <div className="bg-surface-container-lowest rounded-none ambient-shadow border border-outline-variant/10 overflow-hidden flex-1">
        <DataTable
          key={activeId}
          data={filtered}
          columns={report.columns}
          defaultPageSize={10}
          pageSizeOptions={[10, 20, 50]}
          selectable={false}
          showSummary={true}
          maxHeight="calc(100vh - 490px)"
          emptyMessage={
            loading      ? "Loading..." :
            !hasSearched ? "Select a date range and click Apply to load the report." :
                           "No records match the selected filters."
          }
        />
      </div>

    </section>
  );
}
