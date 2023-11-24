using WS_CRM.Config;
using WS_CRM.Feature.Customer;
using WS_CRM.Feature.Customer.dao;
using WS_CRM.Feature.Customer.dto;
using WS_CRM.Feature.Customer.Model;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WS_CRMTest
{
    public class CustomerTest
    {
        public CustomerController controller;
        private Mock<ICustomerDao> custDao;
        public WS_CRM.Config.AppConfig appConfig;

        private int expected = 200;
        private string dummyT = "test";
        private string userNIP = "12345";

        public CustomerTest()
        {
            custDao = new Mock<ICustomerDao>();
            custDao.CallBase = true;
            appConfig = new WS_CRM.Config.AppConfig();
            controller = new CustomerController(custDao.Object);
        }

        [Fact]
        public async void GetListCustomer()
        {
            //var sample = new 
            //custDao.Setup(x => x.GetAll()).ThrowsAsync(new Exception("Test excception"));
            //var exception = await Assert.ThrowsAsync<Exception>(() => controller.GetCustomersList());
            //Assert.Equal("test exception", exception.Message);

            var sample = new List<Customers>();
            //custDao.Setup(x => x.GetAll()).Returns(Task.FromResult(sample));
            var res = (OkObjectResult)controller.GetCustomersList().Result;
            Assert.Equal(expected, res.StatusCode);
            //tes
        }
    }
}