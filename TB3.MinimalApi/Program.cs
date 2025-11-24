using Microsoft.EntityFrameworkCore;
using TB3.Database.AppDbContextModels;
using TB3.MinimalApi.Dtos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/product", () =>
{
    AppDbContext db = new AppDbContext();
    var lts = db.TblProducts
        .AsNoTracking()
        .OrderByDescending(x => x.ProductId)
        .Select(x => new ProductDto.ProductResponseDto()
        {
            ProductId = x.ProductId,
            ProductCode = x.ProductCode,
            ProductName = x.ProductName,
            Price = x.Price,
            Quantity = x.Quantity,
            ProductCategoryCode = x.ProductCategoryCode,
            CreatedDateTime = x.CreatedDateTime,
            ModifiedDateTime = x.ModifiedDateTime
        })
        .ToList();
    
    return Results.Ok(lts);
});

app.MapGet("/product/{id}", (int id) =>
{
    AppDbContext db = new AppDbContext();

    var product = db.TblProducts
        .Where(x => x.DeleteFlag == false)
        .Select(x => new ProductDto.ProductResponseDto()
        {
            ProductId = x.ProductId,
            ProductCode = x.ProductCode,
            ProductName = x.ProductName,
            Price = x.Price,
            Quantity = x.Quantity,
            ProductCategoryCode = x.ProductCategoryCode,
            CreatedDateTime = x.CreatedDateTime,
            ModifiedDateTime = x.ModifiedDateTime
        })
        .FirstOrDefault(x => x.ProductId == id);

    if (product is null)
    {
        return Results.NotFound("Product not found");
    }

    return Results.Ok(product);
});

app.MapPost("/product", (ProductDto.ProductCreateRequestDto request) =>
{
    AppDbContext db = new AppDbContext();
    
    var product = db.TblProducts
        .FirstOrDefault(x => x.ProductCode == request.ProductCode);
    if (product is not null)
    {
        return Results.BadRequest("Product code already exists");
    }
            
    db.TblProducts.Add(new TblProduct()
    {
        ProductCode = request.ProductCode,
        ProductName = request.ProductName,
        Price = request.Price,
        Quantity = request.Quantity,
        ProductCategoryCode = request.ProductCategoryCode,
        CreatedDateTime = DateTime.Now,
        DeleteFlag = false
    });
    int result = db.SaveChanges();
    string message = result > 0 ? "Saving successful" : "Saving failed";
            
    return Results.Ok(message);
});

app.MapPut("/product/{id}", (int id, ProductDto.ProductUpdateRequestDto request) =>
{
    AppDbContext db = new AppDbContext();
    
    var product = db.TblProducts
        .Where(x => x.DeleteFlag == false)
        .FirstOrDefault(x => x.ProductId == id);
            
    if (product is null)
    {
        return Results.NotFound("Product not found");
    }

    product.ProductCode = request.ProductCode;
    product.ProductName = request.ProductName;
    product.Price = request.Price;
    product.Quantity = request.Quantity;
    product.ProductCategoryCode = request.ProductCategoryCode;
    product.ModifiedDateTime = DateTime.Now;
            
    int result = db.SaveChanges();
    string message = result > 0 ? "Updating successful" : "Updating failed";
            
    return Results.Ok(message);
});

app.MapPatch("/product/{id}", (int id, ProductDto.ProductPatchRequestDto request) =>
{
    AppDbContext db = new AppDbContext();
    
    var product = db.TblProducts
        .Where(x => x.DeleteFlag == false)
        .FirstOrDefault(x => x.ProductId == id);
            
    if (product is null)
    {
        return Results.NotFound("Product not found");
    }

    if (!string.IsNullOrEmpty(request.ProductCode))
        product.ProductCode = request.ProductCode;
    if (!string.IsNullOrEmpty(request.ProductName))
        product.ProductName = request.ProductName;
    if (request.Price is not null && request.Price > 0)
        product.Price = request.Price ?? 0;
    if (request.Quantity is not null && request.Quantity > 0)
        product.Quantity = request.Quantity ?? 0;
    if (!string.IsNullOrEmpty(request.ProductCategoryCode))
        product.ProductCategoryCode = request.ProductCategoryCode;
    product.ModifiedDateTime = DateTime.Now;
            
    int result = db.SaveChanges();
    string message = result > 0 ? "Patching successful" : "Patching failed";
            
    return Results.Ok(message);
});

app.MapDelete("/product/{id}", (int id) =>
{
    AppDbContext db = new AppDbContext();
    
    var product = db.TblProducts
        .Where(x => x.DeleteFlag == false)
        .FirstOrDefault(x => x.ProductId == id);
            
    if (product is null)
    {
        return Results.NotFound("Product not found");
    }
            
    product.DeleteFlag = true;
    int result = db.SaveChanges();
    string message = result > 0 ? "Deleting successful" : "Deleting failed";
            
    return Results.Ok(message);
});

app.Run();