using System;
using System.Net.Http;
using System.Xml;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using SavingForASunnyDay.Api;

namespace SavingForASunnyDay
{
    public static class Save
    {
        static string _userid = ""; // national birth number
        static string _clientid = ""; // 
        static string _secret = "";
        static string _hostname = "api.sbanken.no";

        static int _debetThreshold = 1000; //Don't save if balance is below this threshold   
        static int _maxAmountToSave = 300; //Maximum limit for each saving
        static string _debetAccount = ""; //Transfer money from this account
        static string _savingsAccount = ""; //...to this account

        //«Weather forecast from Yr, delivered by the Norwegian Meteorological Institute and NRK» 
        static int postalcode = 5097; //This version uses zip code to define location for forecast. Can easily be rewritten to use place or long./lat. See http://om.yr.no/verdata/xml/.  
        static int amountToSavePrMm = 3; //How many NOK to save for each mm rainfall

        [FunctionName("Rainsaver")]
        public static async void Run([TimerTrigger("0 0 6 * * *")]TimerInfo myTimer, TraceWriter log)
        {
            XmlDocument weatherData = await getWhetherData();
            double precipitation = GetPercipitation(weatherData);
            int amountToSave = CalculateSaving(precipitation, amountToSavePrMm);
            await TransferToSavingsAccount(log, precipitation, amountToSave);
         
        }

        private static async System.Threading.Tasks.Task<XmlDocument> getWhetherData()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();

            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

            var weatherTask = client.GetStringAsync("http://www.yr.no/sted/Norge/postnummer/" + postalcode + "/varsel.xml");

            var msg = await weatherTask;

            XmlDocument weatherData = new XmlDocument();
            weatherData.LoadXml(msg);
            return weatherData;
        }

        private static async System.Threading.Tasks.Task TransferToSavingsAccount(TraceWriter log, double precipitation, int amountToSave)
        {
            try
            {
                var bank = new Bank(_userid, _clientid, _secret, _hostname);
                var debetAccount = await bank.GetAccount(_debetAccount);    
                var savingsAccount = await bank.GetAccount(_savingsAccount);

                if (amountToSave > 0 && debetAccount.available > _debetThreshold)
                {
                    if (amountToSave > _maxAmountToSave) {
                        amountToSave = _maxAmountToSave;
                    }

                    var transferMessage = "Det skal regne " + precipitation + " mm i dag";

                    var transfer = await bank.Transfer(new TransferRequest
                    {
                        fromAccount = debetAccount.accountNumber,
                        toAccount = savingsAccount.accountNumber,
                        amount = amountToSave,
                        message = transferMessage
                    });
                }
            }
            catch (Exception err)
            {
                log.Error(err.Message, err);
                throw err;
            }
        }

        private static int CalculateSaving(double precipitation, int savingFactor)
        {
            return (int)Math.Round(precipitation * savingFactor);
        }

        private static double GetPercipitation(XmlDocument xmlDoc)
        {
            double precipitationForecast = 0;
                       
            string xpath = "weatherdata/forecast/tabular/time[position()<5]";
            var nodes = xmlDoc.SelectNodes(xpath);
            
            foreach (XmlNode childrenNode in nodes) { 
                var p = childrenNode.SelectSingleNode("precipitation");
                // precipitationForecast += Convert.ToDouble(p.Attributes[0].InnerText.Replace(".", ",")); // Simple hack if run with "nb-NO" culture
                precipitationForecast += Convert.ToDouble(p.Attributes[0].InnerText);
            }

            return precipitationForecast;
        }
    }
}
