using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MintCart.Models;

[Table("Usermaster")]
public class Usermaster
{
    [Key]
    public int intUserId { get; set; }
    public string vchUserName { get; set; } = default!;
    public string vchName { get; set; } = default!;
    public bool bitIsActive { get; set; }
}
