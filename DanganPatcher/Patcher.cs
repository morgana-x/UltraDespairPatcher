using Reloaded.Memory.Sigscan.Definitions.Structs;
using System.Diagnostics;

namespace DanganPatcher
{

    public class Patcher
    {
        Reloaded.Memory.Sigscan.Scanner scanner;
        Process proc;
        long baseAddress;
        public Patcher(Process proc)
        {
            this.proc = proc;
            scanner = new Reloaded.Memory.Sigscan.Scanner(proc);
            baseAddress = MemoryRead.GetProcessBaseAddress(proc);

            //ApplyPatches();
        }
        private void ApplyPatches()
        {
            /*
             * Infinite ammo
            */
            /*
            Patch("GetAmmo", "e8 6d 45 04 00 f6 00 04 74 0b b8 fe ff ff ff 48 83 c4 20 5b c3", "90 90 90 90 90 90 90 90 90 90");*/

        }


        private long Sigscan(string pattern, string name)
        {
            PatternScanResult result = scanner.FindPattern(pattern);

            if (!result.Found)
            {
               // Program.PrintFancy(ConsoleColor.Yellow, $"Unable to find pattern \"{name}\"! ({pattern})");
                return 0;
            }
            return baseAddress + result.Offset;
        }

        public bool Patch(Patch patch)
        {
            return Patch(patch.Name, patch.Signature, patch.getReplacement(), patch.PatchOffset);
        }
        public bool Patch(string patchName, string pattern, byte[] replacement, int offset = 0)
        {
            long location = Sigscan(pattern, patchName);
            if (location == 0)
            {
                Program.PrintFancy(ConsoleColor.Yellow, $"Failed to apply patch \"{patchName}\": Failed to find address!");
                return false;
            }
            MemoryRead.WriteMemory((int)proc.Handle, location + offset, replacement);
            Program.PrintFancy(ConsoleColor.Green, $"Applied patch \"{patchName}\" at offset 0x{(offset + location - baseAddress).ToString("X")}!");
            return true;
        }
        public bool Patch(string patchName, string pattern, string replacement, int offset = 0)
        {
            return Patch(patchName, pattern, byteStringToByteArray(replacement), offset);
        }
        private static byte[] byteStringToByteArray(string bytestring)
        {
            string[] stringbytes = bytestring.Split(" ");
            byte[] newbytes = new byte[stringbytes.Length];
            for (int i = 0; i < stringbytes.Length; i++)
            {
                newbytes[i] = byte.Parse(stringbytes[i].ToLower(), System.Globalization.NumberStyles.HexNumber);
            }
            return newbytes;
        }

    }

}