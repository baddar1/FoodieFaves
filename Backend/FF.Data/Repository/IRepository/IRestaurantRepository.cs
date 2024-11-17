using FF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Data.Access.Repository.IRepository
{
    public interface IRestaurantRepository : IRepository<Restaurant>
    {
        void Update(Restaurant obj);
    }
}
