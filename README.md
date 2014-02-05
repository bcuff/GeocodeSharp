GeocodeSharp
============

An async .NET client for the Google geocode API

##Example
```c#
using System;
using System.Linq;
using System.Threading.Tasks;
using GeocodeSharp.Google;

var address = "21 Henrietta St, Bristol, UK";
var client = new GeocodeClient();
var response = await client.GeocodeAddress(address, false);
if (response.Status == GeocodeStatus.Ok)
{
    var firstResult = response.Results.First();
    var location = firstResult.Geometry.Location;
    var lat = location.Latitude;
    var lng = location.Longitude;
    // ...
}
```
