using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Model
{
    public class ConcreteData
    {
        
        private String _code;
      
        private String _name;

        public String Code
        {
            get { return _code; }
            set { _code = value; }
        }


        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public ConcreteData(String code, String name)
        {
            _code = code;
            _name = name;
        }




    }
}
