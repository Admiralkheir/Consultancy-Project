using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace OfficeAppointmentSoftware.MVC.WebUI.Areas.Helper
{
    public class FileHelper
    {
        public class FileResultItem
        {
            public string UploadPath { get; set; }
            public string FileName { get; set; }
        }

        public static void RemoveAllByPath(string[] fileServerPaths)
        {
            foreach (var item in fileServerPaths)
            {
                File.Delete(item);
            }
        }
        public static void RemoveAll(IEnumerable<FileResultItem> fileResultItem, string fileServerPath)
        {
            foreach (var item in fileResultItem)
            {
                if (item.UploadPath != "")
                {
                    var physicalLocation = fileServerPath + item.UploadPath;
                    string path = HttpContext.Current.Server.MapPath(physicalLocation);
                    File.Delete(path);
                }
            }
        }

        public static Dictionary<FileResultItem, FileResultType> FileDocumentUpload(IEnumerable<HttpPostedFileBase> postedFiles, int maxSize, string filePath, string[] documentTypes, params string[] fieldParameters)
        {
            Dictionary<FileResultItem, FileResultType> dictionaryResult = new Dictionary<FileResultItem, FileResultType>();

            if (postedFiles.FirstOrDefault() != null)
            {
                foreach (var item in postedFiles)
                {
                    string contentType = item.ContentType;
                    int contentSize = item.ContentLength;
                    string fileName = Path.GetFileName(item.FileName);

                    if (contentSize <= maxSize)
                    {
                        if (ContentTypeControl(contentType, documentTypes))
                        {
                            string fieldNames = "";
                            for (int i = 0; i < fieldParameters.Length; i++)
                            {
                                fieldNames += fieldParameters[i];
                            }
                            var uploadPath = fieldNames + Guid.NewGuid().ToString().Replace("-", "") + fileName;
                            var physicalPath = Path.Combine(HttpContext.Current.Server.MapPath(filePath) + uploadPath);

                            try
                            {
                                item.SaveAs(physicalPath);
                                dictionaryResult.Add(new FileResultItem() { UploadPath = uploadPath, FileName = item.FileName }, FileResultType.Complete);
                                continue;
                            }
                            catch (Exception)
                            {
                                dictionaryResult.Add(new FileResultItem() { UploadPath = uploadPath, FileName = item.FileName }, FileResultType.Error);
                                return dictionaryResult;
                            }
                        }
                        dictionaryResult.Add(new FileResultItem() { UploadPath = "", FileName = item.FileName }, FileResultType.WrongType);
                        return dictionaryResult;
                    }
                    dictionaryResult.Add(new FileResultItem() { UploadPath = "", FileName = item.FileName }, FileResultType.SizeOver);
                    return dictionaryResult;
                }
            }
            else
            {
                dictionaryResult.Add(new FileResultItem() { UploadPath = "", FileName = "" }, FileResultType.NoneFile);
                return dictionaryResult;
            }
            return dictionaryResult;
        }

        private static bool ContentTypeControl(string fileType, string[] contentTypes)
        {
            for (int i = 0; i < contentTypes.Length; i++)
            {
                if (fileType == contentTypes[i])
                {
                    return true;
                }
            }
            return false;
        }

        public static Dictionary<string, bool> ControlMessages(FileResultType fileTypeResult, int maxSize)
        {
            Dictionary<string, bool> message;
            switch (fileTypeResult)
            {
                case FileResultType.NoneFile:
                    message = new Dictionary<string, bool>();
                    message.Add("Lütfen yüklemek için bir dosya seçiniz", false);
                    return message;
                case FileResultType.Complete:
                    message = new Dictionary<string, bool>();
                    message.Add("Dosya formatı ve boyutu geçerli", true);
                    return message;
                case FileResultType.SizeOver:
                    message = new Dictionary<string, bool>();
                    message.Add("Yüklemek istediğiniz dosya boyutu maksimum " + maxSize + " Byte olabilir. Lütfen dosya boyutunu küçültün.", false);
                    return message;
                case FileResultType.WrongType:
                    message = new Dictionary<string, bool>();
                    message.Add("Geçersiz dosya formatı", false);
                    return message;
                case FileResultType.Error:
                    message = new Dictionary<string, bool>();
                    message.Add("Bekleyenmeyen hata", false);
                    return message;
                default:
                    message = new Dictionary<string, bool>();
                    message.Add("Bekleyenmeyen hata", false);
                    return message;
            }
        }
        public enum FileResultType
        {
            NoneFile = -1,
            Complete = 1,
            SizeOver = 2,
            WrongType = 3,
            Error = 4
        }
    }
}
