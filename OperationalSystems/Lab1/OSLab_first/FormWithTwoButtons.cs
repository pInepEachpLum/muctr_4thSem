using System.CodeDom;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;

namespace OSLab_first
{
    public partial class FormWithTwoButtons : BaseForm
    {
        //Some constant measurements for this class
        private const string FORM_TITLE_TEXT =  "Form with two buttons";
        private const int DISTANCE_BETWEEN_BUTTONS = 300;

        //Definition of buttons
        private Button _buttonFirst = new Button();
        private Button _buttonSecond = new Button();
        
        //Any button defaults for this class
        private const string DEFAULT_BUTTON_TEXT = "BUTTON";
        private int DEFAULT_BUTTON_TOP = (FORM_WINDOW_HEIGHT - DEFAULT_BUTTON_HEIGHT) / 3;

        //Definition of position of left and right buttons on axis-X
        private const int POSITION_OF_LEFT_BUTTON = (FORM_WINDOW_WIDTH - (DEFAULT_BUTTON_WIDTH * 2 + DISTANCE_BETWEEN_BUTTONS)) / 2;
        private const int POSITION_OF_RIGHT_BUTTON = POSITION_OF_LEFT_BUTTON + DEFAULT_BUTTON_WIDTH + DISTANCE_BETWEEN_BUTTONS;
        
        //Statements for events
        private const string IF_MOUSE_ON_BUTTON_TEXT = "Пришел";
        private const string IF_MOUSE_ON_ANOTHER_BUTTON_TEXT = "Ушел";

        //Definition of informating label
        private Label _labelUnderButtons = new Label();

        private const string INFORMATION_LABEL_TEXT = "Please, push any button.";
        private const int DISTANCE_BETWEEN_BUTTONS_AND_LABEL = 100;

        public FormWithTwoButtons()
        {
            InitializeComponent();
            
            //Initialisation of default window settings
            SetWindow();

            //Definition of default properties of buttons
            SetDefaultPropertiesButton(_buttonFirst);
            SetDefaultPropertiesButton(_buttonSecond);

            //Definition of special properties of buttons
            SetButtonLeft(_buttonFirst);
            SetButtonRight(_buttonSecond);

            //Subsribing a buttons to an events of mouse enter, mouse leave and mouse click
            _buttonFirst.MouseEnter += Button_MouseEnter;
            _buttonSecond.MouseEnter += Button_MouseEnter;

            _buttonFirst.MouseLeave += Button_MouseLeave;
            _buttonSecond.MouseLeave += Button_MouseLeave;

            _buttonFirst.MouseClick += Button_MouseClick;
            _buttonSecond.MouseClick += Button_MouseClick;

            //Definition of information signature
            SetInfoLabel(_labelUnderButtons);

            //Adding buttons on form
            this.Controls.Add(_buttonFirst);
            this.Controls.Add(_buttonSecond);
            this.Controls.Add(_labelUnderButtons);
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
            anyButton.Top = DEFAULT_BUTTON_TOP;
        }

        protected override void SetInfoLabel(Label infoLabel)
        {
            base.SetInfoLabel(infoLabel);
            
            infoLabel.Text = INFORMATION_LABEL_TEXT;

            int textWidth = TextRenderer.MeasureText(infoLabel.Text, infoLabel.Font).Width;
            infoLabel.Left = (this.ClientSize.Width - textWidth) / 2;

            infoLabel.Top = DEFAULT_BUTTON_TOP + DEFAULT_BUTTON_HEIGHT + DISTANCE_BETWEEN_BUTTONS_AND_LABEL;
        }
        private void SetButtonLeft(Button buttonLeft)
        {
            buttonLeft.Left = POSITION_OF_LEFT_BUTTON;
        }

        private void SetButtonRight(Button buttonRight)
        {
            buttonRight.Left = POSITION_OF_RIGHT_BUTTON;
        }

        protected override void Button_MouseEnter(object sender, EventArgs e)
        {
            base.Button_MouseEnter(sender, e);

            Button currentButton = sender as Button;
            if (currentButton == null) return;

            currentButton.Text = IF_MOUSE_ON_BUTTON_TEXT;


            if (currentButton == _buttonFirst)
            {
                _buttonSecond.Text = IF_MOUSE_ON_ANOTHER_BUTTON_TEXT;
            }
            else if (currentButton == _buttonSecond)
            {
                _buttonFirst.Text = IF_MOUSE_ON_ANOTHER_BUTTON_TEXT;
            }
        }
        protected override void Button_MouseLeave(object sender, EventArgs e)
        {
            base.Button_MouseLeave(sender, e);

            _buttonFirst.Text = DEFAULT_BUTTON_TEXT;
            _buttonSecond.Text = DEFAULT_BUTTON_TEXT;
        }
        private void Button_MouseClick(object sender, EventArgs e)
        {
            FormWithInputNumber second_form = new FormWithInputNumber();

            second_form.ShowDialog();
        }
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            DEFAULT_FONT?.Dispose();
            base.OnFormClosed(e);
        }

    }
}
