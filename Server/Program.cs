using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Server.Context;
using Server.Hubs;

var builder = WebApplication.CreateBuilder(args);


#region Context
string connectionString = builder.Configuration["SqlServer"];
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connectionString);   
});
builder.Services.AddIdentity<AppUser,AppRole>().AddEntityFrameworkStores<AppDbContext>();
#endregion

#region Cors
builder.Services.AddCors(options => 
    options.AddDefaultPolicy(opt => 
        opt.AllowCredentials()
        .AllowAnyMethod()
        .AllowAnyHeader()
        .SetIsOriginAllowed(opt => true)));
#endregion

builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//app.Use((context, next) =>
//{
//    using (var scope = app.Services.CreateScope())
//    {
//        var userManager = scope.ServiceProvider.GetService<UserManager<AppUser>>();
//        if (!userManager.Users.Any())
//        {
//            AppUser user1 = new AppUser()
//            {
//                UserName = "abdullah",
//                Email = "abdullah@gmail.com",
//            };

//            AppUser user2 = new AppUser()
//            {
//                UserName = "taner",
//                Email = "taner@gmail.com",
//            };

//            AppUser user3 = new AppUser()
//            {
//                UserName = "efrun",
//                Email = "efrun@gmail.com",
//            };

//            AppUser user4 = new AppUser()
//            {
//                UserName = "batuhan",
//                Email = "batuhan@gmail.com",
//            };

//            AppUser user5 = new AppUser()
//            {
//                UserName = "hakan",
//                Email = "hakan@gmail.com",
//            };

//            userManager.CreateAsync(user1, "Password12*").Wait();
//            userManager.CreateAsync(user2, "Password12*").Wait();
//            userManager.CreateAsync(user3, "Password12*").Wait();
//            userManager.CreateAsync(user4, "Password12*").Wait();
//            userManager.CreateAsync(user5, "Password12*").Wait();
//        }
//    }
//    return next();
//});

app.MapHub<ChatHub>("/chatHub");

app.Run();
