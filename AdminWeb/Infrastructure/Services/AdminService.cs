using AdminWeb.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace AdminWeb.Infrastructure.Services
{
    public class AdminService:IAdminService
    {
        private string _connectionString = string.Empty;

        public AdminService(string constr)
        {
            _connectionString = !string.IsNullOrWhiteSpace(constr) ? constr : throw new ArgumentNullException(nameof(constr));
        }

        public async Task<(IEnumerable<Problem> rows, int totalCount)> GetProblemList(
                       int proType
                        ,int pageIndex
                        , int pageSize
                        , string[] asc)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                const string countQuery = @"SELECT COUNT(1)
                      FROM [dbo].[Problem] p
                      /**where**/";
                const string selectQuery = @"with t as
                        (select *, ROW_NUMBER() OVER(/**orderby**/) 
                        AS rownum from [dbo].[Problem]  /**where**/)
                        select * from t where t.rownum between @PageIndex*@PageSize+1 and (@PageIndex+1)*@PageSize";
                SqlBuilder builder = new SqlBuilder();

                var count = builder.AddTemplate(countQuery);
                var selector = builder.AddTemplate(selectQuery, new { PageIndex = pageIndex, PageSize = pageSize });

                //if (!string.IsNullOrEmpty(criteria.Message))
                //{
                //    var msg = "%" + criteria.Message + "%";
                //    builder.Where("l.Message Like @Message", new { Message = msg });
                //}
                if (proType > 0)
                {
                    builder.Where("ProblemType=@ProType",new {ProType=proType });
                }

                foreach (var a in asc)
                {
                    if (!string.IsNullOrWhiteSpace(a))
                        builder.OrderBy(a);
                }

                connection.Open();
                var totalCount =await connection.QueryFirstAsync<int>(count.RawSql, count.Parameters);
                var rows = await connection.QueryAsync<Problem>(selector.RawSql, selector.Parameters);

                return (rows,totalCount);
            }
        }


        public async Task<(IEnumerable<User> rows, int totalCount)> GetUsersList(
                        int pageIndex
                        , int pageSize
                        , string[] asc)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                const string countQuery = @"SELECT COUNT(1)
                      FROM [dbo].[User] p
                      /**where**/";
                const string selectQuery = @"with t as
                        (select *, ROW_NUMBER() OVER(/**orderby**/) 
                        AS rownum from [dbo].[User]  /**where**/)
                        select * from t where t.rownum between @PageIndex*@PageSize+1 and (@PageIndex+1)*@PageSize";
                SqlBuilder builder = new SqlBuilder();

                var count = builder.AddTemplate(countQuery);
                var selector = builder.AddTemplate(selectQuery, new { PageIndex = pageIndex, PageSize = pageSize });

                //if (!string.IsNullOrEmpty(criteria.Message))
                //{
                //    var msg = "%" + criteria.Message + "%";
                //    builder.Where("l.Message Like @Message", new { Message = msg });
                //}
                builder.Where("UserName!='admin'");
                foreach (var a in asc)
                {
                    if (!string.IsNullOrWhiteSpace(a))
                        builder.OrderBy(a);
                }

                connection.Open();
                var totalCount = await connection.QueryFirstAsync<int>(count.RawSql, count.Parameters);
                var rows = await connection.QueryAsync<User>(selector.RawSql, selector.Parameters);

                return (rows, totalCount);
            }
        }

    }
}
