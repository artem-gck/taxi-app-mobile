﻿namespace Taxi_mobile.Models.Api
{
    public class FinishCarResponse
    {
        public Guid CorrelationId { get; set; }
        public Guid OrderId { get; set; }
        public string ErrorMessage { get; set; }
    }
}
