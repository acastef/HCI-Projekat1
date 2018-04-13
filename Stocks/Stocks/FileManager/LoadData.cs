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

        public AbstractData readAbstractDataCSV(String path, String delimiter, String name)
        {
            AbstractData AData;

            String currentPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
      
            using (TextFieldParser parser = new TextFieldParser(currentPath + "\\Files\\" + path))
            {

                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(delimiter);
                Dictionary<String, String> setOfData = new Dictionary<String, String>();
                while (!parser.EndOfData)
                {
                    //Processing row
                    string[] fields = parser.ReadFields();
                    //ConcreteData concreteData = new ConcreteData(fields[0], fields[1]);
                    setOfData[fields[0]] = fields[1];
                }

                AData = new AbstractData(name, setOfData);
            }

            return AData;
        }

        internal int[] ReadMetaData(String path)
        {
            String currentPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;

            using (TextFieldParser parser = new TextFieldParser(currentPath + "\\Files\\" + path))
            {
                int[] intArray = new int[2];

                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(":");
                Dictionary<String, String> listOfParameters = new Dictionary<String, String>();

                intArray[0] = Int32.Parse( parser.ReadLine().Split(':')[1] );
                intArray[1] = Int32.Parse( parser.ReadLine().Split(':')[1] );

                return intArray;
            }
        }

        public List<string> ReadDefaultCurrencies(String path)
        {
            String currentPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;

            using (TextFieldParser parser = new TextFieldParser(currentPath + "\\Files\\" + path))
            {

                List<String> setOfData = new List<String>();
                while (!parser.EndOfData)
                {
                    string currency = parser.ReadLine();
                    setOfData.Add(currency);
                }

                return setOfData;
            }
            
            
        }

        public List<int> ReadRefreshRates(String path)
        {
            String currentPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;

            using (TextFieldParser parser = new TextFieldParser(currentPath + "\\Files\\" + path))
            {

                List<int> setOfData = new List<int>();
                while (!parser.EndOfData)
                {
                    int rate = Int32.Parse( parser.ReadLine().Trim() );
                    setOfData.Add(rate);
                }

                return setOfData;
            }
        }

        public AbstractData ReadStocks()
        {
             return readAbstractDataCSV("stock_list.txt", " ", "Stocks");

        }

        public AbstractData ReadCriptoCurrencies()
        {
            return readAbstractDataCSV("physical_currency_list.csv", ",", "CriptoCurrencies");
           
        }

        public AbstractData ReadCurrencies()
        {
            return readAbstractDataCSV("digital_currency_list.csv", ",", "DigitalCurrencies");

        }

        public Dictionary<String, String> ReadRealTimeParameteres(String path, String delimiter)
        {
            String currentPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;

            using (TextFieldParser parser = new TextFieldParser(currentPath + "\\Files\\" + path))
            {

                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(delimiter);
                Dictionary<String, String> listOfParameters = new Dictionary<String, String>();
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    //String[] keyValue = fields[0].Trim().Split(':');
                    listOfParameters[fields[0].Trim()] = fields[1].Trim();
                }

                return listOfParameters;
            }
        }
    }

    
}
