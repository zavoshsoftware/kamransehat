﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class ConsultationDetailViewModel:BaseViewModel
    {
        public Consultation Consultation { get; set; }
    }
}