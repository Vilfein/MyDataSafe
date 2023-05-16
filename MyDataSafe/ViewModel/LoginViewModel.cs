using System.IO;
using System.Threading.Tasks;

namespace MyDataSafe.ViewModel
{
    public class LoginViewModel
    {
        public string LoginName { get; private set; }
        public string LoginPassword { get; private set; }
        string path = "_Settings.ini";

        public LoginViewModel()
        {
            if (!File.Exists(path))
            {
                File.Create(path).Close();
                using (StreamWriter sw = new StreamWriter(path))
                {
                    sw.WriteLine("Admin");
                    sw.WriteLine("Admin123!");
                    sw.Flush();
                    sw.Close();
                }
            }
        }

        public async Task LoadSetting()
        {
            int i = 0;
            string line;
            using (StreamReader sr = new StreamReader(path))
            {
                while ((line=sr.ReadLine()) != null)
                {                 
                    if (i == 0) 
                        this.LoginName= line;
                    else 
                        this.LoginPassword= line;
                    i++;
                }
                sr.Close();
            }
        }
    }
}
