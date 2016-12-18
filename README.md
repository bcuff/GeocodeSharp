GeocodeSharp
============

An async .NET client for the Google geocode API

The object model closely follows the model [documented here](https://developers.google.com/maps/documentation/geocoding/#GeocodingResponses).

##Example
```c#
using System;
using System.Linq;
using System.Threading.Tasks;
using GeocodeSharp.Google;

var address = "21 Henrietta St, Bristol, UK";
var client = new GeocodeClient();
var response = await client.GeocodeAddress(address);
if (response.Status == GeocodeStatus.Ok)
{
    var firstResult = response.Results.First();
    var location = firstResult.Geometry.Location;
    var lat = location.Latitude;
    var lng = location.Longitude;
    // ...
}
```
