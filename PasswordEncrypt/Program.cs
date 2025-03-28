// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");


string password_encrypt = BCrypt.Net.BCrypt.HashPassword("adminpassword");

Console.WriteLine(password_encrypt);
Console.ReadLine();