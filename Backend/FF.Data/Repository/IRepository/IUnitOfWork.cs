using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Data.Access.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IReviewRepository reviewRepository { get; }
        IRestaurantRepository restaurantRepository { get; }
        IUserRepository userRepository { get; }
        INotificationRepository notificationRepository { get; }

        void Save();
    }
}
