using System;
using System.IO;
using RX;

namespace BaseModel.Fields
{
    /**
     * Interface for model fields, that handle encryption and serialization.
     */
    public interface IModelField
    {
        public Subscription Subscribe(Action onChange);
        public void Serialize(BinaryWriter writer);
        public void Deserialize(BinaryReader reader);
    }
}