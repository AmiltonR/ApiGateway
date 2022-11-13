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
using Ocelot.Authorization;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Values;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;

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

async void Configure(IApplicationBuilder app)
{
    //your code here
    var configuration = new OcelotPipelineConfiguration
    {
        AuthorizationMiddleware = async (ctx, next) =>
        {
            if (Authorize(ctx))
            {
                await next.Invoke();
            }
            else
            {
                ctx.Items.SetError(new UnauthorizedError($"Fail to authorize"));
            }
        }
    };
    //your code here
    await app.UseOcelot(configuration);
}

bool Authorize(HttpContext ctx)
{
    if (ctx.Items.DownstreamRoute().AuthenticationOptions.AuthenticationProviderKey == null) return true;
    else
    {

        bool auth = false;
        Claim[] claims = ctx.User.Claims.ToArray<Claim>();
        Dictionary<string, string> required = ctx.Items.DownstreamRoute().RouteClaimsRequirement;

        string[] role = required.Values.ToArray();


        string[] roleSeparateArray = role[0].Split(',');

        string roleClaims = claims[1].Value;

        foreach (var item in roleSeparateArray)
        {
            if (roleClaims == item)
            {
                auth = true;
            }
        }
        return auth;
    }
}

// Add services to the container.
builder.Services.AddSingleton<MyAggregator>();

builder.Services.AddOcelot().
                    AddDelegatingHandler<NoEncodingHandler>(true)
                    .AddTransientDefinedAggregator<MyAggregator>()
                    .AddTransientDefinedAggregator<TalleresProgramacionAggregator>();

builder.Services.AddCustomJwtAuthentication();


var app = builder.Build();

//Set environment 
app.Environment.EnvironmentName = "Development";

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    builder.Configuration.AddJsonFile("ocelot-dev.json", optional: false, reloadOnChange: true);
}
else
{
    builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
}

//applying CORS
app.UseCors(MyAllowSpecificOrigins);
Configure(app);


//app.UseOcelot().Wait();
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.Run();
