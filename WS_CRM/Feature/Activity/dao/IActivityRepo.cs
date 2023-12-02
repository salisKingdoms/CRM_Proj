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
        public Task DeleteWarrantyById(long id);
        public Task UpdateWarranty(ws_warranty param);

        public Task CreateTicketService(CreateTicket request);
        public Task<List<ws_ticket>> GetAllTicketHeader(GlobalFilter filter);
        public Task<int> RepoGetTotalAllTicket(GlobalFilter filter);
        public  Task UpdateTicketHeader(ws_ticket param);

        public Task CreateTicketUnit(CreateTicketUnit request);
        public Task<List<ws_ticket_unit>> GetAllTicketUnit(string ticket_no);
        public Task<int> RepoGetTotalAllTicketUnit(string ticket_no);
        public Task DeleteTicketUnit(string ticket_no, int? unit_line);
        public Task UpdateTicketUnit(ws_ticket_unit param);
        public  Task<ws_ticket> GetTicketHeaderByTicketNo(string ticket_no);
        public  Task CreateTicketSparepart(CreateTicketSparepart request);
        public  Task<List<ws_ticket_sparepart>> GetAllTicketSparepart(string ticket_no);
        public  Task<int> RepoGetTotalAllTicketSparepart(string ticket_no);
        public  Task DeleteTicketSparepart(string ticket_no, int? unit_line);
        public  Task UpdateTicketSparepart(ws_ticket_unit param);
    }
}

