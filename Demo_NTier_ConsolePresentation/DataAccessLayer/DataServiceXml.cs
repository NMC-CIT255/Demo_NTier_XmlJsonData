using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using Demo_NTier_XmlJsonData.Models;
using Demo_NTier_XmlJsonData.DataAccessLayer;

namespace Demo_NTier_XmlJsonData.DataAccessLayer
{
    public class DataServiceXml : IDataService
    {
        private string _dataFilePath;

        /// <summary>
        /// read the xml file and load a list of character objects
        /// </summary>
        /// <returns>list of characters</returns>
        public IEnumerable<FlintstoneCharacter> ReadAll()
        {
            List<FlintstoneCharacter> characters = new List<FlintstoneCharacter>();

            XmlSerializer serializer = new XmlSerializer(typeof(List<FlintstoneCharacter>));

            try
            {
                StreamReader reader = new StreamReader(_dataFilePath);
                using (reader)
                {
                    characters = (List<FlintstoneCharacter>)serializer.Deserialize(reader);
                }

            }
            catch (Exception)
            {
                throw;
            }

            return characters;
        }

        /// <summary>
        /// write the current list of characters to the xml data file
        /// </summary>
        /// <param name="characters">list of characters</param>
        public void WriteAll(IEnumerable<FlintstoneCharacter> characters)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<FlintstoneCharacter>), new XmlRootAttribute("Characters"));

            try
            {
                StreamWriter writer = new StreamWriter(_dataFilePath);
                using (writer)
                {
                    serializer.Serialize(writer, characters);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataServiceXml()
        {
            _dataFilePath = DataConfig.DataPathXml;
        }
    }
}
