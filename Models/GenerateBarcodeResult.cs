namespace BarcodeAPI.Models
{
    public class GenerateBarcodeResult
    {
        /// <summary>
        /// префикс из справочника Объектов
        /// </summary>
        public string ObjectPrefix { get; set; }
        /// <summary>
        /// уникальный номер ШК
        /// </summary>
        public int UniqueNumber { get; set; }
        /// <summary>
        /// строковое представление ШК, состоит из ObjectPrefix и UniqueNumber
        /// </summary>
        public string BarcodeString { get; set; }

        public GenerateBarcodeResult(string objectPrefix, int uniqueNumber, string barcodeString)
        {
            ObjectPrefix = objectPrefix;
            UniqueNumber = uniqueNumber;
            BarcodeString = barcodeString;
        }
        public GenerateBarcodeResult(Barcode bar)
        {
            ObjectPrefix = bar.ObjectPrefix;
            UniqueNumber = bar.UniqueNumber;
            BarcodeString = bar.BarcodeString;
        }
    }
}
