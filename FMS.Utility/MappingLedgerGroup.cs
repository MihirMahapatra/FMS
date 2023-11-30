namespace FMS.Utility
{
    public static class MappingLedgerGroup
    {

        static MappingLedgerGroup()
        {

            Purchase = Guid.Parse("4458bce5-4546-4120-a7de-03acefd07b85");
            UnsecuredLoan = Guid.Parse("39b5514a-9359-46f3-8c3e-0eabd6880cf6");
            Depreciation = Guid.Parse("c3c725d0-a502-4275-b0f9-1585ab6edcc7");
            CashBankBalance = Guid.Parse("f3eef2dd-09bb-4e21-b036-1e5bba920efe");
            IndirectExpenses = Guid.Parse("58639324-2180-4e0b-932d-33024aa3fc5f");
            LiabilityforExpenses = Guid.Parse("23d58228-db08-4c3d-9177-343b1cfdcf7a");
            FixedAssets = Guid.Parse("4d80e68f-ff00-486a-945a-3941761441d9");
            DirectIncome = Guid.Parse("ea3f21ad-b0d4-4c27-9f9d-3c36a7a585c2");
            Sales = Guid.Parse("15fe2512-d922-45c5-9e03-64c32b903a5b");
            CurrentliabilitiesAndProvisions = Guid.Parse("aca9caf1-ea9b-4602-bb60-6c354eac5ce6");
            CapitalACount = Guid.Parse("9bbc2c1f-ffa4-44b8-9916-6edf0a47d8db");
            OpeningStock = Guid.Parse("bdcf2ee2-8aab-44f6-bd1e-77b53074389a");
            CurrentAssets = Guid.Parse("2fc89e45-7365-46b7-933c-9abae2e5967a");
            DutiesAndTaxes = Guid.Parse("68eeffe7-02f7-4ffc-81b3-aeb0cadc764b");
            IndirectIncome = Guid.Parse("345b0d2a-8fca-414f-a6f2-c5f7fd9246ac");
            DirectExpenses = Guid.Parse("01548ef6-3fe2-4c0f-9a5f-ceed35066136");
            SecuredLoan = Guid.Parse("84a336c6-e48a-43e8-984e-f45b0bf2984f");
        }

        public static Guid Purchase { get; private set; }
        public static Guid UnsecuredLoan { get; private set; }
        public static Guid Depreciation { get; private set; }
        public static Guid CashBankBalance { get; private set; }
        public static Guid IndirectExpenses { get; private set; }
        public static Guid LiabilityforExpenses { get; private set; }
        public static Guid FixedAssets { get; private set; }
        public static Guid DirectIncome { get; private set; }
        public static Guid Sales { get; private set; }
        public static Guid CurrentliabilitiesAndProvisions { get; private set; }
        public static Guid CapitalACount { get; private set; }
        public static Guid OpeningStock { get; private set; }
        public static Guid CurrentAssets { get; private set; }
        public static Guid DutiesAndTaxes { get; private set; }
        public static Guid IndirectIncome { get; private set; }
        public static Guid DirectExpenses { get; private set; }
        public static Guid SecuredLoan { get; private set; }


    }
}
