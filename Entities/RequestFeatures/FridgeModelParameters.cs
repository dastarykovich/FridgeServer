namespace Entities.RequestFeatures
{
    public class FridgeModelParameters : RequestParameters
    {
        public FridgeModelParameters()
        {
            OrderBy = "name";
        }

        public uint MinYear { get; set; }

        public uint MaxYear { get; set; } = int.MaxValue;

        public bool ValideYearRange => MaxYear > MinYear;

        public string? SearchTerm { get; set; }
    }
}
