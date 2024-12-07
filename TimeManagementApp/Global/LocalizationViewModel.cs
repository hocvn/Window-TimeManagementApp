using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Resources;

public class LocalizationViewModel : INotifyPropertyChanged
{
    private readonly ResourceLoader _resourceLoader = ResourceLoader.GetForViewIndependentUse();

    private string _settingsTitle;
    public string SettingsTitle
    {
        get => _settingsTitle;
        set
        {
            _settingsTitle = value;
            OnPropertyChanged();
        }
    }

    private string _learnHowToUseApp;
    public string LearnHowToUseApp
    {
        get => _learnHowToUseApp;
        set
        {
            _learnHowToUseApp = value;
            OnPropertyChanged();
        }
    }

    private string _openLinkButton;
    public string OpenLinkButton
    {
        get => _openLinkButton;
        set
        {
            _openLinkButton = value;
            OnPropertyChanged();
        }
    }

    private string _country;
    public string Country
    {
        get => _country;
        set
        {
            _country = value;
            OnPropertyChanged();
        }
    }

    private string _backgroundTheme;
    public string BackgroundTheme
    {
        get => _backgroundTheme;
        set
        {
            _backgroundTheme = value;
            OnPropertyChanged();
        }
    }

    private string _samplesTheme;
    public string SamplesTheme
    {
        get => _samplesTheme;
        set
        {
            _samplesTheme = value;
            OnPropertyChanged();
        }
    }

    private string _orYouCanUseYourOwnBackground;
    public string OrYouCanUseYourOwnBackground
    {
        get => _orYouCanUseYourOwnBackground;
        set
        {
            _orYouCanUseYourOwnBackground = value;
            OnPropertyChanged();
        }
    }

    private string _database;
    public string Database
    {
        get => _database;
        set
        {
            _database = value;
            OnPropertyChanged();
        }
    }

    private string _account;
    public string Account
    {
        get => _account;
        set
        {
            _account = value;
            OnPropertyChanged();
        }
    }

    public LocalizationViewModel()
    {
        UpdateLocalization();
    }

    public void UpdateLocalization()
    {
        SettingsTitle = _resourceLoader.GetString("settingsTitle.Text");
        LearnHowToUseApp = _resourceLoader.GetString("learnHowtoUseApp.Content");
        OpenLinkButton = _resourceLoader.GetString("openLinkButton.Content");
        Country = _resourceLoader.GetString("country.Text");
        BackgroundTheme = _resourceLoader.GetString("backgroundTheme.Text");
        SamplesTheme = _resourceLoader.GetString("samplesTheme.Text");
        OrYouCanUseYourOwnBackground = _resourceLoader.GetString("orYouCanUseYourOwnBackground.Content");
        Database = _resourceLoader.GetString("database.Text");
        Account = _resourceLoader.GetString("account.Text");

        // Log loaded values for debugging
        Console.WriteLine($"SettingsTitle: {SettingsTitle}");
        Console.WriteLine($"LearnHowToUseApp: {LearnHowToUseApp}");
        Console.WriteLine($"OpenLinkButton: {OpenLinkButton}");
        Console.WriteLine($"Country: {Country}");
        Console.WriteLine($"BackgroundTheme: {BackgroundTheme}");
        Console.WriteLine($"SamplesTheme: {SamplesTheme}");
        Console.WriteLine($"OrYouCanUseYourOwnBackground: {OrYouCanUseYourOwnBackground}");
        Console.WriteLine($"Database: {Database}");
        Console.WriteLine($"Account: {Account}");
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
