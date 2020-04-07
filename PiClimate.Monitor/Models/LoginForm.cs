using System.ComponentModel.DataAnnotations;

namespace PiClimate.Monitor.Models
{
  public class LoginForm
  {
    [Required]
    [DataType(DataType.Text)]
    public string Name { get; set; } = "";

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = "";

    public bool Remember { get; set; } = false;
  }
}
