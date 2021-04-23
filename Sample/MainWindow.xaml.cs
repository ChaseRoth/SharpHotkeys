﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using SharpHotkeys.Hotkeys;

namespace Sample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Hotkey hotkey;

        int counter = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            IntPtr windowHandle = new WindowInteropHelper(this).Handle;

            hotkey = new Hotkey(SharpHotkeys.Enumerations.Keys.F6, SharpHotkeys.Enumerations.Modifiers.MOD_NONE, windowHandle);
            if (!hotkey.TryRegisterHotkey(out uint errCode))
            {
                lbl.Content = errCode;
            }
            else
            {
                hotkey.HotkeyClicked += delegate
                {
                    lbl.Content = counter++;
                };
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            hotkey.TryUnregisterHotkey(out uint errCode);

            base.OnClosing(e);
        }
    }
}
