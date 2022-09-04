using NBitcoin;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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

// app.UseHttpsRedirection();


app.MapGet("/build", () =>
{
    return new Builder().Build();
})
.WithName("GetWeatherForecast");

foreach(var x in new Builder().Build())
{
    Console.WriteLine(x);
}


//. app.Run();