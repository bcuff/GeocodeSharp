using System.Threading.Tasks;

namespace GeocodeSharp.Google
{
    public interface IGeocodeClient
    {
        Task<GeocodeResponse> GeocodeAddress(string address, bool sensor);
    }
}
