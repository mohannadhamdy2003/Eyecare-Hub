namespace EyeCareHub.API.Dtos.Product
{
    public class UpdateProductDto
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

        public int ProductBrandId { get; set; }
        public int ProductTypeId { get; set; }
        public int ProductCategoryId { get; set; }
    }
}
