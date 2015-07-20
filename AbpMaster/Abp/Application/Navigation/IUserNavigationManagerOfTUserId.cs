﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace Abp.Application.Navigation
{
    /// <summary>
    /// Used to manage navigation for users.
    /// </summary>
    public interface IUserNavigationManager<TUserId> where TUserId : struct
    {
        /// <summary>
        /// Gets a menu specialized for given user.
        /// </summary>
        /// <param name="menuName">Unique name of the menu</param>
        /// <param name="userId">User id or null for anonymous users</param>
        Task<UserMenu> GetMenuAsync(string menuName, TUserId? userId);

        /// <summary>
        /// Gets all menus specialized for given user.
        /// </summary>
        /// <param name="userId">User id or null for anonymous users</param>
        Task<IReadOnlyList<UserMenu>> GetMenusAsync(TUserId? userId);
    }
}