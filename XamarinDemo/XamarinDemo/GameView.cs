using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace XamarinDemo
{
    public class GameView : View, IRunnable
    {
        Context mContext;
        public static int screenW, screenH;
        public static bool isDead, isPaused, isUpdating;
        private Paint paint;
        public Player player;
        private Bitmap[] playerImages = new Bitmap[4];

        private Bitmap dpad;
        private float[] dpadPos = new float[2];
        public int[] dpadDirection = new int[2];
        private Handler handler;
        private Paint gray;

        private Bitmap fuelbar;
        private float[] fuelbarPos = new float[4];
        public Bitmap fuelPointer;
        private FuelManager fuel;

        private bool canFire;
        private Bitmap fireButton;
        private float[] firePos = new float[2];
        private Bitmap bulletImage;
        //private float bulletCoolDown;
        private float playerMinY;

        public Bitmap bgLeft;
        public Bitmap bgRight;
        public Bitmap bg;
        public MapController mpC;

        public List<Bullet> bullets = new List<Bullet>();

        private float hudY;
        private float[] bgPos = new float[2];

        private Bitmap[] ship = new Bitmap[2];
        private Bitmap[] heli = new Bitmap[2];
        private Bitmap[] jet = new Bitmap[2];
        private EnemiesManager enemiesManager;
        private int NumberOfEnemies;

        public GameView(Context context) :
        base(context)
        {
            Initialize(context);
        }
        public GameView(Context context, IAttributeSet attrs) :
        base(context, attrs)
        {
            Initialize(context);
        }

        public GameView(Context context, IAttributeSet attrs, int defStyle) :
        base(context, attrs, defStyle)
        {
            Initialize(context);
        }

        private void Initialize(Context ctx)
        {
            mContext = ctx;

            NumberOfEnemies = 10;

            bgLeft = BitmapFactory.DecodeResource(Resources, Resource.Drawable.game_bg_left);
            bgRight = BitmapFactory.DecodeResource(Resources, Resource.Drawable.game_bg_right);
            bg = BitmapFactory.DecodeResource(Resources, Resource.Drawable.game_bg);

            mpC = new MapController(bgLeft, bgRight, bg);

            //bulletCoolDown = 1;
            SetBackgroundColor(Color.MediumBlue);

            bulletImage = BitmapFactory.DecodeResource(Resources, Resource.Drawable.bullet);

            screenW = mContext.Resources.DisplayMetrics.WidthPixels;
            screenH = mContext.Resources.DisplayMetrics.HeightPixels;

            fuelbar = BitmapFactory.DecodeResource(Resources, Resource.Drawable.fuel_bar);
            fuelPointer = BitmapFactory.DecodeResource(Resources, Resource.Drawable.fuel_pointer);
            fireButton = BitmapFactory.DecodeResource(Resources, Resource.Drawable.fire_button);
            fuel = new FuelManager(fuelPointer);

            isDead = isPaused = false;
            playerImages[0] = BitmapFactory.DecodeResource(Resources, Resource.Drawable.player_middle);
            playerImages[1] = BitmapFactory.DecodeResource(Resources, Resource.Drawable.player_left);
            playerImages[2] = BitmapFactory.DecodeResource(Resources, Resource.Drawable.player_right);
            playerImages[3] = BitmapFactory.DecodeResource(Resources, Resource.Drawable.player_explosion);

            isUpdating = true;

            dpad = BitmapFactory.DecodeResource(Resources, Resource.Drawable.d_pad);

            player = new Player(playerImages[0].Width, playerImages[0].Height);
            paint = new Paint { Color = Color.White };

            gray = new Paint { Color = Color.Gray };

            // Enemies
            ship[0] = BitmapFactory.DecodeResource(Resources, Resource.Drawable.ship_l);
            ship[1] = BitmapFactory.DecodeResource(Resources, Resource.Drawable.ship_r);
            heli[0] = BitmapFactory.DecodeResource(Resources, Resource.Drawable.heli_l);
            heli[1] = BitmapFactory.DecodeResource(Resources, Resource.Drawable.heli_r);
            jet[0] = BitmapFactory.DecodeResource(Resources, Resource.Drawable.jet_l);
            jet[1] = BitmapFactory.DecodeResource(Resources, Resource.Drawable.jet_r);
            enemiesManager = new EnemiesManager(ship, heli, jet, NumberOfEnemies, player);

            handler = new Handler();
            handler.Post(this);
        }

        protected override void OnDraw(Canvas canvas)
        {
            if (!isDead && !isPaused)
            {
                hudY = Height * 0.75f;

                bgPos[0] = 0;
                bgPos[1] = Width - bgRight.Width;

                playerMinY = hudY;

                dpadPos[0] = Width * 0.03f;
                dpadPos[1] = Height - (Height * 0.25f) / 2 - dpad.Height / 2;

                firePos[0] = Width - (Width * 0.09f) - fireButton.Width;
                firePos[1] = Height - (Height * 0.25f) / 2 - fireButton.Height / 2;

                fuelbarPos[0] = (Width / 2) - (fuelbar.Width / 2);
                fuelbarPos[1] = Height - (Height * 0.25f) / 2 - fuelbar.Height / 2;
                fuelbarPos[2] = fuelbar.Width;
                fuelbarPos[3] = fuelbar.Height;

                float enemiesMinX = bgPos[0] + bg.Width;
                float enemiesMaxX = Width - bg.Width;

                enemiesManager.Draw(canvas, enemiesMinX, enemiesMaxX, hudY);

                mpC.Draw(canvas, hudY - bgLeft.Height, bgPos, Width);

                canvas.DrawRect(0, hudY, Width, Height, gray);

                fuel.Draw(canvas, fuelbarPos);

                canvas.DrawBitmap(dpad, dpadPos[0], dpadPos[1], paint);
                canvas.DrawBitmap(fuelbar, fuelbarPos[0], fuelbarPos[1], paint);
                canvas.DrawBitmap(fireButton, firePos[0], firePos[1], paint);
                player.Draw(canvas, playerImages);
               
                if (bullets.Count > 0)
                {
                    for (int i = 0; i < bullets.Count; i++)
                    {
                        bullets[i].Draw(canvas, bulletImage);
                    }
                }
            }
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            player.PreUpdate(e);
            mpC.PreUpdate(e);
            
            if (InsideDPAD(e.GetX(), e.GetY()))
            {
                //Toast.MakeText(mContext, "Inside D-PAD", ToastLength.Short).Show();
                DpadDirection(e.GetX(), e.GetY());
            }

            else
            {
                dpadDirection[0] = 0;
                dpadDirection[1] = 0;
            }

            if (InsideFIRE(e.GetX(), e.GetY()) && !mpC.GetCol())
            {
                if (e.Action == MotionEventActions.Down && canFire ||
                e.Action == MotionEventActions.Move && canFire)
                {
                    //Toast.MakeText(mContext, "Inside Fire Button", ToastLength.Short).Show();

                    float bulletX = player.GetX() + (playerImages[0].Width / 2) - (bulletImage.Width / 2);
                    float bulletY = player.GetY() + bulletImage.Height;

                    Bullet bullet = new Bullet(bulletX, bulletY, bulletImage.Width, bulletImage.Height);
                    bullets.Add(bullet);

                    canFire = false;
                }

                else if (e.Action == MotionEventActions.Up)
                    canFire = true;
            }

            return true;
        }

        private bool InsideDPAD(float x, float y)
        {
            if (x > dpadPos[0] && x < dpadPos[0] + dpad.Width &&
                y > dpadPos[1] && y < dpadPos[1] + dpad.Height)
                return true;

            else
                return false;
        }

        private bool InsideFIRE(float x, float y)
        {
            if (x > firePos[0] && x < firePos[0] + fireButton.Width &&
                y > firePos[1] && y < firePos[1] + fireButton.Height)
                return true;

            else
                return false;
        }

        private void DpadDirection(float x, float y)
        {
            if (x < dpadPos[0] + dpad.Width / 3 && y > dpadPos[1] + dpad.Height / 3 && y < dpadPos[1] + 2 * (dpad.Height / 3))
            {
                dpadDirection[0] = -1;
                dpadDirection[1] = 0;
                //Toast.MakeText(mContext, "Left", ToastLength.Short).Show();
            }

            else if (x < dpadPos[0] + dpad.Width / 3 && y < dpadPos[1] + dpad.Height / 3)
            {
                dpadDirection[0] = -1;
                dpadDirection[1] = -1;
                //Toast.MakeText(mContext, "Up-Left", ToastLength.Short).Show();
            }

            else if (x > dpadPos[0] + 2 * (dpad.Width / 3) && y > dpadPos[1] + dpad.Height / 3 && y < dpadPos[1] + 2 * (dpad.Height / 3))
            {
                dpadDirection[0] = 1;
                dpadDirection[1] = 0;
                //Toast.MakeText(mContext, "Right", ToastLength.Short).Show();
            }

            else if (x > dpadPos[0] + 2 * (dpad.Width / 3) && y < dpadPos[1] + dpad.Height / 3)
            {
                dpadDirection[0] = 1;
                dpadDirection[1] = -1;
                //Toast.MakeText(mContext, "Up-Right", ToastLength.Short).Show();
            }

            else if (x > dpadPos[0] + dpad.Width / 3 && x < dpadPos[0] + 2 * (dpad.Width / 3) && y < dpadPos[1] + dpad.Height / 3)
            {
                dpadDirection[0] = 0;
                dpadDirection[1] = -1;
                //Toast.MakeText(mContext, "Up", ToastLength.Short).Show();
            }

            else if (x > dpadPos[0] + dpad.Width / 3 && x < dpadPos[0] + 2 * (dpad.Width / 3) && y > dpadPos[1] + 2 * (dpad.Height / 3))
            {
                dpadDirection[0] = 0;
                dpadDirection[1] = 1;
                //Toast.MakeText(mContext, "Down", ToastLength.Short).Show();
            }

            else if (x < dpadPos[0] + dpad.Width / 3 && y > dpadPos[1] + 2 * (dpad.Height / 3))
            {
                dpadDirection[0] = -1;
                dpadDirection[1] = 1;
                //Toast.MakeText(mContext, "Down-Left", ToastLength.Short).Show();
            }

            else if (x > dpadPos[0] + 2 * (dpad.Width / 3) && y > dpadPos[1] + 2 * (dpad.Height / 3))
            {
                dpadDirection[0] = 1;
                dpadDirection[1] = 1;
                //Toast.MakeText(mContext, "Down-Right", ToastLength.Short).Show();
            }
        }
 
        private void Update()
        {
            player.Update(dpadDirection[0],  playerMinY, mpC.GetCol());
            mpC.Update(player.GetX(), playerImages[0].Width, Width, dpadDirection[1], player);
            enemiesManager.Update(mpC.GetSpeed(), mpC.GetStart());

            if(bullets.Count > 0)
            {
                for(int i = 0; i < bullets.Count; i++)
                {
                    bullets[i].Update();

                if (bullets[i].y < -bulletImage.Height)
                    bullets.Remove(bullets[i]);
                }
            }
            /*
            if(!canFire)
            { 
                //  bulletCoolDown -= timer;
            }
                
            if(bulletCoolDown <= 0)
            {

                canFire = true;
                bulletCoolDown = 1;
            }
            */
        }
       
        public void Run()
        {
            handler.PostDelayed(this, 30);

            Update();
            this.Invalidate();
        }
    }
}