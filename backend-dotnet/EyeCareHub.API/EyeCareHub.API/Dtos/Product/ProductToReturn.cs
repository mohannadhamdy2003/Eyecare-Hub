namespace EyeCareHub.API.Dtos.Product
{
    public class ProductToReturn
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PictureUrl { get; set; }

        public int Sales { get; set; } = 0;
        public int MaxQuantity { get; set; }
        public bool TryAR { get; set; }
        public string SideEffect { get; set; }
        public string Disease { get; set; }

        public string Brand { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }

    }
}
