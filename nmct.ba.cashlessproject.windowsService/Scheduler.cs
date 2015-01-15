using Newtonsoft.Json;
using nmct.ba.cashlessproject.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Thinktecture.IdentityModel.Client;

namespace nmct.ba.cashlessproject.windowsService
{
    public partial class Scheduler : ServiceBase
    {
        TokenResponse token = null;
        private Timer timer = null;
        private int counter = 1;

        private ObservableCollection<ErrorLog> _error;

        public ObservableCollection<ErrorLog> Error
        {
            get { return _error; }
            set { _error = value; }
        }

        public Scheduler()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                token = GetToken();
            }
            catch (Exception ex)
            {

                Library.WriteErrorLog(ex);
            }

            timer = new Timer();
            timer.Interval = 5000;    //every 5 secs
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Tick);
            timer.Enabled = true;
            Library.WriteErrorLog("Window Service started");
        }

        private void timer_Tick(object sender, ElapsedEventArgs e)
        {
            Library.WriteErrorLog("Timer ticked and some job has been done succesfully");
            counter--;
            if (counter <= 0)
            {
                Library.WriteErrorLog("Before getData");
                getData();
            }
        }



        protected override void OnStop()
        {
            timer.Enabled = false;
            Library.WriteErrorLog("Window Service stopped");
        }

        private TokenResponse GetToken()
        {
            Library.WriteErrorLog("GetToken");
            OAuth2Client client = new OAuth2Client(new Uri("http://localhost:55853/token"));
            return client.RequestResourceOwnerPasswordAsync("user", "123", "KvKortrijk").Result;
        }

        private async void getData()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(token.AccessToken);
                HttpResponseMessage response = await client.GetAsync("http://localhost:55853/api/errorlog");
                if (response.IsSuccessStatusCode)
                {
                    Library.WriteErrorLog("Status code success getData");
                    string json = await response.Content.ReadAsStringAsync();
                    Error = JsonConvert.DeserializeObject<ObservableCollection<ErrorLog>>(json);
                }

            }
            writeData();
        }

        private async void writeData()
        {
            foreach (ErrorLog selectedError in Error)
            {
                string input = JsonConvert.SerializeObject(selectedError);

                using (HttpClient client = new HttpClient())
                {
                    client.SetBearerToken(token.AccessToken);
                    HttpResponseMessage response = await client.PostAsync("http://localhost:55853/api/errorlogadmin", new StringContent(input, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        Library.WriteErrorLog("Status code success writeData");
                    }
                }
            }
            deleteLogsClient();
        }

        private async void deleteLogsClient()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(token.AccessToken);
                HttpResponseMessage response = await client.DeleteAsync("http://localhost:55853/api/errorlog");
                if (response.IsSuccessStatusCode) {
                    Library.WriteErrorLog("Status code success deleteLogsClient");
                }
            }
        }
    }
}
