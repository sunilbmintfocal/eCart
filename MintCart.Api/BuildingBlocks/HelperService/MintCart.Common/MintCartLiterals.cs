using System.Diagnostics;
using System.Threading;

namespace MintCart.Common
{
	public class ResponseMessage
    {
        public const string Success = "Request successful.";
        public const string NotFound = "Request not found. The specified uri does not exist.";
        public const string BadRequest = "Request invalid.";
        public const string MethodNotAllowed = "Request responded with 'Method Not Allowed'.";
        public const string NotContent = "Request no content. The specified uri does not contain any content.";
        public const string Exception = "Request responded with exceptions.";
        public const string UnAuthorized = "Request denied. Unauthorized access.";
        public const string Unlicensed = "Request denied. The transaction is a Licensed Transaction and License is not enabled for this Transaction.";
        public const string BlankLoginToken = "Request denied. The login Token passed is Blank. You need to login and get a valid token.";
        public const string InvalidLoginToken = "Request denied. The login Token passed does not exist in License Manager. You need to login and get a valid token.";
        public const string ValidationError = "Request responded with one or more validation errors.";
        public const string Unknown = "Request cannot be processed. Please contact support.";
        public const string Unhandled = "Unhandled Exception occurred. Unable to process the request.";
        public const string MediaTypeNotSupported = "Unsupported Media Type.";
        public const string Forbidden = "Request denied.You are not authorized to access this resource.";
        public const string InvalidUser = "UserName or Password is invalid";
        public const string SessionTimeOut = "Session Expired";
        public const string InvalidAccessContactSupport = "Request denied. Please contact support";
    }

    public class ValidatorMessage
    {
        public const string ValidationException = "Petro Hub Validation Exception";
    }

    public class RefreshTokenDtoValidatorMessage
    {
        public const string AccessTokenValidationException = "Invalid Access Token";
        public const string RefreshTokenValidationException = "Invalid Refresh Token";
    }

    public class ModelValidatorMessage
    {
        public const string PhoneNumberValidationException = "Enter valid Phone Number";
        public const string NameValidationException = "Name is required";
        public const string FirstNameValidationException = "First Name is required";
        public const string LastNameValidationException = "Last Name is required";
        public const string EmailValidationException = "Email is required";
        public const string EmailFormatValidationException = "Email format is invalid";
        public const string PhoneValidationException = "Phone is required";
        public const string ContactPersonNameValidationException = "Contact person name is required";
		public const string PhoneFormatValidationException = "Phone no. format is invalid";
		public const string AddressValidationException = "Address is required";
		public const string RoleValidationException = "Role is required";
        public const string RoleTypeValidationException = "Role is Invalid";
        public const string TenantIdValidationException = "Tenant Id is required";
        public const string TenantOtpValidationException = "OTP is required";
        public const string CategoryValidationException = "Category is required";

        //SaaS Service Messages
        public const string CRNumberValidationException = "CR Number is Required";
        public const string RegisteredAddressValidationException = "Registered Address is Required";
        public const string NationalAddressValidationException = "National Short Address is Required";
        public const string AccountOwnerNameValidationException = "Account Owner Name is Required";
        public const string AccountOwnerEmailValidationException = "Account Owner Email is Required";
		public const string CompanyNameValidationException = "Company Name is Required";
		public const string AccountOwnerPhoneNumberValidationException= "Account Owner Phone Number is Required.";
        public const string SupportUserValidationException = "Support User Id is Required";
        public const string RequestValidationException = "Request message is Required";
        public const string ResponseValidationException = "Response message is Required";
        public const string ReportMasterValidationException = "ReportMasterId is Required";

