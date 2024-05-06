using System.Security.Cryptography;

namespace Pathnostics.Web.Services;

public class HashPasswords
{
    private const int SaltSize = 16; // размер соли

    public  string HashPassword(string password)
    {
        // Генерируем случайную соль
        byte[] salt;
        new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);

        // Создаем объект для хеширования пароля с использованием алгоритма SHA256
        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256);

        // Вычисляем хэш пароля
        byte[] hash = pbkdf2.GetBytes(20); // длина хэша 20 байт

        // Конвертируем соль и хэш в массив байтов
        byte[] hashBytes = new byte[36]; // 16 (размер соли) + 20 (длина хэша)
        Array.Copy(salt, 0, hashBytes, 0, SaltSize);
        Array.Copy(hash, 0, hashBytes, SaltSize, 20);

        // Конвертируем массив байтов в строку Base64
        string hashedPassword = Convert.ToBase64String(hashBytes);

        return hashedPassword;
    }
}