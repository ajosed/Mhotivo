using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mhotivo.Models
{
    [Table("Pensum")]
    public class Pensum
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public virtual Course Course { get; set; }
        public virtual Grade Grade { get; set; }
    }

    public class PensumEditModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Course")]
        public int CourseId { get; set; }

        [Required]
        [Display(Name = "Grade")]
        public Grade Grade { get; set; }

    }

    public class PensumRegisterModel
    {
 
        [Required]
        [Display(Name = "Course")]
        public int CourseId { get; set; }

        [Required]
        [Display(Name = "Grade")]
        public Grade Grade { get; set; }

    }

    public class DisplayPensumModel
    {

        public int Id { get; set; }

        [Display(Name = "Course")]
        public string Course { get; set; }

        [Display(Name = "Grade")]
        public string Grade { get; set; }

    }

}