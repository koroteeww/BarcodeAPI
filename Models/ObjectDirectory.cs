using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarcodeAPI.Models
{
    [Table("ObjectDirectory")]
    /// <summary>
    /// Справочник объектов, для которых генерятся ШК.
    /// </summary>
    public class ObjectDirectory
    {
        [Column("id_ObjectDirectory")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id_ObjectDirectory { get; set; }
        [StringLength(500)]
        /// <summary>
        /// Название объекта. 
        /// Списание, паллет, консолидированная коробка и пр.
        /// </summary>
        public string ObjectTitle { get; set; }
        [StringLength(2)]
        /// <summary>
        /// Префикс Объекта
        /// </summary>
        public string ObjectPrefix { get; set; }
        /// <summary>
        /// дата начала использования объекта, создания записи
        /// </summary>
        public DateTime CreationDate { get; set; }
    }
}
