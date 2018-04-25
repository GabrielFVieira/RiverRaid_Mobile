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
    public class MapController
    {
        private Paint paint;
        private Bitmap leftPart;
        private Bitmap rightPart;
        private Bitmap bg;
        private float y;
        private float speed;
        private bool init, col, start;

        public MapController(Bitmap left, Bitmap right, Bitmap back)
        {
            col = false;
            paint = new Paint();
            paint.Color = Color.White;
            leftPart = left;
            rightPart = right;
            bg = back;
        }

        public bool GetCol() { return col; }
        public float GetSpeed() { return speed; }
        public bool GetStart() { return start;  }

        public void Draw(Canvas canvas, float Y, float[] bgX, float w)
        {
             if (!init)
            {
                y = Y;
                init = true;
            }
            canvas.DrawBitmap(bg, 0, 0, paint);
            canvas.DrawBitmap(bg, w - bg.Width, 0, paint);
            canvas.DrawBitmap(leftPart, bgX[0], y, paint); // Left part of bg1
            canvas.DrawBitmap(rightPart, bgX[1], y, paint); // Right part of bg1
        }

        public void Update(float playerX, float playerW, float wid, int direcition, Player p)
        {
            if (start && !col)
            {
                if (p.isMoving)
                {
                    if (direcition < 0)
                        speed = 20;

                    else if (direcition > 0)
                        speed = 6;

                    else
                        speed = 12;
                }

                else
                    speed = 12;

                if(p.GetCol())
                {
                    col = true;
                    speed = 0;
                }

                if (CheckCollision(playerX, playerW, wid))
                {
                    col = true;
                    p.SetCol(col);
                    speed = 0;
                }

                else
                    y += speed;
            }
        }

        public void PreUpdate(MotionEvent e)
        {
            start = true;
        }

            private bool CheckCollision(float x, float w, float cW)
        {
            if (x > bg.Width && x + w < cW - bg.Width)
                return false;

            else
                return true;
        }

    }
}