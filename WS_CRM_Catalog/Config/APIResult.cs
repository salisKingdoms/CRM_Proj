﻿using System.Net;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace WS_CRM_Catalog.Config
{
    public class APIResult
    {
        public bool is_ok { get; set; }
        public string message { get; set; }
    }
}
