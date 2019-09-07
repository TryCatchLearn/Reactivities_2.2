using System.Threading.Tasks;
using Application.User;

namespace Application.Interfaces
{
    public interface IFacebookAccessor
    {
         Task<FacebookUserInfo> FacebookLogin(string fbAccessToken);
    }
}