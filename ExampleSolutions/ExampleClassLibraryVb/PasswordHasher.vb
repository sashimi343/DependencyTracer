Option Explicit On

Imports System.Linq
Imports System.Security.Cryptography
Imports System.Text

Public Class PasswordHasher
    Public Shared Function HashPassword(password As String) As String
        Return SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(password)).Aggregate(New StringBuilder(), Function(sb, b) sb.AppendFormat("{0:x2}", b)).ToString()
    End Function
End Class
