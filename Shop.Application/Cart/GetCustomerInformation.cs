using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Shop.Domain.Models;

namespace Shop.Application.Cart
{
    public class GetCustomerInformation
    {
        private readonly ISession _session;

        public GetCustomerInformation(ISession session)
        {
            _session = session;
        }
        
        public CustomerInformation Do()
        {
            var stringObject = _session.GetString("customer-info");

            if (string.IsNullOrEmpty(stringObject))
                return null;

            var response = JsonSerializer.Deserialize<CustomerInformation>(stringObject);

            return response;
        }
    }
}