using Taxi_mobile.Models.Api;

namespace Taxi_mobile.Interfaces
{
    public interface IWebService
    {
        public Task<GetAllDriversResponse> GetAllDrivers(string status);
        public Task<GetUserStateResponse> GetUserState(Guid id);
        public Task<AddOrderResponse> PostAddOrder(AddOrderRequest order);
        public Task<ProcessCarResponse> PutProcessOrder(Guid orderId);
        public Task<FinishCarResponse> PutFinishOrder(Guid orderId, FinishCarRequest order);
    }
}
