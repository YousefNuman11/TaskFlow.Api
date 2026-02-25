using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskFlow.Api.Data;
using TaskFlow.Api.DTOs;
using TaskFlow.Api.Models;


namespace TaskFlow.Api.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/register", async (AppDbContext db, RegisterDto dto) =>
        {
            // Check if user exists
            var existingUser = await db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (existingUser != null)
            {
                return Results.BadRequest("User already exists");
            }

            var user = new User
            {
                Email = dto.Email,
                // Use the FULL namespace to avoid conflicts
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            db.Users.Add(user);
            await db.SaveChangesAsync();

            return Results.Ok("User registered successfully");
        });

        app.MapPost("/login", async (AppDbContext db, LoginDto dto) =>
        {
            var user = await db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

            // Use the FULL namespace here too
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
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
                expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return Results.Ok(new { token = jwt });
        });
    }
}