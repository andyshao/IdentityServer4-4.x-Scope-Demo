using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Api1Resource
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    //IdentityServer��ַ
                    options.Authority = "http://localhost:5001";
                    //��ӦIdp��ApiResource��Name
                    options.Audience = "api1";
                    //��ʹ��https
                    options.RequireHttpsMetadata = false;
                });

            services.AddAuthorization(options =>
            {
                //���ڲ�����Ȩ
                options.AddPolicy("WeatherPolicy", builder =>
                {
                    //�ͻ���Scope�а���api1.weather.scope���ܷ���
                    builder.RequireScope("api1.weather.scope");
                });
                //���ڲ�����Ȩ
                options.AddPolicy("TestPolicy", builder =>
                {
                    //�ͻ���Scope�а���api1.test.scope���ܷ���
                    builder.RequireScope("api1.test.scope");
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            //�����֤
            app.UseAuthentication();

            //��Ȩ
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
