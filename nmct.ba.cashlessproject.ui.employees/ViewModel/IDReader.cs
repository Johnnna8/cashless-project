using be.belgium.eid;
using Newtonsoft.Json;
using nmct.ba.cashlessproject.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.ui.employees.ViewModel
{
    public class IDReader
    {
        public static BEID_EIDCard getData()
        {
            try
            {
                BEID_ReaderSet.initSDK();
                BEID_ReaderContext Reader = BEID_ReaderSet.instance().getReader();

                if (Reader.isCardPresent())
                {
                    BEID_EIDCard card = Reader.getEIDCard();

                    if (card.isTestCard())
                    {
                        card.setAllowTestCard(true);
                    }

                    return card;
                }
                else
                {
                    return null;
                }
            }

            catch (BEID_Exception ex)
            {
                IDReader.logError(ex);
                BEID_ReaderSet.releaseSDK();
                return null;
            }
        }

        public static async void logError(BEID_Exception ex)
        {
            ErrorLog errorLog = new ErrorLog();
            errorLog.Timestamp = DateTime.Now;
            errorLog.Message = ex.Message;
            errorLog.Stacktrace = ex.StackTrace;

            RegisterCompany register = new RegisterCompany() { ID = 1 };
            errorLog.Register = register;

            string input = JsonConvert.SerializeObject(errorLog);

            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.PostAsync("http://localhost:55853/api/errorlog", new StringContent(input, Encoding.UTF8, "application/json"));
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("error");
                }
            }
        }
    }
}
