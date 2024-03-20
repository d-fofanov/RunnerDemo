using System.Collections.Generic;
using RunnerModel;
using Settings;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Utils
{
    /**
     * Simple map generator.
     */
    public static class MapGenerator
    {
        public static (MapModel map, List<CoinModel> coins) GenerateMap(int levelNum, StaticSettings settings)
        {
            var result = new MapModel(settings.MapWidth, settings.MapHeight);
            var coinsList = new List<CoinModel>();

            Random.InitState(levelNum);
            var currentHeight = settings.MapHeight / 2;
            var currentWidth = 0;
            while (currentWidth < settings.MapWidth)
            {
                var platformLength = Random.Range(settings.MinPlatformLength, settings.MaxPlatformLength + 1);
                for (var i = 0; i < platformLength && currentWidth + i < settings.MapWidth; i++)
                {
                    result[currentWidth + i, currentHeight] = true;
                }
                
                if (settings.CoinTypes.Count > 0 &&
                    Random.Range(0f, 1f) < settings.CoinChancePerPlatform)
                {
                    var randomCoin = settings.CoinTypes[Random.Range(0, settings.CoinTypes.Count)];
                    coinsList.Add(new CoinModel(randomCoin, new Vector2Int(currentWidth + platformLength / 2, currentHeight + 1)));
                }

                currentWidth += platformLength;
                
                var step = Random.Range(-settings.MaxStepFall, settings.MaxStepRise + 1);
                currentHeight += step;
                if (currentHeight < 0)
                {
                    currentHeight = 0;
                }
                else if (currentHeight >= settings.MapHeight)
                {
                    currentHeight = settings.MapHeight - 1;
                }

                var holeSize = Random.Range(0, settings.MaxHoleSize);
                currentWidth += holeSize;
            }

            return (result, coinsList);
        }
    }
}