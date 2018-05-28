using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace XamarinDemo
{
    class SaveData
    {
        ISharedPreferences prefs;

        public SaveData(Context c)
        {
            prefs = PreferenceManager.GetDefaultSharedPreferences(c);
        }

        public int GetHighScore()
        {
            int highscore = prefs.GetInt("HighScore", 0);

            return highscore;
        }

        public void SetHighScore(int score)
        {
            ISharedPreferencesEditor editor = prefs.Edit();
            editor.PutInt("HighScore", score);
            editor.Apply();
        }
    }
}