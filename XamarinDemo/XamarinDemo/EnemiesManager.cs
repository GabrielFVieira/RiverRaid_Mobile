using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace XamarinDemo
{
    public class EnemiesManager
    {
        public List<Enemies> enemies = new List<Enemies>();
        private int NumberOfEnemies;
        private Bitmap[] shipImages = new Bitmap[2];
        private Bitmap[] heliImages = new Bitmap[2];
        private Bitmap[] jetImages = new Bitmap[2];
        private float minX, maxX, minY;
        private float initialPosX;
        private int[] offset = new int[5];

        public EnemiesManager(Bitmap[] ship, Bitmap[] heli, Bitmap[] jet, int n)
        {
            NumberOfEnemies = n;
            shipImages = ship;
            heliImages = heli;
            jetImages = jet;

            for(int i = 0; i < NumberOfEnemies; i++)
            {
                Bitmap[] enemyImg = new Bitmap[2];

                if (i != 0)
                {
                    Random randomType = new Random();
                    int type = randomType.Next(3);

                    if (type == 0)
                        enemyImg = shipImages;

                    else if (type == 1)
                        enemyImg = heliImages;

                    else
                        enemyImg = jetImages;
                }

                else
                    enemyImg = shipImages;

                Enemies enemy = new Enemies(enemyImg);
                enemies.Add(enemy);
            }
        }

        public void Update(float speed, bool start, float x, float y, float w, float h, Player p, List<Bullet> bullets)
        {
            if (enemies.Count > 0)
            {
                for(int i = 0; i < enemies.Count; i++)
                {
                    if (enemies[i].Destroy())
                        enemies.Remove(enemies[i]);

                    else
                        enemies[i].Update(speed, start, x, y, h, w, bullets);

                    if (enemies[i].colPlayer)
                        p.SetCol(enemies[i].colPlayer);
                }
            }

            if(enemies.Count < NumberOfEnemies)
            {
                Bitmap[] enemyImg = new Bitmap[2];

                Random randomType = new Random();
                int type = randomType.Next(3);

                if (type == 0)
                    enemyImg = shipImages;

                else if (type == 1)
                    enemyImg = heliImages;

                else
                    enemyImg = jetImages;

                Enemies enemy = new Enemies(enemyImg);
                enemies.Add(enemy);
            }
        }

        public void Draw(Canvas canvas, float miX, float maX, float miY)
        {
            minX = miX;
            maxX = maX;
            minY = miY;
            initialPosX = Convert.ToInt32(minY - minY / 2);

            offset[0] = 100;
            offset[1] = 190;
            offset[2] = 280;
            offset[3] = 350;
            offset[4] = 470;

            float[] xPos = new float[6];
            xPos[0] = minX;
            xPos[1] = (maxX + minX) / 2;
            xPos[2] = maxX - shipImages[0].Width;
            xPos[3] = minX + shipImages[0].Width;
            xPos[4] = (maxX - shipImages[0].Width + minX) / 2;
            xPos[5] = maxX - (shipImages[0].Width * 2);

            Random randomX = new Random();
            Random randomY = new Random();

            if (enemies.Count > 0)
            {
                for (int i = 0; i < enemies.Count; i++)
                {
                    float[] pos = new float[2];
                    pos[0] = xPos[randomX.Next(xPos.Length)];

                    float posY = offset[randomY.Next(offset.Length)];

                    pos[1] = initialPosX - posY;
                    initialPosX -= posY;

                    enemies[i].Draw(canvas, pos, minX, maxX, minY);
                }
            }
        }
    }
}