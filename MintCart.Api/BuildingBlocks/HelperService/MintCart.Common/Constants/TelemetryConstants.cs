using System;
using System.Collections.Generic;
using System.Text;

namespace MintCart.Common.Constants
{
    public class TelemetryConstants
    {
        public const string ChannelKey = "TelemetryRawDataChannel";
        public const string PTS = "PTS";
        public const string RawDataFileType = "application/json";
        public const string TelemetryErrorResponseFolder = "ErrorResponse";

        public const int OnDemandRequestRetryMaxCount = 5;
        public const int OnDemandRequestExpiryHours = 2;

        public const int ProbeLowProductLevel = 5;

        public const string InvalidTelemetryDeviceMessage = "Invalid Telemetry Device Id";
        public const string InvalidTelemetryDeviceDateTimeMessage = "Invalid Telemetry Device DateTime";
    }
}
