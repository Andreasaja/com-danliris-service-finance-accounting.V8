using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.CashierApproval;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.CashierApproval;
using Com.Moonlay.Models;
using Com.Moonlay.NetCore.Lib;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.CashierApproval
{
    public class CashierApprovalService : ICashierAprovalService
    {
        private const string UserAgent = "finance-service";
        protected DbSet<VbRequestModel> DbSet;
        public IIdentityService IdentityService;
        public readonly IServiceProvider ServiceProvider;
        public FinanceDbContext DbContext;

        public CashierApprovalService(IServiceProvider serviceProvider, FinanceDbContext dbContext)
        {
            DbContext = dbContext;
            ServiceProvider = serviceProvider;
            DbSet = dbContext.Set<VbRequestModel>();
            IdentityService = serviceProvider.GetService<IIdentityService>();
        }

        public async Task<int> CashierAproval(CashierApprovalViewModel data)
        {

            int updated = 0;

            using (var transaction = DbContext.Database.BeginTransaction())
            {
                try
                {
                    List<string> approvals = new List<string>();

                    if (data.VBRequestCategory.Equals("PO"))
                    {
                        foreach (var item in data.CashierApproval)
                        {
                            approvals.Add(item.VBNo);
                            VbRequestModel model = DbContext.VbRequests.Single(x => x.Id == item.Id);
                            model.Apporve_Status = true;
                            model.Complete_Status = false;
                            model.ApproveDate = DateTimeOffset.UtcNow;
                            model.Amount = item.Amount;

                            EntityExtension.FlagForUpdate(model, IdentityService.Username, UserAgent);
                        }

                        updated = await DbContext.SaveChangesAsync();
                    }
                    else if (data.VBRequestCategory.Equals("NONPO"))
                    {
                        foreach (var item in data.CashierApproval)
                        {
                            approvals.Add(item.VBNo);
                            VbRequestModel model = DbContext.VbRequests.Single(x => x.Id == item.Id);
                            model.Apporve_Status = true;
                            model.Complete_Status = false;
                            model.ApproveDate = DateTimeOffset.UtcNow;

                            EntityExtension.FlagForUpdate(model, IdentityService.Username, UserAgent);
                        }

                        updated = await DbContext.SaveChangesAsync();
                    }
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw e;
                }
            }

            return updated;
        }

        public async Task<int> DeleteCashierAproval(int id)
        {
            int count = 0;

            if (DbContext.VbRequests.Count(x => x.Id == id && !x.IsDeleted).Equals(0))
            {
                return 0;
            }

            using (var transaction = DbContext.Database.BeginTransaction())
            {
                try
                {
                    VbRequestModel approvals = DbContext.VbRequests.Single(x => x.Id == id);

                    if (approvals.VBRequestCategory == "PO")
                    {
                        approvals.Apporve_Status = false;
                        approvals.Complete_Status = false;
                        approvals.ApproveDate = null;
                        approvals.Amount = 0;

                        EntityExtension.FlagForUpdate(approvals, IdentityService.Username, UserAgent);

                        count = await DbContext.SaveChangesAsync();
                    }
                    else if (approvals.VBRequestCategory == "NONPO")
                    {
                        approvals.Apporve_Status = false;
                        approvals.Complete_Status = false;
                        approvals.ApproveDate = null;

                        EntityExtension.FlagForUpdate(approvals, IdentityService.Username, UserAgent);

                        count = await DbContext.SaveChangesAsync();
                    }

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw e;
                }
            }

            return count;
        }

        public Task<int> CreateAsync(VbRequestModel model, CashierApprovalViewModel viewmodel)
        {
            EntityExtension.FlagForCreate(model, IdentityService.Username, UserAgent);

            DbContext.VbRequests.Add(model);

            return DbContext.SaveChangesAsync();
        }

        public Task<VbRequestModel> ReadByIdAsync(int id)
        {
            return DbContext.VbRequests.Where(entity => entity.Id == id).FirstOrDefaultAsync();
        }
    }
}
