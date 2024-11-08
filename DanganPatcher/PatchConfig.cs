
using System.Text.Json;

namespace DanganPatcher
{
    public class PatchGroup
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public bool Enabled { get; set; }
        public Dictionary<string, string> Options { get; set; } = new Dictionary<string, string>();
        public List<Patch> Patches { get; set;} = new List<Patch>();

       

        public void ApplyPatches(DanganPatcher.Patcher patcher)
        {
            if (!Enabled)
            {
                Console.WriteLine($"{Name} is disabled");
                return;
            }
            foreach (var a in Patches)
            {
                a.setPatchGroup(this);
                Console.WriteLine($"Patching {a.Name}");
                patcher.Patch(a);
            }
        }
    }

    public class PatchConfig
    {
        public List<PatchGroup> DespairPatches { get; set; } = new List<PatchGroup>()
        {
            new PatchGroup()
            {
                Name = "Komaru Swag",
                Description = "Enables outfit of choice for Komaru throughout the entire game. Valid values: 01, 02, 03",
                Enabled = true,
                Options = new Dictionary<string, string>(){["{outfit}"]="02"},
                Patches = new List<Patch>()
                {
                        new Patch("Change outfit (Character Switch) Set Companion", "b9 01 00 00 00 e8 49 be 00 00 b1 01 e8 82 6b 06 00 48 8b 43 18 b9 40 00 00 00", "b9 {outfit}" ),
                        new Patch("CreatePlayerChar", "3c 01 75 0e b8 04 00 00 00 66 89 05 74 4a 79 00 eb 07 0f b7 05 6b 4a 79 00 0f b7 c8 e8 df de ff ff", "90 90 90 90 b8 {outfit}"),
                        new Patch("SwitchCharToKomaru", "b9 01 00 00 00 83 48 64 02 66 89 0d 89 f7 2b 00 33 c9 e8 c2 f1 ff ff 33 c0 66 89 05 79 f7 2b 00", "b9 {outfit}"),
                        new Patch("KomaruSwitch2GenoHenshinKaisho", "66 89 35 52 11 2c 00 41 8d 50 64 e8 79 66 06 00 8b ce e8 d2 77 07 00 45 33 c0 41 8d", "c7 05 4F 11 2c 00 {outfit} 00 00 00"),
                }
            },
            new PatchGroup()
            {
                Name = "Infinite Ammo",
                Description = "Enables infinite ammo",
                Enabled = true,
                Options = new Dictionary<string, string>(),
                Patches = new List<Patch>()
                {
                    new Patch("GetAmmo", "e8 6d 45 04 00 f6 00 04 74 0b b8 fe ff ff ff 48 83 c4 20 5b c3", "90 90 90 90 90 90 90 90 90 90"),
                    new Patch("IsAmmoEnabled", "e8 ad 43 04 00 f6 00 04 74 08 b0 01 48 83 c4 20 5b c3 48 8d 0d dd b0 75 00 48 8b c3 66 83 3c 59 ff 0f 95 c0 48 83 c4 20 5b c3", "90 90 90 90 90 90 90 90 90 90"),
                    new Patch("GetDefaultAmmo", "e8 4d 3d 04 00 f6 00 04 74 09 83 c8 ff 48 83 c4 20 5f c3 48 8b 05 f8 d4 78 00 8b cf 48 89 5c 24 30 8b 1c b8 e8 b9 a8 03 00 03 c3", "90 90 90 90 90 90 90 90 90 90"),
                    new Patch("SetInfiniteAmmo", "41 83 3c 08 ff 75 09 83 39 ff 74 04", "90 90 90 90 90 90 90 90 90 90 90 90")
                }
            }
        };
        public static PatchConfig LoadConfig()
        {
            string configFolder = AppDomain.CurrentDomain.BaseDirectory + "\\Config";
            string configPath = configFolder + "\\Patches.json";
            if (!Directory.Exists(configFolder))
                Directory.CreateDirectory(configFolder);

       

            JsonSerializerOptions options = new JsonSerializerOptions() {
                WriteIndented = true, 
                ReadCommentHandling = JsonCommentHandling.Skip, 
                AllowTrailingCommas = true,
                IncludeFields = true
            };

            if (!File.Exists(configPath))
            {
                Console.WriteLine("Creating config file for first time...");
                PatchConfig config = new PatchConfig();

                string serialized = JsonSerializer.Serialize<PatchConfig>(config, options);
                File.WriteAllText(configPath, serialized);
                return config;
            }
            return JsonSerializer.Deserialize<PatchConfig>(File.ReadAllText(configPath), options);
        }
        public void ApplyPatches(Patcher patcher)
        {
            foreach (var a in DespairPatches)
                a.ApplyPatches(patcher);
        }
    }
    
    
}
