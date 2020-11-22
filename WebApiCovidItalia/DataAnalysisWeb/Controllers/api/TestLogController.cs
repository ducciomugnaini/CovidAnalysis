using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace DataAnalysisWeb.Controllers.api
{
    public class CovidItem
    {
        public DateTime data { get; set; }
        public string stato { get; set; }
        public int? codice_regione { get; set; }
        public string denominazione_regione { get; set; }
        public int? ricoverati_con_sintomi { get; set; }
        public int? terapia_intensiva { get; set; }
        public int? totale_ospedalizzati { get; set; }
        public int? isolamento_domiciliare { get; set; }
        public int? totale_positivi { get; set; }
        public int? variazione_totale_positivi { get; set; }
        public int? nuovi_positivi { get; set; }
        public int? dimessi_guariti { get; set; }
        public int? deceduti { get; set; }
        public double? casi_da_sospetto_diagnostico { get; set; }
        public double? casi_da_screening { get; set; }
        public int? totale_casi { get; set; }
        public int? tamponi { get; set; }
        public double? casi_testati { get; set; }
        public double? diff_positivi_tamponi { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class TestLogController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;

        public TestLogController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _logger.LogDebug(1, "NLog injected into HomeController");
        }

        [HttpGet]
        public IEnumerable<CovidItem> Get()
        {
            _logger.LogInformation("Hello, this is the TestLogController!");

            using (WebClient webClient = new WebClient())
            {
                var json = webClient.DownloadString(
                    "https://raw.githubusercontent.com/pcm-dpc/COVID-19/master/dati-json/dpc-covid19-ita-regioni.json");

                var covidItems = JsonConvert.DeserializeObject<List<CovidItem>>(json);

                var tuscany = covidItems.Where(ci => ci.codice_regione == 9).ToList();

                Console.WriteLine("Len: " + tuscany.Count);

                for (int i = 1; i < tuscany.Count; i++)
                {
                    if (tuscany[i].tamponi - tuscany[i - 1].tamponi != 0)
                        tuscany[i].diff_positivi_tamponi = (double)tuscany[i].nuovi_positivi / ((double)tuscany[i].tamponi - tuscany[i - 1].tamponi);
                    else
                        tuscany[i].diff_positivi_tamponi = -1;

                    Console.WriteLine(tuscany[i].data + " | " + tuscany[i].diff_positivi_tamponi * 100 + " %");
                }

                return tuscany;
            }
        }

    }
}
