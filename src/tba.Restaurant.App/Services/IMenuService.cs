using System.Threading.Tasks;
using tba.Restaurant.App.Models;

namespace tba.Restaurant.App.Services
{
    public interface IMenuService
    {
        Task<MenuModel> GetAsync(long userId, long menuId);
        Task<MenuModel> InsertMenuAsync(long userId, MenuRequest menu);
    }
}