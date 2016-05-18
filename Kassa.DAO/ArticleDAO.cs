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

        public IEnumerable<Article> All()
        {
            using (var db = new kassaEntities())
            {
                return db.Article.ToList();
            }
        }

        public void Update(Article article)
        {
            using (var db = new kassaEntities())
            {

                db.Article.Attach(article);
                var entry = db.Entry(article);
                entry.State = EntityState.Modified;

                db.SaveChanges();
            }
        }

        public void Delete(Article article)
        {
            using (var db = new kassaEntities())
            {
                db.Article.Attach(article);
                var entry = db.Entry(article);
                entry.State = EntityState.Deleted;

                db.SaveChanges();
            }
        }

        public void Add(Article article)
        {
            using (var db = new kassaEntities())
            {
                db.Article.Attach(article);
                var entry = db.Entry(article);
                entry.State = EntityState.Added;
                db.SaveChanges();
            }
        }
    }
}
