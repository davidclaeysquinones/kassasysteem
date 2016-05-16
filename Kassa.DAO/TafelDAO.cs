using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kassa.Model;

namespace Kassa.DAO
{
    public class TafelDAO
    {
        public int getAantal()
        {
            using (var db = new kassaEntities())
            {
                return db.Tafel.Count();
            }
        }
        public IEnumerable<Tafel> All()
        {
            using (var db = new kassaEntities())
            {
                return db.Tafel.ToList();
            }
        }
    }
}
