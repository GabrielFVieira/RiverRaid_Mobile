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
    public class FuelManager
    {
        private Bitmap fuelPointer;
        private Paint paint;
        private float maxY, minY, y;
        private bool start;

        public FuelManager(Bitmap image)
        {
            fuelPointer = image;
            paint = new Paint { Color = Color.White };
        }

        public void Update()
        {
            if (y < maxY)
                y = maxY;

            if (y > minY)
                y = minY;
        }

        public void Draw(Canvas canvas, float[]pos)
        {
            if (!start)
            {
                maxY = pos[1] + pos[3] * 0.09f;
                minY = pos[1] + pos[3] * 0.9f;
                y = maxY;
                start = false;
            }
            canvas.DrawBitmap(fuelPointer, pos[0], y, paint);
        }
    }
}