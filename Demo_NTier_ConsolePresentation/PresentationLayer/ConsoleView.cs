using Demo_NTier_XmlJsonData.DataAccessLayer;
using Demo_NTier_XmlJsonData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo_NTier_XmlJsonData.BusinessLayer;

/// <summary>
/// Demo app for XML and JSON serialization
/// </summary>
namespace Demo_NTier_ConsolePresentation.PresentationLayer
{
    public class ConsoleView
    {
        private static FlintstoneCharacterBusiness _fcBusiness = new FlintstoneCharacterBusiness();
        public ConsoleView()
        {
            DisplayWelcomeScreen();
            DisplayMainMenu();
            DisplayClosingScreen();
        }

        private static void DisplayShoppingLists()
        {
            List<FlintstoneCharacter> characters = _fcBusiness.AllFlintstoneCharacters();

            DisplayScreenHeader("Shopping Lists by Character");

            Console.WriteLine();
            Console.WriteLine("\tCharacters");
            Console.WriteLine("\t------------------");
            foreach (var character in characters)
            {
                Console.WriteLine("\t" + character.FullName);

                if (character.GroceryList != null && character.GroceryList.Any())
                {
                    Console.WriteLine("\t\tShopping List");
                    Console.WriteLine("\t\t-------------");
                    foreach (var item in character.GroceryList)
                    {
                        Console.WriteLine("\t\t" + item.Name.PadRight(15) + item.Quantity.ToString().PadLeft(4));
                    }
                }
            }

            DisplayMainMenuPrompt();
        }

