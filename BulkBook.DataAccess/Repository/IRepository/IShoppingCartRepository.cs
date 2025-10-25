using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkBook.DataAccess.Repository.IRepository
{
    public interface IShoppingCartRepository : IRepository<ShoppingCart>
    {
       int IncrementCount(ShoppingCart cart,int count);
        int DecrementtCount(ShoppingCart cart, int count);
    }
}
