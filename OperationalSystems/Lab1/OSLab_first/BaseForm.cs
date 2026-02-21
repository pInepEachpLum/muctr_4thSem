using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


namespace OSLab_first
{
    public partial class BaseForm : Form
    {
        //Some measurements for window
        protected const int FORM_WINDOW_WIDTH = 1000;
        protected const int FORM_WINDOW_HEIGHT = 750;
        protected const string FORM_TITLE_TEXT = "";

        //Any button defaults
        protected const int DEFAULT_BUTTON_WIDTH = 200;
        protected const int DEFAULT_BUTTON_HEIGHT = 100;
        protected Cursor DEFAULT_BUTTON_CURSOR = Cursors.Hand;
        protected Color DEFAULT_BUTTON_COLOR = Color.LightGray;
        protected Color IF_MOUSE_ON_BUTTON_COLOR = Color.LightBlue;
        protected string DEFAULT_BUTTON_TEXT = "";

        //Any text defaults
        protected const string DEFAULT_FONT_TYPE = "Arial";
        protected const int DEFAULT_FONT_SIZE = 16;
        protected Font DEFAULT_FONT = new Font(DEFAULT_FONT_TYPE, DEFAULT_FONT_SIZE);

        //Any text in label deafaults
        protected Color INFORMATION_LABEL_COLOR = Color.DarkGray;
        protected virtual void SetWindow()
        {
            //Inintialising standard statements
            this.ClientSize = new Size(FORM_WINDOW_WIDTH, FORM_WINDOW_HEIGHT);
            this.StartPosition = FormStartPosition.CenterScreen;

            //this.Text = FORM_TITLE_TEXT;

            //Creaing custom window border

            //Creating custom title bar

            //Borders of changing size of window
            this.MinimumSize = new Size(FORM_WINDOW_WIDTH, FORM_WINDOW_HEIGHT);
            this.MaximumSize = new Size(FORM_WINDOW_WIDTH, FORM_WINDOW_HEIGHT);
        }

        /* доделай потом чел
        private void InitializeCustomWindowBorder()
        {
            this.BackColor = Color.Dark
        }

        private void InitializeCustomTitleBar()
        {
            //Window header constants
            const int TITLE_BAR_HEIGHT = 35;
            const int TITLE_BAR_BUTTON_SIZE = 30;

            this.FormBorderStyle = FormBorderStyle.None;

            Panel pnlTitleBar = new Panel();
            pnlTitleBar.

        }
        */

        protected virtual void SetDefaultPropertiesButton(Button anyButton)
        {
            anyButton.Width = DEFAULT_BUTTON_WIDTH;
            anyButton.Height = DEFAULT_BUTTON_HEIGHT;
            anyButton.Cursor = DEFAULT_BUTTON_CURSOR;
            anyButton.Font = DEFAULT_FONT;
            anyButton.BackColor = DEFAULT_BUTTON_COLOR;
            anyButton.FlatStyle = FlatStyle.Flat;
            anyButton.FlatAppearance.BorderSize = 0;
        }

        protected virtual void SetInfoLabel(Label infoLabel)
        {
            infoLabel.Font = DEFAULT_FONT;
            infoLabel.ForeColor = INFORMATION_LABEL_COLOR;
            infoLabel.AutoSize = true;
            infoLabel.MaximumSize = new Size(FORM_WINDOW_WIDTH, FORM_WINDOW_HEIGHT);
        }

        protected virtual void Button_MouseEnter(object sender, EventArgs e)
        {
            Button currentButton = sender as Button;
            if (currentButton == null) return;

            currentButton.BackColor = IF_MOUSE_ON_BUTTON_COLOR;
        }

        protected virtual void Button_MouseLeave(object sender, EventArgs e)
        {
            Button currentButton = sender as Button;
            if (currentButton == null) return;

            currentButton.BackColor = DEFAULT_BUTTON_COLOR;
        }
    }
}
