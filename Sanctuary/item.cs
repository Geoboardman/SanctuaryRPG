﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace CellGo1
{
    public class item
    {
        string name;
        string description;

        public item(string name, string desc)
        {
            this.name = name;
            this.description = desc;
        }
    }
}