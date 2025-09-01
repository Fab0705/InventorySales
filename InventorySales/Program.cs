using InventorySales.Repository;
using InventorySales.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.Add(new ServiceDescriptor(typeof(iProduct), new ProductRepository()));
builder.Services.Add(new ServiceDescriptor(typeof(iCategory), new CategoryRepository()));
builder.Services.Add(new ServiceDescriptor(typeof(iCustomer), new CustomerRepository()));
builder.Services.Add(new ServiceDescriptor(typeof(iUser), new UserRepository()));
builder.Services.Add(new ServiceDescriptor(typeof(iSale), new SaleRepository()));
builder.Services.Add(new ServiceDescriptor(typeof(iAuditLog), new AuditLogRepository()));
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

app.Run();
