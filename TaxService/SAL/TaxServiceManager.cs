using System;
using System.Threading.Tasks;
using Grpc.Core;
using TaxService.DAL;
using TaxService.Models;
using TaxServiceGrpc.Proto;
using static TaxServiceGrpc.Proto.ITaxServiceManager;

namespace TaxService.SAL
{
    public class TaxServiceManager : ITaxServiceManagerBase
    {
        private readonly ITaxDataManager taxDataManager;
        private readonly float taxPercentage = 10;

        public TaxServiceManager(ITaxDataManager taxDataManager)
        {
            this.taxDataManager = taxDataManager;
        }

        public override async Task<TaxReceipt> TaxTransaction(TaxRequest request, ServerCallContext context)
        {
            var tax = await taxDataManager.Insert(new Tax() { Tax = request.Amount / 100 * this.taxPercentage, Amount = request.Amount, Percentage = this.taxPercentage });
            return new TaxReceipt() { Amount = tax.Amount + tax.TaxPaid, TaxPaid = tax.TaxPaid };
        }
    }
}