using System;
using System.Threading.Tasks;
using Grpc.Core;
using TaxServiceGrpc.Proto;
using static TaxServiceGrpc.Proto.ITaxServiceManager;

namespace TaxService.SAL
{
    public class TaxServiceManager : ITaxServiceManagerBase
    {
        public override Task<TaxReceipt> TaxTransaction(TaxRequest request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }
    }
}