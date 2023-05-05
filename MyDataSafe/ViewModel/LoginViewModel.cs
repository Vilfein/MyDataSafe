using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace MyDataSafe.ViewModel
{
    public class LoginViewModel
    {
        public string LoginName { get; set; }
        public string LoginPassword { get; set; }
        protected async Task Serialize(string pass, string name)
        {
            // Serializace

            LoginViewModel loginViewModel = new LoginViewModel
            {
                LoginName = name,
                LoginPassword = pass
            };

            string path = "init.xml";
            if (File.Exists(path))
            {
                XmlSerializer writer = new XmlSerializer(loginViewModel.GetType());
                using (StreamWriter sw = new StreamWriter(path))
                {
                    writer.Serialize(sw, loginViewModel);
                }
            }
            else
            {
                File.Create(path);
                XmlSerializer writer = new XmlSerializer(loginViewModel.GetType());
                using (StreamWriter sw = new StreamWriter(path))
                {
                    writer.Serialize(sw, new LoginViewModel { LoginName="Admin",LoginPassword="password"});
                }
            }
        }

        public async void Edit(LoginViewModel loginViewModel)
        {
            await Serialize(loginViewModel.LoginName, loginViewModel.LoginPassword);
        }

        public async Task<LoginViewModel> Deserialize()
        {
            //Deserializace

            string path = "init.xml";
            LoginViewModel loginViewModel = new LoginViewModel();
            XmlSerializer reader = new XmlSerializer(typeof(LoginViewModel));
            if (File.Exists(path))
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    loginViewModel = (LoginViewModel)reader.Deserialize(sr);                  
                }
            }
            return loginViewModel ?? throw new Exception("Login is null");
        }
    }
}
