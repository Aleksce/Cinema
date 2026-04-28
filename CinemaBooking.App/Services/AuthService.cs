using System.Security.Cryptography;
using System.Text;
using CinemaBooking.App.Data;
using CinemaBooking.App.Models;

namespace CinemaBooking.App.Services;

public class AuthService
{
    public User? Login(string email, string password)
    {
        using var db = new CinemaDbContext();
        var hash = Hash(password);
        return db.Users.FirstOrDefault(x => x.Email == email && x.PasswordHash == hash);
    }

    public (bool Success, string Message) Register(string name, string email, string password)
    {
        using var db = new CinemaDbContext();
        if (db.Users.Any(x => x.Email == email))
        {
            return (false, "Пользователь с таким email уже существует.");
        }

        db.Users.Add(new User
        {
            Name = name,
            Email = email,
            PasswordHash = Hash(password)
        });

        db.SaveChanges();
        return (true, "Регистрация прошла успешно. Теперь можно войти.");
    }

    private static string Hash(string input)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(bytes);
    }
}
