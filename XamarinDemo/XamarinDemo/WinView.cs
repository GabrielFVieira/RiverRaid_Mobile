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
    public class WinView : View
    {
        Context mContext;
        private Bitmap background;
        private Bitmap[] buttons = new Bitmap[2];
        private Paint paint;
        private Activity winActivity;
        public WinView(Context context, Activity a) : base(context)
        {
            Initialize(context);
            winActivity = a;
        }

        private void Initialize(Context ctx)
        {
            mContext = ctx;

            buttons[0] = BitmapFactory.DecodeResource(Resources, Resource.Drawable.Reiniciar);
            buttons[1] = BitmapFactory.DecodeResource(Resources, Resource.Drawable.Voltar);
            background = BitmapFactory.DecodeResource(Resources, Resource.Drawable.TelaDeVitoria);
            paint = new Paint { Color = Color.White };
        }


        List<Pair> positions = new List<Pair>();
        void InitPositions()
        {
            if (positions.Count == 0)
            {
                int x = Width - buttons[0].Width;
                int y = Height - buttons[0].Height * 2;
                positions.Add(new Pair(x, y));
            }

            if (positions.Count == 1)
            {
                int x = 0;
                int y = Height - buttons[1].Height * 2;
                positions.Add(new Pair(x, y));
            }
        }

        void DrawButtons(Canvas canvas)
        {
            InitPositions();
            for (int i = 0; i < positions.Count; i++)
            {
                int x = (int)positions[i].First;
                int y = (int)positions[i].Second;
                canvas.DrawBitmap(buttons[i], x, y, paint);
            }
        }


        protected override void OnDraw(Canvas canvas)
        {
            canvas.DrawBitmap(background, Width / 2 - background.Width / 2, Height / 2 - background.Height / 2, paint);
            DrawButtons(canvas);

        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            int indexHit = IsInsideButton(e.GetX(), e.GetY());
            if (indexHit > -1)
            {
                if (indexHit == 0)
                {
                    mContext.StartActivity(typeof(InGame));
                    winActivity.Finish();
                }

                if (indexHit == 1)
                {
                    winActivity.Finish();
                }
            }

            return false;
        }

        int IsInsideButton(float x, float y)
        {
            for (int i = 0; i < positions.Count; i++)
            {
                int X = (int)positions[i].First;
                int Y = (int)positions[i].Second;

                if (x > X && x < X + buttons[i].Width && y > Y && y < Y + buttons[i].Height)
                {
                    return i;
                }
            }
            return -1;
        }

    }
}