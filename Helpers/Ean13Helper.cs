using System;
namespace BarcodeAPI.Helpers
{
    public static class Ean13Helper
    {
        /// <summary>
        /// Первые три цифры в штрих-коде EAN-13 обозначают региональный код разных стран. 
        /// Например, 460 – 469 — это Россия.
        /// </summary>
        public static string countryCode = "460";

        /// <summary>
        /// 13 - 1 контрольный - 3 на страну = 9
        /// </summary>
        public static string stringFormat = "D9";

        public static string getString(int uniquenum)
        {
            string num = uniquenum.ToString(stringFormat);
            string control = countControl(num, countryCode).ToString("D1");
            string res = countryCode + num + control;

            return res;
        }
        public static int countControl(string inc, string countryCode)
        {
            string barcode = countryCode + inc;
            int totalSum = 0;
            for (int i = 0; i < barcode.Length ; i++)
            {
                var digit = Convert.ToInt32(barcode.Substring(i, 1));
                //totalSum += ((i % 2 == 0) ? digit : (3 * digit));
                // This appears to be backwards but the
                // EAN-13 checksum must be calculated
                // this way to be compatible with UPC-A.
                if ( (i+1) % 2 == 0)
                { // odd
                    totalSum += digit * 3;
                }
                else
                { // even
                    totalSum += digit * 1;
                }
            }
            int check = (10 - (totalSum % 10)) % 10;
            
            //int res = (int)(Math.Ceiling(totalSum / 10.0) * 10.0 - totalSum);

            return check;
        }
    }
}
