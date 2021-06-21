using System;
using LiteDB;

namespace ConsoleApp1
{
    public sealed class KeyBindInfo
    {
        public int Id { get; set; }
        public string Category { get; set; }

        public KeyBindAction Action { get; set; }
    }

    public enum KeyBindAction
    {
        None,

        ConfigWindow,
    }

    class Program
    {
        public static ILiteCollection<KeyBindInfo> KeyBinds;

        static void Main(string[] args)
        {
            var mapper = new BsonMapper();

            mapper.Entity<KeyBindInfo>()
                .Id(x => x.Id, true);
            var UserDB = new LiteDatabase($"Filename=User.db;Mode=Exclusive", mapper);

            KeyBinds = UserDB.GetCollection<KeyBindInfo>();

            // This is working!
            var a = KeyBinds.Exists(x => x.Action == KeyBindAction.ConfigWindow);

            // This is NOT working!
            var tmp = KeyBindAction.ConfigWindow;
            a = KeyBinds.Exists(x => x.Action == tmp); // Crash in NativeAOT
            Console.WriteLine($"next............");

            Console.WriteLine("Done.");
        }
    }
}
