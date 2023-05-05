using Microsoft.EntityFrameworkCore;
using MyDataSafe.Database;
using MyDataSafe.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyDataSafe.ViewModel
{
    public class DataViewModel
    {
        private string foldername = "TempFiles";
        private Context context { get; set; }
        public DataViewModel()
        {
            context = new Context();
           
        }
        public List<DataClass> LoadAllData() => context.Datas.ToList();

        public bool SaveData(string path)
        {
            try
            {

                byte[] fileData = File.ReadAllBytes(path);
                int indexName = path.LastIndexOf("\\");
                int indexType = path.LastIndexOf('.');
                string nameOfFile = path.Substring(indexName+1);
                var fileEntity = new DataClass
                {
                    Name = nameOfFile,
                    Data = fileData,
                    TypeFile = path.Substring(indexType+1)
                };

                context.Datas.Add(fileEntity);
                context.SaveChanges();
                return true;
            }
            catch { return false; }
        }

        public void CleanUp()
        {
            if (Directory.Exists(foldername))
                Directory.Delete(foldername, true);
        }

        public async Task<string> CreateFile(string name)
        {
            if (!Directory.Exists(foldername))
                Directory.CreateDirectory(foldername);

            var fileEntity = LoadAllData().FirstOrDefault(f => f.Name == name);
            var pathtofile = Path.Combine(foldername, fileEntity!.Name);

            using (BinaryWriter bw = new BinaryWriter(new FileStream(pathtofile, FileMode.Create)))
            {
                bw.Write(fileEntity.Data);
                bw.Flush();
                bw.Close();
            }        
            return new FileInfo(pathtofile).FullName; ;
        }
    }
}




