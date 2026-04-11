using System.Collections.Generic;

namespace MintCart.Common
{
    public class FilterConfigInfo
    {
        public string Filter { get; set; }
    }

    public class ReportFilters
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        /// <summary>
        /// List of <see cref="FilterInfo"/> model.
        /// </summary>
        public List<FilterInfos> Filters { get; set; }

        /// <summary>
        /// List of <see cref="ColumnInfo"/> model.
        /// </summary>
        public List<ColumnInfos> Columns { get; set; }
        public List<DataConverterInputs> DataConverterList { get; set; }

        public string EntityName { get; set; }

        public List<ReportFilters> SubReports { get; set; }
    }

    public class FilterInfos
    {
        public string FieldName { get; set; }
        public string Operator { get; set; }
        public string Value { get; set; }
    }

    public class ColumnInfos
    {
        public string Field { get; set; }
    }

    public class DataConverterInputs
    {
        public string ConverterName { get; set; }
        public string Format { get; set; }
        public string Column { get; set; }
    }
}
