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
    public class AwesomeView : View
    {
        Context mContext;
        private Bitmap background;
        private Bitmap[] buttons = new Bitmap[2];
        private Paint paint;

        public AwesomeView(Context context) :
        base(context)
        {
            Initialize(context);
        }
        public AwesomeView(Context context, IAttributeSet attrs) :
        base(context, attrs)
        {
            Initialize(context);
        }

        public AwesomeView(Context context, IAttributeSet attrs, int defStyle) :
        base(context, attrs, defStyle)
        {
            Initialize(context);
        }

        private void Initialize(Context ctx)
        {
            mContext = ctx;

            buttons[0] = BitmapFactory.DecodeResource(Resources, Resource.Drawable.play);
            buttons[1] = BitmapFactory.DecodeResource(Resources, Resource.Drawable.exit_button);
            background = BitmapFactory.DecodeResource(Resources, Resource.Drawable.menu_bg);
            paint = new Paint { Color = Color.White };
        }


        List<Pair> positions = new List<Pair>();
        void InitPositions()
        {
            if (positions.Count == 0)
            {
                int x = Width/2 - buttons[0].Width/2;
                int y = Height/2 - buttons[0].Height/3;
                positions.Add(new Pair(x, y));
            }

            if (positions.Count == 1)
            {
                int x = Width / 2 - buttons[1].Width / 2;
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
            canvas.DrawBitmap(background, 0, 0, paint);
            DrawButtons(canvas);

        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            int indexHit = IsInsidePlay(e.GetX(), e.GetY());
            if (indexHit > -1)
            {
                if (indexHit == 0)
                {
                    Toast.MakeText(mContext, "Start Game", ToastLength.Short).Show();

                    mContext.StartActivity(typeof(InGame));
                }

                if (indexHit == 1)
                {
                    System.Environment.Exit(0);
                }
            }

            return false;
        }
        
        int IsInsidePlay(float x, float y)
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