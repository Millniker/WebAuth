using System.ComponentModel.DataAnnotations;

namespace Pathnostics.Web.Models.DTO;

public class UserDto
{
    [EmailAddress]
    public string Email { get; set; }
    public string Password { get; set; }
}