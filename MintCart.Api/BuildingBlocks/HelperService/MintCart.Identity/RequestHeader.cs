using Microsoft.AspNetCore.Http;

namespace MintCart.Identity
{
    public class RequestHeader
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RequestHeader(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public const string PTSID_KEY = "X-Pts-Id";
        public const string PTSFIRMWAREDATETIME_KEY = "X-Pts-Firmware-Version-DateTime";
        public const string PTSDATASIGNATURE_KEY = "X-Data-Signature";
        public const string LANGUAGE_KEY = "Languagekey";
        private readonly string defaultLanguage = "en";

        #region public
        /// <summary>
        /// Get Enrollment Id from Header
        /// </summary>
        public string Language
        {
            get
            {
                var language = GetHeaderValue(LANGUAGE_KEY);
                return string.IsNullOrEmpty(language) ? defaultLanguage : language;
            }
        }

        /// <summary>
        /// Get PTS Id from Header
        /// </summary>
        public string PtsID
        {
            get
            {
                var ptsId = GetHeaderValue(PTSID_KEY);
                return string.IsNullOrEmpty(ptsId) ? string.Empty : ptsId;
            }
        }

        /// <summary>
        /// Get PTS Firmware date/time from Header
        /// </summary>
        public string PtsFirmWareDateTime
        {
            get
            {
                var ptsFirmWareDateTime = GetHeaderValue(PTSFIRMWAREDATETIME_KEY);
                return string.IsNullOrEmpty(ptsFirmWareDateTime) ? string.Empty : ptsFirmWareDateTime;
            }
        }

        /// <summary>
        /// Get PTS Data Signature from Header
        /// </summary>
        public string PtsDataSignature
        {
            get
            {
                var ptsDataSignature = GetHeaderValue(PTSDATASIGNATURE_KEY);
                return string.IsNullOrEmpty(ptsDataSignature) ? string.Empty : ptsDataSignature;
            }
        }

        #endregion

        #region Private 
        private string GetHeaderValue(string key)
        {
            var request = _httpContextAccessor.HttpContext.Request;

            string value = request.Headers.ContainsKey(key) ? request.Headers[key].ToString() : string.Empty;

            return value;
        }
        #endregion
    }
}

