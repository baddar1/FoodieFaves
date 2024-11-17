using FF.Data.Access.Data;
using FF.Data.Access.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Data.Access.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public IReviewRepository reviewRepository { get; private set; }

        public IRestaurantRepository restaurantRepository { get; private set; }
        public IUserRepository userRepository { get; private set; }
        public INotificationRepository notificationRepository { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            reviewRepository= new ReviewRepository(db);
            restaurantRepository = new RestaurantRepository(db);
            userRepository = new UserRepository(db);
            notificationRepository = new NotificationRepository(db);

        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
