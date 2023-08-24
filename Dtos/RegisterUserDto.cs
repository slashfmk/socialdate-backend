using System.ComponentModel.DataAnnotations;

namespace socialbackend.Dtos;

public class RegisterUserDto
{
    
    [Required]
    public string UserName { get; set; }
    [Required]
    public string Password { get; set; }
    
}