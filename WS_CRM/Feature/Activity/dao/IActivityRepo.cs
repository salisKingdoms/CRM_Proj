using WS_CRM.Feature.Activity.dto;
using WS_CRM.Helper;
using WS_CRM.Feature.Activity.Model;
using System.Linq;
using Dapper;
using System;
using System.Collections.Generic;
using System.IO;

namespace WS_CRM.Feature.Activity.dao
{
    public interface IActivityRepo
    {
        public Task CreateWarranty(CreateActivationWarranty request);
        public Task<List<ws_warranty>> GetAllWarranty();
        public Task<int> RepoGetTotalAllWarranty();
        public Task<ws_warranty> GetWarrantyById(long id);
        public Task DeleteProductById(long id);
        public Task UpdateWarranty(ws_warranty param);

        public Task CreateTicketService(CreateTicket request);
        public Task<List<ws_ticket>> GetAllTicketHeader();
        public Task<int> RepoGetTotalAllTicket(GlobalFilter filter);
        public  Task UpdateTicketHeader(ws_ticket param);
    }
}
