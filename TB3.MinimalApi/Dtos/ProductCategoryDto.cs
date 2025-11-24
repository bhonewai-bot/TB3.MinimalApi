namespace TB3.MinimalApi.Dtos; 

public class ProductCategoryResponseDto
{
    public int ProductCategoryId { get; set; }

    public string ProductCategoryCode { get; set; }

    public string ProductCategoryName { get; set; }
}

public class ProductCategoryCreateRequestDto
{
    public string ProductCategoryCode { get; set; }

    public string ProductCategoryName { get; set; }
}