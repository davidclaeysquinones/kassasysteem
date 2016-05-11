using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kassa.DAO
{
    public class ArticleDAO
    {
        public IEnumerable<Article> All()
        {
            using (var db = new KassaEntities())
            {
                
            }
        }
    }
}
