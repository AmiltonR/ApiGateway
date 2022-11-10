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


builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

// Add services to the container.
builder.Services.AddSingleton<MyAggregator>();

builder.Services.AddOcelot().
                    AddDelegatingHandler<NoEncodingHandler>(true)
                    .AddTransientDefinedAggregator<MyAggregator>()
                    .AddTransientDefinedAggregator<TalleresProgramacionAggregator>();

builder.Services.AddCustomJwtAuthentication();

var app = builder.Build();

// Configure the HTTP request pipeline.


//applying CORS
app.UseCors(MyAllowSpecificOrigins);

async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
        Regex reor = new Regex(@"[^,\s+$ ][^\,]*[^,\s+$ ]");
        MatchCollection matches;

        Regex reand = new Regex(@"[^&\s+$ ][^\&]*[^&\s+$ ]");
        MatchCollection matchesand;
        int cont = 0;
        foreach (KeyValuePair<string, string> claim in required)
        {
            matches = reor.Matches(claim.Value);
            foreach (Match match in matches)
            {
                matchesand = reand.Matches(match.Value);
                cont = 0;
                foreach (Match m in matchesand)
                {
                    foreach (Claim cl in claims)
                    {
                        if (cl.Type == claim.Key)
                        {
                            if (cl.Value == m.Value)
                            {
                                cont++;
                            }
                        }
                    }
                }
                if (cont == matchesand.Count)
                {
                    auth = true;
                    break;
                }
            }
        }
        return auth;
    }
}


app.UseOcelot().Wait();
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.Run();