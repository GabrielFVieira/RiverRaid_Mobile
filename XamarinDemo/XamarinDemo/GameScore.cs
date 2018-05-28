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
    class GameScore
    {
        public static int score;
        private Paint white;

        public GameScore()
        {
            score = 0;

            white = new Paint { Color = Color.White, TextSize = 50};
        }

        public void Draw(Canvas canvas)
        {
            canvas.DrawText("Score: " + score.ToString(), GameView.screenW * 0.02f, GameView.screenH * 0.03f, white);
        }

    }
}