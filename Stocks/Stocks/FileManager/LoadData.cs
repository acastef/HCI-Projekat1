using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using Stocks.Model;

namespace Stocks.FileManager
{
    class LoadData
    {

        public LoadData()
        {

        }

        public AbstractData readAbstractDataCSV(String path, String delimiter)
        {
            AbstractData AData;

            String currentPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
      
            using (TextFieldParser parser = new TextFieldParser(currentPath + "\\Files\\" + path))
            {

                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(delimiter);
                List<ConcreteData> setOfData = new List<ConcreteData>();
                while (!parser.EndOfData)
                {
                    //Processing row
                    string[] fields = parser.ReadFields();
                    ConcreteData concreteData = new ConcreteData(fields[0], fields[1]);
                    setOfData.Add(concreteData);
                }

                AData = new AbstractData("Stocks", setOfData);
            }

            return AData;
        }

        public AbstractData ReadStocks()
        {
             return readAbstractDataCSV("stock_list.txt", " ");

        }

        public AbstractData ReadCriptoCurrencies()
        {
            return readAbstractDataCSV("physical_currency_list.csv", ",");
           
        }

        public AbstractData ReadCurrencies()
        {
            return readAbstractDataCSV("digital_currency_list.csv", ",");

        }
    }

    
}
