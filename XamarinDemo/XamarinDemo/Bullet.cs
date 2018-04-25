using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace XamarinDemo
{
    public class Bullet
    {
        public Paint paint;
        public float x, width, height, speed;
        public float y;
        public bool destroy;
        public Bullet(float X, float Y, float w, float h)
        {
            paint = new Paint();
            paint.Color = Color.White;
            speed = 45f;
            width = w;
            height = h;
            x = X;
            y = Y;
        }

        public void Update(bool col)
        {
            if(!destroy)
                y -= speed;

            if (col)
                destroy = true;
        }

        public void Draw(Canvas canvas, Bitmap bulletImage)
        {
            //canvas.DrawRect(x - width/2, y, x + width, y + height, paint);
            canvas.DrawBitmap(bulletImage, x, y, paint);
        }
    }
}