using System.Collections.Generic;
using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Providers.Authentication
{
    public interface IAuthenticationProvider
    {
        /// <summary>
        /// 
        /// </summary>
        string ProviderName { get; }

        /// <summary>
        /// 
        /// </summary>
        bool CanCreateUser { get; }

        /// <summary>
        /// 
        /// </summary>
        bool CanUpdateUser { get; }

        /// <summary>
        /// 
        /// </summary>
        bool CanResetAuthData { get; }

        /// <summary>
        /// 
        /// </summary>
        bool CanDeleteUser { get; }

        /// <summary>
        /// 
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="authData"></param>
        /// <returns></returns>
        Task<IAuthUser> LoginAsync(AuthData authData);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="authData"></param>
        /// <returns></returns>
        Task<bool> ExistsAsync(AuthData authData);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<int> GetCountAsync(UserRequest request);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<IEnumerable<IUser>> GetAsync(UserRequest request);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<bool> IsValidAsync(IAuthUser user);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<IAuthUser> SaveAsync(IUser user);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(IUser user);
    }
}
