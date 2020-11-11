using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace WebApiCovidItalia.Controllers
{
	public class CovidItem
	{
		public DateTime data { get; set; }
		public string stato { get; set; }
		public int codice_regione { get; set; }
		public string denominazione_regione { get; set; }

		public int nuovi_positivi { get; set; }
		public int tamponi { get; set; }

		public double diff_positivi_tamponi { get; set; }
	}
	
	[ApiController]
	[Route("[controller]")]
	public class CovidController: ControllerBase
	{
		
		[HttpGet]
		public IEnumerable<CovidItem> Get()
		{
			
			using (WebClient webClient = new WebClient())
			{
				var json = webClient.DownloadString(
					"https://raw.githubusercontent.com/pcm-dpc/COVID-19/master/dati-json/dpc-covid19-ita-regioni.json");
				//Console.WriteLine(json);
				var covidItems = JsonConvert.DeserializeObject<List<CovidItem>>(json);

				var tuscany = covidItems.Where(ci => ci.codice_regione == 9).ToList();

				Console.WriteLine("Len: " + tuscany.Count);

				for (int i = 1; i < tuscany.Count; i++)
				{
					if (tuscany[i].tamponi - tuscany[i - 1].tamponi != 0)
						tuscany[i].diff_positivi_tamponi =
							(double) tuscany[i].nuovi_positivi / ((double)tuscany[i].tamponi - tuscany[i - 1].tamponi);
					else
						tuscany[i].diff_positivi_tamponi = -1;

					Console.WriteLine(tuscany[i].data + " | " + tuscany[i].diff_positivi_tamponi*100 +" %");
				}

				return tuscany;
			}
		}
	}
}