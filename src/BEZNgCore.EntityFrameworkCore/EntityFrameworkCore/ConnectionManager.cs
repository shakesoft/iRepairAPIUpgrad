using Abp.Data;
using Abp.Domain.Entities;
using Abp.EntityFrameworkCore;
using Abp.MultiTenancy;
using BEZNgCore.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEZNgCore.EntityFrameworkCore
{
    public class ConnectionManager
    {
        private readonly IDbContextProvider<BEZNgCoreDbContext> _dbContextProvider;
        private readonly IActiveTransactionProvider _transactionProvider;
        public ConnectionManager(IDbContextProvider<BEZNgCoreDbContext> dbContextProvider, IActiveTransactionProvider transactionProvider)
        {
            _dbContextProvider = dbContextProvider;
            _transactionProvider = transactionProvider;
        }
        public void EnsureConnectionOpen(MultiTenancySides? multiTenancySide)
        {
            var Context = _dbContextProvider.GetDbContext(multiTenancySide);
            var connection = Context.Database.GetDbConnection();

            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
        }
        public void EnsureConnectionClose(MultiTenancySides? multiTenancySide)
        {
            var Context = _dbContextProvider.GetDbContext(multiTenancySide);
            var connection = Context.Database.GetDbConnection();

            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }
        public DbCommand CreateCommand(string commandText, CommandType commandType, Dictionary<string, string> parameters, MultiTenancySides? multiTenancySide)
        {
            var Context = _dbContextProvider.GetDbContext();
            var command = Context.Database.GetDbConnection().CreateCommand();
            command.CommandTimeout = 180;
            command.CommandText = commandText;
            command.CommandType = commandType;
            command.Transaction = GetActiveTransaction(multiTenancySide);

            foreach (var item in parameters)
            {
                try
                {
                    var p = command.CreateParameter();
                    p.ParameterName = item.Key;
                    p.Value = item.Value;
                    command.Parameters.Add(p);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

            return command;
        }
        public DbCommand CreateCommandOnly(string commandText, CommandType commandType, MultiTenancySides? multiTenancySide)
        {
            var Context = _dbContextProvider.GetDbContext(multiTenancySide);
            var command = Context.Database.GetDbConnection().CreateCommand();
            command.CommandTimeout = 180;
            command.CommandText = commandText;
            command.CommandType = commandType;
            command.Transaction = GetActiveTransaction(multiTenancySide);

            return command;
        }
        public DbCommand CreateCommandSP(string commandText, CommandType commandType, MultiTenancySides? multiTenancySide, Microsoft.Data.SqlClient.SqlParameter[] parameters)//params SqlParameter[] parameters //SqlParameter[] parameters
        {
            var context = _dbContextProvider.GetDbContext(multiTenancySide);
            var command = context.Database.GetDbConnection().CreateCommand();
            command.CommandTimeout = 180;
            command.CommandText = commandText;
            command.CommandType = commandType;
            command.Transaction = GetActiveTransaction(multiTenancySide);
            foreach (var param in parameters)
            {
                command.Parameters.Add(param);

            }

            return command;
        }
        private DbTransaction GetActiveTransaction(MultiTenancySides? multiTenancySide)
        {
            return (DbTransaction)_transactionProvider.GetActiveTransaction(new ActiveTransactionProviderArgs
            {
                {"ContextType", typeof(BEZNgCoreDbContext) },
                {"MultiTenancySide", multiTenancySide }
            });
        }

    }
}
