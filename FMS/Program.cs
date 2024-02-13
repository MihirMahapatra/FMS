using AutoMapper;
using FMS.Api.Email.EmailService;
using FMS.Db.Context;
using FMS.Db.DbEntity;
using FMS.Model;
using FMS.Model.AutoMapper;
using FMS.Repository.Account;
using FMS.Repository.Accounting;
using FMS.Repository.Admin;
using FMS.Repository.Devloper;
using FMS.Repository.Master;
using FMS.Repository.Reports;
using FMS.Repository.Transaction;
using FMS.Service.Account;
using FMS.Service.Accounting;
using FMS.Service.Admin;
using FMS.Service.Devloper;
using FMS.Service.Master;
using FMS.Service.Reports;
using FMS.Service.Transaction;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using NLog;
using NLog.Web;
using System.Configuration;
using System.Runtime.InteropServices;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
try
{
    var builder = WebApplication.CreateBuilder(args);
    //***************************************************Add Connection to Db**************************************//
    builder.Services.AddDbContext<AppDbContext>(option =>
    option.UseSqlServer(builder.Configuration.GetConnectionString("DBCS")).EnableSensitiveDataLogging(builder.Environment.IsDevelopment())
    .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
    );
    //****************************************************Email setup************************************************//
    builder.Services.Configure<SMTPConfigModel>(builder.Configuration.GetSection("SMTPConfig"));
    //*************************************************Dependancy Injection***************************************// 
    builder.Services.AddScoped<IAccountRepo, AccountRepo>();
    builder.Services.AddScoped<IAccountSvcs, AccountSvcs>();
    builder.Services.AddScoped<IEmailService, EmailService>();
    builder.Services.AddScoped<IAdminSvcs, AdminSvcs>();
    builder.Services.AddScoped<IAdminRepo, AdminRepo>();
    builder.Services.AddScoped<IDevloperSvcs, DevloperSvcs>();
    builder.Services.AddScoped<IDevloperRepo, DevloperRepo>();
    builder.Services.AddScoped<IMasterSvcs, MasterSvcs>();
    builder.Services.AddScoped<IMasterRepo, MasterRepo>();
    builder.Services.AddScoped<ITransactionSvcs, TransactionSvcs>();
    builder.Services.AddScoped<ITransactionRepo, TransactionRepo>();
    builder.Services.AddScoped<IAccountingSvcs, AccountingSvcs>();
    builder.Services.AddScoped<IAccountingRepo, AccountingRepo>();
    builder.Services.AddScoped<IReportSvcs, ReportSvcs>();
    builder.Services.AddScoped<IReportRepo, ReportRepo>();
    //*****************************************************AutoMapper*****************************************//
    var automapper = new MapperConfiguration(option => option.AddProfile(new MappingProfile()));
    IMapper mapper = automapper.CreateMapper();
    builder.Services.AddSingleton(mapper);
    //*******************************************************Session********************************************//
    builder.Services.AddDistributedMemoryCache();
    builder.Services.AddSession(/*option => option.IdleTimeout = TimeSpan.FromDays(1)*/);
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddSingleton(option => option.GetService<IHttpContextAccessor>().HttpContext.Session);
    //*******************************************************Identity*********************************************//
    builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
    {
        //*password*//
        options.Password.RequiredLength = 8;
        options.Password.RequiredUniqueChars = 1;
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = false;
        //*login attempt*//
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        //*email conformation*//
        //options.SignIn.RequireConfirmedEmail = true;
    }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
    //**************************************************set token expiry*******************************************//
    builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
    {
        options.TokenLifespan = TimeSpan.FromDays(1);
    });
    //****************************************************Api service***********************************************//
    builder.Services.AddHttpClient();
    //****************************************************Claims based authorization*******************************// 
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("Create", policy =>
        {
            policy.RequireClaim("Create Role", "true");
        });
        options.AddPolicy("Edit", policy =>
        {
            policy.RequireClaim("Edit Role", "true");
        });
        options.AddPolicy("Delete", policy =>
        {
            policy.RequireClaim("Delete Role", "true");
        });
    });
    //****************************************************Authentication*******************************// 
    builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
       //options.Cookie.Expiration = TimeSpan.FromDays(1);
    });
    //****************************************************Data Protection*******************************// 
    builder.Services.AddDataProtection().PersistKeysToDbContext<AppDbContext>();
    //****************************************************Global Autherization****************************************************//
    builder.Services.AddControllersWithViews(options =>
    {
        var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
        options.Filters.Add(new AuthorizeFilter(policy));

    }).AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ContractResolver = new DefaultContractResolver();
    });

    //builder.Services.AddAntiforgery(options => options.HeaderName = "XSRF-TOKEN");

    //************************************************runtime compilation for razor page view**************************************************//
#if DEBUG
    builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
#endif
    //*******************************************************Nlog Configuration***************************************************************//
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();
    builder.Services.AddLogging(options =>
    {
        options.AddConsole();
        options.AddDebug();
    });
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }
    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseSession();
    app.UseCookiePolicy();
   
    app.UseRouting();
    app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllerRoute(name: "default", pattern: "{controller=Account}/{action=LandingPage}/{id?}");
    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex);
    throw;
}
finally
{
    LogManager.Shutdown();
}

