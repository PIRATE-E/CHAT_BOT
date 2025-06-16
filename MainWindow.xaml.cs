using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Python.Runtime;

namespace CHAT_BOT;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private bool response_done;
    private string response_str; // Added private variable

    public MainWindow()
    {
        Python.Runtime.Runtime.PythonDLL =
            "C:\\Users\\pirat\\AppData\\Local\\Programs\\Python\\Python313\\python313.dll";
        PythonEngine.Initialize(); // this is for making my python script runnable
        InitializeComponent();
        PythonEngine.BeginAllowThreads();

        // this is to load login page 
        MainContent.Content = new UserControlLogin();

        // adding event handler for response event
        // this is for closing  
        this.Closing += MainWindow_OnClosing;
    }

    private void MainWindow_OnClosing(object? sender, CancelEventArgs e)
    {
        Console.WriteLine("application is closing");

        try
        {
            // Give the UserControlHome a chance to complete its unload operations
            if (MainContent.Content is UserControlHome userControlHome)
            {
                Console.WriteLine("Attempting to save history before shutdown");
                // these methods are not being called by unload even so calling them explicitly
                userControlHome.save_history();

                // Explicitly trigger unload events
                if (MainContent is FrameworkElement frameworkElement)
                {
                    frameworkElement.RaiseEvent(new RoutedEventArgs(UnloadedEvent, frameworkElement));
                }
                
                // Allow a short time for the unload event to complete
                System.Threading.Thread.Sleep(500);
                
                userControlHome.kill_ollama();
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine($"Error during UserControlHome cleanup: {exception.Message}");
            Console.WriteLine($"Stack trace: {exception.StackTrace}");
        }
        
        try
        {
            // Ensure Python is still initialized before shutting down
            if (PythonEngine.IsInitialized)
            {
                PythonEngine.Shutdown();
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine($"Error shutting down Python: {exception.Message}");
        }
    }
}
