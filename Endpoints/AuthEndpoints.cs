using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskFlow.Api.Data;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/register", async (AppDbContext db, RegisterDto dto) =>
        {
            var user = new User
            {
                Email = dto.Email,
                PasswordHash = PasswordHasher.Hash(dto.Password)
            };

            db.Users.Add(user);
            await db.SaveChangesAsync();

            return Results.Ok();
        });


        app.MapPost("/login", async (AppDbContext db, LoginDto dto) =>
        {
            var hash = PasswordHasher.Hash(dto.Password);

            var user = db.Users.FirstOrDefault(u =>
                u.Email == dto.Email &&
                u.PasswordHash == hash);
            if (user is null)
            {
                return Results.Unauthorized();   
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("THIS_IS_A_SUPER_LONG_SECRET_KEY_FOR_JWT_123456789"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return Results.Ok(new { token = jwt });


        });
    }
}