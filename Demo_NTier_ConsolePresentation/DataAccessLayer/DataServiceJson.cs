using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using Demo_NTier_XmlJsonData.Models;
using Demo_NTier_XmlJsonData.DataAccessLayer;
using Newtonsoft;
using Newtonsoft.Json;

namespace Demo_NTier_XmlJsonData.DataAccessLayer
{
    public class DataServiceJson : IDataService
    {
        private string _dataFilePath;


        /// <summary>
        /// read the json file and load a list of character objects
        /// </summary>
        /// <returns>list of characters</returns>
        public IEnumerable<FlintstoneCharacter> ReadAll()
        {
            List<FlintstoneCharacter> characters;

            try
            {
                using (StreamReader sr = new StreamReader(_dataFilePath))
                {
                    string jsonString = sr.ReadToEnd();

                    characters = JsonConvert.DeserializeObject<List<FlintstoneCharacter>>(jsonString);
                }

            }
            catch (Exception)
            {
                throw;
            }

            return characters;
        }

        /// <summary>
        /// write the current list of characters to the json data file
        /// </summary>
        /// <param name="characters">list of characters</param>
        public void WriteAll(IEnumerable<FlintstoneCharacter> characters)
        {
            //RootObject rootObject = new RootObject();
            //rootObject.Characters = new FlintstoneCharacter();
            //rootObject.Characters.Character = characters as List<CharFlintstoneCharacteracter>;

            string jsonString = JsonConvert.SerializeObject(characters, Formatting.Indented);

            try
            {
                StreamWriter writer = new StreamWriter(_dataFilePath);
                using (writer)
                {
                    writer.WriteLine(jsonString);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataServiceJson()
        {
            _dataFilePath = DataConfig.DataPathJson;
        }
    }
}
