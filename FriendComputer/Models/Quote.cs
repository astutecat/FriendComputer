using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FriendComputer.Models
{
  public class Quote
  {
    [Required]
    public string Author { get; set; }

    [Required]
    public string Channel { get; set; }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public string Text { get; set; }
  }
}
