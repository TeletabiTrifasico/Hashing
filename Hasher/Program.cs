public static class Hasher
{
    public static string HashPassword(string password)
    {
        // Hash the password with a salt
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
}