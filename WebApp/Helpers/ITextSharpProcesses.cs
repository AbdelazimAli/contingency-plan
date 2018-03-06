using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using Db.Persistence;


namespace WebApp.Helpers
{
    public class ITextSharpProcesses
    {
       public static byte[] CompineFilesIntoPDF(HttpPostedFileBase[] UploadedFiles, out string contentType)
        {
            string Dirpath = Path.GetTempPath();// HttpContext.Current.Server.MapPath("~/Files/PDFs");
            List<string> AllCurrentFiles = new List<string>();
            contentType = string.Empty;
            try
            {
                byte[] Result;

                if (!CanCompineFiles(UploadedFiles, out Result, out contentType))
                    return Result;

               
                //if (!Directory.Exists(Dirpath))
                    //Directory.CreateDirectory(Dirpath);

                Random Rnd_FileNames = new Random();

                string FileNameWithExt = GetRandomFileName(Rnd_FileNames) + ".pdf";
                string FileNameWithExt_ImagePDF = GetRandomFileName(Rnd_FileNames) + ".pdf";

                string FullPath = Dirpath + "/" + FileNameWithExt;
                string FullPath_ImagePDF = Dirpath + "/" + FileNameWithExt_ImagePDF;

                AllCurrentFiles.Add(FileNameWithExt);
                AllCurrentFiles.Add(FileNameWithExt_ImagePDF);

                DeleteIfExists(Dirpath, AllCurrentFiles);
                CreateDocument(Dirpath, FullPath, FullPath_ImagePDF, UploadedFiles, ref AllCurrentFiles, Rnd_FileNames);

                ReadBytes_ContentType(FullPath, out Result, out contentType);
                DeleteIfExists(Dirpath, AllCurrentFiles);

                return Result;

            }
            catch
            {
                DeleteIfExists(Dirpath, AllCurrentFiles);
                return null;
            }
        }

      
        private static string GetRandomFileName(Random rnd)
        {
            string BasicName = DateTime.Now.ToString("yyyyMMddmmss") + DateTime.Now.Millisecond.ToString();
            try
            {
                return rnd.Next(9999, 9999999).ToString() + BasicName;
            }
            catch
            {
                return BasicName;
            }
        }
        private static bool CanCompineFiles(HttpPostedFileBase[] UploadedFiles, out byte[] Result, out string contentType)
        {
            contentType = string.Empty;
            Result = null;
            try
            {
                if (UploadedFiles == null)
                {
                    return false;
                }

             
                if (UploadedFiles.Count() == 1 /*!Images[0].ContentType.Contains("doc")*/)
                {
                    Constants.Enumerations.UploadFileTypesEnum FileType = GetFileType(UploadedFiles[0]);
                    if (FileType == Constants.Enumerations.UploadFileTypesEnum.Pdf || FileType == Constants.Enumerations.UploadFileTypesEnum.Image)
                    {
                        contentType = UploadedFiles[0].ContentType.Replace("application/octet-stream", "image/jpeg");
                        Result = ImageProcesses.ReadFully(UploadedFiles[0].InputStream);
                        return false;
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        private static void CreateDocument(string Dirpath, string FullPath, string FullPath_ImagePDF, HttpPostedFileBase[] Images, ref List<string> AllCurrentFiles, Random Rnd_FileNames)
        {
            bool ImagePDFDocumentCreated = CompineImages(FullPath_ImagePDF, Images);
            CompinePDFFiles(ImagePDFDocumentCreated, FullPath, FullPath_ImagePDF, Images, Rnd_FileNames, Dirpath, AllCurrentFiles);

        }
        public static void CompinePDFFiles(bool ImagePDFDocumentCreated, string FullPath, string FullPath_ImagePDF, HttpPostedFileBase[] Images, Random Rnd_FileNames, string Dirpath, List<string> AllCurrentFiles)
        {
            using (FileStream stream_FinalFile = new FileStream(FullPath, FileMode.OpenOrCreate))
            {
                Document pdfDoc = new Document();
                PdfCopy pdf = new PdfCopy(pdfDoc, stream_FinalFile);
                pdfDoc.Open();

                if (ImagePDFDocumentCreated)
                    pdf.AddDocument(new PdfReader(FullPath_ImagePDF));

                foreach (var im in Images)
                {

                    Constants.Enumerations.UploadFileTypesEnum FileType = GetFileType(im);
                    switch (FileType)
                    {
                        case Constants.Enumerations.UploadFileTypesEnum.Pdf:
                            pdf.AddDocument(new PdfReader(im.InputStream));
                            break;

                        //case Constants.Enumerations.UploadFileTypesEnum.Word:
                        //    string RandomFName = GetRandomFileName(Rnd_FileNames);

                        //    string CurrentWordRandomFileName = RandomFName + Path.GetExtension(im.FileName);
                        //    string NewPdfRandomFileName = RandomFName + ".pdf";

                        //    string CurrentWordFullPath = Dirpath + @"\" + CurrentWordRandomFileName;
                        //    string NewPdfFullPath = Dirpath + @"\" + NewPdfRandomFileName;

                        //    CovertWordToPDF(im, CurrentWordFullPath, NewPdfFullPath);

                        //    AllCurrentFiles.Add(CurrentWordRandomFileName);
                        //    AllCurrentFiles.Add(NewPdfRandomFileName);

                        //    pdf.AddDocument(new PdfReader(NewPdfFullPath));
                        //    break;
                      

                        default:
                            break;
                    }

                }
                pdfDoc.Close();
            }
        }
        private static bool CompineImages(string FullPath_ImagePDF, HttpPostedFileBase[] Images)
        {
            bool ImagePDFDocumentCreated = false;
            try
            {
                var stream_Images = new FileStream(FullPath_ImagePDF, FileMode.OpenOrCreate);

                var doc_Images = new Document();
                PdfWriter.GetInstance(doc_Images, stream_Images);
                doc_Images.Open();

                foreach (var im in Images)
                {
                    if (/*IsImage(im)*/GetFileType(im) == Constants.Enumerations.UploadFileTypesEnum.Image)
                    {
                        Image png = Image.GetInstance(im.InputStream);
                        png.ScaleToFit(doc_Images.PageSize.Width - 100, doc_Images.PageSize.Height - 100);
                        png.Border = Rectangle.BOX;
                        png.BorderColor = BaseColor.WHITE;
                        png.BorderWidth = 3;
                        png.Alignment = Element.ALIGN_CENTER;
                        doc_Images.Add(png);

                        ImagePDFDocumentCreated = true;
                    }

                }

                if (ImagePDFDocumentCreated)
                    doc_Images.Close();
            }
            catch
            {

            }
            return ImagePDFDocumentCreated;
        }

   

        //private static void CovertWordToPDF(HttpPostedFileBase wordFile, string CurrentWordFullPath, string NewPdfFullPath)
        //{

        //    try
        //    {
        //        // Create a new Microsoft Word application object
        //        Microsoft.Office.Interop.Word.Application word = new Microsoft.Office.Interop.Word.Application();

        //        // C# doesn't have optional arguments so we'll need a dummy value
        //        object oMissing = System.Reflection.Missing.Value;

        //        word.Visible = false;
        //        word.ScreenUpdating = false;

        //        wordFile.SaveAs(CurrentWordFullPath);

        //        // Cast as Object for word Open method
        //        Object filename = (Object)CurrentWordFullPath;

        //        // Use the dummy value as a placeholder for optional arguments
        //        Microsoft.Office.Interop.Word.Document doc = word.Documents.Open(ref filename, ref oMissing,
        //            ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
        //            ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
        //            ref oMissing, ref oMissing, ref oMissing, ref oMissing);
        //        doc.Activate();

        //        // NewPdfFullPath = CurrentWordFullPath.Replace(".docx", ".pdf").Replace(".doc", ".pdf");
        //        object outputFileName = (object)NewPdfFullPath;
        //        object fileFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF;

        //        // Save document into PDF Format
        //        doc.SaveAs(ref outputFileName,
        //            ref fileFormat, ref oMissing, ref oMissing,
        //            ref oMissing, ref oMissing, ref oMissing, ref oMissing,
        //            ref oMissing, ref oMissing, ref oMissing, ref oMissing,
        //            ref oMissing, ref oMissing, ref oMissing, ref oMissing);

        //        // Close the Word document, but leave the Word application open.
        //        // doc has to be cast to type _Document so that it will find the
        //        // correct Close method.                
        //        object saveChanges = Microsoft.Office.Interop.Word.WdSaveOptions.wdDoNotSaveChanges;
        //        ((Microsoft.Office.Interop.Word._Document)doc).Close(ref saveChanges, ref oMissing, ref oMissing);
        //        doc = null;

        //        // word has to be cast to type _Application so that it will find
        //        // the correct Quit method.
        //        ((Microsoft.Office.Interop.Word._Application)word).Quit(ref oMissing, ref oMissing, ref oMissing);
        //        word = null;
        //    }
        //    catch
        //    {

        //    }
        //}


        private static void ReadBytes_ContentType(string FullPath, out byte[] Result, out string contentType)
        {
            Result = null;
            contentType = string.Empty;
            try
            {
                FileStream fStream = new FileStream(FullPath, FileMode.Open, FileAccess.Read);
                contentType = MimeMapping.GetMimeMapping(FullPath);
                Result = ImageProcesses.ReadFully(fStream);
                fStream.Close();
            }
            catch
            {

            }
        }

        private static void DeleteIfExists(string Dirpath, List<string> AllCurrentFiles)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            AllCurrentFiles.ForEach(a =>
            {
                string FullPath = Dirpath + "/" + a;
                if (File.Exists(FullPath))
                    File.Delete(FullPath);
            });

        }

        private static Constants.Enumerations.UploadFileTypesEnum GetFileType(HttpPostedFileBase f)
        {
            try
            {
                if (f.ContentType.Contains("image")||f.ContentType.Contains("octet-stream"))
                    return Constants.Enumerations.UploadFileTypesEnum.Image;

                if (f.ContentType.Contains("wordprocessingml"))
                    return Constants.Enumerations.UploadFileTypesEnum.Word;

                if (f.ContentType.Contains("pdf"))
                    return Constants.Enumerations.UploadFileTypesEnum.Pdf;
            }
            catch
            {

            }
            return Constants.Enumerations.UploadFileTypesEnum.Unknown;
        }
    }
}