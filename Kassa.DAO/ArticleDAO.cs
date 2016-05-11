using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Kassa.Model;

namespace Kassa.DAO
{
    public class ArticleDAO
    {
        public int getAantal()
        {
            using (var db = new kassaEntities())
            {
                return db.Article.Count();
            }
        }
    }
}
