using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using TestWebApi.Service.Helper;

namespace TestWebApi.Service.Models
{
    [DataContract(Namespace = "http://serialize")]
    public class Collection
    {
        [DataMember(EmitDefaultValue = false)]
        public Guid Id { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int DisplayIndex { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string DisplayName { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Guid> Games { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Guid> SubCollections { get; set; }
        
        public static Collection CreateCollection(int index, string name, List<Guid> games)
        {
            return new Collection
            {
                Id = Guid.NewGuid(),
                DisplayIndex = index,
                DisplayName = name,
                Games = games
            };
        }
    }

    public static class CollectionList
    {
        private static readonly string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Collections.xml");
        public static readonly List<Collection> Items;

        static CollectionList()
        {
            Items = DataStorageHelper.Deserialize<List<Collection>>(FilePath);
            GenerateTestList();
        }

        private static void GenerateTestList()
        {
            for (var i = 0; i < 30; i++)
            {
                var name = DataGenerationHelper.GenerateName(10);
                var gameGuidList = DataGenerationHelper.GetRandomGameGuidList(GameList.Items);
                var collection = Collection.CreateCollection(i, name, gameGuidList);
                Items.Add(collection);
            }

            foreach (var collection in Items)
            {
                var collectionGuidList = DataGenerationHelper.GetRandomCollectionGuidList(Items, collection.Id);
                if (collectionGuidList.Count > 0) collection.SubCollections = collectionGuidList;
            }

            Update();
        }

        public static void Update()
        {
            var xml = DataStorageHelper.Serialize(Items.GetType(), Items);
            xml.Save(FilePath);
        }
    }
}