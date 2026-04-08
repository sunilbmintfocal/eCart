export default function Inventory() {
  return (
    <section className="px-8 py-8 min-h-screen bg-surface">
      <div className="flex justify-between items-end mb-10">
        <div>
          <span className="text-primary text-[10px] font-medium uppercase tracking-[0.2em] mb-1.5 block">Asset Ledger</span>
          <h1 className="text-4xl font-headline font-bold text-on-surface tracking-tight mb-2">Inventory Registry</h1>
          <p className="text-on-surface-variant font-body font-medium text-sm">A precision-curated collection of enterprise assets and stock movements.</p>
        </div>
        <button className="flex items-center gap-2 px-6 py-2.5 btn-gradient rounded-none font-bold text-sm">
          <span className="material-symbols-outlined text-lg">add</span>
          <span>Register Asset</span>
        </button>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-4 gap-6 mb-12">
        <div className="bg-surface-container-lowest p-6 rounded-none ambient-shadow border border-outline-variant/5">
          <p className="text-[10px] font-bold text-on-surface-variant uppercase tracking-widest mb-2 font-label">Total SKUs</p>
          <p className="text-3xl font-bold text-on-surface font-headline tracking-tighter">1,482</p>
        </div>
        <div className="bg-surface-container-lowest p-6 rounded-none ambient-shadow border border-outline-variant/5">
          <p className="text-[10px] font-bold text-on-surface-variant uppercase tracking-widest mb-2 font-label">Registry Value</p>
          <p className="text-3xl font-bold text-on-surface font-headline tracking-tighter">$4.2M</p>
        </div>
        <div className="bg-surface-container-lowest p-6 rounded-none ambient-shadow border border-outline-variant/5">
          <p className="text-[10px] font-bold text-error uppercase tracking-widest mb-2 font-label">Stock Critical</p>
          <p className="text-3xl font-bold text-error font-headline tracking-tighter">24</p>
        </div>
        <div className="bg-surface-container-lowest p-6 rounded-none ambient-shadow border border-outline-variant/5">
          <p className="text-[10px] font-bold text-primary uppercase tracking-widest mb-2 font-label">New Entries</p>
          <p className="text-3xl font-bold text-primary font-headline tracking-tighter">+12</p>
        </div>
      </div>

      <div className="flex gap-4 mb-10 items-center bg-surface-container-low/40 p-1.5 rounded-none border border-outline-variant/10">
        <span className="text-[10px] font-bold text-on-surface-variant uppercase tracking-[0.2em] ml-4 mr-2">Classification</span>
        <button className="px-5 py-2 rounded-none bg-surface-container-lowest text-on-surface text-xs font-bold shadow-sm border border-outline-variant/10 transition-all">All Assets</button>
        <button className="px-5 py-2 rounded-none text-on-surface-variant text-xs font-bold hover:bg-surface-container-low transition-all">Furniture</button>
        <button className="px-5 py-2 rounded-none text-on-surface-variant text-xs font-bold hover:bg-surface-container-low transition-all">Lighting</button>
        <button className="px-5 py-2 rounded-none text-on-surface-variant text-xs font-bold hover:bg-surface-container-low transition-all">Art & Decor</button>
        <div className="ml-auto flex gap-2 pr-1">
          <button className="p-2 bg-surface-container-lowest rounded-none text-on-surface-variant border border-outline-variant/10 hover:text-primary transition-all">
            <span className="material-symbols-outlined text-lg" style={{fontVariationSettings: "'FILL' 1"}}>grid_view</span>
          </button>
          <button className="p-2 bg-surface-container-lowest rounded-none text-on-surface-variant border border-outline-variant/10 hover:text-primary transition-all">
            <span className="material-symbols-outlined text-lg">list</span>
          </button>
        </div>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-8">
        {[
          { id: 'FUR-ARM-001', name: 'Verona Velvet Armchair', category: 'Seating', price: '$1,240', qty: '3 Units', status: 'Low Stock', statusBg: 'bg-error-container/20', statusText: 'text-error', img: 'https://lh3.googleusercontent.com/aida-public/AB6AXuBduJh2k1WWyjOGKVuWFNLU72naCzpY7DGWPVQH-nqgASUf8NlOAZVRj7AkEOqHWtfjAk0RuEx6LbOmQBJKxEaDXIKUD9pi8EbvYh5iN3ogPv5NubbljJGzBYOdh0h1Vbt084MWCwUcTT5wKDLdzo9XhiMDCw6-57vfeaKik27G9Vwjtcm49mzwbgmmdS-aSWn91MBC9TCDm905NLpU4FkQ4w8AZxabte_aF2d7gAO8LzxneEZCfCFY3ntFHq_qSwRDMcE3_NoXpg' },
          { id: 'LIT-PEN-042', name: 'Aurelius Brass Pendant', category: 'Lighting', price: '$890', qty: '18 Units', status: 'In Stock', statusBg: 'bg-primary-container/20', statusText: 'text-primary', img: 'https://lh3.googleusercontent.com/aida-public/AB6AXuDCevqNM4YxtYC8FDQjDKB6u4ckL7QkrvimSlm6FzgREnL0ve7CUw58pIrJaOZFeaXohwMvw_S5dOyGEEd2mz-TjxgOFv70frarkJNt6SrogcRKDdFXcggpJlxwd1N9-5GPrl2b6Ey7Yyh6zvjqknf920HH-IJPzFOJfq1fMgRoKfVOgO_kljFHIzqFvZCqTb9cosmVmoMf33mHr4WQrwXYLwV3vHBewHVN9EUlP23ud2mDqbStLaLyHPJhUdw2ZKjSQUZq5za8hQ' },
          { id: 'DEC-CER-019', name: 'Nordic Earth Vase', category: 'Ceramics', price: '$420', qty: '0 Units', status: 'Out of Stock', statusBg: 'bg-surface-container-high', statusText: 'text-on-surface-variant', img: 'https://lh3.googleusercontent.com/aida-public/AB6AXuA0YA5GR94FguAfhpts3Xcyac9tGXU6QnpvfrVgNlxPensPhIG6f-8k0OJDIA4hJ1UjluO7Rqi2Pq2DovLt5MsZg6w7h-EV4HoEljjZVIAivBFWHcAjM0-A2dKNymDtIfHcu7qgH13uZze2K0_RTdJO7AVeeIGnN1IbzSqlWfeRRODs_ioC3TkcUQPpqDsGG_lhCnhKS5MbzNEJSl3aF2zVPHMCyE2BJrIeRsYpYjP4QvAcs2vqfG-yJSaq4OZ5jOYn9EN91rsCNg', grayscale: true },
          { id: 'FUR-TBL-088', name: 'Calacatta Low Table', category: 'Tables', price: '$2,100', qty: '5 Units', status: 'In Stock', statusBg: 'bg-primary-container/20', statusText: 'text-primary', img: 'https://lh3.googleusercontent.com/aida-public/AB6AXuAGP5jsoRUL2axYhBHKUDN-tKKfx32mMQngpYypfkR9IZFkT_xUi1ZfBrDuiIEjOO1Fm-9f2HSnCuTpRvOYJUFczZtT1XOQBi1SHDu3zExsaDfoov-zRtSyPFaaV9HpdTJbKQsNghYhGxjreJznBm4ZiK4D9tmYUpTeQZ-SslSugVG_0HITw3qwi2uGw05nDHTlAGotE1beXUB8dF7Ht8u0NZ6ZmswoVJZwl-WQv1Qk9WiAzSf9K62kIZ9Oa2Nf4L1oK_ELjwUNtw' },
        ].map((item, i) => (
          <div key={i} className="bg-surface-container-lowest rounded-none overflow-hidden group ambient-shadow border border-outline-variant/5 transition-all duration-300 hover:translate-y-[-4px]">
            <div className={`relative h-64 bg-surface-container-low overflow-hidden ${item.grayscale ? 'grayscale opacity-70' : ''}`}>
              <img alt={item.name} className="w-full h-full object-cover transition-transform duration-500 group-hover:scale-105" src={item.img} />
              <div className="absolute top-4 right-4">
                <span className={`px-2.5 py-1 ${item.statusBg} ${item.statusText} text-[10px] font-bold rounded-none uppercase tracking-wider`}>{item.status}</span>
              </div>
            </div>
            <div className="p-6">
              <div className="flex justify-between items-start mb-1.5">
                <p className="text-[10px] font-bold text-primary uppercase tracking-[0.1em]">{item.category}</p>
                <p className="text-lg font-bold text-on-surface">{item.price}</p>
              </div>
              <h3 className="text-md font-bold text-on-surface font-headline mb-3 group-hover:text-primary transition-colors">{item.name}</h3>
              <div className="flex items-center justify-between pt-4 border-t border-outline-variant/10">
                <div className="flex items-center gap-2">
                  <span className="text-[10px] font-bold text-on-surface-variant uppercase tracking-widest">Qty:</span>
                  <span className={`text-xs font-bold ${item.qty === '0 Units' ? 'text-error' : 'text-on-surface'}`}>{item.qty}</span>
                </div>
                <button className="text-on-surface-variant hover:text-primary transition-colors">
                  <span className="material-symbols-outlined text-xl">more_horiz</span>
                </button>
              </div>
            </div>
          </div>
        ))}
      </div>

      <div className="mt-16 flex items-center justify-between border-t border-outline-variant/10 pt-8">
        <span className="text-[10px] font-bold text-on-surface-variant uppercase tracking-widest">Page 01 of 186</span>
        <div className="flex gap-1.5">
          <button className="w-8 h-8 flex items-center justify-center rounded-none bg-primary text-on-primary font-bold text-[10px] shadow-sm">1</button>
          <button className="w-8 h-8 flex items-center justify-center rounded-none hover:bg-surface-container-low text-on-surface-variant font-bold text-[10px] transition-all">2</button>
          <button className="w-8 h-8 flex items-center justify-center rounded-none hover:bg-surface-container-low text-on-surface-variant font-bold text-[10px] transition-all">3</button>
          <span className="px-2 self-center text-on-surface-variant/20 font-bold">...</span>
          <button className="w-8 h-8 flex items-center justify-center rounded-none hover:bg-surface-container-low text-on-surface-variant font-bold text-[10px] transition-all">186</button>
        </div>
      </div>
    </section>
  );
}
