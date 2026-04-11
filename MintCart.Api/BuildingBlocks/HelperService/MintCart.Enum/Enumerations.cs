using System.ComponentModel;

namespace MintCart.Enum
{
    public enum Roles
    {
        SuperAdmin,
        Admin,
        Operator
    }

    public enum RolesHierarchy
    {
        SuperAdmin = 1,
        Admin = 2,
        Operator = 3
    }

    public enum LMWindABIdentityClaims
    {
        UserId,
        UserName,
        EmailAddress,
        EnrollmentId,
        UserRoles,
        FirstName,
        LastName,
        UserProfileId,
        Permission,
        //Role
    }
    public enum RegistrationStatus
    {
        Pending,
        Approved,
        Rejected
    }
    public enum Permissions
    {
        Permission1,
        Permission2,
        Permission3,
        Permission4,
        TenantManagement
    }

    public enum RegistrationStep
    {
        Step1 = 1,
        Step2 = 2,
        Step3 = 3,
        Step4 = 4,
        Completed = 10
    }

    public enum Dropdowns
    {
        ServiceRequestType,
        ServiceRequestSubType,
        Location,
        StationType,
        FuelType,
        EventType,
        MeasurementType,
        MeasurementReason,
        InspectionType,
        OwnershipType,
        City,
        Region,
        District,
        ItemType,
        CoreItemType,
        ItemBrand,
        NoteType,
        CheckListType,
        BuildingType,
        ArabicModels,
        DispenserBrand,
        StationBusinessType,
        ExtinguisherType,
        ExtinguisherModel,
        ExtinguisherBrand,
        ExtinguisherSize,
        ExtinguisherTankModel,
        Gender,
        CRNumber,
        Station,
        Nozzle,
        Dispenser,
        Port,
        MainOffice,
        BillType,
        MeansOfPaymentType,
        TransactionType,
        MaintenanceUserGroup,
        MaintenanceUserType,
        Privilages,
        SubscriptionPlan,
        ExtinguisherTankSize
    }


    public enum Dashboard
    {
        TotalSaleMade,
        TotalCashSale,
        TotalPOSSale,
        TotalFuelAvailable,
        TotalFuelSold,
        FuelrequestData,
        CashCollection,
        Ordered,
        Sold,
        RequestReceived,
        CollectedAmount,
        DepositedAmount,
        Delivered,
        SalesByRegion,
        SalesByCity,
        TopStationSales,
        SalesByPayment,
        SumOfSales,
        Cash,
        POS,
        StationSales,
        SumOfSalesByPayment,
        SalesByFuelType,
        EventByTypeBarChart,
        EventByTypePieChart,
        TheftEvents,
        NonTheftEvents,
        TheftByStation,
        ServiceCompletion,
        ExpiredDocumentsByMonth,
        N,
        Y,
        StationTotalSalesAmount,
        StationNameAndAmount,
        FuelDeliveryByType,
        FuelDeliveryByDateAndType,
        FuelDeliveryForLineChart
    }

    public enum LanguageType
    {
        [Description("English")]
        en,

        [Description("Arabic")]
        ar,
    }

    public enum FuelTransactionType
    {
        Refuel,
        Sale,
        Calibration,
        [Description("Tank Status")]
        TankStatus,
        Testing,
        TheftOfFuelFromNozzle
    }

    public enum TelemetryJsonResponsePacketType
    {
        UploadAlertRecord,
        UploadTankMeasurement,
        UploadStatus,
        UploadPumpTransaction,
        DateTime,
        ProbeTankVolumeForHeight,
        SdInformation,
        TanksConfiguration
    }

    public enum TelemetryJsonRequestPacketType
    {
        GetSdInformation,
        ProbeGetTankVolumeForHeight,
        GetDateTime,
        GetTanksConfiguration
    }

    public enum TelemetryOnDemandPublishRequestStatus
    {
        Pending = 1,
        Completed = 2
    }

