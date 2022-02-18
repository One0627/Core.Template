using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Core.Template.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Panda.DynamicWebApi;
using Swashbuckle.AspNetCore.Swagger;

namespace Core.Template.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            #region Cors
            services.AddCors(options => options.AddPolicy("AllowCors", policy =>
            {
                policy.AllowAnyOrigin() // .WithOrigins("url") 
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .WithExposedHeaders("Content-Disposition");//添加自定义header
            }));
            #endregion
            #region MVC + GlobalFilters

            //注入全局异常捕获
            services.AddMvc(options =>
            {
                // 全局异常过滤
                //options.Filters.Add(typeof(Filters.GlobalExceptionFilter));
                // 全局路由权限公约
                //options.Conventions.Insert(0, new GlobalRouteAuthorizeConvention());
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
            .AddJsonOptions(options =>
             {
                 //忽略循环引用
                 options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                 //不使用驼峰样式的key
                 options.SerializerSettings.ContractResolver = new DefaultContractResolver();
             });
            #endregion
            services.AddDynamicWebApi(new DynamicWebApiOptions
            {
                RemoveControllerPostfixes = new List<string>() { "Service" }
            });
            #region Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1.0.0",
                    Title = "Core.Template.Api 接口文档",
                    Description = "接口说明文档",
                    Contact = new Contact { Name = "WatchMan", Email = "79060087@qq.com", Url = "https://github.com/One0627" }
                });
                c.DocInclusionPredicate((docName, description) => true);
                //添加注释显示
                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //c.IncludeXmlComments(xmlPath);
                //c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Core.Template.Services.xml"));
                //向http header添加Jwt Authorize
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "使用Bearer的JWT Authorization header。 示例：\"Bearer {token}\"(注意空格)",
                    Name = "Authorization",//Jwt default param name
                    In = "header",//Jwt store address
                    Type = "apiKey"//Security scheme type
                });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", new string[] { } }
                });
            });

            #endregion

            #region AutoFac DI
            //实例化 AutoFac  容器   
            var builder = new ContainerBuilder();

            //将services填充到Autofac容器生成器中
            builder.Populate(services);

            //使用已进行的组件登记创建新容器
            var ApplicationContainer = builder.Build();
            #endregion
            return new AutofacServiceProvider(ApplicationContainer);//第三方IOC接管 core内置DI容器
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiHelp V1");
                c.RoutePrefix = "";
            });
            //app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
