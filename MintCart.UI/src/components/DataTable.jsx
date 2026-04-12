import { useState } from 'react';

/**
 * A reusable data table component designed for the MintCart enterprise aesthetic.
 *
 * @param {Array}    data            - The full JSON dataset (array of objects).
 * @param {Array}    columns         - Column config: { header, key, render, className, headerClassName, align }
 * @param {number}   defaultPageSize - Default rows per page (default: 10). Set to 0 to disable pagination.
 * @param {string}   className       - Additional table container styling.
 * @param {boolean}  hoverEffect     - Whether to show hover highlights on rows.
 * @param {string}   emptyMessage    - Message shown when data is empty.
 * @param {boolean}  selectable      - Enable row click-to-select highlighting (default: true).
 * @param {Function} onRowSelect     - Callback fired with the selected row item (or null on deselect).
 */
export default function DataTable({
  data = [],
  columns = [],
  defaultPageSize = 10,
  pageSizeOptions = [10, 20, 50],
  className = '',
  hoverEffect = true,
  emptyMessage = 'No records found.',
  selectable = true,
  onRowSelect = null,
}) {
  const [currentPage, setCurrentPage] = useState(1);
  const [pageSize, setPageSize] = useState(defaultPageSize);
  const [selectedRowId, setSelectedRowId] = useState(null);

  const handleRowClick = (item, rowIndex) => {
    if (!selectable) return;
    const id = item.id ?? rowIndex;
    const next = selectedRowId === id ? null : id;
    setSelectedRowId(next);
    onRowSelect?.(next !== null ? item : null);
  };

  const paginate = pageSize > 0;
  const totalPages = paginate ? Math.ceil(data.length / pageSize) : 1;
  const startIndex = paginate ? (currentPage - 1) * pageSize : 0;
  const visibleData = paginate ? data.slice(startIndex, startIndex + pageSize) : data;

  const goToPage = (page) => setCurrentPage(Math.min(Math.max(page, 1), totalPages));

  const handlePageSizeChange = (e) => {
    setPageSize(Number(e.target.value));
    setCurrentPage(1);
  };

  return (
    <div className={className}>
      {/* Table */}
      <div className="overflow-x-auto">
        <table className="w-full text-left border-collapse">
          <thead className="bg-primary/20 text-primary sticky top-0 z-10">
            <tr>
              {columns.map((col, index) => (
                <th
                  key={index}
                  className={`px-6 py-4 text-xs font-bold uppercase tracking-[0.2em] border-r border-primary/20 last:border-r-0 ${col.headerClassName || ''} ${
                    col.align === 'center' ? 'text-center' : col.align === 'right' ? 'text-right' : ''
                  }`}
                >
                  {col.header}
                </th>
              ))}
            </tr>
          </thead>
          <tbody className="divide-y divide-outline-variant/10 text-on-surface">
            {visibleData.length > 0 ? (
              visibleData.map((item, rowIndex) => {
                const rowId = item.id ?? rowIndex;
                const isSelected = selectable && selectedRowId === rowId;
                return (
                  <tr
                    key={rowId}
                    onClick={() => handleRowClick(item, rowIndex)}
                    className={[
                      selectable ? 'cursor-pointer' : '',
                      isSelected
                        ? 'bg-primary/10 border-l-2 border-l-primary'
                        : hoverEffect
                        ? 'hover:bg-primary/5 border-l-2 border-l-transparent'
                        : 'border-l-2 border-l-transparent',
                      'transition-all duration-150 group',
                    ].join(' ')}
                  >
                    {columns.map((col, colIndex) => (
                      <td
                        key={colIndex}
                        className={`px-6 py-2 text-sm font-medium border-r border-outline-variant/10 last:border-r-0 ${col.className || ''} ${
                          col.align === 'center' ? 'text-center' : col.align === 'right' ? 'text-right' : ''
                        }`}
                      >
                        {col.render ? col.render(item) : item[col.key]}
                      </td>
                    ))}
                  </tr>
                );
              })
            ) : (
              <tr>
                <td
                  colSpan={columns.length}
                  className="px-6 py-12 text-center text-sm font-medium text-on-surface-variant italic"
                >
                  {emptyMessage}
                </td>
              </tr>
            )}
          </tbody>
        </table>
      </div>

      {/* Pagination */}
      {paginate && (
        <div className="flex items-center justify-between border-t border-outline-variant/10 px-6 py-4">
          {/* Record count — left */}
          <span className="text-[11px] font-bold text-on-surface-variant tracking-wide">
            Showing {data.length === 0 ? 0 : startIndex + 1}–{Math.min(startIndex + pageSize, data.length)} of {data.length}
          </span>

          {/* Page nav + controls — right */}
          <div className="flex items-center gap-6">
            {/* Go to Page — only when multiple pages */}
            {totalPages > 1 && (
              <div className="flex items-center gap-2">
                <span className="text-[11px] font-bold text-on-surface-variant tracking-wide whitespace-nowrap">Go to page</span>
                <select
                  value={currentPage}
                  onChange={(e) => goToPage(Number(e.target.value))}
                  className="bg-surface-container-lowest border border-outline-variant/20 rounded-none px-2 py-1 text-[11px] font-bold text-on-surface-variant focus:border-primary focus:outline-none transition-all appearance-none cursor-pointer hover:bg-surface-container-low min-w-[50px] text-center"
                >
                  {[...Array(totalPages)].map((_, i) => (
                    <option key={i + 1} value={i + 1}>{i + 1}</option>
                  ))}
                </select>
              </div>
            )}

            {/* Page buttons — max 3 visible */}
            {totalPages > 1 && (
              <div className="flex gap-1.5 items-center border-l border-outline-variant/15 pl-6">
                <button
                  disabled={currentPage === 1}
                  onClick={() => goToPage(currentPage - 1)}
                  className="px-4 py-1.5 rounded-none border border-outline-variant/20 text-xs font-bold text-on-surface-variant hover:bg-surface-container-low transition-all disabled:opacity-30 disabled:pointer-events-none"
                >
                  Previous
                </button>
                
                {(() => {
                  const maxVisible = 3;
                  let start = Math.max(1, currentPage - Math.floor(maxVisible / 2));
                  let end = Math.min(totalPages, start + maxVisible - 1);
                  if (end - start + 1 < maxVisible) start = Math.max(1, end - maxVisible + 1);
                  
                  return [...Array(end - start + 1)].map((_, i) => {
                    const pageNum = start + i;
                    return (
                      <button
                        key={pageNum}
                        onClick={() => goToPage(pageNum)}
                        className={`w-8 h-8 flex items-center justify-center rounded-none font-bold text-[10px] transition-all ${
                          currentPage === pageNum
                            ? 'bg-primary text-on-primary shadow-sm'
                            : 'hover:bg-surface-container-low text-on-surface-variant'
                        }`}
                      >
                        {pageNum}
                      </button>
                    );
                  });
                })()}

                <button
                  disabled={currentPage === totalPages}
                  onClick={() => goToPage(currentPage + 1)}
                  className="px-4 py-1.5 rounded-none border border-outline-variant/20 text-xs font-bold text-on-surface-variant hover:bg-surface-container-low transition-all disabled:opacity-30 disabled:pointer-events-none"
                >
                  Next
                </button>
              </div>
            )}

            {/* Items-per-page selector */}
            <div className="flex items-center gap-2 border-l border-outline-variant/15 pl-6">
              <span className="text-[11px] font-bold text-on-surface-variant tracking-wide whitespace-nowrap">Rows per page</span>
              <select
                value={pageSize}
                onChange={handlePageSizeChange}
                className="bg-surface-container-lowest border border-outline-variant/20 rounded-none px-2 py-1 text-[11px] font-bold text-on-surface-variant focus:border-primary focus:outline-none transition-all appearance-none cursor-pointer hover:bg-surface-container-low"
              >
                {pageSizeOptions.map((opt) => (
                  <option key={opt} value={opt}>{opt}</option>
                ))}
              </select>
            </div>
          </div>
        </div>
      )}

    </div>
  );
}

