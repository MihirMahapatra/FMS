namespace FMS.Utility
{
    public static class MappingProductType
    {
        static MappingProductType()
        {
            RawMaterial = Guid.Parse("B524F4A7-1BB2-4347-84AE-E0DA56EB4A31");
            FinishedGood = Guid.Parse("A4AB180B-ACC7-44CE-AEF7-C588D41EDD5C");
        }
        public static Guid RawMaterial { get; private set; }
        public static Guid FinishedGood { get; private set; }

    }
}
