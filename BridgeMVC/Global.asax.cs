using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace BridgeMVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
            DocumentDBRepository<BridgeMVC.Models.BFinancial>.Initialize();
            DocumentDBRepository<BridgeMVC.Models.BIORA>.Initialize();
            DocumentDBRepository<BridgeMVC.Models.BProduct>.Initialize();
            DocumentDBRepository<BridgeMVC.Models.BUser>.Initialize();
            DocumentDBRepository<BridgeMVC.Models.BRule>.Initialize();
            DocumentDBRepository<BridgeMVC.Models.BBridge>.Initialize();
            DocumentDBRepository<BridgeMVC.Models.BFinancial>.Initialize();
            DocumentDBRepository<BridgeMVC.Models.Item>.Initialize();
            DocumentDBRepository<BridgeMVC.Models.Job>.Initialize();
            DocumentDBRepository<BridgeMVC.Models.IORA>.Initialize();
            DocumentDBRepository<BridgeMVC.Models.Customer>.Initialize();
            DocumentDBRepository<BridgeMVC.Models.Employee>.Initialize();
            DocumentDBRepository<BridgeMVC.Models.Order>.Initialize();
            DocumentDBRepository<BridgeMVC.Models.Product>.Initialize();
            DocumentDBRepository<BridgeMVC.Models.Rule>.Initialize();
            DocumentDBRepository<BridgeMVC.Models.TechCheckLSA>.Initialize();
            DocumentDBRepository<BridgeMVC.Models.BTechChecklist>.Initialize();
            DocumentDBRepository<BridgeMVC.Models.BList>.Initialize();
            DocumentDBRepository<BridgeMVC.Models.BEmail>.Initialize();
            DocumentDBRepository<BridgeMVC.Models.BLSACert>.Initialize();
        }
    }
}
