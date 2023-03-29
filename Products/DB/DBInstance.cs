using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.DB
{
    public class DBInstance
    {
        private static DbProductContext instance;
        public static DbProductContext GetInstance()
        {
            if(instance == null)
                instance = new DbProductContext();
            return instance;
        }
    }
}
