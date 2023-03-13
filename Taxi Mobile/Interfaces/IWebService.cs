using Taxi_mobile.Models;

namespace Taxi_mobile.Interfaces
{
    public interface IWebService
    {
        public Task<GetAllDriversResponse> GetAllDrivers(string status);
    }
}
