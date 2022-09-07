using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BarcodeAPI.Models
{
    [Table("BarcodeHistory")]
    public class BarcodeHistory
    {
        [NotMapped]
        public Barcode barcodeItem { get; set; }

        public BarcodeHistory()
        { }
        
        public BarcodeHistory(Barcode item)
        {
            this.barcodeItem = item;
            ArchivingDateTime = DateTime.UtcNow;
            IsAutoArchiving = true;

            id_ModuleDirectory = item.id_ModuleDirectory;
            id_ObjectDirectory = item.id_ObjectDirectory;
            Identifier = item.Identifier;
            StringIdentifier = item.StringIdentifier;
            ObjectPrefix = item.ObjectPrefix;
            UniqueNumber = item.UniqueNumber;
            BarcodeString = item.BarcodeString;
        }

        [Column("id_BarcodeHistory")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id_BarcodeHistory { get; set; }
        /// <summary>
        /// Дата и время переноса ШК в архив
        /// </summary>
        public DateTime ArchivingDateTime { get; set; }
        /// <summary>
        /// Признак автоматического переноса в архив.
        /// False - ШК перенесен из модуля, в котором он признан неактивным
        /// True - ШК перенесен автоматически по истечению срока хранения
        /// </summary>
        public Boolean IsAutoArchiving { get; set; }

        /// <summary>
        /// ИД справочника Модулей
        /// </summary>
        public long id_ModuleDirectory { get; set; }

        /// <summary>
        /// ИД справочника объектов
        /// </summary>
        public long id_ObjectDirectory { get; set; }

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
        [StringLength(2)]
        /// <summary>
        /// Префикс Объекта
        /// </summary>
        public string ObjectPrefix { get; set; }
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
