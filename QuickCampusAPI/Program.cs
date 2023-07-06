using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.Services;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;
using System.Reflection;
using System.Text;
using static QuickCampus_Core.ViewModel.ClientVM;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "MyAllowSpecificOrigins",
                      policy =>
                      {
                          policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                      });
});

//Add services to the container.
builder.Services.AddDbContext<QuikCampusDevContext>(
    x => x.UseSqlServer(
        builder.Configuration.GetConnectionString("ConnectionString"))
    );

builder.Services.AddControllers();
builder.Services.AddScoped<IValidator<AdminLogin>, AdminLoginValidator>();
builder.Services.AddScoped<IValidator<ClientVM>, ClientValidator>();



builder.Services.AddControllers()
                .AddFluentValidation(options =>
                {
                    // Validate child properties and root collection elements
                    options.ImplicitlyValidateChildProperties = true;
                    options.ImplicitlyValidateRootCollectionElements = true;

                    // Automatic registration of validators in assembly
                    options.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
                });

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
    options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))

        };
    });
builder.Services.AddTransient<IApplicationUserRepo, ApplicationUserService>();
builder.Services.AddTransient<IApplicantRepo, ApplicantRepoServices>();
builder.Services.AddScoped<ICampusRepo, CampusService>();
builder.Services.AddScoped<ICountryRepo, CountryService>();
builder.Services.AddScoped<ICollegeRepo, CollegeRepo>();
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IClientRepo, ClientRepo>();
builder.Services.AddScoped<IRoleRepo, RoleRepo>();
builder.Services.AddScoped<IAccount, AccountService>();
builder.Services.AddScoped<IClientRepo, ClientRepo>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();

app.UseCors("MyAllowSpecificOrigins");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
