using Microsoft.EntityFrameworkCore;
using PeopleComments.Dll.DbContexts;
using PeopleComments.Dll.Repositories.Account;
using PeopleComments.Dll.Repositories.Comment;
using PeopleComments.Dll.Services.Account;
using PeopleComments.Dll.Services.Comment;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.ReturnHttpNotAcceptable = true;
})
    .AddNewtonsoftJson();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



// To provide a ConnectionString using a appsettings.json
// its important to be sure your key exist
builder.Services.AddDbContext<AccountCommentsContext>(
    dbContextOptions => dbContextOptions.UseSqlite(
        builder.Configuration["ConnectionStrings:AccountCommentsInfoDBConnectionString"]));


builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ICommentService, CommentService>();

builder.Services.AddScoped<IAccountCommentInfoRepository, AccountCommentInfoRepository>();
builder.Services.AddScoped<ICommentInfoRepository, CommentInfoRepository>();





builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());



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
