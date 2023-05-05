using Microsoft.EntityFrameworkCore;
using MyDataSafe.Database;
using MyDataSafe.Model;
using MyDataSafe.Service;
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
        private DBService service { get; set; }
        public DataViewModel(DBService service) => this.service = service;
        
        public List<DataClass> LoadAllData() => service.GetAllModels();

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

               service.SaveModel(fileEntity);
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

        public delegate void Metoda();

        public void RemoveFile(DataClass dataClass, Metoda M)
        {
            if (service.DeleteModel(dataClass))
                MessageBox.Show("deleted");
            M();
        }
    }
}




