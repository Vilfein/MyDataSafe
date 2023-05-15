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
        public List<DataClass> GetAllModels() => context.Datas.Include(x => x.Color).ToList();

        /// <summary>
        /// Returns all stored files asynchonously
        /// </summary>
        /// <returns>All stored files</returns>
        public async Task<List<DataClass>> GetAllModelsAsync() => await context.Datas.Include(x => x.Color).ToListAsync();

        public DataClass GetModel(string name) => context.Datas.Include(x => x.Color).FirstOrDefault(x => x.Name == name) ?? null;

        public async Task<DataClass> GetModelAsync(string name) => await context.Datas.FirstOrDefaultAsync(x => x.Name == name) ?? null;

        public void UpdateModel(DataClass input) 
        {
            context.Datas.Update(input);
            context.SaveChanges();
        }

        public async Task UpdateModelAsync(DataClass input)
        {
            await Task.Run(() => context.Datas.Update(input));
        }

        public bool DeleteModel(DataClass input)
        {
            try
            {
                context.Datas.Remove(input);
                context.SaveChanges();
                return true;
            }
            catch { return false; }
        }

        public List<DataColor> GetColors() => context.Colors.ToList();
    }
}
