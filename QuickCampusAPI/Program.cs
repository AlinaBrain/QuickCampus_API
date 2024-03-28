using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QuickCampus_Core.Interfaces;
using QuickCampus_Core.Services;
using QuickCampus_Core.ViewModel;
using QuickCampus_DAL.Context;
using QuickCampusAPI.Utility;
using System.Reflection;
using System.Text;
using static QuickCampus_Core.ViewModel.ApplicantViewModel;
using static QuickCampus_Core.ViewModel.ClientVM;
using static QuickCampus_Core.ViewModel.CollegeVM;
using static QuickCampus_Core.ViewModel.UserVm;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyAllowSpecificOrigins",
        builder => builder
            //.SetIsOriginAllowedToAllowWildcardSubdomains()
            //.WithOrigins("http://localhost:4200", "https://localhost:90", "https://*.sentridocs.com", "http://*.sentridocs.com")
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()
        //.AllowCredentials()
        );
});
//Add services to the container.
builder.Services.AddDbContext<BtprojecQuickcampusContext>(
    x => x.UseSqlServer(
        builder.Configuration.GetConnectionString("ConnectionString"))
    );
builder.Services.AddControllers();
builder.Services.AddScoped<IValidator<AdminLogin>, AdminLoginValidator>();
builder.Services.AddScoped<IValidator<UserVm>, UserValidator>();
builder.Services.AddScoped<IValidator<ClientUpdateRequest>, ClientValidatorRequest>();
builder.Services.AddScoped<IValidator<CollegeVM>, CollegeValidator>();
builder.Services.AddScoped<IValidator<ApplicantViewModel>, ApplicantValidator>();
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

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Quick Campus API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});
builder.Services.AddAuthentication(auth =>
{
    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
builder.Services.AddTransient<IApplicationUserRepo, ApplicationUserService>();
builder.Services.AddTransient<IApplicantRepo, ApplicantRepoServices>();
builder.Services.AddScoped<ICampusRepo, CampusService>();
builder.Services.AddScoped<ICountryRepo, CountryService>();
builder.Services.AddScoped<ICollegeRepo, CollegeRepo>();
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IRoleRepo, RoleRepo>();
builder.Services.AddScoped<IAccount, AccountService>();
builder.Services.AddScoped<IClientRepo, ClientRepo>();
builder.Services.AddScoped<IStateRepo, StateServices>();
builder.Services.AddScoped<IQuestion,QuestionService>();
builder.Services.AddScoped<IUserRoleRepo,UserRoleService>();
builder.Services.AddScoped<ICityRepo,CityServices>(); 
builder.Services.AddScoped<IQuestionOptionRepo, QuestionOptionService>();
builder.Services.AddScoped<IGroupRepo,GroupServices>();
builder.Services.AddScoped<ISectionRepo, SectionServices>();
builder.Services.AddScoped<ICompanyRepo, CompanyRepo>();
builder.Services.AddScoped<IStatusRepo, StatusRepo>();
builder.Services.AddScoped<IUserAppRoleRepo, UserAppRoleRepo>();
builder.Services.AddScoped<IUserRoleRepo, UserRoleService>();
builder.Services.AddScoped<IMstQualificationRepo, MstQualificationRepo>();
builder.Services.AddScoped<ICampusWalkinCollegeRepo, CampusCollegeService>();
builder.Services.AddScoped<QuestionTypeRepo, QuestionTypeService>();
builder.Services.AddTransient<ValidationFilterAttribute>();
builder.Services.Configure<ApiBehaviorOptions>(options
    => options.SuppressModelStateInvalidFilter = true);
builder.Services.AddScoped<QuickCampus_Core.Common.Helper.ProcessUploadFile>();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{

    app.UseFileServer(new FileServerOptions
    {
        // FileProvider = new PhysicalFileProvider(@"D:\Quick Campus\QuickCampusAPI\wwwroot\UploadFiles"),
        //FileProvider=new PhysicalFileProvider(@"D:\QuickCampus_01-02-2024\QuickCampus_API\Quick_Campus\QuickCampusAPI\wwwroot\UploadFiles"),
        FileProvider = new PhysicalFileProvider(@"D:\QuickCampus_01-02-2024\QuickCampus_API\Quick_Campus\QuickCampusAPI\wwwroot\UploadFiles"),
        
        RequestPath = new PathString("/UploadFiles"),
        EnableDirectoryBrowsing = false
    });
}
else
{

}
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseCors("MyAllowSpecificOrigins");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.Run();
