
namespace DanganPatcher
{
    public class Patch
    {
        public string Name { get; set; }

        public string Signature { get; set; }
        public string Replacement { get; set; }

        public int PatchOffset { get; set; }

        private PatchGroup PatchGroup;

        public Patch(string patchName, string signaturePattern, string replacement, int offset = 0, Dictionary<string, string> options = null, string description = "Dummy Description")
        {
            Name = patchName;
            Signature = signaturePattern;
            Replacement = replacement;
            PatchOffset = offset;
        }
        public Patch()
        { 
        }
        public void setPatchGroup(PatchGroup group)
        {
            PatchGroup = group;
        }
        public string getReplacement()
        {
            if (PatchGroup == null)
                return Signature;
            string value = Replacement;
            foreach (var a in PatchGroup.Options)
                value = value.Replace(a.Key, a.Value);
            return value;
        }
    }
}
