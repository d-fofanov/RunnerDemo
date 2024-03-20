using System.IO;
using RX;

namespace BaseModel.Fields
{
    /**
     * Model field for set of values.
     */
    public class ModelSet<T> : ReactiveSet<T>, IModelField
    {
        private BaseModelField<T> _internalField = null;
        
        public void Serialize(BinaryWriter writer)
        {
            _internalField ??= ModelFieldUtil.Create<T>();
            
            writer.Write(_set.Count);
            foreach (var item in _set)
            {
                _internalField.Value = item;
                _internalField.Serialize(writer);
            }
        }

        public void Deserialize(BinaryReader reader)
        {
            _internalField ??= ModelFieldUtil.Create<T>();
            
            var count = reader.ReadInt32();
            _set.Clear();
            for (int i = 0; i < count; i++)
            {
                _internalField.Deserialize(reader);
                _set.Add(_internalField.Value);
            }
        }
    }
}