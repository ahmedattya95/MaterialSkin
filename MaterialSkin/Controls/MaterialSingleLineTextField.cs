﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MaterialSkin.Controls
{
    public class MaterialSingleLineTextField : Control, IMaterialControl
    {
        //Properties for managing the material design properties
        public int Depth { get; set; }
        public MaterialSkinManager SkinManager { get { return MaterialSkinManager.Instance; } }

        public string Text { get { return baseTextBox.Text; } set { baseTextBox.Text = value;  } }
        public string Hint { get; set; }

        private readonly TextField baseTextBox = new TextField();
        public MaterialSingleLineTextField()
        {
            baseTextBox = new TextField
            {
                BorderStyle = BorderStyle.None,
                Font = SkinManager.FONT_BODY1,
                ForeColor = SkinManager.GetMainTextColor(),
                Location = new Point(0, 0),
                Text = Text,
                Width = Width,
                Height = Height - 3
            };

            if (!Controls.Contains(baseTextBox) && !DesignMode)
            {
                Controls.Add(baseTextBox);
            }

            baseTextBox.GotFocus += (sender, args) => Invalidate();
            baseTextBox.LostFocus += (sender, args) => Invalidate();
            baseTextBox.TextChanged += BaseTextBoxOnTextChanged;
        }

        private void BaseTextBoxOnTextChanged(object sender, EventArgs eventArgs)
        {
            if (baseTextBox.Text != Hint && baseTextBox.Text == "")
            {
                baseTextBox.Text = Hint;
                baseTextBox.ForeColor = SkinManager.GetDisabledOrHintColor();
            }
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            if (DesignMode){pevent.Graphics.Clear(Color.Black);return;}

            var g = pevent.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            g.Clear(Parent.BackColor);

            g.FillRectangle(baseTextBox.Focused ? SkinManager.PrimaryColorBrush : SkinManager.GetDividersBrush(), baseTextBox.Location.X, baseTextBox.Bottom + 1, baseTextBox.Width, 1);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            baseTextBox.Location = new Point(0, 0);
            baseTextBox.Width = Width;
            if (Height != baseTextBox.Height + 3)
            {
                Height = baseTextBox.Height + 3;
            }
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            baseTextBox.BackColor = Parent.BackColor;
            baseTextBox.ForeColor = SkinManager.GetMainTextColor();
        }
    }
}
