using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using BarcodeAPI.Models;
using System.Text;
using BarcodeAPI.DB;
using System.Linq;
using Swashbuckle.AspNetCore.Annotations;
using BarcodeAPI.Helpers;

namespace BarcodeAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class BarcodeController : Controller
    {
        private readonly ILogger<BarcodeController> _logger;
        private readonly BarcodeDbContext dbContext;
        private Random rnd;

        public BarcodeController(ILogger<BarcodeController> logger, BarcodeDbContext dbContextt)
        {
            _logger = logger;
            dbContext = dbContextt;
            rnd = new Random();
        }

        [HttpGet]
        [SwaggerOperation(Summary = "тестовая операция, проверка соеднения с БД", OperationId = "GetBarcode", Description = "тестовая операция, проверка соеднения с БД")]
        public string GetBarcode()
        {
            //test DB
            var bc = dbContext.Barcode.First();
            string bs = bc.BarcodeString;
            var module = dbContext.ModuleDirectory.Where(m => m.id_ModuleDirectory == bc.id_ModuleDirectory).First();
            var obj = dbContext.ObjectDirectory.Where(ob => ob.id_ObjectDirectory == bc.id_ObjectDirectory).First();

            string res = TestGenerateBarcode();
            res = res + " из БД первый ШК: " + bs + " Модуль=" + module.ModuleTitle + " Объект=" + obj.ObjectTitle;
            return res;
        }

        /// <summary>
        /// Одна из букв префикса должна явно указывать на то, что буквы - английские
        /// уникальный номер 6-9 цифр
        /// </summary>
        /// <returns>строка штрихкода</returns>
        private string TestGenerateBarcode()
        {
            string engLetter = EngLetters.EngLettersArray[rnd.Next(EngLetters.EngLettersArray.Length)];
            string secondLetter = EngLetters.AvailableLettersArray[rnd.Next(EngLetters.AvailableLettersArray.Length)];
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 8; i++)
            {
                sb.Append(rnd.Next(1, 10).ToString());
            }
            return engLetter + secondLetter + sb.ToString();
        }

        [HttpPost]
        [SwaggerOperation(Summary = "запрос на генерацию ШК")]
        public List<GenerateBarcodeResult> GenerateBarcode(string ModuleTitle, string ObjectTitle, int Identifier, string StringIdentifier, int count)
        {
            //Обратиться с запросом на генерацию ШК может любой метод 
            //Входные данные: ModuleTitle, ObjectTitle, Identifier, количество генерируемых ШК
            //Действия: цикл по количеству генерируемых ШК
            //Определение последнего уникального номера для указанного префикса, +1 к номеру, Новая запись в БД
            //Выходные данные: ObjectPrefics, UniqueNumber или код ошибки

            List<GenerateBarcodeResult> result = new List<GenerateBarcodeResult>();
            var module = dbContext.ModuleDirectory.Where(m => m.ModuleTitle == ModuleTitle).First();
            var obj = dbContext.ObjectDirectory.Where(ob => ob.ObjectTitle == ObjectTitle).First();
            //check
            if (module == null)
            {
                //не создаем, бросаем исключение
                throw new Exception("module not found");
            }

            if (obj == null)
            {
                //не создаем, бросаем исключение
                throw new Exception("object not found");
            }
            //search last (max) number with prefix
            var whereres = dbContext.Barcode.Where(bar => bar.ObjectPrefix == obj.ObjectPrefix);
            int lastbarcode = 0;
            //what if whereres not found?
            if (whereres == null || whereres.Count() == 0)
            {
                lastbarcode = 0;
            }
            else
            {
                lastbarcode = whereres.Max(oo => oo.UniqueNumber);
            }
            

            for (int i = 0; i < count; i++)
            {
                Barcode bar = new Barcode();

                if (string.IsNullOrEmpty(StringIdentifier))
                    bar.Identifier = Identifier;
                else
                    bar.StringIdentifier = StringIdentifier;
                //fields
                bar.id_ObjectDirectory = obj.id_ObjectDirectory;
                bar.ObjectPrefix = obj.ObjectPrefix;
                bar.id_ModuleDirectory = module.id_ModuleDirectory;
                bar.DateTimeBarCodeRequest = DateTime.UtcNow;
                //unique number
                bar.UniqueNumber = lastbarcode + i + 1;
                //string length 10 (2 prefix + 8 digits)
                bar.BarcodeString = bar.ObjectPrefix + Ean13Helper.getString(bar.UniqueNumber);
                //add to db
                dbContext.Barcode.Add(bar);
                //add to list result
                GenerateBarcodeResult res = new GenerateBarcodeResult(bar);

                result.Add(res);
            }
            //save db
            dbContext.SaveChanges();

            return result;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "запрос на чтение ШК по префиксу и числовому номеру шк")]
        public ReadResult Read(string objectPrefix, int UniqueNumber)
        {
            //Входные данные: ObjectPrefics, UniqueNumber
            //Действия: чтение строки из db_BarCode или db_BarCodeHistory, удовлетворяющей условию
            //Выходные данные: ModuleTitle, ObjectTitle, Identifier
            ReadResult read = new ReadResult();
            var whereres = dbContext.Barcode.Where(bar=>bar.ObjectPrefix==objectPrefix && bar.UniqueNumber==UniqueNumber);
            if (whereres == null || whereres.Count()==0 )
            {
                var wherebchist = dbContext.BarcodeHistory.Where(bar => bar.UniqueNumber == UniqueNumber && bar.ObjectPrefix == objectPrefix);
                if (wherebchist != null && wherebchist.Count() > 0)
                {
                    var bchist = wherebchist.First();
                    var obj = dbContext.ObjectDirectory.Where(ob => ob.id_ObjectDirectory == bchist.id_ObjectDirectory).FirstOrDefault();
                    var module = dbContext.ModuleDirectory.Where(m => m.id_ModuleDirectory == bchist.id_ModuleDirectory).FirstOrDefault();
                    read.Identifier = bchist.Identifier;
                    read.StringIdentifier = bchist.StringIdentifier;
                    read.ObjectTitle = obj.ObjectTitle;
                    read.ModuleTitle = module.ModuleTitle;                    
                }
            }
            else
            {
                var bc = whereres.First();
                var obj = dbContext.ObjectDirectory.Where(ob => ob.id_ObjectDirectory == bc.id_ObjectDirectory).FirstOrDefault();
                var module = dbContext.ModuleDirectory.Where(m => m.id_ModuleDirectory == bc.id_ModuleDirectory).FirstOrDefault();
                read.Identifier = bc.Identifier;
                read.StringIdentifier = bc.StringIdentifier;
                read.ObjectTitle = obj.ObjectTitle;
                read.ModuleTitle = module.ModuleTitle;
            }

            return read;
        }
        [HttpGet]
        [SwaggerOperation(Summary = "запрос на чтение ШК по строке, для ТСД")]
        public ReadResult ReadTSD(string scannedCode)
        {

            //Получил комментарий зачем нужно две таблицы: ШК(db_BarCode) и Архив(История db_BarCodeHistory)
            //По идее первая таблица должна быть быстрее(оперативнее отвечать, лучше проиндексирована) потому что ее запрашивают ТСД по вайфай
            //
            //А после 90 дней товар уже точно "доедет" и нет потребности быстро сканировать, можно и поискать подольше
            //
            //поэтому надо думать как ускорить поиск для ТСД
            //возможно сделать отдельный метод АПИ, возможно добавить индексы в таблицу db_BarCode
            //
            ReadResult read = new ReadResult();
            //
            //какие есть варианты: 1) индекс по BarcodeString 
            //2)если мы знаем что формат на входе всегда 2 буквы и номер
            //  - можно отсечь 2 буквы в начале и искать по номеру UniqueNumber
            //
            //предусмотреть вариант когда scannedCode это число и поискать по столбцам UniqueNumber и Identifier
            int scannedInt;
            bool isNumeric = int.TryParse(scannedCode, out scannedInt);
            if (isNumeric)
            {
                var whereId = dbContext.Barcode.Where(bar => bar.Identifier == scannedInt);
                if (whereId.Any())
                {
                    var first = whereId.FirstOrDefault();
                    var obj = dbContext.ObjectDirectory.Where(ob => ob.id_ObjectDirectory == first.id_ObjectDirectory).FirstOrDefault();
                    var module = dbContext.ModuleDirectory.Where(m => m.id_ModuleDirectory == first.id_ModuleDirectory).FirstOrDefault();
                    read.Identifier = first.Identifier;
                    read.StringIdentifier = first.StringIdentifier;
                    read.ObjectTitle = obj.ObjectTitle;
                    read.ModuleTitle = module.ModuleTitle;
                }
            }
            else
            {
                var whereres = dbContext.Barcode.Where(bar => bar.BarcodeString == scannedCode);
                if (whereres == null || whereres.Count() == 0)
                {
                    //scan history
                    var whereresH = dbContext.BarcodeHistory.Where(barh => barh.BarcodeString == scannedCode);
                    if (whereresH.Count() > 0)
                    {
                        var bchist = whereresH.First();
                        var obj = dbContext.ObjectDirectory.Where(ob => ob.id_ObjectDirectory == bchist.id_ObjectDirectory).FirstOrDefault();
                        var module = dbContext.ModuleDirectory.Where(m => m.id_ModuleDirectory == bchist.id_ModuleDirectory).FirstOrDefault();
                        read.Identifier = bchist.Identifier;
                        read.StringIdentifier = bchist.StringIdentifier;
                        read.ObjectTitle = obj.ObjectTitle;
                        read.ModuleTitle = module.ModuleTitle;
                    }
                }
                else
                {
                    var bc = whereres.First();
                    var obj = dbContext.ObjectDirectory.Where(ob => ob.id_ObjectDirectory == bc.id_ObjectDirectory).FirstOrDefault();
                    var module = dbContext.ModuleDirectory.Where(m => m.id_ModuleDirectory == bc.id_ModuleDirectory).FirstOrDefault();
                    read.Identifier = bc.Identifier;
                    read.StringIdentifier = bc.StringIdentifier;
                    read.ObjectTitle = obj.ObjectTitle;
                    read.ModuleTitle = module.ModuleTitle;
                }
            }
            return read;
        }
         
        [HttpGet]
        [SwaggerOperation(Summary = "принудительный запуск архивации для ШК старше 90 дней")]
        public bool LaunchArchive()
        {
            //Ненужные записи должны переносится из основной таблицы в таблицу архивных записей.Способы:
            //    Принудительно после окончания действия идентификатора, для которого генерировался ШК(на совести разработчика)
            //    Автоматически после истечения 90 дней существования ШК.
            //    Входные данные:
            //    Действия: перенос строки, удовлетворяющую условию, из db_BarCode в db_BarCodeHistory
            //    Выходные данные: нет
            DateTime dtNow = DateTime.UtcNow;
            List<BarcodeHistory> toArchive = new List<BarcodeHistory>();
            var allBarcodes = dbContext.Barcode.ToList();
            foreach (var item in allBarcodes)
            {
                //check dt
                if ((dtNow - item.DateTimeBarCodeRequest).TotalDays > 90)
                {
                    //archive
                    BarcodeHistory hist = new BarcodeHistory(item);
                    toArchive.Add(hist);
                }
            }
            //process archive
            foreach (var archItem in toArchive)
            {
                dbContext.BarcodeHistory.Add(archItem);
                dbContext.Barcode.Remove(archItem.barcodeItem);
            }

            if (toArchive.Count > 0)
            {
                dbContext.SaveChanges();
                return true;
            }
            else
                return false;
            
        }

        [HttpGet]
        [SwaggerOperation(Summary = "принудительный запуск архивации для ШК по идентификатору Объекта для которого был сгенерирован ШК")]
        public bool LaunchArchiveById(int identifier)
        {
            //Ненужные записи должны переносится из основной таблицы в таблицу архивных записей.Способы:
            //    Принудительно после окончания действия идентификатора, для которого генерировался ШК(на совести разработчика)
            //    Автоматически после истечения 90 дней существования ШК.
            //    Входные данные:
            //    Действия: перенос строки, удовлетворяющую условию, из db_BarCode в db_BarCodeHistory
            //    Выходные данные: 
            
            var whereRes = dbContext.Barcode.Where(bar => bar.Identifier == identifier);
            if (whereRes.Count() > 0)
            {
                DateTime dtNow = DateTime.UtcNow;
                List<BarcodeHistory> toArchive = new List<BarcodeHistory>();
                var allBarcodes = whereRes.ToList();
                foreach (var item in allBarcodes)
                {
                    //archive
                    BarcodeHistory hist = new BarcodeHistory(item);
                    hist.IsAutoArchiving = false;
                    toArchive.Add(hist);
                }

                //process archive
                foreach (var archItem in toArchive)
                {
                    dbContext.BarcodeHistory.Add(archItem);
                    dbContext.Barcode.Remove(archItem.barcodeItem);
                }

                if (toArchive.Count > 0)
                {
                    dbContext.SaveChanges();
                }

                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
