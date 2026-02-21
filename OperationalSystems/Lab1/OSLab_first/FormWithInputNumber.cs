using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.IO.IsolatedStorage;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;

namespace OSLab_first
{
    public partial class FormWithInputNumber : BaseForm
    {
        //Some constant general parameters
        private const int PIX_OFFSET = 50;

        //Some constant parameters of window
        private const string FORM_TITLE_TEXT = "Input form";

        //Some constant parameters for start button
        private const string DEFAULT_BUTTON_TEXT = "START";

        Button _startButton = new Button();

        //Definition of informating label
        private Label _toDoLabel = new Label();
        private Label _outputLabel = new Label();

        private Font FONT_OUTPUT_LABEL = new Font("Arial", 32);
        //80 percents from form width
        private const int WIDTH_OUTPUT_LABEL = FORM_WINDOW_WIDTH / 100 * 80;
        private const int LEFT_POINT_OUPUT_LABEL = (FORM_WINDOW_WIDTH - WIDTH_OUTPUT_LABEL) / 2;
        private const int TOP_POINT_OUTPUT_LABEL = (FORM_WINDOW_HEIGHT - PIX_OFFSET * 3);

        private const string INFORMATION_LABEL_TEXT = @"Enter a random real number.
After clicking on the ""start"" button, 
your number will be converted randomly.
If you delete your number from the input
line and enter a new one, all convertions 
will be performed with the new number.";

        //Things for logic of convert number
        private int globalCounterOfClickingStartButton = 0;
        private int codeOfUsedOperation = 0;
        // 0 - none operation
        // 1 - addition
        // 2 - subtraction
        // 3 - multiplication
        // 4 - division
        private double value;
        private double secondValue;
        private double resValue;

        //Definition of textBox
        private const int DEFAULT_TEXTBOX_WIDTH = 300;
        private const int DEFAULT_TEXTBOX_HEIGHT = DEFAULT_BUTTON_HEIGHT;
        private TextBox _inputString = new TextBox();
        public FormWithInputNumber()
        {
            InitializeComponent();

            //Work with window
            SetWindow();

            //Work with buttons
            SetDefaultPropertiesButton(_startButton);
            SetPositionOfStartButton(_startButton);

            _startButton.MouseEnter += Button_MouseEnter;
            _startButton.MouseLeave += Button_MouseLeave;
            _startButton.MouseClick += Button_MouseClick;

            //Work with labels
            SetInfoLabel(_toDoLabel, INFORMATION_LABEL_TEXT, PIX_OFFSET, PIX_OFFSET);


            SetInfoLabel(_outputLabel, "", LEFT_POINT_OUPUT_LABEL, TOP_POINT_OUTPUT_LABEL);
            SetAdditionalSettingsForOutputLabel(_outputLabel);

            //Work with textBox
            SetDefaultPropetriesTextBox(_inputString);
            SetPositionTextBox(_inputString);

            this.Controls.Add(_startButton);
            this.Controls.Add(_toDoLabel);
            this.Controls.Add(_outputLabel);
            this.Controls.Add(_inputString);
        }

        protected override void SetWindow()
        {
            base.SetWindow();
            this.Text = FORM_TITLE_TEXT;
        }

        protected override void SetDefaultPropertiesButton(Button anyButton)
        {
            base.SetDefaultPropertiesButton(anyButton);
            anyButton.Text = DEFAULT_BUTTON_TEXT;
        }

        private void SetPositionOfStartButton(Button currentButton)
        {
            currentButton.Left = this.ClientSize.Width - currentButton.Width - PIX_OFFSET;
            currentButton.Top = 0 + PIX_OFFSET;
        }

        private void SetDefaultPropetriesTextBox(TextBox textBox)
        {
            textBox.Font = DEFAULT_FONT;
            textBox.Width = DEFAULT_TEXTBOX_WIDTH;
            textBox.Height = DEFAULT_TEXTBOX_HEIGHT;
            textBox.BorderStyle = BorderStyle.FixedSingle;
            textBox.TextAlign = HorizontalAlignment.Right;
            textBox.PlaceholderText = "0.0";    
        }

        private void SetPositionTextBox(TextBox textBox)
        {
            textBox.Left = this.ClientSize.Width - textBox.Width - PIX_OFFSET;
            textBox.Top = _startButton.Bottom + PIX_OFFSET;
        }

        protected virtual void SetInfoLabel(Label currentLabel, string text, int leftOffset, int topOffset)
        {
            SetInfoLabel(currentLabel);
            currentLabel.Text = text;
            currentLabel.Left = leftOffset;
            currentLabel.Top = topOffset;
        }

        private void SetAdditionalSettingsForOutputLabel(Label currentLabel)
        {
            currentLabel.Font = FONT_OUTPUT_LABEL;
            currentLabel.BorderStyle = BorderStyle.Fixed3D;
            currentLabel.MaximumSize = new Size(WIDTH_OUTPUT_LABEL, 0);
            currentLabel.MinimumSize = new Size(WIDTH_OUTPUT_LABEL, 0);
        }
        private string SetOutputString(double result, double userNumber, double secondRandNumber, int codeOfOperation)
        {
            switch (codeOfOperation)
            {
                case 1:
                    return $"{userNumber} + {secondRandNumber} = {result}";
                case 2:
                    return $"{userNumber} - {secondRandNumber} = {result}";
                case 3:
                    return $"{userNumber} * {secondRandNumber} = {result}";
                case 4:
                    return $"{userNumber} / {secondRandNumber} = {result}";
            }

            return $"Black box result: {result}";

        }

        private void Button_MouseClick(object sender, MouseEventArgs e)
        {
            if (globalCounterOfClickingStartButton == 0)
            {
                if (double.TryParse(_inputString.Text, out value))
                {
                    resValue = ConvertNumber(value, out secondValue, out codeOfUsedOperation);
                    _outputLabel.Text = SetOutputString(resValue, value, secondValue, codeOfUsedOperation);
                }
                else
                {
                    _outputLabel.Text = "unknown error";
                }
            }

            if (globalCounterOfClickingStartButton > 0)
            {

            }
        }

        private double ConvertNumber(double number, out double randomSecondNumber, out int usedMode)
        {
            usedMode = 0;

            Random random = new Random();
            int mode = random.Next(1, 5);

            double randomSecondMember = random.Next(1, 10);

            randomSecondNumber = randomSecondMember;
            switch (mode)
            {
                case 1:
                    usedMode = 1;
                    return number + randomSecondMember;
                case 2:
                    usedMode = 2;
                    return number - randomSecondMember;
                case 3:
                    usedMode = 3;
                    return number * randomSecondMember;
                case 4:
                    usedMode = 4;
                    return number / randomSecondMember;
            }

            return 0;
        }
    }
}
