using System.Data.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DataLibrary.BlogDemo.Interceptors
{
    public class CommandInterceptor : DbCommandInterceptor
    {
        public override DbDataReader ReaderExecuted(DbCommand command, 
            CommandExecutedEventData eventData, DbDataReader result)
        {
            return base.ReaderExecuted(command, eventData, result);
        }
    }
}