        /// <summary>
        /// Display Main Menu
        /// </summary>
        private static void DisplayMainMenu()
        {
            bool quitApplication = false;
            char menuChoiceKey;

            do
            {
                DisplayScreenHeader("Main Menu");

                //
                // get the user's menu choice
                //
                Console.WriteLine("\ta) List All Characters");
                Console.WriteLine("\tb) Character Detail");
                Console.WriteLine("\tc) Add Character");
                Console.WriteLine("\td) Delete Character");
                Console.WriteLine("\te) Update Character");
                Console.WriteLine("\tq) Quit");
                Console.Write("\n\tEnter Choice: ");
                menuChoiceKey = Console.ReadKey().KeyChar;

                //
                // process user's choice
                //
                switch (menuChoiceKey)
                {
                    case 'a':
                        DisplayAllCharacters();
                        break;

                    case 'b':
                        DisplayCharacterDetail();
                        break;

                    case 'c':
                        DisplayAddCharacter();
                        break;

                    case 'd':
                        DisplayDeleteCharacter();
                        break;

                    case 'e':
                        DisplayUpdateCharacter();
                        break;

                    case 'q':
                        quitApplication = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\t*************************************************");
                        Console.WriteLine("\t   Please indicate your choice with a letter.");
                        Console.WriteLine("\t**************************************************");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitApplication);
        }

        /// <summary>
        /// SCREEN: update a character
        /// </summary>
        static void DisplayUpdateCharacter()
        {
            List<FlintstoneCharacter> characters = _fcBusiness.AllFlintstoneCharacters();

            DisplayScreenHeader("Update Character");

            int id = DisplayGetCharacterIdFromList(characters);

            FlintstoneCharacter character = _fcBusiness.FlintstoneCharacterById(id);

            DisplayScreenHeader("Update Character");

            if (_fcBusiness.FileIoStatus == FileIoMessage.Complete)
            {
                Console.Write($"Current First Name: {character.FirstName}   New First Name: ");
                character.FirstName = Console.ReadLine();

                Console.Write($"Current Last Name: {character.LastName}   New ");
                character.LastName = Console.ReadLine();

                GetInteger("Current Age:", 1, 200, out int age);
                character.Age = age;

                Console.Write($"Current Gender: {character.Gender}   New ");
                Enum.TryParse(Console.ReadLine(), out FlintstoneCharacter.GenderType gender);
                character.Gender = gender;

                Console.Write($"Current Image File: {character.ImageFileName}   New ");
                character.ImageFileName = Console.ReadLine();

                GetInteger("Gross Salary:", 1, 1000000, out int grossSalary);
                character.Age = grossSalary;

                Console.Write($"Current Description: {character.Description}   New ");
                character.Description = Console.ReadLine();
            }
            else
            {
                // process file IO error message
            }

            _fcBusiness.UpdateFlintstoneCharacter(character);

            if (_fcBusiness.FileIoStatus == FileIoMessage.Complete)
            {
                Console.WriteLine($"{character.FullName} updated.");
            }
            else
            {
                // process file IO error message
            }

            DisplayMainMenuPrompt();
        }

        /// <summary>
        /// SCREEN: display delete a character
        /// </summary>
        static void DisplayDeleteCharacter()
        {
            List<FlintstoneCharacter> characters = _fcBusiness.AllFlintstoneCharacters();

            int id = DisplayGetCharacterIdFromList(characters);

            _fcBusiness.DeleteFlintstoneCharacter(id);

            if (_fcBusiness.FileIoStatus == FileIoMessage.Complete)
            {
                Console.WriteLine();
                Console.WriteLine("Character deleted successfully.");
            }
            else
            {
                // process file IO error message
            }

            DisplayMainMenuPrompt();
        }

        /// <summary>
        /// SCREEN: add a character
        /// </summary>
        static void DisplayAddCharacter()
        {
            FlintstoneCharacter character = new FlintstoneCharacter();

            DisplayScreenHeader("Add a New Character");

            Console.Write("First Name:");
            character.FirstName = Console.ReadLine();

            Console.Write("Last Name:");
            character.LastName = Console.ReadLine();

            GetInteger("Age:", 1, 200, out int age);
            character.Age = age;

            Console.Write("Gender:");
            Enum.TryParse(Console.ReadLine(), out FlintstoneCharacter.GenderType gender);
            character.Gender = gender;

            Console.Write("Image File:");
            character.ImageFileName = Console.ReadLine();

            GetInteger("Gross Salary:", 1, 1000000, out int grossSalary);
            character.Age = grossSalary;

            Console.Write("Description:");
            character.Description = Console.ReadLine();

            character.GroceryList = new List<GroceryItem>();
            GetInteger("Number of Grocery Items:", 1, 20, out int numberOfGroceryItems);
            for (int i = 0; i < numberOfGroceryItems; i++)
            {
                GroceryItem groceryItem = new GroceryItem();

                Console.Write("Grocery Item Name:");
                groceryItem.Name = Console.ReadLine();

                GetInteger("Grocery Item Quantity:", 1, 200, out int quantity);
                groceryItem.Quantity = quantity;

                character.GroceryList.Add(groceryItem);
            }

            _fcBusiness.AddFlintstoneCharacter(character);

            if (_fcBusiness.FileIoStatus == FileIoMessage.Complete)
            {
                Console.WriteLine();
                Console.WriteLine($"{character.FirstName} {character.LastName} added successfully");
            }
            else
            {
                // process file IO error message
            }

            DisplayMainMenuPrompt();
        }

        /// <summary>
        /// SCREEN: display a single character's information
        /// </summary>
        static void DisplayCharacterDetail()
        {
            List<FlintstoneCharacter> characters = _fcBusiness.AllFlintstoneCharacters();

            int id = DisplayGetCharacterIdFromList(characters);

            FlintstoneCharacter character = _fcBusiness.FlintstoneCharacterById(id);

            if (_fcBusiness.FileIoStatus == FileIoMessage.Complete)
            {
                DisplayScreenHeader("Character Information");
                DisplayCharacterInfo(character);
            }
            else
            {
                // process file IO error message
            }

            DisplayMainMenuPrompt();
        }

        /// <summary>
        /// list all character's full names and ids and query the user for an id
        /// </summary>
        /// <param name="characters">character id</param>
        /// <returns></returns>
        static int DisplayGetCharacterIdFromList(List<FlintstoneCharacter> characters)
        {
            bool validResponse = false;
            int id = -1;
            List<int> validIds = characters.Select(c => c.Id).OrderBy(characterId => characterId).ToList();

            do
            {
                DisplayScreenHeader("Choose Character");

                Console.WriteLine(
                    "Name".PadRight(20) +
                    "Id".PadRight(4)
                    );
                Console.WriteLine(
                    "-------------".PadRight(20) +
                    "-----".PadRight(4)
                    );

                foreach (var character in characters)
                {
                    Console.WriteLine(
                        character.FullName.PadRight(20) +
                        character.Id.ToString().PadRight(4)
                        );
                }
                Console.WriteLine();
                Console.Write("Enter Id:");
                if (!int.TryParse(Console.ReadLine(), out id))
                {
                    Console.WriteLine("Please enter an integer value for the ID.");
                    DisplayContinuePrompt();
                }
                else if (!validIds.Contains(id))
                {
                    Console.WriteLine("Please enter a valid Id shown above.");
                    DisplayContinuePrompt();
                }
                else
                {
                    validResponse = true;
                }
            } while (!validResponse);

            return id;
        }

        /// <summary>
        /// display all character property values
        /// </summary>
        /// <param name="character">character</param>
        static void DisplayCharacterInfo(FlintstoneCharacter character)
        {
            Console.WriteLine($"Last Name: {character.LastName}");
            Console.WriteLine($"First Name: {character.FirstName}");
            Console.WriteLine($"Age: {character.Age}");
            Console.WriteLine($"Gender: {character.Gender}");
            Console.WriteLine($"Average Annual Gross: {character.AverageAnnualGross:c}");
            Console.WriteLine($"Description: \n{character.Description}");
        }

        /// <summary>
        /// SCREEN: display a table of all characters: first name, last name, and id
        /// </summary>
        static void DisplayAllCharacters()
        {
            List<FlintstoneCharacter> characters = _fcBusiness.AllFlintstoneCharacters();

            if (_fcBusiness.FileIoStatus == FileIoMessage.Complete)
            {
                DisplayScreenHeader("Flintstone Characters");

                Console.WriteLine(
                    "First Name".PadRight(15) +
                    "Last Name".PadRight(15) +
                    "Id".PadRight(4)
                    );
                Console.WriteLine(
                    "-------------".PadRight(15) +
                    "-------------".PadRight(15) +
                    "-----".PadRight(4)
                    );

                foreach (var character in characters)
                {
                    Console.WriteLine(
                        character.FirstName.PadRight(15) +
                        character.LastName.PadRight(15) +
                        character.Id.ToString().PadRight(4)
                        );
                }
            }
            else
            {
                // process file IO error message
            }

            DisplayMainMenuPrompt();
        }

        #region HELPER METHODS

        /// <summary>
        /// get a valid integer from the player - note: if max and min values are both 0, range validation is disabled
        /// </summary>
        /// <param name="prompt">prompt message in console</param>
        /// <param name="minimumValue">min. value</param>
        /// <param name="maximumValue">max. value</param>
        /// <param name="integerChoice">out value</param>
        /// <returns></returns>
        static bool GetInteger(string prompt, int minimumValue, int maximumValue, out int integerChoice)
        {
            bool validResponse = false;
            integerChoice = 0;

            //
            // validate on range if either minimumValue and maximumValue are not 0
            //
            bool validateRange = (minimumValue != 0 || maximumValue != 0);

            while (!validResponse)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out integerChoice))
                {
                    if (validateRange)
                    {
                        if (integerChoice >= minimumValue && integerChoice <= maximumValue)
                        {
                            validResponse = true;
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine($"You must enter an integer value between {minimumValue} and {maximumValue}. Please try again.");
                            DisplayContinuePrompt();
                        }
                    }
                    else
                    {
                        validResponse = true;
                    }
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine($"You must enter an integer value. Please try again.");
                    DisplayContinuePrompt();
                }
            }

            Console.CursorVisible = false;

            return true;
        }

        /// <summary>
        /// display continue prompt
        /// </summary>
        static void DisplayContinuePrompt()
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }

        /// <summary>
        /// display main menu prompt
        /// </summary>
        static void DisplayMainMenuPrompt()
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to return to the main menu.");
            Console.ReadKey();
        }

        /// <summary>
        /// display screen header
        /// </summary>
        static void DisplayScreenHeader(string headerText)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\t" + headerText);
            Console.WriteLine();
        }

        /// <summary>
        /// display welcome screen
        /// </summary>
        static void DisplayWelcomeScreen()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tDemonstration:");
            Console.WriteLine("\t\tN-Tier CRUD with XML or JSON Persistence");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// display closing screen
        /// </summary>
        static void DisplayClosingScreen()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tThank you for using our app.");
            Console.WriteLine();

            Console.WriteLine("\tPress any key to exit.");
            Console.ReadKey();
        }

        #endregion
    }
}
