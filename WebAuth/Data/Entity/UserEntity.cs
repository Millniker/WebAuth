using Microsoft.AspNetCore.Identity;

namespace Pathnostics.Web.Models.Entity;

public class UserEntity
{ 
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}