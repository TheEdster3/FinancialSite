using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using Newtonsoft.Json;
using BadAPI.Helpers;
using Avapi;
using Avapi.AvapiTIME_SERIES_DAILY;
using Avapi.AvapiTIME_SERIES_INTRADAY;


namespace BadAPI.Controllers
{
    [Route("api/FinanceController")]
    public class FinanceController : Controller
    {
        
        [HttpGet("[action]")]
        async public Task<Finance[]> Finance()
        {
            return await parseFinance();
        }

        private async Task<Finance[]> parseFinance()
        {
            IAvapiConnection connection = AvapiConnection.Instance;
            connection.Connect($"{BadAPI.Startup.Configuration["APIKeys:AlphaVantage"]}");

            // Get the Int_TIME_SERIES_INTRADAY query object
            Int_TIME_SERIES_INTRADAY time_intraday = 
	            connection.GetQueryObject_TIME_SERIES_INTRADAY();

            // Perform the Int_TIME_SERIES_INTRADAY request and get the result
            IAvapiResponse_TIME_SERIES_INTRADAY time_series_intradayResponse = 
            time_intraday.Query(
                 "MS",
                 Const_TIME_SERIES_INTRADAY.TIME_SERIES_INTRADAY_interval.n_1min,
                 Const_TIME_SERIES_INTRADAY.TIME_SERIES_INTRADAY_outputsize.compact);
            


            List<Finance[]> financeResults = new List<Finance[]>();
            var data = time_series_intradayResponse.Data;
            foreach(var timeseries in data.TimeSeries)
            {
                Finance[] finance = new Finance[1];
                finance[0] = new Finance();
                if(finance[0] != null)
                {
                    finance[0].symbol = data.MetaData.Symbol;
                    finance[0].date = timeseries.DateTime.ToString();
                    finance[0].open = timeseries.open;
                    finance[0].close = timeseries.close;
                    finance[0].high = timeseries.high;
                    finance[0].low = timeseries.low;
                    finance[0].volume = timeseries.volume;
                    financeResults.Add(finance);
                }
            }
            
            return CombineResponses(financeResults);
        }

        private Finance[] CombineResponses(List<Finance[]> finance)
        {
            List<Finance> combinedResponse = new List<Finance>();
            for (int i = 0; i < finance.Count(); i++)
            {
                combinedResponse.AddRange(finance[i]);
            }

            return combinedResponse.ToArray();
        } //Formatted to a single, correct JSON result
    }
}
