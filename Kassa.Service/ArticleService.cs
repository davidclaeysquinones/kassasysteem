﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kassa.DAO;
using Kassa.Model;

namespace Kassa.Service
{
    public class ArticleService
    {
        private ArticleDAO articleDAO;
        public ArticleService()
        {
            articleDAO = new ArticleDAO();
        }

        public int getAantal()
        {
            return articleDAO.getAantal();
        }

        public IEnumerable<Article> All()
        {
            return articleDAO.All();
        }
    }
}
