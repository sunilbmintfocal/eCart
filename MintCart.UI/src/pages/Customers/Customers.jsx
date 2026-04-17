import { useState, useEffect } from 'react';
import DataTable from '../../components/DataTable';
import { getCustomers, upsertCustomer } from '../../api/customers';
import { useUI } from '../../context/UIContext';

export default function Customers() {
  const { showToast } = useUI();
  const [customers, setCustomers] = useState([]);
  const [searchTerm, setSearchTerm] = useState('');
  const [loading, setLoading] = useState(true);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedCustomer, setSelectedCustomer] = useState(null);
  const [selectedForMerge, setSelectedForMerge] = useState([]);
  const [isMergeModalOpen, setIsMergeModalOpen] = useState(false);

  const toggleForMerge = (customer) => {
    const cid = customer.Id || customer.id;
    setSelectedForMerge(prev =>
      prev.find(c => (c.Id || c.id) === cid)
        ? prev.filter(c => (c.Id || c.id) !== cid)
        : [...prev, customer]
    );
  };

  const handleMergeSubmit = async () => {
    try {
      setLoading(true);
      // Dummy API Call Simulation
      await new Promise(resolve => setTimeout(resolve, 1500));

      showToast(`${selectedForMerge.length} Profiles merged successfully!`);
      setSelectedForMerge([]);
      setIsMergeModalOpen(false);
      fetchCustomers();
    } catch (error) {
      console.error('Merge failed', error);
      alert('Failed to merge contacts.');
    } finally {
      setLoading(false);
    }
  };
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

  const handleEdit = (customer) => {
    setSelectedCustomer(customer);
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

  const [saving, setSaving] = useState(false);
  const [errors, setErrors] = useState({});

  const validateForm = (data) => {
    const newErrors = {};
    if (!data.CustomerName) newErrors.CustomerName = 'Customer Name is required';
    else if (data.CustomerName.length < 2) newErrors.CustomerName = 'Name is too short';

    if (!data.PhoneNo) newErrors.PhoneNo = 'Phone Number is required';
    else if (!/^\d{10}$/.test(data.PhoneNo)) newErrors.PhoneNo = 'Must be exactly 10 digits';

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSave = async (e) => {
    e.preventDefault();
    const formData = new FormData(e.target);
    const customerData = {
      Id: selectedCustomer.Id || selectedCustomer.id || 0,
      CustomerName: formData.get('CustomerName').trim(),
      PhoneNo: formData.get('PhoneNo').trim(),
      Address: formData.get('Address'),
      ShippingAddress: formData.get('ShippingAddress'),
      GSTINNumber: formData.get('GSTINNumber'),
      State: formData.get('State'),
      IsActive: formData.get('IsActive') === 'on',
      AddedDate: selectedCustomer.AddedDate || selectedCustomer.addedDate || null
    };

    if (!validateForm(customerData)) return;

    try {
      setSaving(true);
      await upsertCustomer(customerData);

      setIsModalOpen(false);
      showToast('Customer saved successfully!');
      fetchCustomers();
    } catch (error) {
      console.error('Failed to save customer', error);
      alert('Error saving customer. Please try again.');
    } finally {
      setSaving(false);
    }
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
      header: 'Registration Date',
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
      render: (customer) => {
        const isSelected = selectedForMerge.find(c => (c.Id || c.id) === (customer.Id || customer.id));
        return (
          <div className="flex items-center justify-end gap-1">
            <button
              onClick={() => toggleForMerge(customer)}
              className={`w-10 h-10 flex items-center justify-center rounded-none transition-all active:scale-95 group ${isSelected ? 'bg-primary text-white' : 'hover:bg-primary/10 text-primary'}`}
              title={isSelected ? "Remove from merge" : "Add for merging"}
            >
              <span className={`material-symbols-outlined text-[20px] ${isSelected ? '' : 'group-hover:scale-110'} transition-transform`}>
                {isSelected ? 'check_circle' : 'merge_type'}
              </span>
            </button>
            <button
              onClick={() => handleEdit(customer)}
              className="w-10 h-10 flex items-center justify-center rounded-none hover:bg-primary/10 text-primary transition-all active:scale-95 group"
              title="Edit Customer"
            >
              <span className="material-symbols-outlined text-[20px] group-hover:scale-110 transition-transform">edit_calendar</span>
            </button>
          </div>
        );
      }
    }
  ];

  return (
    <section className="p-8 bg-surface min-h-screen">
      <div className="flex justify-between items-center mb-10">
        <div>
          <h1 className="text-2xl font-headline font-bold text-on-surface tracking-tight">Customer Management</h1>
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
          </button>          {selectedForMerge.length > 0 && (
            <div className="flex items-center gap-2 animate-in fade-in slide-in-from-right-4">
              <button
                onClick={() => setSelectedForMerge([])}
                className="flex items-center gap-2 px-4 h-11 border border-outline-variant/30 text-on-surface-variant hover:text-error hover:border-error/30 transition-all rounded-none font-bold text-[10px] uppercase tracking-widest"
                title="Clear all selections"
              >
                <span className="material-symbols-outlined text-base">backspace</span>
                Clear
              </button>
              <button
                onClick={() => setIsMergeModalOpen(true)}
                className="flex items-center gap-2 px-6 h-11 border-2 border-primary text-primary hover:bg-primary/5 transition-all rounded-none font-bold text-xs uppercase tracking-widest whitespace-nowrap"
              >
                <span className="material-symbols-outlined text-base">merge_type</span>
                Merge Profiles ({selectedForMerge.length})
              </button>
            </div>
          )}

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
            <form onSubmit={handleSave}>
              <div className="px-8 py-6 border-b border-outline-variant/10 flex justify-between items-center bg-surface-container-low/30">
                <div>
                  <h3 className="text-xl font-headline font-bold text-on-surface tracking-tight">
                    {selectedCustomer.Id || selectedCustomer.id ? 'Edit Customer Profile' : 'Register New Customer'}
                  </h3>
                  {(selectedCustomer.Id || selectedCustomer.id) && (
                    <p className="text-[10px] font-bold text-primary uppercase tracking-widest mt-1">Ref: {selectedCustomer.Id || selectedCustomer.id}</p>
                  )}
                </div>
                <button type="button" onClick={() => setIsModalOpen(false)} className="text-on-surface-variant hover:text-error transition-colors">
                  <span className="material-symbols-outlined">close</span>
                </button>
              </div>

              <div className="p-8 max-h-[70vh] overflow-y-auto custom-scrollbar">
                <div className="grid grid-cols-2 gap-x-8 gap-y-6 pt-2">
                  <div className="col-span-1">
                    <label className="text-[10px] font-bold text-on-surface-variant uppercase tracking-widest mb-2 block">Customer Name</label>
                    <input
                      name="CustomerName"
                      type="text"
                      disabled={saving}
                      onFocus={() => setErrors({ ...errors, CustomerName: null })}
                      defaultValue={selectedCustomer.CustomerName || selectedCustomer.customerName}
                      className={`w-full bg-surface-container-lowest border ${errors.CustomerName ? 'border-error' : 'border-outline-variant/20'} rounded-none px-4 py-2.5 text-sm font-medium text-on-surface focus:border-primary transition-all outline-none disabled:opacity-50`}
                    />
                    {errors.CustomerName && (
                      <p className="text-[10px] font-bold text-error uppercase tracking-widest mt-1.5 animate-in fade-in slide-in-from-top-1">{errors.CustomerName}</p>
                    )}
                  </div>

                  <div className="col-span-1">
                    <label className="text-[10px] font-bold text-on-surface-variant uppercase tracking-widest mb-2 block">Phone Number</label>
                    <input
                      name="PhoneNo"
                      type="tel"
                      disabled={saving}
                      onFocus={() => setErrors({ ...errors, PhoneNo: null })}
                      onInput={(e) => {
                        e.target.value = e.target.value.replace(/\D/g, '').slice(0, 10);
                      }}
                      defaultValue={selectedCustomer.PhoneNo || selectedCustomer.phoneNo}
                      className={`w-full bg-surface-container-lowest border ${errors.PhoneNo ? 'border-error' : 'border-outline-variant/20'} rounded-none px-4 py-2.5 text-sm font-medium text-on-surface focus:border-primary transition-all outline-none disabled:opacity-50`}
                      placeholder="Enter 10 digit number"
                    />
                    {errors.PhoneNo && (
                      <p className="text-[10px] font-bold text-error uppercase tracking-widest mt-1.5 animate-in fade-in slide-in-from-top-1">{errors.PhoneNo}</p>
                    )}
                  </div>

                  <div className="col-span-2">
                    <label className="text-[10px] font-bold text-on-surface-variant uppercase tracking-widest mb-2 block">Permanent Address</label>
                    <textarea
                      name="Address"
                      rows={2}
                      disabled={saving}
                      defaultValue={selectedCustomer.Address || selectedCustomer.address}
                      className="w-full bg-surface-container-lowest border border-outline-variant/20 rounded-none px-4 py-2.5 text-sm font-medium text-on-surface focus:border-primary transition-all outline-none disabled:opacity-50"
                    />
                  </div>

                  <div className="col-span-2">
                    <label className="text-[10px] font-bold text-on-surface-variant uppercase tracking-widest mb-2 block">Shipping Address</label>
                    <textarea
                      name="ShippingAddress"
                      rows={2}
                      disabled={saving}
                      defaultValue={selectedCustomer.ShippingAddress || selectedCustomer.shippingAddress}
                      className="w-full bg-surface-container-lowest border border-outline-variant/20 rounded-none px-4 py-2.5 text-sm font-medium text-on-surface focus:border-primary transition-all outline-none disabled:opacity-50"
                    />
                  </div>

                  <div className="col-span-1">
                    <label className="text-[10px] font-bold text-on-surface-variant uppercase tracking-widest mb-2 block">GSTIN Number</label>
                    <input
                      name="GSTINNumber"
                      type="text"
                      disabled={saving}
                      defaultValue={selectedCustomer.GSTINNumber || selectedCustomer.gstinNumber}
                      className="w-full bg-surface-container-lowest border border-outline-variant/20 rounded-none px-4 py-2.5 text-sm font-medium text-on-surface focus:border-primary transition-all outline-none disabled:opacity-50"
                    />
                  </div>

                  <div className="col-span-1">
                    <label className="text-[10px] font-bold text-on-surface-variant uppercase tracking-widest mb-2 block">State / Region</label>
                    <input
                      name="State"
                      type="text"
                      disabled={saving}
                      defaultValue={selectedCustomer.State || selectedCustomer.state}
                      className="w-full bg-surface-container-lowest border border-outline-variant/20 rounded-none px-4 py-2.5 text-sm font-medium text-on-surface focus:border-primary transition-all outline-none disabled:opacity-50"
                    />
                  </div>

                  <div className="col-span-1 flex items-center gap-4">
                    <label className="flex items-center gap-3 cursor-pointer group">
                      <input
                        name="IsActive"
                        type="checkbox"
                        disabled={saving}
                        defaultChecked={selectedCustomer.IsActive ?? selectedCustomer.isActive}
                        className="w-4 h-4 rounded-none border-outline-variant accent-primary disabled:opacity-50"
                      />
                      <span className="text-[10px] font-bold text-on-surface-variant uppercase tracking-widest">Account Active</span>
                    </label>
                  </div>
                </div>
              </div>

              <div className="p-8 border-t border-outline-variant/10 bg-surface-container-low/30 flex gap-3">
                <button
                  type="button"
                  disabled={saving}
                  onClick={() => setIsModalOpen(false)}
                  className="flex-1 px-6 py-3 border border-outline-variant/20 text-on-surface rounded-none font-bold text-xs uppercase tracking-widest hover:bg-surface-container-low transition-all disabled:opacity-50"
                >
                  Cancel
                </button>
                <button
                  type="submit"
                  disabled={saving}
                  className="flex-1 px-6 py-3 btn-gradient rounded-none font-bold text-xs uppercase tracking-widest shadow-lg shadow-primary/10 flex items-center justify-center gap-2 disabled:opacity-80"
                >
                  {saving ? (
                    <>
                      <span className="material-symbols-outlined text-base animate-spin">sync</span>
                      Saving...
                    </>
                  ) : (
                    selectedCustomer.Id || selectedCustomer.id ? 'Save Changes' : 'Create Customer'
                  )}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}

      {/* Merge Contact Modal */}
      {isMergeModalOpen && (
        <div className="fixed inset-0 z-[60] flex items-center justify-center p-4">
          <div className="absolute inset-0 bg-surface-container-highest/60 backdrop-blur-sm" onClick={() => setIsMergeModalOpen(false)}></div>
          <div className="bg-surface relative z-10 w-full max-w-xl rounded-none ambient-shadow border border-outline-variant/10 overflow-hidden animate-in fade-in zoom-in duration-200">
            <div className="px-8 py-6 border-b border-outline-variant/10 flex justify-between items-center bg-surface-container-low/30">
              <div>
                <h3 className="text-xl font-headline font-bold text-on-surface tracking-tight">Merge Customer Profiles</h3>
                <p className="text-[10px] font-bold text-on-surface-variant uppercase tracking-widest mt-1">Reviewing {selectedForMerge.length} candidates</p>
              </div>
              <button onClick={() => setIsMergeModalOpen(false)} className="text-on-surface-variant hover:text-error transition-colors">
                <span className="material-symbols-outlined">close</span>
              </button>
            </div>

            <div className="p-8">
              <div className="space-y-4 max-h-[40vh] overflow-y-auto custom-scrollbar pr-2">
                {selectedForMerge.map((contact, index) => (
                  <div key={contact.Id || contact.id} className="flex items-center justify-between p-4 bg-surface-container-lowest border border-outline-variant/10 shadow-sm relative group">
                    <div className="flex items-center gap-4">
                      <div className="w-8 h-8 flex items-center justify-center bg-primary/10 text-primary font-bold text-xs">
                        {index + 1}
                      </div>
                      <div>
                        <p className="text-sm font-bold text-on-surface leading-tight">{contact.CustomerName || contact.customerName}</p>
                        <p className="text-[11px] text-on-surface-variant font-medium mt-0.5 tracking-tight">{contact.PhoneNo || contact.phoneNo}</p>
                      </div>
                    </div>
                    <button
                      onClick={() => toggleForMerge(contact)}
                      className="text-on-surface-variant hover:text-error opacity-0 group-hover:opacity-100 transition-all p-1"
                    >
                      <span className="material-symbols-outlined text-base">delete</span>
                    </button>
                  </div>
                ))}
              </div>

              <div className="mt-8 p-4 bg-primary/5 border border-primary/10 flex items-start gap-3">
                <span className="material-symbols-outlined text-primary text-sm mt-0.5">info</span>
                <p className="text-[11px] text-on-surface-variant leading-relaxed">
                  Merging these profiles will combine order history and activity logs into a single master identity. This action <span className="text-primary font-bold">cannot be undone</span>.
                </p>
              </div>
            </div>

            <div className="p-8 border-t border-outline-variant/10 bg-surface-container-low/30 flex gap-3">
              <button
                onClick={() => setIsMergeModalOpen(false)}
                className="flex-1 px-6 py-3 border border-outline-variant/20 text-on-surface rounded-none font-bold text-xs uppercase tracking-widest hover:bg-surface-container-low transition-all"
              >
                Cancel
              </button>
              <button
                onClick={handleMergeSubmit}
                disabled={loading || selectedForMerge.length < 2}
                className="flex-1 px-6 py-3 btn-gradient rounded-none font-bold text-xs uppercase tracking-widest shadow-lg shadow-primary/10 flex items-center justify-center gap-2 disabled:opacity-50"
              >
                {loading ? (
                  <>
                    <span className="material-symbols-outlined text-base animate-spin">sync</span>
                    Processing...
                  </>
                ) : (
                  'Confirm & Merge'
                )}
              </button>
            </div>
          </div>
        </div>
      )}

    </section>
  );
}
