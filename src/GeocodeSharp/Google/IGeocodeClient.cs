using System.Threading.Tasks;

namespace GeocodeSharp.Google
{
    public interface IGeocodeClient
    {
        Task<GeocodeResponse> GeocodeAddress(string address, string region = null);
    }
}
