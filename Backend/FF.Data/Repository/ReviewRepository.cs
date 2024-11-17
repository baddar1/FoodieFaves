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
    public class ReviewRepository : Repository<Review>, IReviewRepository
    {
        private readonly ApplicationDbContext _db;

        public ReviewRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Review obj)
        {
            _db.Reviews.Update(obj);
        }
    }
}
