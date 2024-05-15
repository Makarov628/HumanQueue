

using HQ.Domain.Common.Models;
using HQ.Domain.UserAggregate.ValueObjects;

namespace HQ.Domain.UserAggregate;

public sealed class UserAggregate: AggregateRoot<UserId> 
{
    public string LastName { get; private set; }
    public string FirstName { get; private set; }
    public string Login { get; private set; }
    public string Password { get; private set; }
    public string? Email { get; private set; }
    public bool IsAdmin { get; private set; }

    private UserAggregate(
        UserId userId,
        string lastName,
        string firstName,
        string login,
        string password,
        string? email,
        bool isAdmin
    ): base(userId)
    {
        LastName = lastName;
        FirstName = firstName;
        Login = login;
        Password = password;
        Email = email;
        IsAdmin = isAdmin;
    }

    public static UserAggregate Create(
        string lastName, 
        string firstName, 
        string login, 
        string password,
        string? email,
        bool isAdmin
    )
    {
        return new UserAggregate(
            UserId.CreateUnique(),
            lastName,
            firstName,
            login,
            password,
            email,
            isAdmin
        );
    }

    public void MarkAsAdmin()
    {
        IsAdmin = true;
    }

    public void MarkAsOperator()
    {
        IsAdmin = false;
    }

    protected UserAggregate() { }
}