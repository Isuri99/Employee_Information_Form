using EmployeeAPI.Utilities;

var builder = WebApplication.CreateBuilder(args);

// Add Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Register Utility class for Dependency Injection
builder.Services.AddScoped<DataBaseUtilities>();

//Enable CORS so Angular (port 4200) can talk to API
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAngular", b => {
        b.WithOrigins("http://localhost:4200")
         .AllowAnyMethod()
         .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowAngular");

app.UseAuthorization();

// Enable Controller routing
app.MapControllers();

app.Run();
