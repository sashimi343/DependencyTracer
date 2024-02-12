using System.Collections.Generic;
using System.Linq;

namespace ExampleClassLibrary
{
    public class UserRepository
    {
        private List<User> users = new List<User>()
        {
            new User("administrator@example.com", "P@ssw0rd", "Administrator"),
            new User("alice1234@example.com", "P@ssw0rd", "Alice"),
            new User("bob1234@example.com", "P@ssw0rd", "Bob")
        };

        internal UserRepository()
        {
        }

        public IEnumerable<User> GetAllUsers()
        {
            return users;
        }

        public User FindUserByEmail(string email)
        {
            return users.FirstOrDefault(u => u.Email == email);
        }
    }
}
