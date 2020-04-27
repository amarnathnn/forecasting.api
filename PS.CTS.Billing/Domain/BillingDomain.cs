using PS.CTS.Billing.Repository;
using PS.CTS.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PS.CTS.Billing.Domain
{
    public class BillingDomain:IBillingDomain
    {
        private readonly IBillingRepository _billingRepo;

        public BillingDomain(IBillingRepository billingRepository)
        {
            _billingRepo = billingRepository;
        }
        public List<BillingInfo> GetBillingInfo(SearchRequest searchRequest)
        {
            return _billingRepo.GetBillingInfo(searchRequest);
        }
    }
}
