﻿using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace OnePageTest.Extention
{
    public static class FileManager
    {
        //public static bool IsImage(this IFormFile file)
        //{
        //    return file.ContentType.Contains("image/");
        //}
        //public static bool IsSizeOk(this IFormFile file,int mb)
        //{
        //    return file.Length / 1024 / 1024 <= mb;
        //}
        //public static string SaveImage(this IFormFile file,string root,string folder)
        //{
        //    string rootPath = Path.Combine(root,folder);
        //    string fileName = Guid.NewGuid().ToString() + file.FileName;
        //    string fullPath = Path.Combine(rootPath,fileName);
        //    using (FileStream fs=new FileStream(fullPath,FileMode.Create))
        //    {
        //        file.CopyTo(fs);
        //    }
        //    return fileName;
        //}

        public static bool IsImage(this IFormFile file)
        {
            return file.ContentType.Contains("image/");
        }
        public static bool IsSizeOk(this IFormFile file,int mb)
        {
            return file.Length / 1024 / 1024 <= mb;
        }
        public static string SaveImage(this IFormFile file,string root,string folder)
        {
            string path = Path.Combine(root,folder);
            string fileName=Guid.NewGuid().ToString()+file.FileName;
            string fullPath = Path.Combine(path,fileName);
            using (FileStream fs=new FileStream(fullPath,FileMode.Create))
            {
                file.CopyTo(fs);
            }
            return fileName;
        }
    }
}
