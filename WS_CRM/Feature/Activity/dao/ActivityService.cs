using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using WS_CRM.Helper;
using WS_CRM.Feature.Activity.Model;
using WS_CRM.Feature.Activity.dto;
using AutoMapper;
using WS_CRM.Config;

namespace WS_CRM.Feature.Activity.dao {
    
    public class ActivityService
    {
         IActivityRepo _actDao;
         private readonly IMapper _mapper;
        
        public ActivityService(IActivityRepo actDao,IMapper mapper)
        {
            _actDao = actDao;
            _mapper = mapper;
            
        }

        public async Task<List<WarrantyListRespon>> GetListWaranty(GlobalFilter request)
        {
            List<WarrantyListRespon> warranties = new List<WarrantyListRespon>();
            var data = await _actDao.GetAllWarranty(request);
            if(data!= null)
            {
    
                var mappingData = data.Select(x=> new WarrantyListRespon
                {
                    WarrantyCode = x.warranty_code,
                    CompanyCode = x.company_code,
                    InvoiceNumber = x.invoice_no,
                    InvoiceDate = x.invoice_date,
                    ArticleUnitCode = x.article_code,
                    ArticleUnitName = x.article_name,
                    SerialNumber = x.serial_no,
                    StartDateWarranty = x.start_date,
                    ExpiredDate = x.end_date,
                    ActivatedBy = x.activate_by,
                    ActivatedOn = x.activate_on,
                    Status = x.active,
                    CreatedBy = x.created_by,
                    CreatedOn = x.created_on,
                    UpdatedBy = x.modified_by,
                    UpdatedOn = x.modified_on
                });

                warranties = mappingData.ToList();
            }

            return warranties;
        }
    }
}