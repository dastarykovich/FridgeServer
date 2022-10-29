namespace Entities.DataTransferObjects
{
    public class FridgeModelDto
    {
        public Guid Id { get; set; }

        public Guid FridgeId { get; set; }

        public string? Name { get; set; }

        public int Year { get; set; }
    }
}
