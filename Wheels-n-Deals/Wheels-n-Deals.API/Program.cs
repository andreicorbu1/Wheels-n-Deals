using Wheels_n_Deals.API.Infrastructure.Middlewares;
using Wheels_n_Deals.API.Settings;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
Dependencies.Inject(builder);

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Access-Control-Allow-Origin");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