    public enum TelemetryOnDemandRequestStatus
    {
        Pending = 0,
        InProgress = 1,
        Completed = 2,
        Failed = 3
    }

    public enum TelemetryOnDemandRequestPriorityLevel
    {
        Low = 0,
        Medium = 1,
        High = 2,
        Critical = 3
    }

    public enum MapMasterDataList
    {
        MapStationId = 0,
        MapTankId = 1,
        MapDispenserId = 2,
        MapNozzleId = 3,
        MapUserId = 4,
        MapFuelGradeId = 5,
        MapAlertDeviceTypeId = 6,
        MapAlertCode = 7
    }

    public enum RawDataValidationList
    {
        ValidateDate = 0
    }

    public enum ErrorType
    {
        StationMap = 0,
        TankMap = 1,
        DispenserMap = 2,
        NozzleMap = 3,
        UserMap = 4,
        FuelGradeIDMap = 5,
        InvalidServerDate = 6,
        InvalidDevice = 7,
        AlertDeviceTypeMap = 8,
        AlertCodeMap = 9
    }

    public enum AutomatedFuelRequestSteps
    {
        [Description("Validate")]
        Validate = 1,

        [Description("Request Tank Configuration")]
        RequestTankConfiguration = 2,

        [Description("Receive Tank Configuration")]
        ReceivedTankConfiguration = 3,

        [Description("Request Tank Volume")]
        RequestTankVolume = 4,

        [Description("Receive Tank Volume")]
        ReceivedTankVolume = 5,

        [Description("Validate Again")]
        ValidateAgain = 6,

        [Description("Validate Fuel Request Quantity")]
        ValidateFuelRequestQuantity = 7,

        [Description("Fuel Request Created")]
        FuelRequestCreated = 8,

    }

    public enum FuelResponseStatus
    {
        Completed,
        Error,
        [Description("Validation Failed")]
        ValidationFailed
    }
    public enum StationType
    {
        Automatic
    }

    public enum OnDemandRequestActions
    {
        FuelRequest
    }

    public enum AutomatedFuelRequestType
    {
        [Description("Telemetry Alert")]
        TelemetryAlert = 1,
        [Description("System Set Minimum Fuel Level")]
        SystemSetMinimumFuelLevel = 2
    }

    public enum FuelRequestStatus
    {
        Pending,
        Completed,
        [Description("Yet to Approve")]
        YetToApprove
    }

    public enum AutomatedFuelRequestStatus
    {
        Pending = 1,
        Completed = 2
    }
    public enum AutomatedFuelRequestDetailsStatus
    {
        Completed = 1,
        Failed = 2
    }


    public enum ServiceRequestType
    {
        [Description("CWG (Civil group)")]
        CWG,
        [Description("EWG (Electric group)")]
        EWG
    }

    public enum ServiceRequestResponseRate
    {
        Urgent,
        Normal
    }

    public enum ServiceRequestRequestedTo
    {
        External,
        Internal
    }

    public enum BuildingType
    {
        Main
    }

    public enum TankMeasurementAlarms
    {
        CriticalHighProduct = 0,
        HighProduct = 1,
        LowProduct = 2,
        CriticalLowProduct = 3,
        HighWater = 4,
        TankLeakage = 5
    }

    public enum Status
    {
        OK = 0,
        Error = 1
    }

    public enum TankMeasurementAlarmIcons
    {
        None,
        Error,
        CrticialHigh,
        High,
        Low,
        CriticalLow
    }

    public enum TankLevelStatus
    {
        Green,
        Yellow,
        Red
    }
    public enum TelemetryDeviceConnectionEvent
    {
        Connected = 1,
        Disconnected = 2
    }

    public enum TelemetryDataReprocessingStatus
    {
        Successful = 1,
        Failed = 2
    }

    public enum DashboardCategories
    {
        [Description("Sales")]
        Sales = 1,
        [Description("Events")]
        Events = 2
    }


}
