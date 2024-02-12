using ExampleClassLibrary;
using System;

namespace ExampleSolution
{
    internal class Program
    {
        private enum CommandLineArguments
        {
            Email,
            Password
        }

        private static readonly UserRepository _repository = UserRepositoryFactory.GetUserRepository();

        static void Main(string[] args)
        {
            if (args.Length != Enum.GetValues(typeof(CommandLineArguments)).Length)
            {
                Console.WriteLine("Usage: ExampleSolution <email> <password>");
                return;
            }

            var user = _repository.FindUserByEmail(args[(int)CommandLineArguments.Email]);

            if (user != null && user.Authenticate(args[(int)CommandLineArguments.Password]))
            {
                Console.WriteLine($"Welcome, {user.Name}!");
            }
            else
            {
                Console.WriteLine("Invalid email or password.");
            }
        }
    }
}
