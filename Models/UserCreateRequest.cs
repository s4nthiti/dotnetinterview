using System.ComponentModel.DataAnnotations;

namespace DotnetInterview.Models;

public class UserCreateRequest
{
    [Required, MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required, EmailAddress, MaxLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required, RegularExpression(@"^0\d{9}$", ErrorMessage = "Phone must be 10 digits starting with 0")]
    public string Phone { get; set; } = string.Empty;

    [Required]
    public string ProfileBase64 { get; set; } = string.Empty;

    [Required, RegularExpression(@"^\d{2}/\d{2}/\d{4}$", ErrorMessage = "BirthDay must be DD/MM/YYYY")]
    public string BirthDay { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string Occupation { get; set; } = string.Empty;

    [Required, MaxLength(20)]
    public string Gender { get; set; } = string.Empty;
}
