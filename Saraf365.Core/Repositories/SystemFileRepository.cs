using Saraf365.Core.RepositoryDefinition;
using RockCandy.Web.Framework.Core.Enumerations;
using RockCandy.Web.Framework.Core.HelperClasses;
using RockCandy.Web.Framework.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Saraf365.Core;
using System.IO;
using System.Security.Cryptography;
using System.Drawing.Imaging;
using System.Drawing;
using System.Web;

namespace Saraf365.Core.Repositories
{
    public class SystemFileRepository : GenericRepository<SystemFile>
    {
        public SystemFileRepository() { }
        public SystemFileRepository(Saraf365Entities db, bool disableLazyLoading = false) : base(db,disableLazyLoading) { }

        public Expression<Func<SystemFile, bool>> GetPredicate(List<SearchModel> searchModels)
        {
            List<Expression<Func<SystemFile, bool>>> predicates = new List<Expression<Func<SystemFile, bool>>>();
            foreach (var item in searchModels)
            {
                if (item.IsInvolve)
                {
                    switch (item.MentionField.Name)
                    {
                        case "xFileName":
                            Expression<Func<SystemFile, bool>> temp = null;
                            switch (item.NoneNumericalOperation)
                            {
                                case NoneNumericalOperationType.Equal:
                                    temp = t => t.xFileName == (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.NotEqual:
                                    temp = t => t.xFileName != (string)item.Value;
                                    break;
                                case NoneNumericalOperationType.Contains:
                                    temp = t => t.xFileName.Contains((string)item.Value);
                                    break;
                                case NoneNumericalOperationType.EmptyOrNull:
                                    temp = t => t.xFileName == "" || t.xFileName == null;
                                    break;
                                default:
                                    break;
                            }
                            if (temp != null)
                                predicates.Add(temp);
                            break;
                        

                        default:
                            break;
                    }
                }
            }
            Expression<Func<SystemFile, bool>> res = null;
            ParameterExpression op = Expression.Parameter(typeof(SystemFile), "SystemFile");
            foreach (var item in predicates)
            {
                if (res == null)
                {
                    res = item;
                }
                else
                {

                    res = res.And(item);

                }

            }

            return res;
            //http://blogs.msdn.com/b/meek/archive/2008/05/02/linq-to-entities-combining-predicates.aspx
        }

        public SystemFile GetByMd5(string msd5)
        {
            return (from s in db.SystemFile where s.xMD5==msd5 select s).SingleOrDefault();
        }

        public void DeleteWithFileData(long systemFileID)
        {
            

            
            using (FileDataRepository fdr = new FileDataRepository())
            {
                foreach (var item in fdr.GetBySystemFileID(systemFileID))
                {
                    fdr.Delete(item.xID);
                }

            }
            Delete(systemFileID);
            

        }

        public long InsertImageFormTelegramBot(MemoryStream file,string fileName, bool generateThumbnail = true)
        {

          
            SystemFile sfInstance = new SystemFile();


            file.Seek(0, SeekOrigin.Begin);
            byte[] buffer = new byte[file.Length];
            file.Read(buffer, 0, buffer.Length);


            sfInstance.xAddingDate = DateTime.Now;
            sfInstance.xContentType = "image/jpeg";
            sfInstance.xFileExtention = Path.GetExtension(fileName);
            sfInstance.xFileName = new Random().Next(111111, 999999) + "_" + fileName;
            sfInstance.xIsActive = true;
            using (SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider())
            {
                sfInstance.xMD5 = Convert.ToBase64String(sha1.ComputeHash(buffer));
            }

            var currentlyExist = GetByMd5(sfInstance.xMD5);
            if(currentlyExist!=null)
            {
                return currentlyExist.xID;
            }



            var fdInstance = new FileData();
            fdInstance.xData = buffer;
            fdInstance.xIsThumbnail = false;

            sfInstance.FileData.Add(fdInstance);

            if (generateThumbnail)
            {
                try
                {
                    file.Seek(0, SeekOrigin.Begin);
                    Image image = Image.FromStream(file);
                    Image thumb = null;
                    if (image.Width >= image.Height)
                    {
                        int newWidth = 500;
                        if (image.Width > 500)
                        {
                            int ratio = newWidth * 100 / image.Width;
                            int newHeight = ratio * image.Height / 100;
                            thumb = image.GetThumbnailImage(newWidth, newHeight, () => false, IntPtr.Zero);

                        }

                    }
                    else
                    {
                        int newHeight = 500;
                        if (image.Height > 500)
                        {
                            int ratio = newHeight * 100 / image.Height;
                            int newWidth = ratio * image.Width / 100;
                            thumb = image.GetThumbnailImage(newWidth, newHeight, () => false, IntPtr.Zero);

                        }

                    }

                    if (thumb != null)
                    {
                        MemoryStream ms = new MemoryStream();

                        var fdtInstance = new FileData();
                        thumb.Save(ms, image.RawFormat);
                        fdtInstance.xData = ms.ToArray();
                        fdtInstance.xIsThumbnail = true;

                        sfInstance.FileData.Add(fdtInstance);
                    }



                }
                catch
                {
                    return -2; ;//exception in generating thumbnail
                }
            }




            Insert(sfInstance);
            Save();
            return sfInstance.xID;
        }
        public long InsertImage(HttpPostedFileBase file, List<string> validExtentions,long maxFileSize,bool generateThumbnail=true)
        {

            try
            {
                Image image = Image.FromStream(file.InputStream);
            }
            catch
            {
                return -1;//invalid image
            }

            if (validExtentions.IndexOf(Path.GetExtension(file.FileName)) < 0)
            {
                return -3;//invalid Extention
            }


            if (file.ContentLength> maxFileSize)
            {
                return -4;//invalid FileSize
            }

            SystemFile sfInstance = new SystemFile();


            file.InputStream.Seek(0, SeekOrigin.Begin);
            byte[] buffer = new byte[file.InputStream.Length];
            file.InputStream.Read(buffer, 0, buffer.Length);


            sfInstance.xAddingDate = DateTime.Now;
            sfInstance.xContentType = file.ContentType;
            sfInstance.xFileExtention = Path.GetExtension(file.FileName);
            sfInstance.xFileName = new Random().Next(111111, 999999) + "_" + file.FileName;
            sfInstance.xIsActive = true;
            using (SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider())
            {
                sfInstance.xMD5 = Convert.ToBase64String(sha1.ComputeHash(buffer));
            }
            var currentlyExist = GetByMd5(sfInstance.xMD5);
            if (currentlyExist != null)
            {
                return currentlyExist.xID;
            }

            var fdInstance = new FileData();
            fdInstance.xData = buffer;
            fdInstance.xIsThumbnail = false;

            sfInstance.FileData.Add(fdInstance);

            if (generateThumbnail)
            {
                try
                {
                    file.InputStream.Seek(0, SeekOrigin.Begin);
                    Image image = Image.FromStream(file.InputStream);
                    Image thumb = null;
                    if (image.Width >= image.Height)
                    {
                        int newWidth = 500;
                        if (image.Width > 500)
                        {
                            int ratio = newWidth * 100 / image.Width;
                            int newHeight = ratio * image.Height / 100;
                            thumb = image.GetThumbnailImage(newWidth, newHeight, () => false, IntPtr.Zero);

                        }

                    }
                    else
                    {
                        int newHeight = 500;
                        if (image.Height > 500)
                        {
                            int ratio = newHeight * 100 / image.Height;
                            int newWidth = ratio * image.Width / 100;
                            thumb = image.GetThumbnailImage(newWidth, newHeight, () => false, IntPtr.Zero);

                        }

                    }

                    if (thumb != null)
                    {
                        MemoryStream ms = new MemoryStream();

                        var fdtInstance = new FileData();
                        thumb.Save(ms, image.RawFormat);
                        fdtInstance.xData = ms.ToArray();
                        fdtInstance.xIsThumbnail = true;

                        sfInstance.FileData.Add(fdtInstance);
                    }



                }
                catch
                {
                    return -2; ;//exception in generating thumbnail
                }
            }

           


            Insert(sfInstance);
            Save();
            return sfInstance.xID;
        }


    }
}
