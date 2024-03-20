using System.IO;

namespace BaseModel.Fields
{
    /**
     * Pseudo-field serialized in-between real fields to provide meta information.
     */
    public class MetaField
    {
        public const int OwnLength = 8;
        
        public int Index { get; private set; }
        public int Length { get; private set; }

        public void ResetTo(int index, int length)
        {
            Index = index;
            Length = length;
        }
        
        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Index);
            writer.Write(Length);
        }

        public void Deserialize(BinaryReader writer)
        {
            Index = writer.ReadInt32();
            Length = writer.ReadInt32();
        }
    }
}