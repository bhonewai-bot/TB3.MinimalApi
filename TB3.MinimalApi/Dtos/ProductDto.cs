namespace TB3.MinimalApi.Dtos;

public class ProductDto
{
    public class ProductResponseDto
    {
        public int ProductId { get; set; }
        
        public string ProductCode { get; set; }

        public string ProductName { get; set; }
        
        public decimal Price { get; set; }

        public int Quantity { get; set; }
        
        public string ProductCategoryCode { get; set; }

        public DateTime? CreatedDateTime { get; set; }

        public DateTime? ModifiedDateTime { get; set; }
    }

    public class ProductCreateRequestDto
    {
        public string ProductCode { get; set; }
        
        public string ProductName { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
        
        public string ProductCategoryCode { get; set; }
    }
    
    public class ProductUpdateRequestDto
    {
        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
        
        public string ProductCategoryCode { get; set; }
    }
    
    public class ProductPatchRequestDto
    {
        public string? ProductCode { get; set; }
        
        public string? ProductName { get; set; }

        public int? Quantity { get; set; }

        public decimal? Price { get; set; }
        
        public string? ProductCategoryCode { get; set; }
    }
}