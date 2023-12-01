using WS_CRM_Customer.Config;
using WS_CRM_Customer.Feature.Customer;
using WS_CRM_Customer.Feature.Customer.Dao;
using WS_CRM_Customer.Feature.Customer.Dto;
using WS_CRM_Customer.Feature.Customer.Model;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WS_CRM_Customer.Feature.Customer.Dao;

namespace WS_CRM_CustomerTest
{
    public class CustomerTest
    {
        public CustomerController controller;
        private Mock<ICustomerRepo> custDao;
        public WS_CRM_Customer.Config.AppConfig appConfig;

        private int expected = 200;
        private string dummyT = "test";
        private string userNIP = "12345";

        public CustomerTest()
        {
            custDao = new Mock<ICustomerRepo>();
            custDao.CallBase = true;
            appConfig = new WS_CRM_Customer.Config.AppConfig();
            controller = new CustomerController(custDao.Object);
        }

        [Fact]
        public async void GetListCustomer()
        {
            //var sample = new List<Customers>();

            var res = (OkObjectResult)controller.GetCustomersList().Result;
            Assert.Equal(expected, res.StatusCode);

        }
    }
}