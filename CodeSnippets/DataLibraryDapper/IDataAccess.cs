using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLibraryDapper
{
    public interface IDataAccess: IDisposable
    {
        IEnumerable<U> LoadData<T, U>(string sql, T parameters);
        Task<IEnumerable<U>> LoadDataAsync<T, U>(string sql, T parameters);
        int SaveData<T>(string sql, T parameters);
        Task<int> SaveDataAsync<T>(string sql, T parameters);
    }
}