using CustomersWebApi.DistributedCache;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IDistributedCachingService, DistributedCachingService>();

builder.Services.AddDistributedMemoryCache(opt => {

});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(c=>
    {
        
    });
    app.UseSwaggerUI(c => {
        //c.SwaggerEndpoint("/swagger/v1/swagger.json", "CustomersWebAPI V1");
        //c.DisableValidator();
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.Run();