        //Master Service Messages
        public const string BrandValidationException = "Brand Id is required";
        public const string ModelValidationException = "Model Id is required";
        public const string SpeedOfDeliveryValidationException = "Speed Of Delivery is required";
        public const string WarrantyStartDateValidationException = "Warranty Start Date Detail is required";
        public const string WarrantyEndDateValidationException = "Warranty End Date is required";
        public const string WarrantyExpiryDateValidationException = "Warranty Expiry Date is required";
        public const string ZeroValueValidationException = "Zero Value is required";
        public const string NozzleCountValidationException = "Nozzle Count is required";
        public const string CounterMappingValidationException = "Counter Mapping is required";
        public const string StationIdValidationException = "Station Id is required";
        public const string StationNumberValidationException = "Station Number is required";
        public const string LocationValidationException = "Location is required";
        public const string TypeValidationException = "Type is required";
        public const string StationTypeValidationException = "Station Type is required";
        public const string ModelOfTankValidationException = "Model Of Tank is required";
        public const string FuelTypeValidationException = "Fuel Type is required";
        public const string TankSupplierNameValidationException = "Tank Supplier Name is required";
        public const string TankSupplierPhoneValidationException = "Tank Supplier Phone is required";
        public const string TankSupplierEmailValidationException = "Tank Supplier Email is required";
        public const string HoseSpaceFromGroundValidationException = "Hose Space From Ground is required";
        public const string HumidityVolumeValidationException = "Humidity Volume From Ground is required";
        public const string CorrectionValueValidationException = "Correction Value From Ground is required";
		public const string EventTypeIdValidationException = "Event Type Id is required";
		public const string NameOfStationValidationException = "Name of station is required";
		public const string DescriptionValidationException = "Description is required";
		public const string TypeOfNoteValidationException = "Type of note is required";
		public const string AddNoteValidationException = "Add note field is required";
		public const string NozzleNumberValidationException = "Nozzle number is required";
        public const string NozzleIdValidationException = "Nozzle Id is required";
        public const string BrandOfDispenserValidationException = "Brand of dispenser is required";
		public const string QuantityForConsumptionValidationException = "Quantity for consumption is required";
		public const string LastInspectionByOwnerValidationException = "Last inspection by owner is required";
		public const string LastTaqyeesPeriodicInspectionDateValidationException = "Last Taqyees periodic inspection Date is required";
		public const string LastTaqyeesMaintenanceInspectionDateValidationException = "Last Taqyees maintenance inspection Date is required";
        public const string InitialReadingOfTankValidationException = "Initial reading of tank is required";
        public const string FinalReadingOftankValidationException = "Final reading of tank is required";
        public const string ItemDescrValidationException = "Item Description is required";
        public const string ItemExpiryDateValidationException = "Item Expiry Date is required";
        public const string ItemTypeIdValidationException = "Item Type Id is required";
        public const string ContractStartDateValidationException = "Contract Start Date is required";
        public const string ExtinguisherTypeIdValidationException = "Extinguisher Type Id is required";
        public const string ContractEndDateValidationException = "Contract End Date is required";
        public const string FromDateValidationException = "From Date is required";
        public const string ToDateValidationException = "To Date is required";
        public const string RetailPriceValidationException = "Retail Price is required";
        public const string WholesalePriceValidationException = "Wholesale Price is required";
        public const string TenantValidationException = "Tenant is required";
        public const string StationValidationException = "Station is required";
        public const string StationCRDetailValidationException = "Station CR Detail is required";
        public const string FrequencyOfInspectionReminderValidationException = "Frequency of Inspection Reminder is required";
        public const string TaqyeessInspectionDateValidationException = "Last Taqyeess Inspection Date is required";
        public const string CRExpiryValidationException = "CR Expiry Date is required";
        public const string GoogleMapUrlValidationException = "Google Map Url is required";
        public const string StationNameValidationException = "Station Name is required";
        public const string RegionValidationException = "Region is required";
        public const string LocationIdValidationException = "Location Id is required";
        public const string ItemNumberValidationException = "Item Number is required";
        public const string ContentExpiryDateValidationException = "Content Expiry Date is required";
		public const string LocationOfStationValidationException = "Station Location is required";
        public const string NormalRateValidationException = "Normal Rate is required";
        public const string SpecialRateValidationException = "Normal Rate is required";
        public const string StartDateValidationException = "Start Date is required";
        public const string EndDateValidationException = "End Date is required";
        public const string FuelQuotaValidationException = "Fuel Quota is required";
        public const string UsedQuotaValidationException = "Used Quota is required";
        public const string FuelTypeIdValidationException = "Fuel Type Id is required";
        public const string SpecialPriceValidationException = "Special Price is required";
        public const string NormalPriceValidationException = "Normal Price is required";
        public const string PriceValidationException = "Price is required";
        public const string ShiftValidationException = "Shift Id is required";
        public const string LastInspectionDateValidationException = "Last Inspection Date is required";
        public const string AdditionalPhoneNumberValidationException = "Enter valid Additional Phone Number";
		public const string NationalAdditionalPhoneNumberValidationException = "Enter valid  National Additional Phone Number";
        public const string DustVolumeValidationException = "Dust Volume is required";
        public const string UnusableVolumeValidationException = "Unusable Volume is required";
        public const string CurentFuelVolumeValidationException = "Current Fuel Volume is required";
        public const string TotalFuelDispencedVolumeValidationException = "Total Fuel Dispenced Volume is required";
        public const string LastInitialReadingValidationException = "Last Initial Reading is required";
        public const string DispenserNumberValidationException = "Dispenser Number is required";

