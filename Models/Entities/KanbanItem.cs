using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Squabble.Models.Entities;

namespace Squabble.Models
{
    public class KanbanItem
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int KanbanItemID { get; set; }
        [ForeignKey("User")]
        [Required]
        public int UserID { get; set; }
        public virtual User User { get; set; }

        [RegularExpression("^.{1,20}$", ErrorMessage = "The list name can't be more than 20 characters.")]
        [Required(ErrorMessage = "The list must have a name.")]
        public string ListName { get; set; }

        [RegularExpression("^.{1,20}$", ErrorMessage = "The item name can't be more than 20 characters.")]
        [Required(ErrorMessage = "The item must have a name.")]
        public string ItemName { get; set; }

        [Required(ErrorMessage = "There must be a position.")]
        public int Position { get; set; }
    }
}
