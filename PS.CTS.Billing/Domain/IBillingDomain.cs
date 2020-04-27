using PS.CTS.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PS.CTS.Billing.Domain
{
    public interface IBillingDomain
    {
        List<BillingInfo> GetBillingInfo(SearchRequest searchRequest);
    }
}
