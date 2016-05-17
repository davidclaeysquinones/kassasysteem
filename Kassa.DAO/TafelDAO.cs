using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Dynamic;
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

        public void Update(Tafel tafel)
        {
            using (var db = new kassaEntities())
            {
               
                db.Tafel.Attach(tafel);
                var entry = db.Entry(tafel);
                entry.State=EntityState.Modified;
                
                db.SaveChanges();
            }
        }

        public void Delete(Tafel tafel)
        {
            using (var db = new kassaEntities())
            {
                db.Tafel.Attach(tafel);
                var entry = db.Entry(tafel);
                entry.State = EntityState.Deleted;

                db.SaveChanges();
            }
        }
    }
}
