using System;
using System.Collections.Generic;
using System.Linq;
using TestWebApi.Service.Enums;
using TestWebApi.Service.Models;

namespace TestWebApi.Service.Helper
{
    public class DataGenerationHelper
    {
        #region Generate games and collections lists

        public static string GenerateName(int len)
        {
            string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "l", "n", "p", "q", "r", "s", "sh", "zh", "t", "v", "w", "x" };
            string[] vowels = { "a", "e", "i", "o", "u", "ae", "y" };
            var name = "";
            name += consonants[GenName.Next(consonants.Length)].ToUpper();
            name += vowels[GenName.Next(vowels.Length)];
            var b = 2; //b tells how many times a new letter has been added. It's 2 right now because the first two letters are already in the name.
            while (b < len)
            {
                name += consonants[GenName.Next(consonants.Length)];
                b++;
                name += vowels[GenName.Next(vowels.Length)];
                b++;
            }
            return name;
        }

        public static DateTime GetRandomDate()
        {
            var start = new DateTime(2010, 1, 1);
            var range = (DateTime.Today - start).Days;
            return start.AddDays(GenDate.Next(range));
        }

        public static Category GetRandomCategory()
        {
            var range = Enum.GetValues(typeof(Category)).Length;
            return (Category) GenCategory.Next(range);
        }

        public static List<Guid> GetRandomGameGuidList(IReadOnlyList<Game> games)
        {
            var guidList = new List<Guid>();
            for (var i = 0; i < 10; i++)
            {
                guidList.Add(games[GenGame.Next(games.Count)].Id);
            }

            return guidList;
        }

        public static List<Guid> GetRandomCollectionGuidList(IReadOnlyList<Collection> collections, Guid id)
        {
            var guidList = new List<Guid>();
            for (var i = 0; i < 5; i++)
            {
                var collection = collections[GenCollection.Next(collections.Count)];
                if (IsItemInSubCollection(collections, collection, id)) continue;
                guidList.Add(collection.Id);
            }
            return guidList;
        }

        private static bool IsItemInSubCollection(IReadOnlyList<Collection> collections, Collection collection, Guid id)
        {
            //recursive check sub collections to avoid references to each other
            if (collection.Id == id) return true;
            if (collection.SubCollections == null) return false;
            foreach (var itemId in collection.SubCollections)
            {
                if (itemId == id) return true;
                var subCollection = collections.Single(x => x.Id == itemId);
                if (IsItemInSubCollection(collections, subCollection, id)) return true;
            }
            return false;
        }

        #endregion

        private static readonly Random GenDate = new Random();
        private static readonly Random GenCategory = new Random();
        private static readonly Random GenName = new Random();
        private static readonly Random GenGame = new Random();
        private static readonly Random GenCollection = new Random();
    }
}