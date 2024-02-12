namespace ExampleClassLibrary
{
    public static class UserRepositoryFactory
    {
        public static UserRepository GetUserRepository()
        {
            return new UserRepository();
        }
    }
}
