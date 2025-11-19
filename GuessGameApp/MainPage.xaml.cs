using System.Globalization;

namespace GuessGameApp;

public partial class MainPage : ContentPage
{
    private double _firstNumber;
    private double _secondNumber;
    private string _currentOperation;
    private Button? _lastSelectedButton;
    private string _activeField = "first";


    public MainPage()
    {
        InitializeComponent();
    }
    
    private string NormalizeOp(string raw) => raw switch
    {
        "–" => "-",   
        "×" => "*",
        "÷" => "/",
        _ => raw
    };
    
    private void OnOperationClicked(object sender, EventArgs e)
    {
        if (_lastSelectedButton != null)
            _lastSelectedButton.BackgroundColor = (Color)Application.Current.Resources["OperationButtonColor"];
        Button button = (Button)sender;
        _currentOperation = NormalizeOp(button.Text);
        button.BackgroundColor = (Color)Application.Current.Resources["AccentCyan"];
        _lastSelectedButton = button;
    }
    

    private void OnClearClicked(object sender, EventArgs e)
    {
        ResultLabel.Text = "0";
        ExpressionLabel.Text = "";
        _currentOperation = "";
        FirstNumberEntry.Text = string.Empty;
        SecondNumberEntry.Text = string.Empty;
        
        _firstNumber = 0;
        _secondNumber = 0;
        _currentOperation = string.Empty;
        
        if (_lastSelectedButton != null)
            _lastSelectedButton.BackgroundColor = (Color)Application.Current.Resources["OperationButtonColor"];
    }
    
    private void OnClearEntryClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(SecondNumberEntry.Text))
        {
            SecondNumberEntry.Text = "";
            return;
        }
        
        if (!string.IsNullOrEmpty(FirstNumberEntry.Text))
        {
            FirstNumberEntry.Text = "";
        }
    }
    
    private void OnDecimalClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(FirstNumberEntry.Text))
        {
            FirstNumberEntry.Text = "0.";
            return;
        }
        
        if (!_currentOperation.Any())
        {
            if (!FirstNumberEntry.Text.Contains("."))
                FirstNumberEntry.Text += ".";
            return;
        }
        
        if (string.IsNullOrWhiteSpace(SecondNumberEntry.Text))
        {
            SecondNumberEntry.Text = "0.";
            return;
        }

        if (!SecondNumberEntry.Text.Contains("."))
            SecondNumberEntry.Text += ".";
    }
    
    private void OnFirstFocused(object sender, FocusEventArgs e)
    {
        _activeField = "first";
    }

    private void OnSecondFocused(object sender, FocusEventArgs e)
    {
        _activeField = "second";
    }
    
    private void OnSquareClicked(object sender, EventArgs e)
    {
        if (_activeField == "first")
        {
            if (!string.IsNullOrWhiteSpace(FirstNumberEntry.Text))
            {
                //umarım olursun
                if (!FirstNumberEntry.Text.EndsWith("²"))
                    FirstNumberEntry.Text += "²";
            }

            _currentOperation = "^2";  //aktif ettik umarım
        }
        else
        {
            if (!string.IsNullOrWhiteSpace(SecondNumberEntry.Text))
            {
                if (!SecondNumberEntry.Text.EndsWith("²"))
                    SecondNumberEntry.Text += "²";
            }

            _currentOperation = "^2";
        }
    }

    
    private void OnSqrtClicked(object sender, EventArgs e)
    {
        if (_activeField == "first")
        {
            if (!string.IsNullOrWhiteSpace(FirstNumberEntry.Text))
            {
                if (!FirstNumberEntry.Text.StartsWith("√"))
                    FirstNumberEntry.Text = "√" + FirstNumberEntry.Text;
            }

            _currentOperation = "sqrt";
        }
        else
        {
            if (!string.IsNullOrWhiteSpace(SecondNumberEntry.Text))
            {
                if (!SecondNumberEntry.Text.StartsWith("√"))
                    SecondNumberEntry.Text = "√" + SecondNumberEntry.Text;
            }

            _currentOperation = "sqrt";
        }
    }
    
    private double ParseNumber(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return 0;

        // kare
        if (input.EndsWith("²"))
        {
            string numberPart = input.Replace("²", "");
            if (double.TryParse(numberPart, out double n))
                return Math.Pow(n, 2);
        }

        // karekok
        if (input.StartsWith("√"))
        {
            string numberPart = input.Replace("√", "");
            if (double.TryParse(numberPart, out double n))
                return Math.Sqrt(n);
        }
        
        if (double.TryParse(input, out double normal))
            return normal;

        return 0;
    }

    
    private void OnCalculateClicked(object sender, EventArgs e)
    {
       
        System.Diagnostics.Debug.WriteLine($"DEBUG: First Number Entry Text is '{FirstNumberEntry.Text}'");
        System.Diagnostics.Debug.WriteLine($"DEBUG: Second Number Entry Text is '{SecondNumberEntry.Text}'");
        
        _firstNumber = ParseNumber(FirstNumberEntry.Text);
        _secondNumber = ParseNumber(SecondNumberEntry.Text);

        if (double.IsNaN(_firstNumber) || double.IsNaN(_secondNumber))
        {
            DisplayAlert("ERROR", "PLEASE ENTER A VALID NUMBER:", "OK");
            return;
        }
        

        if (string.IsNullOrEmpty(_currentOperation))
        {
            DisplayAlert("ERROR", "PLEASE CHOOSE AN OPERATION", "OK");
            return;
        }

        double result = 0;

        switch (_currentOperation)
        {
            case "+":
                result = _firstNumber + _secondNumber;
                break;
            case "-":
                result = _firstNumber - _secondNumber;
                break;
            case "*":  
                result = _firstNumber * _secondNumber;
                break;
            case "/":
                if (_secondNumber == 0)
                {
                    DisplayAlert("ERROR", "NO NUMBER CAN BE DIVIDED BY ZERO", "OK");
                    return;
                }
                result = _firstNumber / _secondNumber;
                break;
        }
        ResultLabel.Text = result.ToString(CultureInfo.InvariantCulture);
        ExpressionLabel.Text = $"{_firstNumber} {_currentOperation} {_secondNumber}";

    }
}