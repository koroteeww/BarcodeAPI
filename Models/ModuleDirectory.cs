using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarcodeAPI.Models
{
    [Table("ModuleDirectory")]
    /// <summary>
    /// Справочник модулей, которые обращаются для генерации ШК
    /// </summary>
    public class ModuleDirectory
    {
        [Column("id_ModuleDirectory")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id_ModuleDirectory { get; set; }
        [StringLength(500)]
        /// <summary>
        /// название модуля
        /// </summary>
        public string ModuleTitle { get; set; }

        
    }
}
