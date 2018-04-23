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
    public class Player
    {
        private Paint paint;
        private float x, y, width, height, speedX, speedY;
        public bool isMoving, isMovingLeft;
        public float minY;
        public int curDirection;
        public bool hasCollided;
        public Player(float w, float h)
        {
            paint = new Paint();
            paint.Color = Color.White;
            speedX = 15f;
            speedY = 10f;
            width = w;
            curDirection = 0;
            height = h;
            x = (GameView.screenW / 2) - (width / 2);
            y = GameView.screenH * 0.7f - height/2;

            isMoving = isMovingLeft = false;
        }

        public float GetX() { return x; }
        public float GetY() { return y; }
        public float GetW() { return width; }
        public float GetH() { return height; }

        public void Draw(Canvas canvas, Bitmap[] playerImage)
        {
            canvas.DrawBitmap(playerImage[curDirection], x, y, paint);
        }

        public void Update(int directionX, float mY, bool col)
        {
            hasCollided = col;
            if (hasCollided)
                isMoving = false;


            if (directionX < 0)
                curDirection = 1;

            else if (directionX > 0)
                curDirection = 2;

            else
                curDirection = 0;


            minY = mY;

            if (isMoving)
            {
                x += speedX * directionX;
            }

            else if (isMoving == false && hasCollided == false)
                curDirection = 0;

            else
            {
                curDirection = 3;
            }
            CollisionWithScreen();
        }

        private void CollisionWithScreen()
        {
            if (x < 0)
                x += speedX;
            else if (x + width > GameView.screenW)
                x -= speedX;

            if (y < 0)
                y += speedY;
            else if (y + height > minY)
                y -= speedY;
        }
        
        public void PreUpdate(MotionEvent e)
        {
            if (e.Action == MotionEventActions.Down ||
                e.Action == MotionEventActions.Move)
            {
                isMoving = true;
               // isMovingLeft = x > e.RawX; // x = true || false
            }
            else if (e.Action == MotionEventActions.Up)
                isMoving = false;
        }
    }
}