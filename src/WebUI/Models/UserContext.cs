using System.Security.Principal;

namespace CleanArchitecture.WebUI.Models;

public class UserContext : IUserContext
{
    public string HashedAccountId { get; set; } = default!;
    public IPrincipal User { get; set; } = default!;
}
