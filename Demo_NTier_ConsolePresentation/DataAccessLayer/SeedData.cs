using Demo_NTier_XmlJsonData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_NTier_XmlJsonData.DataAccessLayer
{
    public class SeedData
    {
        public static List<FlintstoneCharacter> GenerateListOfCharacters()
        {
            List<FlintstoneCharacter> characters = new List<FlintstoneCharacter>()
            {
                new FlintstoneCharacter()
                {
                    Id = 1,
                    LastName = "Flintstone",
                    FirstName = "Fred",
                    Age = 28,
                    ImageFileName = "fred_flintstone.jpg",
                    Description = "Fred is the main character of the series. He's an accident-prone bronto-crane operator at the Slate Rock and Gravel Company and the head of the Flintstone household. He is quick to anger (usually over trivial matters), but is nonetheless a very loving husband and father. He's also good at bowling and is a member of the fictional 'Loyal Order of Water Buffaloes' (Lodge No. 26), a men-only club paralleling real-life fraternities such as the Loyal Order of Moose.",
                    Gender = FlintstoneCharacter.GenderType.Male,
                    GroceryList = new List<GroceryItem>()
                    {
                        new GroceryItem()
                        {
                            Name = "Apples",
                            Quantity = 20
                        },
                        new GroceryItem()
                        {
                            Name = "Oranges",
                            Quantity = 13
                        },
                        new GroceryItem()
                        {
                            Name = "Bananas",
                            Quantity = 2
                        }
                    }
                },
                new FlintstoneCharacter()
                {
                    Id = 3,
                    LastName = "Flintstone",
                    FirstName = "Wilma",
                    Age = 28,
                    Description = "Wilma is Fred's wife. She is more intelligent and level-headed than her husband, though she often has a habit of spending money recklessly (with Betty and her catchphrase being 'Da-da-da duh da-da CHARGE IT!!'). She's often a foil to Fred's poor behavior.",
                    ImageFileName = "wilma_flintstone.jpg",
                    Gender = FlintstoneCharacter.GenderType.Female,
                    GroceryList = new List<GroceryItem>()
                    {
                        new GroceryItem()
                        {
                            Name = "Bread",
                            Quantity = 2
                        },
                        new GroceryItem()
                        {
                            Name = "Milk",
                            Quantity = 1
                        },
                        new GroceryItem()
                        {
                            Name = "Butter",
                            Quantity = 2
                        }
                    }
                },
                new FlintstoneCharacter()
                {
                    Id = 4,
                    LastName = "Flintstone",
                    FirstName = "Pebbles",
                    Age = 1,
                    Gender = FlintstoneCharacter.GenderType.Female,
                    ImageFileName = "Pebbles_Flintstone.jpg",
                    Description = "Pebbles is the Flintstones' infant daughter, who is born near the end of the third season.",
                    GroceryList = new List<GroceryItem>()
                },
            };

            return characters;
        }
    }
}
