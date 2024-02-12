Option Explicit On

Imports System

Public Class User
    Public Property Email As String
    Public Property Password As String
    Public Property Name As String

    Public Sub New(email As String, password As String, name As String)
        Me.Email = email
        Me.Password = PasswordHasher.HashPassword(password)
        Me.Name = name
    End Sub

    Public Function Authenticate(password As String) As Boolean
        Return PasswordHasher.HashPassword(password) = Me.Password
    End Function

    Public Sub ChangePassword(currentPassword As String, newPassword As String)
        If (Authenticate(currentPassword)) Then
            Me.Password = PasswordHasher.HashPassword(newPassword)
        End If
    End Sub
End Class
