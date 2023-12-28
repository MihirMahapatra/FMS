namespace FMS.Utility
{
    public static class MappingLabourType
    {
        static MappingLabourType()
        {
            Production = Guid.Parse("5E514855-55A0-459C-8B8B-DEF7696D9AD0");
            Service = Guid.Parse("6C2758A2-79B5-43A6-8851-C6F975433B0F");
        }
        public static Guid Production { get; set; }
        public static Guid Service { get; set; }
    }
}
