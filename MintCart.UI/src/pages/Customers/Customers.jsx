import { useState, useEffect } from 'react';
import DataTable from '../../components/DataTable';
import { getCustomers } from '../../api/customers';

export default function Customers() {
  const [customers, setCustomers] = useState([]);
  const [searchTerm, setSearchTerm] = useState('');
  const [loading, setLoading] = useState(true);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedCustomer, setSelectedCustomer] = useState(null);

  const fetchCustomers = async () => {
    try {
      setLoading(true);
      const response = await getCustomers(); 
      const customerList = response?.Data || response?.data || (Array.isArray(response) ? response : []);
      setCustomers(customerList);
    } catch (error) {
      console.error('Failed to fetch customers', error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchCustomers();
  }, []);

  const handleCreate = () => {
    setSelectedCustomer({}); // Empty object for new customer
    setIsModalOpen(true);
  };

  const filteredCustomers = customers.filter(customer => {
    if (!searchTerm) return true;
    const s = searchTerm.toLowerCase();
    // Search across all relevant backend fields (checking both casing versions)
    return (
      (customer.CustomerName || customer.customerName || '').toLowerCase().includes(s) ||
      (customer.PhoneNo || customer.phoneNo || '').toLowerCase().includes(s) ||
      (customer.Address || customer.address || '').toLowerCase().includes(s) ||
      (customer.GSTINNumber || customer.gstinNumber || '').toLowerCase().includes(s) ||
      (customer.State || customer.state || '').toLowerCase().includes(s)
    );
  });

  const handleEdit = (customer) => {
    setSelectedCustomer(customer);
    setIsModalOpen(true);
  };

  const columns = [
    {
      header: 'Sl.No',
      align: 'center',
      className: 'w-20 text-on-surface-variant font-bold',
      render: (item) => customers.indexOf(item) + 1
    },
    {
      header: 'Name',
      className: 'font-bold text-on-surface',
      render: (item) => item.CustomerName || item.customerName || '-'
    },
    {
      header: 'Phone Number',
      className: 'text-on-surface-variant',
      render: (item) => item.PhoneNo || item.phoneNo || '-'
    },
    {
      header: 'Added Date',
      className: 'text-on-surface-variant',
      render: (item) => {
        const date = item.AddedDate || item.addedDate;
        return date ? new Date(date).toLocaleDateString(undefined, { year: 'numeric', month: 'short', day: 'numeric' }) : '-';
      }
    },
    {
      header: 'Status',
      align: 'center',
      render: (customer) => {
        const isActive = customer.IsActive ?? customer.isActive;
        return (
          <span className={`inline-block w-24 text-center px-3 py-1 rounded-none text-[10px] font-bold uppercase tracking-wider ${isActive ? 'bg-primary-container/20 text-primary' : 'bg-error-container/20 text-error'}`}>
            {isActive ? 'Active' : 'Inactive'}
          </span>
        );
      }
    },
    {
      header: 'Actions',
      align: 'right',
      render: (customer) => (
        <button
          onClick={() => handleEdit(customer)}
          className="w-10 h-10 flex items-center justify-center rounded-none hover:bg-primary/10 text-primary transition-all active:scale-95 group"
          title="Edit Customer"
        >
          <span className="material-symbols-outlined text-[20px] group-hover:scale-110 transition-transform">edit_calendar</span>
        </button>
      )
    }
  ];

  return (
    <section className="p-8 bg-surface min-h-screen">
      <div className="flex justify-between items-center mb-10">
        <div>
          <h1 className="text-2xl font-headline font-bold text-on-surface tracking-tight italic">Customer Management</h1>
        </div>
        <div className="flex gap-4 items-center">
          <div className="relative group">
            <span className="material-symbols-outlined absolute left-3 top-1/2 -translate-y-1/2 text-on-surface-variant text-base group-focus-within:text-primary transition-colors">search</span>
            <input
              type="text"
              placeholder="Search customers..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              className="pl-10 pr-4 py-2 bg-surface-container-lowest border border-outline-variant/20 rounded-none text-[13px] font-medium text-on-surface focus:border-primary focus:ring-1 focus:ring-primary/10 transition-all outline-none min-w-[280px] h-11"
            />
          </div>

          <button 
            onClick={fetchCustomers}
            disabled={loading}
            className="w-11 h-11 flex items-center justify-center border border-outline-variant/20 text-on-surface-variant hover:text-primary hover:bg-surface-container-low transition-all disabled:opacity-50"
            title="Refresh list"
          >
            <span className={`material-symbols-outlined text-[20px] ${loading ? 'animate-spin' : ''}`}>refresh</span>
          </button>

          <button 
            onClick={handleCreate}
            className="flex items-center gap-2 px-6 h-11 btn-gradient rounded-none font-bold text-xs uppercase tracking-widest whitespace-nowrap"
          >
            <span className="material-symbols-outlined text-base">person_add</span>
            New Customer
          </button>
        </div>
      </div>

      <div className="bg-surface-container-lowest rounded-none ambient-shadow border border-outline-variant/10 overflow-hidden">
        <DataTable 
          data={filteredCustomers} 
          columns={columns} 
          defaultPageSize={10} 
          emptyMessage={loading ? "Loading customers..." : "No records found."}
        />
      </div>

      {/* Edit/Create Modal */}
      {isModalOpen && selectedCustomer && (
        <div className="fixed inset-0 z-[60] flex items-center justify-center p-4">
          <div className="absolute inset-0 bg-surface-container-highest/60 backdrop-blur-sm" onClick={() => setIsModalOpen(false)}></div>
          <div className="bg-surface relative z-10 w-full max-w-2xl rounded-none ambient-shadow border border-outline-variant/10 overflow-hidden animate-in fade-in zoom-in duration-200">
            <div className="px-8 py-6 border-b border-outline-variant/10 flex justify-between items-center bg-surface-container-low/30">
              <div>
                <h3 className="text-xl font-headline font-bold text-on-surface tracking-tight">
                  {selectedCustomer.Id || selectedCustomer.id ? 'Edit Customer Profile' : 'Register New Customer'}
                </h3>
                {(selectedCustomer.Id || selectedCustomer.id) && (
                  <p className="text-[10px] font-bold text-primary uppercase tracking-widest mt-1">Ref: {selectedCustomer.Id || selectedCustomer.id}</p>
                )}
              </div>
              <button onClick={() => setIsModalOpen(false)} className="text-on-surface-variant hover:text-error transition-colors">
                <span className="material-symbols-outlined">close</span>
              </button>
            </div>
            
            <div className="p-8 max-h-[70vh] overflow-y-auto custom-scrollbar">
              <div className="grid grid-cols-2 gap-x-8 gap-y-6">
                {/* Basic Info */}
                <div className="col-span-2 border-b border-outline-variant/5 pb-2 mb-2">
                  <h4 className="text-[11px] font-bold text-primary uppercase tracking-widest">Primary Identity</h4>
                </div>
                
                <div className="col-span-1">
                  <label className="text-[10px] font-bold text-on-surface-variant uppercase tracking-widest mb-2 block">Customer Name</label>
                  <input
                    type="text"
                    defaultValue={selectedCustomer.CustomerName || selectedCustomer.customerName}
                    className="w-full bg-surface-container-lowest border border-outline-variant/20 rounded-none px-4 py-2.5 text-sm font-medium text-on-surface focus:border-primary transition-all outline-none"
                  />
                </div>
                
                <div className="col-span-1">
                  <label className="text-[10px] font-bold text-on-surface-variant uppercase tracking-widest mb-2 block">Phone Number</label>
                  <input
                    type="text"
                    defaultValue={selectedCustomer.PhoneNo || selectedCustomer.phoneNo}
                    className="w-full bg-surface-container-lowest border border-outline-variant/20 rounded-none px-4 py-2.5 text-sm font-medium text-on-surface focus:border-primary transition-all outline-none"
                  />
                </div>

                <div className="col-span-2">
                  <label className="text-[10px] font-bold text-on-surface-variant uppercase tracking-widest mb-2 block">Permanent Address</label>
                  <textarea
                    rows={2}
                    defaultValue={selectedCustomer.Address || selectedCustomer.address}
                    className="w-full bg-surface-container-lowest border border-outline-variant/20 rounded-none px-4 py-2.5 text-sm font-medium text-on-surface focus:border-primary transition-all outline-none"
                  />
                </div>

                <div className="col-span-2">
                  <label className="text-[10px] font-bold text-on-surface-variant uppercase tracking-widest mb-2 block">Shipping Address</label>
                  <textarea
                    rows={2}
                    defaultValue={selectedCustomer.ShippingAddress || selectedCustomer.shippingAddress}
                    className="w-full bg-surface-container-lowest border border-outline-variant/20 rounded-none px-4 py-2.5 text-sm font-medium text-on-surface focus:border-primary transition-all outline-none"
                  />
                </div>

                {/* Tax / Registry Info */}
                <div className="col-span-2 border-b border-outline-variant/5 pb-2 mt-4 mb-2">
                  <h4 className="text-[11px] font-bold text-primary uppercase tracking-widest">Registry & Compliance</h4>
                </div>

                <div className="col-span-1">
                  <label className="text-[10px] font-bold text-on-surface-variant uppercase tracking-widest mb-2 block">GSTIN Number</label>
                  <input
                    type="text"
                    defaultValue={selectedCustomer.GSTINNumber || selectedCustomer.gstinNumber}
                    className="w-full bg-surface-container-lowest border border-outline-variant/20 rounded-none px-4 py-2.5 text-sm font-medium text-on-surface focus:border-primary transition-all outline-none"
                  />
                </div>

                <div className="col-span-1">
                  <label className="text-[10px] font-bold text-on-surface-variant uppercase tracking-widest mb-2 block">State / Region</label>
                  <input
                    type="text"
                    defaultValue={selectedCustomer.State || selectedCustomer.state}
                    className="w-full bg-surface-container-lowest border border-outline-variant/20 rounded-none px-4 py-2.5 text-sm font-medium text-on-surface focus:border-primary transition-all outline-none"
                  />
                </div>

                <div className="col-span-1 flex items-center gap-4 pt-4">
                  <label className="flex items-center gap-3 cursor-pointer group">
                    <input 
                      type="checkbox" 
                      defaultChecked={selectedCustomer.IsActive ?? selectedCustomer.isActive}
                      className="w-4 h-4 rounded-none border-outline-variant accent-primary"
                    />
                    <span className="text-[10px] font-bold text-on-surface-variant uppercase tracking-widest">Account Active</span>
                  </label>
                </div>
              </div>
            </div>

            <div className="p-8 border-t border-outline-variant/10 bg-surface-container-low/30 flex gap-3">
              <button
                onClick={() => setIsModalOpen(false)}
                className="flex-1 px-6 py-3 border border-outline-variant/20 text-on-surface rounded-none font-bold text-xs uppercase tracking-widest hover:bg-surface-container-low transition-all"
              >
                Cancel
              </button>
              <button
                onClick={() => setIsModalOpen(false)}
                className="flex-1 px-6 py-3 btn-gradient rounded-none font-bold text-xs uppercase tracking-widest shadow-lg shadow-primary/10"
              >
                Update Profile
              </button>
            </div>
          </div>
        </div>
      )}
    </section>
  );
}
