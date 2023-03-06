using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AllIn1VfxToolkit
{
    public static class AllIn1VfxNoiseCreator
    {
        public static Texture2D PerlinNoise(Texture2D tex, float scale, int randomSeed, bool tileable)
        {
            int texWidth = tex.width;
            int texHeight = tex.height;

            Random.InitState(randomSeed);
            float randomOffset = Random.Range(-100f, 100f);

            for(int i = 0; i < texHeight; i++)
            {
                for(int j = 0; j < texWidth; j++)
                {
                    tex.SetPixel(j, i, CalculatePerlinColor(j, i, scale, randomOffset, texWidth, texHeight));
                }
            }
            tex.Apply();
            
            Texture2D finalPerlin = new Texture2D(texHeight, texWidth);
            finalPerlin.SetPixels(tex.GetPixels());

            if(tileable)
            {
                for(int i = 0; i < texHeight; i++)
                {
                    for(int j = 0; j < texWidth; j++)
                    {
                        finalPerlin.SetPixel(j, i, PerlinBorderless(j, i, scale, randomOffset, texWidth, texHeight, tex));
                    }
                }   
            }

            finalPerlin.Apply();
            return finalPerlin;
        }

        private static Color CalculatePerlinColor(int x, int y, float scale, float offset, int width, int height)
        {
            float xCoord = (x + offset) / width * scale;
            float yCoord = (y + offset) / height * scale;

            float perlin = Mathf.PerlinNoise(xCoord, yCoord);
            return new Color(perlin, perlin, perlin, 1);
        }
        
        private static Color PerlinBorderless(int x, int y, float scale, float offset, int width, int height, Texture2D previousPerlin)
        {
            int iniX = x;
            int iniY = y;
            float u = (float)x / width;
            float v = (float)y / height;

            if(u > 0.5f) x = width - x;
            if(v > 0.5f) y = height - y;
            
            offset += 23.43f;
            float xCoord = (x + offset) / width * scale;
            float yCoord = (y + offset) / height * scale;
            float perlin = Mathf.PerlinNoise(xCoord, yCoord);
            Color newPerlin = new Color(perlin, perlin, perlin, 1);

            float edge = Mathf.Max(u, v);
            edge = Mathf.Max(edge, Mathf.Max(1f - u, 1f - v));
            edge = Mathf.Pow(edge, 10f);

            return Color.Lerp(previousPerlin.GetPixel(iniX, iniY), newPerlin, edge);
        }
    }
}