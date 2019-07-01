using System;
using System.Linq;
using System.Text;
using API.Filter;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Domain.Models;
using Domain.Options;
using Emails;
using FluentValidation.AspNetCore;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;
using Persistence;
using Repositories;
using Repositories.Interfaces;
using Services;
using Services.Identity;
using Services.Interfaces;
using Services.MappingProfiles;
using Services.Validators;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("Mssql"));
            });

            services.Configure<RedisOptions>(options =>
            {
                options.ConnectionString = Configuration.GetConnectionString("Redis");
            });

            services.AddTransient<DbContext, ApplicationDbContext>();
            services.AddTransient<RedisDbContext, RedisDbContext>();

            services.AddTransient<IMailSenderService, MailSenderService>();
            services.Configure<SmtpConfiguration>(Configuration.GetSection("Smtp"));
            services.AddSingleton<SmtpConfiguration>();
            services.AddTransient<IRazorViewToStringRenderer, RazorViewToStringRenderer>();

            services.AddAutoMapper(typeof(UserMappingProfile).Assembly);

            UserManagementAndAuth(services);

            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IRefreshTokenService, RefreshTokenService>();

            services.AddTransient<IDishesService, DishesService>();

            services.AddSwaggerDocument(options =>
            {
                options.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Description = "Type into the textbox: Bearer {your JWT token}."
                });

                options.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
            });

            services.AddCors();

            services.AddMvc(options =>
                {
                    options.Filters.Add(typeof(ExceptionFilter));
                })
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<UserDtoValidator>())
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            return ConfigureAutofac(services);
        }

        public IServiceProvider ConfigureAutofac(IServiceCollection services)
        {
            var builder = new ContainerBuilder();
            builder.Populate(services);

            builder.RegisterGeneric(typeof(RepositorySql<>)).Named("sql", typeof(IRepository<>)).InstancePerDependency();
            builder.RegisterGeneric(typeof(RepositoryRedis<>)).Named("redis", typeof(IRepository<>)).InstancePerDependency();

            builder.RegisterType<UnitOfWorkSql>().Keyed<IUnitOfWork>("sql").InstancePerDependency();
            builder.RegisterType<UnitOfWorkRedis>().Keyed<IUnitOfWork>("redis").InstancePerDependency();

            var container = builder.Build();
            return container.Resolve<IServiceProvider>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors(
                options => options.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader()
            );

            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseAuthentication();

            app.UseHttpsRedirection();
            app.UseMvc();
        }

        private void UserManagementAndAuth(IServiceCollection services)
        {
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;
            });

            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromHours(3);
            });

            var sKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["JwtOptions:SecretKey"]));
            services.Configure<JwtOptions>(opt =>
            {
                opt.Issuer = Configuration["JwtOptions:Issuer"];
                opt.Audience = Configuration["JwtOptions:Audience"];
                opt.SigningCredentials = sKey;
            });
            services.AddSingleton<JwtOptions>();

            services.AddIdentityCore<User>().AddDefaultTokenProviders();
            services.AddScoped<IUserStore<User>, UserStore>();

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = Configuration["JwtOptions:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = Configuration["JwtOptions:Audience"],
                        ValidateLifetime = true,
                        IssuerSigningKey = sKey,
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });
                //.AddGoogle(options =>
                //{
                //    options.ClientId = Configuration["Auth:Google:ClientId"];
                //    options.ClientSecret = Configuration["Auth:Google:ClientSecret"];
                //    options.SignInScheme = IdentityConstants.ExternalScheme;
                //});

            services.AddAuthorization();
        }
    }
}