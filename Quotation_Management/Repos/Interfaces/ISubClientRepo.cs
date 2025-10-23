using Quotation_Management.Models;
using Quotation_Management.Repos.Base;
using Quotation_Management.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quotation_Management.Repos.Interfaces
{
    public interface ISubClientRepo: IRepo<SubClients>
    {
        List<AssignClientViewModel> GetClientAssigns(long clientId);

        List<AssignClientViewModel> GetClientAssigned(string id);
    }
}
