using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Shop.Domain.Models;

namespace Shop.Application.Cart
{
    public class AddCustomerInformation
    {
        private readonly ISession _session;

        public AddCustomerInformation(ISession session)
        {
            _session = session;
        }

        public void Do(CustomerInformation request)
        {
            var stringObject = JsonSerializer.Serialize(request);
            
            _session.SetString("customer-info", stringObject);
        }
    }
}