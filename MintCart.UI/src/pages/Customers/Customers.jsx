import { useState, useEffect, useRef } from 'react';
import DataTable from '../../components/DataTable';
import ConfirmDialog from '../../components/ConfirmDialog';
import { getCustomers, getCustomersPaged, upsertCustomer, mergeCustomers } from '../../api/customers';
import { useUI } from '../../context/UIContext';

export default function Customers() {
  const { showToast } = useUI();
  const [customers, setCustomers] = useState([]);
  const [totalCount, setTotalCount] = useState(0);
  const [page, setPage] = useState(1);
  const [pageSize, setPageSize] = useState(10);
  const [searchTerm, setSearchTerm] = useState('');
  const [activeSearch, setActiveSearch] = useState('');
  const [hasSearched, setHasSearched] = useState(false);
  const [searchNonce, setSearchNonce] = useState(0);
  const [loading, setLoading] = useState(false);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedCustomer, setSelectedCustomer] = useState(null);
  const [selectedForMerge, setSelectedForMerge] = useState([]);
  const [isMergeModalOpen, setIsMergeModalOpen] = useState(false);
  const [showMergeConfirm, setShowMergeConfirm] = useState(false);

  const toggleForMerge = (customer) => {
    const cid = customer.Id || customer.id;
    setSelectedForMerge(prev =>
      prev.find(c => (c.Id || c.id) === cid)
        ? prev.filter(c => (c.Id || c.id) !== cid)
        : [...prev, customer]
    );
  };

  const handleMergeSubmit = () => {
    setShowMergeConfirm(true);
  };

  const performMerge = async () => {
    setShowMergeConfirm(false);
    try {
      setLoading(true);

const customerIds = selectedForMerge.map(c => c.Id || c.id);
      const formData = mergeFormRef.current ? new FormData(mergeFormRef.current) : null;
      const customerDetails = {
        CustomerName: formData?.get('CustomerName') || mergeEditCustomer?.CustomerName || mergeEditCustomer?.customerName || '',
        PhoneNo: formData?.get('PhoneNo') || mergeEditCustomer?.PhoneNo || mergeEditCustomer?.phoneNo || '',
        Address: formData?.get('Address') || mergeEditCustomer?.Address || mergeEditCustomer?.address || '',
        ShippingAddress: formData?.get('ShippingAddress') || mergeEditCustomer?.ShippingAddress || mergeEditCustomer?.shippingAddress || '',
        GSTINNumber: formData?.get('GSTINNumber') || mergeEditCustomer?.GSTINNumber || mergeEditCustomer?.gstinNumber || '',
        State: formData?.get('State') || mergeEditCustomer?.State || mergeEditCustomer?.state || '',
        IsActive: formData ? formData.get('IsActive') === 'on' : (mergeEditCustomer?.IsActive ?? true),
      };
      await mergeCustomers(customerIds, customerDetails);

      showToast(`${selectedForMerge.length} Profiles merged successfully!`);
      setSelectedForMerge([]);
      setMergeEditCustomer(null);
      setIsMergeModalOpen(false);
      setPage(1);
      if (hasSearched) fetchCustomers(1, pageSize, activeSearch);
    } catch (error) {
      console.error('Merge failed', error);
      alert('Failed to merge contacts.');
    } finally {
      setLoading(false);
    }
  };
  const fetchCustomers = async (p = page, ps = pageSize, s = activeSearch) => {
    try {
      setLoading(true);
      const response = await getCustomersPaged(p, ps, s);
      const result = response?.Data || response?.data || response;
      setCustomers(result?.Items || result?.items || []);
      setTotalCount(result?.TotalCount ?? result?.totalCount ?? 0);
    } catch (error) {
      console.error('Failed to fetch customers', error);
    } finally {
      setLoading(false);
    }
  };

  const handleSearch = () => {
    setHasSearched(true);
    setActiveSearch(searchTerm);
    setPage(1);
    setSearchNonce(n => n + 1);
  };

  // Fetch on search (button click) and on pagination changes after the first search
  useEffect(() => {
    if (hasSearched) {
      fetchCustomers(page, pageSize, activeSearch);
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [page, pageSize, activeSearch, hasSearched, searchNonce]);

  const handleCreate = () => {
    setSelectedCustomer({}); // Empty object for new customer
    setIsModalOpen(true);
  };

  const handleEdit = (customer) => {
    setSelectedCustomer(customer);
    setIsModalOpen(true);
  };


  const [saving, setSaving] = useState(false);
  const [errors, setErrors] = useState({});
  const [mergeEditCustomer, setMergeEditCustomer] = useState(null);
  const [mergeErrors, setMergeErrors] = useState({});
  const mergeFormRef = useRef(null);
  const [popupEditCustomer, setPopupEditCustomer] = useState(null);

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
      if (hasSearched) fetchCustomers(page, pageSize, activeSearch);
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
      render: (item, index) => index + 1
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
              onKeyDown={(e) => { if (e.key === 'Enter') handleSearch(); }}
              className="pl-10 pr-4 py-2 bg-surface-container-lowest border border-outline-variant/20 rounded-none text-[13px] font-medium text-on-surface focus:border-primary focus:ring-1 focus:ring-primary/10 transition-all outline-none min-w-[280px] h-11"
            />
          </div>

          <button
            onClick={handleSearch}
            disabled={loading}
            className="flex items-center gap-2 px-6 h-11 border border-outline-variant/20 text-on-surface-variant hover:text-primary hover:bg-surface-container-low transition-all rounded-none font-bold text-xs uppercase tracking-widest disabled:opacity-50"
            title="Search customers"
          >
            <span className="material-symbols-outlined text-base">search</span>
            Search
          </button>

          <button
            onClick={() => { if (hasSearched) fetchCustomers(page, pageSize, activeSearch); }}
            disabled={loading || !hasSearched}
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
              {selectedForMerge.length > 1 && (
                <button
                  onClick={() => { setMergeErrors({}); setMergeEditCustomer(selectedForMerge[0] || null); setIsMergeModalOpen(true); }}
                  className="flex items-center gap-2 px-6 h-11 border-2 border-primary text-primary hover:bg-primary/5 transition-all rounded-none font-bold text-xs uppercase tracking-widest whitespace-nowrap"
                >
                  <span className="material-symbols-outlined text-base">merge_type</span>
                  Merge Profiles ({selectedForMerge.length})
                </button>
              )}
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
          data={customers}
          columns={columns}
          defaultPageSize={10}
          pageSizeOptions={[10, 20, 50, 100]}
          emptyMessage={loading ? "Loading customers..." : (hasSearched ? "No records found." : "Click Search to load customer records.")}
          serverSide={true}
          totalCount={totalCount}
          page={page}
          onPageChange={setPage}
          onPageSizeChange={(newSize) => { setPageSize(newSize); setPage(1); }}
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
          <div className="absolute inset-0 bg-surface-container-highest/60 backdrop-blur-sm" onClick={() => { setIsMergeModalOpen(false); setMergeEditCustomer(null); }}></div>
          <div className={`bg-surface relative z-10 w-full rounded-none ambient-shadow border border-outline-variant/10 overflow-hidden animate-in fade-in zoom-in duration-200 flex flex-col transition-all ${mergeEditCustomer ? 'max-w-5xl' : 'max-w-xl'}`}>

            {/* Header */}
            <div className="px-8 py-6 border-b border-outline-variant/10 flex justify-between items-center bg-surface-container-low/30 shrink-0">
              <div>
                <h3 className="text-xl font-headline font-bold text-on-surface tracking-tight">Merge Customer Profiles</h3>
                <p className="text-[10px] font-bold text-on-surface-variant uppercase tracking-widest mt-1">Reviewing {selectedForMerge.length} candidates</p>
              </div>
              <button onClick={() => { setIsMergeModalOpen(false); setMergeEditCustomer(null); }} className="text-on-surface-variant hover:text-error transition-colors">
                <span className="material-symbols-outlined">close</span>
              </button>
            </div>

            {/* Body — two-panel when editing */}
            <div className="flex flex-1 min-h-0 overflow-hidden">

              {/* Left panel — customer list */}
              <div className={`flex flex-col ${mergeEditCustomer ? 'w-96 shrink-0 border-r border-outline-variant/10' : 'flex-1'}`}>
                <div className="flex-1 overflow-y-auto custom-scrollbar p-6 space-y-2">
                  {selectedForMerge.map((contact, index) => (
                    <div
                      key={contact.Id || contact.id}
                      className="flex items-center justify-between p-3 border border-outline-variant/10 bg-surface-container-lowest shadow-sm relative group transition-all hover:border-outline-variant/30"
                    >
                      <div className="flex items-center gap-3 min-w-0">
                        <div className="w-7 h-7 shrink-0 flex items-center justify-center bg-primary/10 text-primary font-bold text-xs">
                          {index + 1}
                        </div>
                        <div className="min-w-0">
                          <p className="text-sm font-bold text-on-surface leading-tight">{contact.CustomerName || contact.customerName}</p>
                          <p className="text-[11px] text-on-surface-variant font-medium mt-0.5">{contact.PhoneNo || contact.phoneNo}</p>
                        </div>
                      </div>
                      <div className="flex items-center gap-0.5 shrink-0 ml-2 opacity-0 group-hover:opacity-100 transition-all">
                        <button
                          onClick={() => setPopupEditCustomer(contact)}
                          className="text-on-surface-variant hover:text-primary p-1 transition-colors"
                          title="Edit customer details"
                        >
                          <span className="material-symbols-outlined text-base">edit</span>
                        </button>
                        <button
                          onClick={() => toggleForMerge(contact)}
                          className="text-on-surface-variant hover:text-error p-1 transition-colors"
                          title="Remove from merge"
                        >
                          <span className="material-symbols-outlined text-base">delete</span>
                        </button>
                      </div>
                    </div>
                  ))}
                </div>

              </div>

              {/* Right panel — edit form */}
              {mergeEditCustomer && (
                <form key={mergeEditCustomer.Id || mergeEditCustomer.id} ref={mergeFormRef} onSubmit={(e) => e.preventDefault()} className="flex-1 flex flex-col min-h-0 overflow-hidden">
                  <div className="flex-1 overflow-y-auto custom-scrollbar p-6">
                    <div className="grid grid-cols-2 gap-x-6 gap-y-5">
                      <div className="col-span-1">
                        <label className="text-[10px] font-bold text-on-surface-variant uppercase tracking-widest mb-2 block">Customer Name</label>
                        <input
                          name="CustomerName"
                          type="text"
                          disabled={loading}
                          onFocus={() => setMergeErrors({ ...mergeErrors, CustomerName: null })}
                          defaultValue={mergeEditCustomer.CustomerName || mergeEditCustomer.customerName}
                          className={`w-full bg-surface-container-lowest border ${mergeErrors.CustomerName ? 'border-error' : 'border-outline-variant/20'} rounded-none px-4 py-2.5 text-sm font-medium text-on-surface focus:border-primary transition-all outline-none disabled:opacity-50`}
                        />
                        {mergeErrors.CustomerName && <p className="text-[10px] font-bold text-error uppercase tracking-widest mt-1.5">{mergeErrors.CustomerName}</p>}
                      </div>

                      <div className="col-span-1">
                        <label className="text-[10px] font-bold text-on-surface-variant uppercase tracking-widest mb-2 block">Phone Number</label>
                        <input
                          name="PhoneNo"
                          type="tel"
                          disabled={loading}
                          onFocus={() => setMergeErrors({ ...mergeErrors, PhoneNo: null })}
                          onInput={(e) => { e.target.value = e.target.value.replace(/\D/g, '').slice(0, 10); }}
                          defaultValue={mergeEditCustomer.PhoneNo || mergeEditCustomer.phoneNo}
                          className={`w-full bg-surface-container-lowest border ${mergeErrors.PhoneNo ? 'border-error' : 'border-outline-variant/20'} rounded-none px-4 py-2.5 text-sm font-medium text-on-surface focus:border-primary transition-all outline-none disabled:opacity-50`}
                          placeholder="Enter 10 digit number"
                        />
                        {mergeErrors.PhoneNo && <p className="text-[10px] font-bold text-error uppercase tracking-widest mt-1.5">{mergeErrors.PhoneNo}</p>}
                      </div>

                      <div className="col-span-2">
                        <label className="text-[10px] font-bold text-on-surface-variant uppercase tracking-widest mb-2 block">Permanent Address</label>
                        <textarea
                          name="Address"
                          rows={2}
                          disabled={loading}
                          defaultValue={mergeEditCustomer.Address || mergeEditCustomer.address}
                          className="w-full bg-surface-container-lowest border border-outline-variant/20 rounded-none px-4 py-2.5 text-sm font-medium text-on-surface focus:border-primary transition-all outline-none disabled:opacity-50"
                        />
                      </div>

                      <div className="col-span-2">
                        <label className="text-[10px] font-bold text-on-surface-variant uppercase tracking-widest mb-2 block">Shipping Address</label>
                        <textarea
                          name="ShippingAddress"
                          rows={2}
                          disabled={loading}
                          defaultValue={mergeEditCustomer.ShippingAddress || mergeEditCustomer.shippingAddress}
                          className="w-full bg-surface-container-lowest border border-outline-variant/20 rounded-none px-4 py-2.5 text-sm font-medium text-on-surface focus:border-primary transition-all outline-none disabled:opacity-50"
                        />
                      </div>

                      <div className="col-span-1">
                        <label className="text-[10px] font-bold text-on-surface-variant uppercase tracking-widest mb-2 block">GSTIN Number</label>
                        <input
                          name="GSTINNumber"
                          type="text"
                          disabled={loading}
                          defaultValue={mergeEditCustomer.GSTINNumber || mergeEditCustomer.gstinNumber}
                          className="w-full bg-surface-container-lowest border border-outline-variant/20 rounded-none px-4 py-2.5 text-sm font-medium text-on-surface focus:border-primary transition-all outline-none disabled:opacity-50"
                        />
                      </div>

                      <div className="col-span-1">
                        <label className="text-[10px] font-bold text-on-surface-variant uppercase tracking-widest mb-2 block">State / Region</label>
                        <input
                          name="State"
                          type="text"
                          disabled={loading}
                          defaultValue={mergeEditCustomer.State || mergeEditCustomer.state}
                          className="w-full bg-surface-container-lowest border border-outline-variant/20 rounded-none px-4 py-2.5 text-sm font-medium text-on-surface focus:border-primary transition-all outline-none disabled:opacity-50"
                        />
                      </div>

                      <div className="col-span-1">
                        <label className="flex items-center gap-3 cursor-pointer">
                          <input
                            name="IsActive"
                            type="checkbox"
                            disabled={loading}
                            defaultChecked={mergeEditCustomer.IsActive ?? mergeEditCustomer.isActive}
                            className="w-4 h-4 rounded-none border-outline-variant accent-primary disabled:opacity-50"
                          />
                          <span className="text-[10px] font-bold text-on-surface-variant uppercase tracking-widest">Account Active</span>
                        </label>
                      </div>
                    </div>
                  </div>

                </form>
              )}
            </div>

            {/* Footer */}
            <div className="px-8 py-5 border-t border-outline-variant/10 bg-surface-container-low/30 flex gap-3 shrink-0">
              <button
                onClick={() => { setIsMergeModalOpen(false); setMergeEditCustomer(null); }}
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

      {/* Merge Confirmation Warning */}
      <ConfirmDialog
        open={showMergeConfirm}
        variant="danger"
        title="Permanently Delete & Merge?"
        message={
          <>
            This will permanently <span className="font-bold text-error">delete {selectedForMerge.length - 1} customer record{selectedForMerge.length - 1 === 1 ? '' : 's'}</span> and combine all their sales, complaints, and recharge history into a single primary customer profile.
          </>
        }
        note="This action cannot be undone"
        confirmLabel="Delete & Merge"
        confirmIcon="delete_forever"
        loading={loading}
        onConfirm={performMerge}
        onCancel={() => setShowMergeConfirm(false)}
      />

      {/* Per-customer Edit Popup (within merge flow) */}
      {popupEditCustomer && (
        <div className="fixed inset-0 z-[70] flex items-center justify-center p-4">
          <div className="absolute inset-0 bg-surface-container-highest/60 backdrop-blur-sm" onClick={() => setPopupEditCustomer(null)}></div>
          <div className="bg-surface relative z-10 w-full max-w-2xl rounded-none ambient-shadow border border-outline-variant/10 overflow-hidden animate-in fade-in zoom-in duration-200">
            <div>
              <div className="px-8 py-6 border-b border-outline-variant/10 flex justify-between items-center bg-surface-container-low/30">
                <div>
                  <h3 className="text-xl font-headline font-bold text-on-surface tracking-tight">Edit Customer Profile</h3>
                  <p className="text-[10px] font-bold text-primary uppercase tracking-widest mt-1">Ref: {popupEditCustomer.Id || popupEditCustomer.id}</p>
                </div>
                <button type="button" onClick={() => setPopupEditCustomer(null)} className="text-on-surface-variant hover:text-error transition-colors">
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
                      readOnly
                      defaultValue={popupEditCustomer.CustomerName || popupEditCustomer.customerName}
                      className="w-full bg-surface-container-lowest border border-outline-variant/20 rounded-none px-4 py-2.5 text-sm font-medium text-on-surface outline-none opacity-70"
                    />
                  </div>

                  <div className="col-span-1">
                    <label className="text-[10px] font-bold text-on-surface-variant uppercase tracking-widest mb-2 block">Phone Number</label>
                    <input
                      name="PhoneNo"
                      type="tel"
                      readOnly
                      defaultValue={popupEditCustomer.PhoneNo || popupEditCustomer.phoneNo}
                      className="w-full bg-surface-container-lowest border border-outline-variant/20 rounded-none px-4 py-2.5 text-sm font-medium text-on-surface outline-none opacity-70"
                    />
                  </div>

                  <div className="col-span-2">
                    <label className="text-[10px] font-bold text-on-surface-variant uppercase tracking-widest mb-2 block">Permanent Address</label>
                    <textarea name="Address" rows={2} readOnly defaultValue={popupEditCustomer.Address || popupEditCustomer.address}
                      className="w-full bg-surface-container-lowest border border-outline-variant/20 rounded-none px-4 py-2.5 text-sm font-medium text-on-surface outline-none opacity-70" />
                  </div>

                  <div className="col-span-2">
                    <label className="text-[10px] font-bold text-on-surface-variant uppercase tracking-widest mb-2 block">Shipping Address</label>
                    <textarea name="ShippingAddress" rows={2} readOnly defaultValue={popupEditCustomer.ShippingAddress || popupEditCustomer.shippingAddress}
                      className="w-full bg-surface-container-lowest border border-outline-variant/20 rounded-none px-4 py-2.5 text-sm font-medium text-on-surface outline-none opacity-70" />
                  </div>

                  <div className="col-span-1">
                    <label className="text-[10px] font-bold text-on-surface-variant uppercase tracking-widest mb-2 block">GSTIN Number</label>
                    <input name="GSTINNumber" type="text" readOnly defaultValue={popupEditCustomer.GSTINNumber || popupEditCustomer.gstinNumber}
                      className="w-full bg-surface-container-lowest border border-outline-variant/20 rounded-none px-4 py-2.5 text-sm font-medium text-on-surface outline-none opacity-70" />
                  </div>

                  <div className="col-span-1">
                    <label className="text-[10px] font-bold text-on-surface-variant uppercase tracking-widest mb-2 block">State / Region</label>
                    <input name="State" type="text" readOnly defaultValue={popupEditCustomer.State || popupEditCustomer.state}
                      className="w-full bg-surface-container-lowest border border-outline-variant/20 rounded-none px-4 py-2.5 text-sm font-medium text-on-surface outline-none opacity-70" />
                  </div>

                  <div className="col-span-1 flex items-center gap-4">
                    <label className="flex items-center gap-3 cursor-pointer">
                      <input name="IsActive" type="checkbox" disabled
                        defaultChecked={popupEditCustomer.IsActive ?? popupEditCustomer.isActive}
                        className="w-4 h-4 rounded-none border-outline-variant accent-primary opacity-70" />
                      <span className="text-[10px] font-bold text-on-surface-variant uppercase tracking-widest">Account Active</span>
                    </label>
                  </div>
                </div>
              </div>

              <div className="p-8 border-t border-outline-variant/10 bg-surface-container-low/30 flex gap-3">
                <button type="button" onClick={() => setPopupEditCustomer(null)}
                  className="flex-1 px-6 py-3 border border-outline-variant/20 text-on-surface rounded-none font-bold text-xs uppercase tracking-widest hover:bg-surface-container-low transition-all">
                  Cancel
                </button>
              </div>
            </div>
          </div>
        </div>
      )}

    </section>
  );
}
