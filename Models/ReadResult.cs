using System;
namespace BarcodeAPI.Models
{
    public class ReadResult
    {
        public string ModuleTitle { get; set; }
        public string ObjectTitle { get; set; }

        /// <summary>
        /// Идентификатор объекта, для которого генерируется ШК
        /// Номер паллета, номер списания и пр. 
        /// </summary>
        public int Identifier { get; set; }
        /// <summary>
        /// Строковый Идентификатор объекта, для которого генерируется ШК
        /// применяется в том случае если невозможен числовой идентификатор
        /// </summary>
        public string StringIdentifier { get; set; }
    }
}
