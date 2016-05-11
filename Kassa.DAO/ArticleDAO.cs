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
        public IEnumerable<Article> All()
        {
            using (var db = new kassaEntities())
            {
                
            }
        }
    }
}
