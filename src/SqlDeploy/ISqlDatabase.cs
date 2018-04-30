using System;
using System.Threading.Tasks;

namespace SqlDeploy
{
    /// <summary>
    /// Iternface to access to the temporary database
    /// </summary>
    public interface ISqlDatabase: IDisposable
    {
        /// <summary>
        /// Return connections tring to the new database
        /// </summary>
        /// <returns></returns>
        string ConnectionString { get; }

        /// <summary>
        /// Delete temporary database
        /// </summary>
        /// <returns></returns>
        Task DeleteAsync();
    }
}
