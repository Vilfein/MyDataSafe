using Microsoft.EntityFrameworkCore;
using MyDataSafe.Database;
using MyDataSafe.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyDataSafe.Service
{
    public class DBService
    {
        private readonly Context context;
        public DBService() 
        {
            context = new Context();
        }

        /// <summary>
        /// Save the file into database
        /// </summary>
        /// <param name="DC">The file to save</param>
        public void SaveModel(DataClass DC) 
        { 
            context.Datas.Add(DC);
            context.SaveChanges();
        }
        public async Task SaveModelAsync(DataClass DC)
        {
            await context.Datas.AddAsync(DC);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Returns all stored files
        /// </summary>
        /// <returns>All stored files</returns>
        public List<DataClass> GetAllModels() => 
            context.Datas.Include(x => x.Color).ToList();

        /// <summary>
        /// Returns all stored files asynchonously
        /// </summary>
        /// <returns>All stored files</returns>
        public async Task<List<DataClass>> GetAllModelsAsync() => 
            await context.Datas.Include(x => x.Color).ToListAsync();

        /// <summary>
        /// Returns concrete model by its name.If is null throws NullReferenceException.
        /// </summary>
        /// <param name="name">Name of DataClass model</param>
        /// <returns>A model by name</returns>
        public DataClass GetModel(string name) => 
            context.Datas.Include(x => x.Color).FirstOrDefault(x => x.Name == name) ?? throw new System.NullReferenceException();

        /// <summary>
        /// Asynchronously returns a concrete model. If is null throws NullReferenceException.
        /// </summary>
        /// <param name="name">Name of DataClass model</param>
        /// <returns>A model by name</returns>
        public async Task<DataClass> GetModelAsync(string name) => 
            await context.Datas.FirstOrDefaultAsync(x => x.Name == name) ?? throw new System.NullReferenceException();

        /// <summary>
        /// Udpates selected DataClass properties
        /// </summary>
        /// <param name="input">DataClass reference</param>
        public void UpdateModel(DataClass input) 
        {
            context.Datas.Update(input);
            context.SaveChanges();
        }

        /// <summary>
        /// Asynchronously udpates selected DataClass properties
        /// </summary>
        /// <param name="input">DataClass reference</param>
        public async Task UpdateModelAsync(DataClass input)
        {
             context.Update(input);
             context.SaveChangesAsync();
        }

        /// <summary>
        /// Removes the model from database
        /// </summary>
        /// <param name="input">A DataClass reference which should be removed</param>
        /// <returns>true if input is deletedm else false</returns>
        public bool DeleteModel(DataClass input)
        {
            try
            {
                context.Remove(input);
                context.SaveChanges();
                return true;
            }
            catch { return false; }
        }
        /// <summary>
        /// Returns DataColors
        /// </summary>
        /// <returns>Colors</returns>
        public List<DataColor> GetColors() => context.Colors.OrderBy(x => x.Name).ToList();
    }
}
