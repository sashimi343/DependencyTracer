Option Explicit On

Imports ExampleClassLibraryVb

Module Module1

    Private ReadOnly _repository As UserRepository = UserRepositoryFactory.GetUserRepository()

    Private Enum CommandLineArguments
        Email
        Password
    End Enum

    Sub Main(args As String())
#If DEBUG Then
        args = New String() {"administrator@example.com", "P@ssw0rd"}
#End If
        If args.Length <> [Enum].GetValues(GetType(CommandLineArguments)).Length Then
            Console.WriteLine("Usage: ExampleSolution <email> <password>")
            Return
        End If

        Dim user = _repository.FindUserByEmail(args(CType(CommandLineArguments.Email, Integer)))

        If user IsNot Nothing AndAlso user.Authenticate(args(CType(CommandLineArguments.Password, Integer))) Then
            Console.WriteLine($"Welcome, {user.Name}!")
        Else
            Console.WriteLine("Invalid email or password.")
        End If
    End Sub

End Module
