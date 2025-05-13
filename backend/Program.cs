using System.Net.Http.Headers;
using MovieCompare.Backend.Extensions;
using MovieCompare.Backend.Services;
using Polly;
using Polly.Extensions.Http;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMemoryCache();
builder.Services.AddCors(opt =>
    opt.AddPolicy("AllowLocalfront", p =>
        p.WithOrigins("http://localhost:5173")
         .AllowAnyHeader()
         .AllowAnyMethod()
    )
);

//config
var cinemaExternalApiConfig = builder.Configuration.GetSection("CinemaExternalApi");

//polly
var retryPolicy = HttpPolicyExtensions
    .HandleTransientHttpError()
    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromMilliseconds(200 * retryAttempt));

builder.Services
    .AddHttpClient("cinemaExternalApi", c =>
    {
        c.BaseAddress = new Uri(cinemaExternalApiConfig["BaseUrl"]);
        c.DefaultRequestHeaders.Add("x-access-token", cinemaExternalApiConfig["ApiToken"]);
    })
    .AddPolicyHandler(retryPolicy);


//DI
builder.Services.AddScoped<IExternalMovieService, ExternalMovieService>();
builder.Services.AddScoped<IMovieService, MovieService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowLocalfront");
app.MapControllers();

app.Run();
