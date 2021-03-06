﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kassa.DAO;
using Kassa.Model;

namespace Kassa.Service
{
    public class TafelService
    {
        private TafelDAO tafelDAO;
        public TafelService()
        {
            tafelDAO = new TafelDAO();
        }

        public int getAantal()
        {
            return tafelDAO.getAantal();
        }

        public IEnumerable<Tafel> All()
        {
            return tafelDAO.All();
        }

        public void Update(Tafel tafel)
        {
            tafelDAO.Update(tafel);
        }

        public void Delete(Tafel tafel)
        {
            tafelDAO.Delete(tafel);
        }

        public void Add(Tafel tafel)
        {
            tafelDAO.Add(tafel);
        }
    }
}
