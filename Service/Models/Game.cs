using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using TestWebApi.Service.Enums;
using TestWebApi.Service.Helper;

namespace TestWebApi.Service.Models
{
    [DataContract(Namespace = "http://serialize")]
    public class Game
    {
        [DataMember(EmitDefaultValue = false)]
        public Guid Id { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int DisplayIndex { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string DisplayName { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DateTime ReleaseDate { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public Category Category { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public Thumbnail Thumbnail { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Device> Devices { get; set; }

        public static Game CreateGame(Category category, int index, string name, DateTime release)
        {
            return new Game
            {
                Id = Guid.NewGuid(),
                Category = category,
                DisplayIndex = index,
                DisplayName = name,
                ReleaseDate = release,
                Devices = index % 3 == 0
                    ? new List<Device> {Device.Mobile}
                    : index % 5 == 0
                        ? new List<Device> {Device.Desktop, Device.Mobile}
                        : new List<Device> {Device.Desktop},
                Thumbnail = new Thumbnail
                    {Height = 800, Width = 600, Image = $"https://cdnfake.gameserver.com/{name}.jpg"}
            };
        }
    }

    public class Thumbnail
    {
        public string Image { get; set; }
        public ushort Height { get; set; }
        public ushort Width { get; set; }
    }

    public static class GameList
    {
        private static readonly string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Games.xml");
        public static readonly List<Game> Items;

        static GameList()
        {
            Items = DataStorageHelper.Deserialize<List<Game>>(FilePath);
            GenerateTestList();
        }

        private static void GenerateTestList()
        {
            for (var i = 0; i < 300; i++)
            {
                var gameName = DataGenerationHelper.GenerateName(10);
                var releaseDate = DataGenerationHelper.GetRandomDate();
                var category = DataGenerationHelper.GetRandomCategory();
                var item = Game.CreateGame(category, i, gameName, releaseDate);
                Items.Add(item);
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