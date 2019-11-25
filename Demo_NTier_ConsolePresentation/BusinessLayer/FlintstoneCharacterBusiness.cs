using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo_NTier_XmlJsonData.Models;
using Demo_NTier_XmlJsonData.DataAccessLayer;

namespace Demo_NTier_XmlJsonData.BusinessLayer
{
    public class FlintstoneCharacterBusiness
    {
        public FileIoMessage FileIoStatus { get; set; }

        public FlintstoneCharacterBusiness()
        {

        }

        /// <summary>
        /// retrieve a character using the repository
        /// </summary>
        /// <returns>character</returns>
        private FlintstoneCharacter GetCharacter(int id)
        {
            FlintstoneCharacter character = null;
            FileIoStatus = FileIoMessage.None;

            try
            {
                using (FlintstoneCharacterRepository fsRepository = new FlintstoneCharacterRepository())
                {
                    character = fsRepository.GetById(id);
                };

                if (character != null)
                {
                    FileIoStatus = FileIoMessage.Complete;
                }
                else
                {
                    FileIoStatus = FileIoMessage.RecordNotFound;
                }
            }
            catch (Exception)
            {
                FileIoStatus = FileIoMessage.FileAccessError;
            }

            return character;
        }

        /// <summary>
        /// retrieve a list of all characters using the repository
        /// </summary>
        /// <returns>all characters</returns>
        private List<FlintstoneCharacter> GetAllCharacters()
        {
            List<FlintstoneCharacter> characters = null;
            FileIoStatus = FileIoMessage.None;

            try
            {
                using (FlintstoneCharacterRepository fsRepository = new FlintstoneCharacterRepository())
                {
                    characters = fsRepository.GetAll() as List<FlintstoneCharacter>;
                };

                if (characters != null)
                {
                    FileIoStatus = FileIoMessage.Complete;
                }
                else
                {
                    FileIoStatus = FileIoMessage.NoRecordsFound;
                }
            }
            catch (Exception)
            {
                FileIoStatus = FileIoMessage.FileAccessError;
            }

            return characters;
        }

        /// <summary>
        /// provide a list of all characters
        /// </summary>
        /// <returns>list of all characters</returns>
        public List<FlintstoneCharacter> AllFlintstoneCharacters()
        {
            return GetAllCharacters() as List<FlintstoneCharacter>;
        }

        /// <summary>
        /// retrieve a character by id 
        /// </summary>
        /// <param name="id">character id</param>
        /// <returns>character</returns>
        public FlintstoneCharacter FlintstoneCharacterById(int id)
        {
            return GetCharacter(id);
        }

        /// <summary>
        /// add a new character
        /// </summary>
        /// <param name="character">character to add</param>
        public void AddFlintstoneCharacter(FlintstoneCharacter character)
        {
            try
            {
                if (character != null)
                {
                    using (FlintstoneCharacterRepository fsRepository = new FlintstoneCharacterRepository())
                    {
                        fsRepository.Add(character);
                    };

                    FileIoStatus = FileIoMessage.Complete;
                }
            }
            catch (Exception)
            {
                FileIoStatus = FileIoMessage.FileAccessError;
            }
        }

        /// <summary>
        /// retrieve a character by id 
        /// </summary>
        /// <param name="id">character id</param>
        public void DeleteFlintstoneCharacter(int id)
        {
            try
            {
                if (GetCharacter(id) != null)
                {
                    using (FlintstoneCharacterRepository repo = new FlintstoneCharacterRepository())
                    {
                        repo.Delete(id);
                    }

                    FileIoStatus = FileIoMessage.Complete;
                }
                else
                {
                    FileIoStatus = FileIoMessage.RecordNotFound;
                }
            }
            catch (Exception)
            {
                FileIoStatus = FileIoMessage.FileAccessError;
            }
        }

        public void UpdateFlintstoneCharacter(FlintstoneCharacter updatedCharacter)
        {
            try
            {
                if (GetCharacter(updatedCharacter.Id) != null)
                {
                    using (FlintstoneCharacterRepository repo = new FlintstoneCharacterRepository())
                    {
                        repo.Update(updatedCharacter);
                    }

                    FileIoStatus = FileIoMessage.Complete;
                }
                else
                {
                    FileIoStatus = FileIoMessage.RecordNotFound;
                }
            }
            catch (Exception)
            {
                FileIoStatus = FileIoMessage.FileAccessError;
            }
        }

    }
}
