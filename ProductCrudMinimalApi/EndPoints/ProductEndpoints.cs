using ProductCrudMinimalApi.Models.Data;
using ProductCrudMinimalApi.Models.DTO;
using ProductCrudMinimalApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ProductCrudMinimalApi.EndPoints
{
    public static class ProductEndpoints
    {
        public static void MapProductEndpoints(this WebApplication app)
        {
            //Get All Products

            app.MapGet("/api/products", async (AppDbContext db, int pageNumber , int pageSize) =>
            {
                var products = await db.Products.Skip((pageNumber-1)*pageSize).Take(pageSize).ToListAsync();
                if (!products.Any())
                {
                    return Results.NotFound("No Product Exists");
                }

                return Results.Ok(products);
            }).Produces<List<Product>>(200).Produces(404);

            //Get Product By Id

            app.MapGet("/api/products/{id:int}", async (int id, AppDbContext db) =>
            {
                var product = await db.Products.FindAsync(id);
                if (product != null)
                {
                    return Results.Ok(product);
                }

                return Results.NotFound("No product exists with this Id");
            }).Produces<Product>(200).Produces(404);

            //Add New Product

            app.MapPost("/api/products", async (AppDbContext db, ProductDto dto) =>
            {
                var productExists = await db.Products.AnyAsync(u => u.ProductName == dto.ProductName);
                if (!productExists)
                {
                    var newProduct = new Product()
                    {
                        ProductName = dto.ProductName,
                        Price = dto.Price,
                    };

                    await db.Products.AddAsync(newProduct);
                    await db.SaveChangesAsync();
                    return Results.Ok("Product Added Successfully");
                }
                return Results.BadRequest("Product Already Exists");
            }).Produces(400).Produces(200);

            //Update Product

            app.MapPut("/api/products/{id}", async (AppDbContext db, ProductDto dto, int id) =>
            {
                var product = await db.Products.FindAsync(id);

                if (product != null)
                {
                    product.ProductName = dto.ProductName;
                    product.Price = dto.Price;

                    await db.SaveChangesAsync();
                    return Results.Ok("Product Updated Successfully");

                }
                return Results.NotFound("No Product Exist");
            }).Produces(404).Produces(200);

            //Delete Product

            app.MapDelete("/api/products/{id}", async (AppDbContext db, int id) =>
            {
                var product = await db.Products.FindAsync(id);
                if (product != null)
                {
                    db.Products.Remove(product);
                    await db.SaveChangesAsync();

                    return Results.Ok("Product Deleted Successfully");
                }

                return Results.NotFound("No such product exists");
            }).Produces(200).Produces(404);

        }
    }
}
