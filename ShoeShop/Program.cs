using BusinessObject.banner;
using BusinessObject.category;
using BusinessObject.contact;
using BusinessObject.news;
using BusinessObject.product;
using BusinessObject.user;
using DataAccess.Models; // Import DbContext
using DataAccess.Repository.banner;
using DataAccess.Repository.Base;
using DataAccess.Repository.category;
using DataAccess.Repository.contact;
using DataAccess.Repository.news;
using DataAccess.Repository.product;
using DataAccess.Repository.user;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian hết hạn session
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true; // Bắt buộc cookie của session
});
builder.Services.AddDistributedMemoryCache();
// Cấu hình DbContext với SQL Server
builder.Services.AddDbContext<PROJECT_PRN212Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyContr")));


// Đăng ký các Repository
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IBannerRepository, BannerRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<INewsRepository, NewsRepository>();
builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Đăng ký UnitOfWork và các Service liên quan
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

// Đăng ký các Service cho Business Logic
builder.Services.AddScoped<IProductService,ProductService>();
builder.Services.AddScoped<IBannerService, BannerService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<INewsService, NewsService>();
builder.Services.AddScoped<IUserService, UserService>();

// Đăng ký Razor Pages
builder.Services.AddRazorPages();

var app = builder.Build();

// Cấu hình middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;
app.UseSession();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
