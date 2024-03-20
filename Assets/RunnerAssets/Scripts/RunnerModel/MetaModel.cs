using BaseModel;
using RX;

namespace RunnerModel
{
    /**
     * Read-only version of the meta model.
     */
    public class ROMetaModel
    {
        public ReadOnlyReactiveProperty<long> TotalPoints => _metaModel.TotalPoints;
        public ReadOnlyReactiveProperty<int> Level => _metaModel.Level;

        private readonly MetaModel _metaModel;

        public ROMetaModel(MetaModel metaModel)
        {
            _metaModel = metaModel;
        }
    }

    /**
     * Meta model, contains all the persisted meta-data.
     */
    public class MetaModel : BaseModelStorage
    {
        public ReactiveProperty<long> TotalPoints => Get<long>(1);
        public ReactiveProperty<int> Level => Get<int>(2);

        public static MetaModel Create()
        {
            var model = new MetaModel();
            model.TotalPoints.Value = 0;
            return model;
        }
    }
}