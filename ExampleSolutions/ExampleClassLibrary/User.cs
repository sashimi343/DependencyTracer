namespace ExampleClassLibrary
{
    public class User
    {
        public string Email { get; private set; }
        public string Password { get; private set; }
        public string Name { get; set; }

        public User(string email, string password, string name)
        {
            Email = email;
            Password = PasswordHasher.HashPassword(password);
            Name = name;
        }

        public bool Authenticate(string password)
        {
            return PasswordHasher.HashPassword(password) == Password;
        }

        public void ChangePassword(string currentPassword, string newPassword)
        {
            if (Authenticate(currentPassword))
            {
                Password = PasswordHasher.HashPassword(newPassword);
            }
        }
    }
}
