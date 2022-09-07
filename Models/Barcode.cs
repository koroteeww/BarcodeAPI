using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarcodeAPI.Models
{
    [Table("Barcode")]
    public class Barcode
    {
        [Column("id_Barcode")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id_Barcode { get; set; }

        
        /// <summary>
        /// дата и время поступления запроса на генерацию ШК
        /// </summary>
        public DateTime DateTimeBarCodeRequest { get; set; }
        /// <summary>
        /// ИД справочника Модулей
        /// </summary>
        public long id_ModuleDirectory { get; set; }
        /// <summary>
        /// ИД справочника объектов
        /// </summary>
        public long id_ObjectDirectory { get; set; }

        [StringLength(2)]
        /// <summary>
        /// Префикс Объекта
        /// </summary>
        public string ObjectPrefix { get; set; }

        /// <summary>
        /// Идентификатор объекта, для которого генерируется ШК
        /// Номер паллета, номер списания и пр. 
        /// </summary>
        public int Identifier { get; set; }
        [StringLength(500)]
        /// <summary>
        /// Строковый Идентификатор объекта, для которого генерируется ШК
        /// применяется в том случае если невозможен числовой идентификатор
        /// </summary>
        public string StringIdentifier { get; set; }
        /// <summary>
        /// уникальный номер ШК
        /// </summary>
        public int UniqueNumber { get; set; }
        [StringLength(15)]
        /// <summary>
        /// строковое представление ШК, состоит из ObjectPrefix и UniqueNumber
        /// </summary>
        public string BarcodeString { get; set; }
    }
}
