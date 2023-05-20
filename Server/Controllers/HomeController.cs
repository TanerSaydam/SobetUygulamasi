using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Context;
using Server.Dtos;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HomeController : ControllerBase
{
    private readonly AppDbContext _context;    
    private readonly UserManager<AppUser> _userManager;
    public HomeController(AppDbContext context, UserManager<AppUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        AppUser user = await _userManager.FindByNameAsync(loginDto.UserName);
        if (user == null) throw new Exception("Kullanıcı bulunamadı!");

        return Ok(user);
    }

    [HttpGet("[action]/{userId}")]
    public async Task<IActionResult> GetUser(int userId, CancellationToken cancellationToken)
    {
        IList<UserDto> users =
            await _userManager.Users
            .Where(p => p.Id != userId)
            .Select(s => new UserDto
            {
                UserName = s.UserName,
                Id = s.Id
            })
            .ToListAsync(cancellationToken);

        return Ok(users);
    }
}
