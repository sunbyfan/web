using AdminWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminWeb.Infrastructure.Services
{
    public interface IAdminService
    {
        Task<(IEnumerable<Problem> rows, int totalCount)> GetProblemList(
                        int proType
                        , int pageIndex
                        , int pageSize
                        , string[] asc);

        Task<(IEnumerable<User> rows, int totalCount)> GetUsersList(
                        int pageIndex
                        , int pageSize
                        , string[] asc);
    }
}
