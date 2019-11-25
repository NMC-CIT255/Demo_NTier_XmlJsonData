using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo_NTier_XmlJsonData.Models;

namespace Demo_NTier_XmlJsonData.DataAccessLayer
{
    public class DataConfig
    {
        //
        // set the type of persistence
        //
        //public static DataType dataType = DataType.XML;
        public static DataType dataType = DataType.JSON;

        public static string DataPathJson => @"DataAccessLayer\DataJson\FlintstoneCharacters.json";
        public static string DataPathXml => @"DataAccessLayer\DataXml\FlintstoneCharacters.xml";
        public static string ImagePath => @"Images\";
    }
}
