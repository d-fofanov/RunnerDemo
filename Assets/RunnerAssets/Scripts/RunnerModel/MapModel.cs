namespace RunnerModel
{
    /**
     * Read-only version of the runner map.
     */
    public class RORunnerMap
    {
        protected readonly uint[] _map;
        protected readonly int _width;
        protected readonly int _height;
        
        public int Width => _width;
        public int Height => _height;
        
        public bool this[int x, int y]
        {
            get => (_map[x] >> y & 1u) != 0;
        }
        
        public RORunnerMap(uint[] map, int width, int height)
        {
            if (height > 32)
                throw new System.ArgumentException("Height must be less than 32");
            
            _map = map;
            _width = width;
            _height = height;
        }

        public uint GetColumn(int x)
        {
            return _map[x];
        }
    }
    
    /**
     * Contains the map model used to build level's visuals.
     */
    public class MapModel : RORunnerMap
    {
        public new bool this[int x, int y]
        {
            get => base[x, y];
            set
            {
                if (value)
                    _map[x] |= 1u << y;
                else
                    _map[x] &= ~(1u << y);
            }
        }
        
        public MapModel(uint[] map, int width, int height) : base(map, width, height)
        {
        }

        public MapModel(int width, int height) : base(new uint[width], width, height)
        {
        }
    }
}