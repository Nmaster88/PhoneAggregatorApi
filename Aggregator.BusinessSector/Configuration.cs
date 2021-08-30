using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aggregator.BusinessSector
{
    internal class Configuration
    {
        public static readonly int Timeout = 60000; // 60s

        public static string BaseUrl
        {
            get
            {
                string url = "https://challenge-business-sector-api.meza.talkdeskstg.com";

                if (string.IsNullOrWhiteSpace(url))
                    throw new InvalidOperationException("Invalid configuration option 'SectorApiUrl' in app settings.");

                return url;
            }
        }
    }
}