        //fuelManagementService
        public const string RequestTypeValidationException = "Request Type is required";
        public const string UnitPriceValidationException = "Unit Price is required";
        public const string TotalPriceValidationException = "Total Price is required";
        public const string TankIdValidationException = "Tank Id must be a valid GUID";
        public const string StationIdGuidValidationException = "Station Id must be a valid GUID";
        public const string MeasurementReasonIdValidationException = "Measurement Reason Id must be a valid GUID";
        public const string MeasurementTypeIdValidationException = "Measurement Type Id must be a valid GUID";
        public const string CheckListValidationException = "CheckList Id is required";
        public const string CheckListFieldNameValidationException = "CheckList Field Name is required";
        public const string InspectionValidationException = "Inspection Id is required";

        //Sales Service Messages
        public const string BillTypeValidationException = "Type of Bill is required";
        public const string TransactionTypeValidationException= "Type of Transaction is required";
        public const string BilledToValidationException = "Billed To is required";
        public const string VehicleNumberValidationException = "Vehicle Number is required";
        public const string CounterNumberValidationException = "No. of the Counter is required";
        public const string QuantityValidationException = "Quantity is required";
        public const string MeansOfPaymentValidationException = "Means if Payment is required";
        public const string ItemCountValidationException = "Count of Item is required";
        public const string ModelTypeValidationException = "Model Type is required";
        public const string SizeValidationException = "Extinguisher size is required";
        public const string LastUsageDateValidationException = "Last Usage Date is required";
        public const string BuildingIdException = "Building Id is required";
        public const string ExtinguisherLocationValidationException = "Location of Extinguisher is required";
        public const string SupplierNameValidationException = "Supplier Name is required";
        public const string SupplierIdValidationException = "Supplier Id is required";
        public const string SupplierEmailValidationException = "Supplier Email is required";
        public const string SupplierContactValidationException = "Supplier Phone is required";
        public const string MaintenanceIntervalValidationException = "Maintenance Interval is required";
        public const string MaintenanceCylePeriodValidationException = "Maintenance Cyle Period is required";
        public const string TotalBillWithoutVatValidationException = "Total Bill Without Vat is required";
        public const string TotalBillWithVatValidationException = "Total Bill With Vat is required";
        public const string UnitCostValidationException = "Unit Cost is required";
        public const string TotalCostValidationException = "Total Cost is required";
        public const string TransactionDetailsVatValidationException = "Transaction details are required.";

		//Remainder Service Messages
		public const string IntervalValidationException = "Interval is required";
        public const string ReferenceIdValidationException = "Reference Id is required";
        public const string ModuleValidationException = "Module is required";
        public const string NotificationChannelValidationException = "Notification Channel is required";
        public const string SentOnValidationException = "Sent On is required";
        public const string KeyValidationException = "Key is required";
        public const string IdValidationException = "Identfier is requered";
        public const string URLValidationException = "URL is requered";
        public const string Char25LimitValidationException = "Allowed maximum 25 characters";
        public const string ValueValidationException = "Value is required";

        // Service Request message
        public const string ServiceRequestId = "ServiceRequest Id is required";
        public const string Note = "Note is required";

        //Dashboard
        public const string POSSaleCostValidationException = "POS Sale Cost is required";
        public const string SaleCostValidationException = "Sale Cost is required";
        public const string FuelSaleValidationException = "Fuel Sale is required";

