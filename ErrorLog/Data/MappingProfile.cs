﻿using AutoMapper;
using ErrorLog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ErrorLog.Data
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<App, AppModel>();
            CreateMap<Log, LogModel>();
            CreateMap<Log, AppLogModel>();
        }
    }
}