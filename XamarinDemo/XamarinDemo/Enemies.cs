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
    public class Enemies
    {
        private Bitmap imageL, imageR;
        private Paint paint;
        private float speedX, speedY, x, y, minX, maxX, minY;
        private bool canMove, facingLeft, destroy, init;
        public bool colPlayer;

        public void SetMove(bool M) { canMove = M; }
        public bool Destroy() { return destroy; }
        public bool ColWithPlayer() { return colPlayer; }

        public Enemies(Bitmap[] img)
        {
            paint = new Paint { Color = Color.White };
            destroy = false;
            canMove = false;
            imageL = img[0];
            imageR = img[1];


            Random random = new Random();

            int rand = random.Next(2);

            if(rand == 0)
            {
                speedX = 6;
                facingLeft = true;
            }

            else
            {
                speedX = -6;
                facingLeft = false;
            }

        }

        public void Update(float spY, bool move, float pX, float pY, float pH, float pW, List<Bullet> bullets)
        {
            if (!destroy)
            {
                speedY = spY;

                if (speedY == 0)
                    canMove = false;

                else
                    canMove = move;

                if (canMove)
                {
                    y += speedY;
                    x -= speedX;

                    CollidedWithWall();
                    CollidedWithPlayer(pX, pY, pW, pH);
                }

                if (y > minY)
                    destroy = true;


                foreach(Bullet b in bullets)
                {
                    if(x < b.x + b.width && x + imageL.Width> b.x && y < b.y + b.height && y + imageL.Height > b.y)
                    {
                        b.destroy = true;
                        destroy = true;
                    }
                }


            }
        }
        
        public void CollidedWithPlayer(float X, float Y, float W, float H)
        {

            if(x < X + W && x + imageL.Width > X && y < Y + H &&  y + imageL.Height > Y)
            {
                colPlayer = true;
                destroy = true;
            }
        }

            public void CollidedWithWall()
        {
            if(x <= minX && facingLeft)
            {
                speedX *= -1;
                facingLeft = false;
            }

            else if(x + imageR.Width >= maxX && !facingLeft)
            {
                speedX *= -1;
                facingLeft = true;
            }

            if(y > minY)
            {
                destroy = true;
            }
        }

        public void Draw(Canvas canvas, float[] pos, float miX, float maX, float miY)
        {
            if (!destroy)
            {
                if (!init)
                {
                    x = pos[0];
                    y = pos[1];
                    minX = miX;
                    maxX = maX;
                    minY = miY;
                    init = true;
                }

                if (facingLeft)
                    canvas.DrawBitmap(imageL, x, y, paint);

                else
                    canvas.DrawBitmap(imageR, x, y, paint);
            }
        }
    }
}