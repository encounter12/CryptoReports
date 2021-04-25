using Microsoft.AspNetCore.Mvc;

namespace CryptoReports.WebReportDesigner.Controllers
{
    using Microsoft.AspNetCore.Mvc; 
    using System.IO; 
    using Telerik.Reporting.Cache.File; 
    using Telerik.Reporting.Services; 
    using Telerik.Reporting.Services.AspNetCore; 
    using Telerik.WebReportDesigner.Services; 
    using Telerik.WebReportDesigner.Services.Controllers; 

    [Route("api/reportdesigner")] 
    public class ReportDesignerController : ReportDesignerControllerBase 
    { 
        public ReportDesignerController(IReportDesignerServiceConfiguration reportDesignerServiceConfiguration, IReportServiceConfiguration reportServiceConfiguration)
            : base(reportDesignerServiceConfiguration, reportServiceConfiguration)
        {
        }
    } 
}