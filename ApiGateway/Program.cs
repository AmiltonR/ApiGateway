//var builder = WebApplication.CreateBuilder(args);

//Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
//var app = builder.Build();

//Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}


//app.Run();
using ApiGateway;
using ApiGateway.Agregators;
using JwtAuthenticationManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Values;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("*").AllowAnyHeader()
                                                  .AllowAnyMethod()
                                                  ; ;
                      });
});


builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

// Add services to the container.
builder.Services.AddSingleton<MyAggregator>();

builder.Services.AddOcelot().
                    AddDelegatingHandler<NoEncodingHandler>(true)
                    .AddTransientDefinedAggregator<MyAggregator>();

builder.Services.AddCustomJwtAuthentication();

var app = builder.Build();

// Configure the HTTP request pipeline.


//applying CORS
app.UseCors(MyAllowSpecificOrigins);

app.UseOcelot().Wait();
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.Run();