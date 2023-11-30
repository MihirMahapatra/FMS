using FMS.Model.CommonModel;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Model.ViewModel
{
    public class CompanyDetailsViewModel : Base
    {
        public CompanyDetailsViewModel()
        {
            GetCompany = new CompanyDetailsModel();
        }
        public CompanyDetailsModel GetCompany { get; set; }
    }
}