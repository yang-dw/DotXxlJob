﻿using DotXxlJob.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ASPNetCoreExecutor
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
          
            services.AddXxlJobExecutor(Configuration);
            services.AddDefaultXxlJobHandlers();// add httpHandler;

            services.AddSingleton<IJobHandler, DemoJobHandler>(); // 添加自定义的jobHandler
            services.AddSingleton<IJobHandler, DemoJobHandler2>(); // 添加自定义的jobHandler
            

            services.AddAutoRegistry(); // 自动注册
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //启用XxlExecutor
            app.UseMiddleware<XxlJobExecutorMiddleware>();
        }
    }
}