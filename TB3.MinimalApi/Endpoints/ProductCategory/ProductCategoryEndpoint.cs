namespace TB3.MinimalApi.Endpoints.Product;

public static class ProductCategoryEndpoint
{
    public static void UseProductCategoryEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/product-category", () =>
        {
            AppDbContext db = new AppDbContext();
            
            List<ProductCategoryDto.ProductCategoryResponseDto> lts = db.TblProductCategories
                .OrderByDescending(x => x.ProductCategoryId)
                .Select(x => new ProductCategoryDto.ProductCategoryResponseDto()
                {
                    ProductCategoryId = x.ProductCategoryId,
                    ProductCategoryCode = x.ProductCategoryCode,
                    ProductCategoryName = x.ProductCategoryName
                })
                .ToList();

            return Results.Ok(lts);
        })
        .WithTags("ProductCategory");

        app.MapPost("/product-category", (ProductCategoryDto.ProductCategoryCreateRequestDto request) =>
        {
            AppDbContext db = new AppDbContext();
            
            db.TblProductCategories.Add(new TblProductCategory()
            {
                ProductCategoryCode = request.ProductCategoryCode,
                ProductCategoryName = request.ProductCategoryName
            });
            int result = db.SaveChanges();
            string message = result > 0 ? "Saving successful" : "Saving failed";
            
            return Results.Ok(message);
        })
        .WithTags("ProductCategory");
    }
}