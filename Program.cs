using Microsoft.EntityFrameworkCore;
using Music_Club.Models;
using Music_Club.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddDistributedMemoryCache();// ��������� IDistributedMemoryCache
builder.Services.AddSession();  // ��������� ������� ������

// �������� ������ ����������� �� ����� ������������
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");

// ��������� �������� ApplicationContext � �������� ������� � ����������
builder.Services.AddDbContext<MusicClubContext>(options => options.UseSqlServer(connection));

// ��������� ������� MVC
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IRepository<Users>, UserRepository>();
builder.Services.AddScoped<IRepository<MusicClip>, MusicClipRepository>();


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