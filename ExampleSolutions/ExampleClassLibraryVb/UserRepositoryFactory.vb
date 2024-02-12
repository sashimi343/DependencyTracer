Option Explicit On

Public Module UserRepositoryFactory
    Public Function GetUserRepository() As UserRepository
        Return New UserRepository()
    End Function
End Module

