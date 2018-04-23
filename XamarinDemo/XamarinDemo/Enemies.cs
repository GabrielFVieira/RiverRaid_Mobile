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
        private Player player;

        public void SetMove(bool M) { canMove = M; }
        public bool Destroy() { return destroy; }

        public Enemies(Bitmap[] img, Player p)
        {
            paint = new Paint { Color = Color.White };
            destroy = false;
            canMove = false;
            p = player;
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

        public void Update(float spY, bool move)
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
                }

                if (y > minY)
                    destroy = true;
            }
        }
        /*
        public void CollidedWithPlayer()
        {
            if(x < player.GetX() + player.GetW && x + imageL.Width >)
        }*/

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