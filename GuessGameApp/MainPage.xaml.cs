using System.Globalization;

namespace GuessGameApp;

public partial class MainPage : ContentPage
{
    private double _firstNumber;
    private double _secondNumber;
    private string _currentOperation;
    private Button? _lastSelectedButton;

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
        FirstNumberEntry.Text = string.Empty;
        SecondNumberEntry.Text = string.Empty;
        
        _firstNumber = 0;
        _secondNumber = 0;
        _currentOperation = string.Empty;
        
        if (_lastSelectedButton != null)
            _lastSelectedButton.BackgroundColor = (Color)Application.Current.Resources["OperationButtonColor"];
    }
    
    private void OnCalculateClicked(object sender, EventArgs e)
    {
       
        System.Diagnostics.Debug.WriteLine($"DEBUG: First Number Entry Text is '{FirstNumberEntry.Text}'");
        System.Diagnostics.Debug.WriteLine($"DEBUG: Second Number Entry Text is '{SecondNumberEntry.Text}'");
        

        bool isFirstNumberValid = double.TryParse(
            FirstNumberEntry.Text, 
            NumberStyles.Any,
            CultureInfo.InvariantCulture, 
            out _firstNumber);
        
        bool isSecondNumberValid = double.TryParse(
            SecondNumberEntry.Text, 
            NumberStyles.Any,
            CultureInfo.InvariantCulture, 
            out _secondNumber);

        if (!isFirstNumberValid || !isSecondNumberValid)
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
    }
}