using Quotation_Management.Models;
using Quotation_Management.Repos.Base;
using Quotation_Management.Repos.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quotation_Management.Repos
{
    public class ClientRepo : RepoBase<Clients>, IClientRepo
    {

        public long CodeGenerate()
        {
            var f = Table.ToList();
            if (f.Count == 0)
            {
                return 0;
            }

            else
            {
                return f.Count + 0;
            }

        }
    }
}
