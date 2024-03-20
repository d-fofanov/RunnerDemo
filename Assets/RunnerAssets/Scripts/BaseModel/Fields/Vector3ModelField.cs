using System.IO;
using UnityEngine;

namespace BaseModel.Fields
{
    /**
     * Model field for Vector3 values.
     */
    public class Vector3ModelField : BaseModelField<Vector3>
    {
        protected override void SerializeImpl(BinaryWriter writer, Vector3 val)
        {
            writer.Write(val.x);
            writer.Write(val.y);
            writer.Write(val.z);
        }

        protected override Vector3 DeserializeImpl(BinaryReader reader)
        {
            return new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }
    }
}