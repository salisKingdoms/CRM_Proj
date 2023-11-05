using WS_CRM.Feature.Customer.dto;
using WS_CRM.Feature.Customer.Model;
using AutoMapper;

namespace WS_CRM.Helper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // CreateRequest -> User
            CreateMap<CustomerRespon, Customers>();
        }
            
    }
}
