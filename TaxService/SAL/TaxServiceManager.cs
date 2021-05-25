using System;
using System.Threading.Tasks;
using Grpc.Core;
using TaxService.DAL;
using TaxService.Models;
using TaxServiceGrpc.Protos;
using static TaxServiceGrpc.Protos.ITaxService;

namespace TaxService.SAL
{
    public class TaxServiceManager : ITaxServiceBase
    {
        private readonly ITaxDataManager taxDataManager;
        private readonly float taxPercentage = 1;

        public TaxServiceManager(ITaxDataManager taxDataManager)
        {
            this.taxDataManager = taxDataManager;
        }

        public override async Task<TaxReceipt> TaxTransaction(TaxRequest request, ServerCallContext context)
        {
            var tax = await taxDataManager.Insert(new Tax() { TaxPaid = request.Amount / 100 * this.taxPercentage, Amount = request.Amount, Percentage = this.taxPercentage });
            return new TaxReceipt() { TaxToPay = tax.TaxPaid };
        }
    }
}