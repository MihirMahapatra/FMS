namespace FMS.Utility
{
    public static class MappingLedgers
    {
        static MappingLedgers()
        {
            SundryCreditors = Guid.Parse("d982b189-3326-430d-acde-13c12bba7992");
            SundryDebtors = Guid.Parse("fbf4a6c7-c33d-4ad0-b7a5-abb319cc1b93");
            LabourCharges = Guid.Parse("7F740148-ED36-48AD-B194-031BC717842C");
            LabourAccount = Guid.Parse("F07A3165-D63B-4DAE-A820-EC79D83363B1");
            SalesAccount = Guid.Parse("1ecff7d8-702b-4dcd-93c5-b95a67e36fc9");
            PurchaseAccount = Guid.Parse("75e1fe3d-047d-41ad-a138-f0bb5bbc8b1f");
            PurchaseReturnAccount = Guid.Parse("712D600B-DFD6-4704-9E32-317FE62499A9");
            SalesReturnAccount = Guid.Parse("80025398-C02F-4A1A-9DB7-8A21F9EFD9EF");
            CashAccount = Guid.Parse("701C663E-DAC3-4A39-8D2A-36EB68426B54");
            BankAccount = Guid.Parse("9BFA6931-977F-4A3D-A582-DA5F1F4AB773");
            TransportingChargeRecive = Guid.Parse("9EFD7830-125A-40E3-8F44-68AB03F52591");
            TransportingChargePayment = Guid.Parse("D281CBFB-3CAC-4C6A-8CE1-7B51973B8CA4");
        }
        public static Guid SundryCreditors { get; private set; }
        public static Guid SundryDebtors { get; private set; }
        public static Guid LabourCharges { get; private set; }
        public static Guid LabourAccount { get; private set; }
        public static Guid SalesAccount { get; private set; }
        public static Guid PurchaseAccount { get; private set; }
        public static Guid PurchaseReturnAccount { get; private set; }
        public static Guid SalesReturnAccount { get; private set; }
        public static Guid CashAccount { get; private set; }
        public static Guid BankAccount { get; private set; }
        public static Guid TransportingChargeRecive { get; private set; }
        public static Guid TransportingChargePayment { get; private set; }
    }
}