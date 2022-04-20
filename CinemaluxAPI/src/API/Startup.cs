using CinemaluxAPI.Auth;
using CinemaluxAPI.Services;
using CinemaluxAPI.Multimedia;
using CinemaluxAPI.Service.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using CinemaluxAPI.Common.Extensions;
using CinemaluxAPI.Service.Web.Genres;
using CinemaluxAPI.Service.Web.Movies;
using CinemaluxAPI.Services.Screenings;
using CinemaluxAPI.Service.Web.Contracts;
using CinemaluxAPI.Services.Reservations;
using Microsoft.Extensions.Configuration;
using CinemaluxAPI.DAL.CinemaluxCatalogue;
using CinemaluxAPI.Service.Web.MovieReviews;
using CinemaluxAPI.DAL.OrganizationDbContext;
using CinemaluxAPI.Service.Cinemalux.Contracts;
using CinemaluxAPI.Service.Contracts;
using CinemaluxAPI.Service.Halls;
using CinemaluxAPI.Services.Types;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using IAuthorizationService = CinemaluxAPI.Auth.IAuthorizationService;

namespace CinemaluxAPI.API
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {   
            // Add DB Context
            services.AddScoped<CinemaluxDbContext>();
            services.AddScoped<OrganizationDbContext>();

            // Add Controllers
            services.AddControllers();
            services.AddCors();
            
            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddAuthorization();
            
            services.AddControllers(options =>
                options.Filters.Add(new HttpResponseExceptionFilter()));
            
            // Generate swagger
            services.AddSwaggerGen();
            
            // Add application based services 
            
            // Cinemalux
            services.AddTransient<ITypesService, TypesService>();
            services.AddTransient<IOrdersService, OrdersService>();
            // services.AddTransient<IMoviesService, MoviesService>();            
            services.AddTransient<ITicketService , TicketService>();      
            services.AddTransient<IHallService , HallsService>();
            services.AddTransient<IDiscountService, DiscountService>();            
            services.AddTransient<IEmployeeService, EmployeeService>();
            services.AddTransient<IScreeningService, ScreeningsService>();
            services.AddTransient<IReservationService, ReservationService>();
            services.AddTransient<IAuthorizationService, AuthorizationService>();
            
            // Web
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IGenresService, GenresService>();
            services.AddTransient<IMovieReviewsService, MovieReviewsService>();
            services.AddTransient<IOrganizationMoviesService, OrganizationMoviesService>();
            
            // Imager
            services.AddTransient<IMultimediaService, MultimediaService>();
            
            services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = int.MaxValue;
            });
        } 

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cinemalux Local API"); 
                });
            }
            
            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            
            app.UseMiddleware<JWTMiddleware>();
            
            app.UseRouting();
            
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}