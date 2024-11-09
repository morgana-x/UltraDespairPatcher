
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
                Description = "Enables infinite ammo (INDEV, doesn't work on loading a save that has ammo values already set, start from a chapter!)",
                Enabled = false,
                Options = new Dictionary<string, string>(),
                Patches = new List<Patch>()
                {
                    new Patch("GetAmmo", "e8 6d 45 04 00 f6 00 04 74 0b b8 fe ff ff ff 48 83 c4 20 5b c3", "90 90 90 90 90 90 90 90 90 90"),
                    new Patch("IsAmmoEnabled", "e8 ad 43 04 00 f6 00 04 74 08 b0 01 48 83 c4 20 5b c3 48 8d 0d dd b0 75 00 48 8b c3 66 83 3c 59 ff 0f 95 c0 48 83 c4 20 5b c3", "90 90 90 90 90 90 90 90 90 90"),
                    new Patch("GetDefaultAmmo", "e8 4d 3d 04 00 f6 00 04 74 09 83 c8 ff 48 83 c4 20 5f c3 48 8b 05 f8 d4 78 00 8b cf 48 89 5c 24 30 8b 1c b8 e8 b9 a8 03 00 03 c3", "90 90 90 90 90 90 90 90 90 90"),
                    new Patch("SetInfiniteAmmo", "41 83 3c 08 ff 75 09 83 39 ff 74 04", "90 90 90 90 90 90 90 90 90 90 90 90")
                }
            },

            new PatchGroup()
            {
                Name = "God mode",
                Description = "Komaru doesn't take damage no more!",
                Enabled = false,
                Patches = new List<Patch>()
                {
                    new Patch("TakeDamage", "66 2b 86 58 05 00 00 66 89 05 35 90 76 00 79 0a 41 8b c6 66 89 05 29 90 76 00 0f b7 c8 e8 93 e9 06 00", "90 90 90 90 90 90 90")
                }
            },
            new PatchGroup()
            {
                Name = "1 Heart",
                Description = "Komaru has a max health of 1 heart.",
                Enabled = true,
                Patches = new List<Patch>()
                {
                    new Patch("GetMaxHealth", "66 41 03 d0 41 f6 c1 04 74 06 8b 88 a4", "B8 01 00 00 00", 16),
                    new Patch("SetDefaultHealth", "66 41 03 c8 41 f6 c1 04 74 06 8b 82 a4 00 00 00 66 03 c1 66 89 05 7d f2 71 00 66 89 05 a6 c9 6e 00 c3", "C7 05 7A F2 71 00 01 00 00 00 90 90 90 90", 22)
                }
            },

            new PatchGroup()
            {
                Name = "Infinite Battery Life (Real)",
                Description = "Genocider Jack never runs out of battery!",
                Enabled = true,
                Patches = new List<Patch>()
                {
                    //new Patch("DepleteBattery", "f3 0f 5c c8 f3 0f 11 0d 85 01 76 00 e9 2e 01 00 00 8b 05 f6 28 79 00 48 8b 1d 7f 2a 79 00 0f ba e0 1c 72 6a f6 05 f2 00 76 00 04 75 42", "90 90 90 90"),
                    new Patch("DepleteBattery2", "f3 0f 5c c8 f3 0f 11 0d 24 01 76 00 eb 27 a9 00 00 c0 00 75 18 e8 02 0f 04 00 44 89 35 0f 01 76 00 0f b7 c8 66 89 05 01 01 76 00", "90 90 90 90")
                }
            }
        };
        static string configFolder = AppDomain.CurrentDomain.BaseDirectory + "\\Config";
        static string configPath = configFolder + "\\Patches.json";
        private static void SaveConfig(PatchConfig config)
        {
            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                WriteIndented = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true,
                IncludeFields = true
            };
            string serialized = JsonSerializer.Serialize<PatchConfig>(config, options);
            File.WriteAllText(configPath, serialized);
        }
        public static PatchConfig LoadConfig()
        {

            if (!Directory.Exists(configFolder))
                Directory.CreateDirectory(configFolder);

       

            JsonSerializerOptions options = new JsonSerializerOptions() {
                WriteIndented = true, 
                ReadCommentHandling = JsonCommentHandling.Skip, 
                AllowTrailingCommas = true,
                IncludeFields = true
            };
            var defaultConfig = new PatchConfig();
            if (!File.Exists(configPath))
            {
                Console.WriteLine("Creating config file for first time...");
                SaveConfig(defaultConfig);
                return defaultConfig;
            }

            var config = JsonSerializer.Deserialize<PatchConfig>(File.ReadAllText(configPath), options);
            if (config.DespairPatches.Count < defaultConfig.DespairPatches.Count)
            {
                for (int i = config.DespairPatches.Count; i < defaultConfig.DespairPatches.Count; i++)
                    config.DespairPatches.Add(defaultConfig.DespairPatches[i]);
                SaveConfig(config);
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
