using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Model
{
    public class AbstractData
    {
        private String _name;
        private List<ConcreteData> _concreteDataCollection;


        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }
        
        public List<ConcreteData> ConcreteDataCollection
        {
            get { return _concreteDataCollection; }
            set { _concreteDataCollection = value; }
        }

        public AbstractData(String name, List<ConcreteData> collection )
        {
            _name = name;
            _concreteDataCollection = collection;

        }






    }
}
