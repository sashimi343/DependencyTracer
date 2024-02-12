Option Explicit On

Imports System.Collections.Generic
Imports System.Linq

Public Class UserRepository
    Private users As New List(Of User) From {
        New User("administrator@example.com", "P@ssw0rd", "Administrator"),
        New User("alice1234@example.com", "P@ssw0rd", "Alice"),
        New User("bob1234@example.com", "P@ssw0rd", "Bob")
    }

    Friend Sub New()
    End Sub

    Public Function GetAllUsers() As IEnumerable(Of User)
        Return users
    End Function

    Public Function FindUserByEmail(email As String) As User
        Return users.FirstOrDefault(Function(u) u.Email = email)
    End Function
End Class