        //TelemetryDevice
        public const string PTSIdValidationException = "PTSId is required";
        public const string StatusValidationException = "Status is required";
        public const string PacketIdValidationException = "Packet Id is required";
        public const string DataPacketTypeValidationException = "Data Packet Type is required";
        public const string ConfigurationIdValidationException = "ConfigurationId is required";

        //FuelType
        public const string FuelTypeUnitValidationException = "Unit is required";
        public const string FuelGradeIDValidationException = "FuelGradeID is required";
    }

    public class Keywords
    {
        public const string CalibrationZeroValue = "SetZeroValue";
        public const string CalibrateReading = "CalibrateReading";
        public const string CalibrateByZero = "CalibrateByZero";
        public const string CalibrateByReading = "CalibrateByReading";
        public const string TransactionRefuel = "Refuel";
        public const string TransactionSale = "Sale";
        public const string TransactionCalibration = "Calibration";
        public const string DailyNozzleFuelOperationStatusCompleted = "Completed";
        public const string DailyNozzleFuelOperationStatusPending = "Pending";
        public const string CalibrationEventDescription = "Calibration";
    }

    public class Identifiers
    {
        public const string TheftFromFuelNozzleId = "h5ed3b1c-2e12-4a5c-850b-4a6341a49e0d";
        public const string TheftOfFuelFromTankId = "e7d8e50e-3c35-48d7-b08d-8e1d51ff9a9d";
        public const string LossOfFuelFromTankId = "d93a8e8f-0f62-4545-82cf-b12f2d03e1ae";
        public const string FuelTypePetrolId = "1a3c5b8e-1cf1-4d5b-b679-6a9d01c1a0fe";
        public const string FuelTypeDieselId = "2b4d6e9f-2cf2-4e6c-a780-7b8e02d2b1df";
        public const string FuelType91Id = "3c5d7f9a-3df3-4f7d-b881-8c9e03e3c2ab";
        public const string FuelType95Id = "4d6e8f0a-4ef4-4e8f-c982-9d0f04f4d3bc";
        public const string FuelTypeBenzeneId = "40497f43-0f97-4104-bad9-8975dd334883";
        public const string EventTypeLossOfFuelFromTank = "d93a8e8f-0f62-4545-82cf-b12f2d03e1ae";
        public const string TheftofCashId = "f63f5f3f-71c7-4d7a-b3b8-13b5e5e5398d";
        public const string CashDifferenceId = "g48f1ff5-28f4-4148-9e97-381b8753d3f3";
        public const string FuelDifferenceId = "75c90618-81b6-4af2-bc84-e21d1fd7d25b";
    }

    public class EventType
    {
		public const string FireIncidentId = "aaac2d48-75e8-4e15-94b8-2b2e951f71fd";
		public const string AccidentId = "b57a78fb-6b9a-4472-8057-3831cc4edee8";
        public const string ViolationId = "c2d4e60d-6169-4923-9ad1-b54e8572fc82";
		public const string LossOfFuelFromTankId = "d93a8e8f-0f62-4545-82cf-b12f2d03e1ae";
		public const string TheftOfFuelFromTankId = "e7d8e50e-3c35-48d7-b08d-8e1d51ff9a9d";
		public const string TheftofCashId = "f63f5f3f-71c7-4d7a-b3b8-13b5e5e5398d";
		public const string CashDifferenceId = "g48f1ff5-28f4-4148-9e97-381b8753d3f3";
		public const string TheftFromFuelNozzleId = "h5ed3b1c-2e12-4a5c-850b-4a6341a49e0d";
	}

    public class InspectionType
    {
        public const string TQPeriodicId = "a5c23d36-93f2-4f20-b6e3-3a4e0f013201";
        public const string TQMaintenanceId = "c14b7ba9-221b-4a18-bf3c-7c9827e3b1de";
        public const string TQSuddenId = "e6bc098e-2228-4619-8e8f-2f4a091e7415";
        public const string InternalMaintenanceId = "0f3e5b05-fc8e-4b12-b9e0-4c05e570fc39";
        public const string InternalInspectionId = "7b792f70-df80-486b-b4de-3b159f4bb495";
    }
}
