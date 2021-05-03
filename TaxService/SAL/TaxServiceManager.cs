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

        public TaxServiceManager(ITaxDataManager taxDataManager)
        {
            this.taxDataManager = taxDataManager;
        }

        public override async Task<TaxReceipt> TaxTransaction(TaxRequest request, ServerCallContext context)
        {
            var tax = await taxDataManager.Insert(new Tax() { TaxPaid = request.Amount / 100, Amount = request.Amount });
            return new TaxReceipt() { Amount = tax.Amount - tax.TaxPaid, TaxPaid = tax.TaxPaid };
        }
    }
}