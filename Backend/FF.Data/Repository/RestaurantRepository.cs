using FF.Data.Access.Data;
using FF.Data.Access.Repository.IRepository;
using FF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Data.Access.Repository
{
    public class RestaurantRepository : Repository<Restaurant>, IRestaurantRepository
    {
        private readonly ApplicationDbContext _db;

        public RestaurantRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Restaurant obj)
        {
            _db.Restaurants.Update(obj);
        }

    }
}
