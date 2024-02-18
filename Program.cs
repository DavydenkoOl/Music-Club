using Microsoft.EntityFrameworkCore;
using Music_Club.Models;
using Music_Club.Repository;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddDistributedMemoryCache();// добавляем IDistributedMemoryCache
builder.Services.AddSession();  // Добавляем сервисы сессии

// Получаем строку подключения из файла конфигурации
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");

// добавляем контекст ApplicationContext в качестве сервиса в приложение
builder.Services.AddDbContext<MusicClubContext>(options => options.UseSqlServer(connection));

// Добавляем сервисы MVC
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10); // Длительность сеанса (тайм-аут завершения сеанса)
    options.Cookie.Name = "Session"; // Каждая сессия имеет свой идентификатор, который сохраняется в куках.

});
// Добавляем сервисы MVC
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IRepository<Users>, UserRepository>();
builder.Services.AddScoped<IRepository<MusicClip>, MusicClipRepository>();
builder.Services.AddScoped<IRepository<Genre>, GenreRepository>();

var app = builder.Build();


app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=MusicClips}/{action=Index}/{id?}");

app.Run();
