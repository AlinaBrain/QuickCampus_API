using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.Services;
using QuickCampus_DAL.Context;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddCors(c =>
//{
//    c.AddPolicy("AllowOrigin", options => options.WithOrigins("http://localhost:4200", "http://abc1234.etrueconcept.com:4200").AllowAnyHeader().AllowAnyMethod());
//});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "MyAllowSpecificOrigins",
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:4200",
                                              "http://localhost:4200");
                      });
});

//Add services to the container.
builder.Services.AddDbContext<QuikCampusContext>(
    x => x.UseSqlServer(
        builder.Configuration.GetConnectionString("ConnectionString"))
    );


//builder.Services.AddCors(options => {
//options.AddPolicy("Policy1", builder => {
//    builder.WithOrigins("http://localhost:4200");
//});

//    options.AddPolicy("Policy2", builder => {
//        builder.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
//    });
//});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
    options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddTransient<IApplicationUserRepo, ApplicationUserService>();
builder.Services.AddTransient<IApplicantRepo, ApplicantRepoServices>();
builder.Services.AddScoped<ICampusRepo, CampusService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("MyAllowSpecificOrigins");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
