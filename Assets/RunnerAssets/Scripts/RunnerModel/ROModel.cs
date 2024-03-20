
using Settings;

namespace RunnerModel
{
    /**
     * Read-only version of the entire model.
     * Has a global visibility.
     */
    public class ROModel
    {
        public StaticSettings Settings { get; }
        public ROMetaModel ROMeta { get; }
        public ROGameplayModel ROGame { get; }

        public ROModel(StaticSettings settings, RunnerModel.MetaModel meta, GameplayModel game)
        {
            Settings = settings;

            ROMeta = new ROMetaModel(meta);
            ROGame = new ROGameplayModel(game);
        }
    }
}