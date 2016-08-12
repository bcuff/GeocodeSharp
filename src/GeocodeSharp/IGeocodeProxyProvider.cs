using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace GeocodeSharp
{
    public interface IGeocodeProxyProvider
    {
        HttpWebRequest CreateRequest(string url);
    }
}
