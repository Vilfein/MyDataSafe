﻿using MyDataSafe.Model;
using MyDataSafe.Service;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MyDataSafe.ViewModel
{
    public class DataViewModel
    {
        /// <summary>
        /// Name of folder
        /// </summary>
        private string foldername = "TempFiles";
        /// <summary>
        /// Database service class
        /// </summary>
        private DBService service { get; set; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service">Reference to DB service</param>
        public DataViewModel(DBService service) => this.service = service;

        public List<DataClass> LoadAllData() => service.GetAllModels();


        public async Task<List<DataClass>> LoadAllDataAsync() => service.GetAllModels();

        public List<DataColor> LoadColors() => service.GetColors();

        public async Task<bool> SaveDataAsync(string path)
        {
            try
            {
                byte[] fileData = File.ReadAllBytes(path);
                int indexName = path.LastIndexOf("\\");
                int indexType = path.LastIndexOf('.');
                string nameOfFile = path.Substring(indexName + 1);

                string TheName = nameOfFile.Substring(0, nameOfFile.LastIndexOf("."));

                var fileEntity = new DataClass
                {
                    Name = TheName,
                    Data = fileData,
                    TypeFile = path.Substring(indexType + 1)
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

        public async Task<string> CreateFileAsync(string name)
        {
            if (!Directory.Exists(foldername))
                Directory.CreateDirectory(foldername);

            var fileEntity = LoadAllData().FirstOrDefault(f => f.Name == name);

            var pathtofile = Path.Combine(foldername, $"{fileEntity!.Name}.{fileEntity.TypeFile}");
            if (!File.Exists(pathtofile))
            {
                using (BinaryWriter bw = new BinaryWriter(new FileStream(pathtofile, FileMode.Create)))
                {
                    bw.Write(fileEntity.Data);
                    bw.Flush();
                    bw.Close();
                }
            }
            return new FileInfo(pathtofile).FullName; ;
        }

        public void UpdateFile(DataClass dataClass)
        {
            service.UpdateModel(dataClass);
        }
        public async Task UpdateFileAsync(DataClass dataClass)
        {
             await service.UpdateModelAsync(dataClass);
        }

        public async Task UpdateFileAsync(DataClass dataClass, PMethod? M)
        {
            await service.UpdateModelAsync(dataClass);
            M?.Invoke();
        }


        public delegate void PMethod();

        public void RemoveFile(DataClass dataClass, PMethod? M)
        {
            if (service.DeleteModel(dataClass))
                M?.Invoke();
        }

        public async Task RemoveFileAsync(DataClass dataClass, PMethod? M)
        {
            if (service.DeleteModel(dataClass))
                M?.Invoke();
        }
    }
}




