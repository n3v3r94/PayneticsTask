
using Microsoft.EntityFrameworkCore;
using Paynetics.BusinessLogic.Services;
using Paynetics.Data;
using Paynetics.BusinessLogic.Services.XML;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Http.Features;

namespace Paynetics.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<PayneticsDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddControllers();
            builder.Services.AddTransient<IPartnerService,PartnerService>();
            builder.Services.AddTransient<IMerchantService,MerchantService>();
            builder.Services.AddTransient<ITransactionService,TransactionService>();
            builder.Services.AddTransient<IXMLService,XMLService>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = 3221225472; // 3 gb
            });

            builder.Services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = 3221225472; // 3 gb
            });

            builder.Services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 3221225472; // 3 gb in bytes
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
