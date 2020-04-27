using PS.CTS.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PS.CTS.Billing.Repository
{
    public interface IBillingRepository
    {
        List<BillingInfo> GetBillingInfo(SearchRequest request);
    }
}
