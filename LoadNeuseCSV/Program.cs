using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;
using LINQtoCSV;
using LoadNeuseCSV.Properties;

namespace LoadNeuseCSV
{
    internal class SectionCsv
    {
        [CsvColumn(Name = "id_item", FieldIndex = 1)]
        public int IdItem { get; set; }

        [CsvColumn(Name = "itemudfc1", FieldIndex = 2)]
        public string Itemudfc1 { get; set; }

        [CsvColumn(Name = "itemudfc2", FieldIndex = 3)]
        public string Itemudfc2 { get; set; }

        [CsvColumn(Name = "itemudfc3", FieldIndex = 4)]
        public string Itemudfc3 { get; set; }

        [CsvColumn(Name = "itemudfc4", FieldIndex = 5)]
        public string Itemudfc4 { get; set; }

        [CsvColumn(Name = "itemudfc5", FieldIndex = 6)]
        public string Itemudfc5 { get; set; }

        [CsvColumn(Name = "desc2", FieldIndex = 7)]
        public string Desc2 { get; set; }

        [CsvColumn(Name = "desc1", FieldIndex = 8)]
        public string Desc1 { get; set; }

        [CsvColumn(Name = "webdesc", FieldIndex = 9)]
        public string Webdesc { get; set; }

        [CsvColumn(Name = "qoh", FieldIndex = 10)]
        public float Qoh { get; set; }

        [CsvColumn(Name = "commitqty", FieldIndex = 11)]
        public float Commitqty { get; set; }

        [CsvColumn(Name = "price1", FieldIndex = 12)]
        public float Price1 { get; set; }

        [CsvColumn(Name = "price2", FieldIndex = 13)]
        public float Price2 { get; set; }

        [CsvColumn(Name = "price3", FieldIndex = 14)]
        public float Price3 { get; set; }

        [CsvColumn(Name = "price4", FieldIndex = 15)]
        public float Price4 { get; set; }

        [CsvColumn(Name = "price5", FieldIndex = 16)]
        public float Price5 { get; set; }

        [CsvColumn(Name = "retail", FieldIndex = 17)]
        public float Retail { get; set; }

        [CsvColumn(Name = "disc1", FieldIndex = 18)]
        public float Disc1 { get; set; }

        [CsvColumn(Name = "mfg", FieldIndex = 19)]
        public string Mfg { get; set; }

        [CsvColumn(Name = "style", FieldIndex = 20)]
        public string Style { get; set; }

        [CsvColumn(Name = "weight", FieldIndex = 21)]
        public float Weight { get; set; }

        [CsvColumn(Name = "height", FieldIndex = 22)]
        public float Height { get; set; }

        [CsvColumn(Name = "width", FieldIndex = 23)]
        public float Width { get; set; }

        [CsvColumn(Name = "length", FieldIndex = 24)]
        public float Length { get; set; }

        [CsvColumn(Name = "vendor", FieldIndex = 25)]
        public string Vendor { get; set; }

        [CsvColumn(Name = "xref", FieldIndex = 26)]
        public string Xref { get; set; }

        [CsvColumn(Name = "upc", FieldIndex = 27)]
        public string Upc { get; set; }
    }

    internal class Program
    {
        public static string id { get; set; }
        private static void Main(string[] args)
        {

            var counter = 0;
         
            TextReader reader = File.OpenText(Settings.Default.CSVFile);
            var csv = new CsvReader(reader);
            csv.Configuration.Delimiter = "@";
            csv.Configuration.HasHeaderRecord = false;
            csv.Configuration.IgnoreQuotes = false;
            csv.Configuration.Quote = '"';
            csv.Configuration.IgnoreReadingExceptions = true;
            csv.Configuration.WillThrowOnMissingField = false;

            while (csv.Read())
            {
               
// or ...
                try
                {
                    String intUPC;
                    csv.TryGetField(26, out intUPC);
                 //   Console.WriteLine(intUPC.ToString());

                    String intPrice;
                    csv.TryGetField(16, out intPrice);
                //    Console.WriteLine(intPrice.ToString());

                    if (intUPC.Trim().Length > 0)
                    {
                        Int64 intUPC64 = Convert.ToInt64(intUPC);
                        intUPC = Convert.ToString(intUPC64);
                    }



                    // var record = csv.GetRecord<SectionCsv>();
                   // id = record.Upc;

                    //Data maping object to our database
                    if (intUPC.Trim().Length > 0)
                    { 
                    
                    var dc = new ProductsDataContext();
                    var myProduct = new product();

                    IQueryable<product> q =
                        from a in dc.GetTable<product>()
                        where (a.pID.CompareTo(intUPC) == 0)
                        select a;
                    if (q.Any())
                    {
                        ++counter;
                        Console.WriteLine(counter);
                        myProduct.pPrice = Convert.ToDouble(intPrice);
                        dc.SubmitChanges();
                    }
                    }

                    // executes the appropriate commands to implement the changes to the database
                }

                catch (Exception e)
                {
                    // Process all exceptions generated while processing the file

                    Console.WriteLine(id);
                 
                }
            }
        }
    }
}
    
