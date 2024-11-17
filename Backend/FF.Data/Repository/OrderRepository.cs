using FF.Data.Access.Data;
using FF.Data.Access.Repository;
using FF.Data.Repository.IRepository;
using FF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Data.Repository
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {

        private readonly ApplicationDbContext _db;
        public OrderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
