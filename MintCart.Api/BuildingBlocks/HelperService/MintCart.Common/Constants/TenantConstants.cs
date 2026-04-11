using System.Collections.Generic;

namespace MintCart.Common.Constants
{
    public class TenantConstants
    {
        public const string System = "MintCart system";
        public const string AppUrl = "www.future-hubs.com";
    }

    public class LanguageTextKeys
    {

        public const string ReportPrintedByTextHeader = "GetReportPrintedByTextHeader";
        public const string ReportPageTextHeader = "GetReportPageTextHeader";
        public const string System = "GetMintCartSystem";
        public const string AppUrl = "GetMintCartUrl";
        public const string ReportCRNumberTextHeader = "GetReportCRNumberTextHeader";
        public const string ReportDateTextHeader = "GetReportDateTextHeader";
        public const string ReportOfTextHeader = "GetReportOfTextHeader";
        public const string ReportStartsFromTextHeader = "GetReportStartsFromTextHeader";
        public const string ReportToTextHeader = "GetReportToTextHeader";
    }

    public static class LanguageKeysHashSet
    {
        public static readonly HashSet<string> ReportKeys = new HashSet<string>
    {
        LanguageTextKeys.ReportPrintedByTextHeader,
        LanguageTextKeys.System,
        LanguageTextKeys.AppUrl,
        LanguageTextKeys.ReportCRNumberTextHeader,
        LanguageTextKeys.ReportOfTextHeader,
        LanguageTextKeys.ReportDateTextHeader,
        LanguageTextKeys.ReportStartsFromTextHeader,
        LanguageTextKeys.ReportToTextHeader,
        LanguageTextKeys.ReportPageTextHeader
    };
    }
}
