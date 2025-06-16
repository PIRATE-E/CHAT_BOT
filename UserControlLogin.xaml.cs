using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Python.Runtime;

namespace CHAT_BOT
{
    /// <summary>
    /// Interaction logic for UserControlLogin.xaml
    /// </summary>
    public partial class UserControlLogin : UserControl
    {
        private readonly MainWindow? _main;


        private dynamic _sys;

        private string _pythonPath;
        private dynamic _loginAi;

        private dynamic _rememberMePyClass;


        private readonly Storyboard _focusStoryboard;
        private readonly Storyboard _unfocusStoryboard;
        private bool _rememberMe;

        public UserControlLogin()
        {
            _main = (MainWindow)Application.Current.MainWindow; // this is to access the main window

            InitializeComponent();

            Task.Run(() => button_manager());
            this.Loaded += Window_Loaded;

            // Initialize storyboards
            _focusStoryboard = (Storyboard)FindResource("PlaceholderFocusAnimation");
            _unfocusStoryboard = (Storyboard)FindResource("PlaceholderUnfocusAnimation");

            _pythonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..",
                "..", "..",
                "python");
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // setting up the python environment
            using (Py.GIL())
            {
                using (dynamic sys = Py.Import("sys"))
                {
                    sys.path.append(_pythonPath);
                }
            }


            // this to show animation after the load of the window

            await Task.Delay(1000);

            UserNameBox.Focus();

            await Task.Delay(200);
            // If there's text already in the boxes, move placeholders up
            UpdatePlaceholderPosition(UserNameBox, UserNamePlaceHolder, UserNameCanvas);
            UpdatePlaceholderPosition(PasswordBox, PasswordPlaceHolder, PasswordCanvas);

            await remember_me_feature();
        }

        private async Task remember_me_feature()
        {
            try
            {
                await Task.Run(() =>
                {
                    using (Py.GIL())
                    {
                        using (dynamic py = Py.Import("main"))
                        {
                            {
                                _loginAi = py.LoginAI();
                                _loginAi.create_table();

                                string username, password;

                                _rememberMePyClass = py.MakeRememberMe();

                                var loadObj = _rememberMePyClass.load_data();

                                if (loadObj != null)
                                {
                                    username = loadObj[0];
                                    password = loadObj[1];

                                    Console.WriteLine($"username: {username}, password: {password}");
                                    Console.WriteLine($"remember me feature is working");

                                    bool status = _loginAi.login(username, password);

                                    if (status)
                                    {
                                        UserControlHome.CurrentUser = username;
                                        Dispatcher.Invoke(After_success);
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("username: null, password: null");
                                }
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() => MessageBox.Show(ex.Message));
                Console.WriteLine(ex);
            }
        }

        private void UsernameBox_GotFocus(object sender, RoutedEventArgs e)
        {
            AnimatePlaceholder(UserNamePlaceHolder, UserNameCanvas, true);
        }

        private void UsernameBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(UserNameBox.Text))
            {
                AnimatePlaceholder(UserNamePlaceHolder, UserNameCanvas, false);
            }
        }

        private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            AnimatePlaceholder(PasswordPlaceHolder, PasswordCanvas, true);
        }

        private void PasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(PasswordBox.Password))
            {
                AnimatePlaceholder(PasswordPlaceHolder, PasswordCanvas, false);
            }
        }

        private void AnimatePlaceholder(TextBlock placeholder, Canvas canvas, bool isFocused)
        {
            Storyboard storyboard = isFocused ? _focusStoryboard.Clone() : _unfocusStoryboard.Clone();

            foreach (Timeline animation in storyboard.Children)
            {
                Storyboard.SetTarget(animation, placeholder);
            }

            storyboard.Begin();
        }

        private void UpdatePlaceholderPosition(Control inputControl, TextBlock placeholder, Canvas canvas)
        {
            bool hasContent = false;

            if (inputControl is TextBox textBox)
            {
                hasContent = !string.IsNullOrEmpty(textBox.Text);
            }
            else if (inputControl is PasswordBox passwordBox)
            {
                hasContent = !string.IsNullOrEmpty(passwordBox.Password);
            }

            if (hasContent || inputControl.IsFocused)
            {
                Canvas.SetTop(placeholder, -20);
                placeholder.FontSize = 10;
                placeholder.Foreground = new SolidColorBrush(
                    (Color)ColorConverter.ConvertFromString("#4361EE"));
            }
        }

        private async Task button_manager()
        {
            //await Task.Delay(1000);
            while (true)
            {
                Dispatcher.Invoke(() =>
                {
                    if (string.IsNullOrEmpty(UserNameBox.Text) || string.IsNullOrEmpty(PasswordBox.Password))
                    {
                        LoginButton.IsEnabled = false;
                        LoginButton.Opacity = 0.3;

                        SignupButton.IsEnabled = false;
                        SignupButton.Opacity = 0.3;

                        if (UserNameBox.Text.Length >= 2)
                        {
                            // Add logic here if needed
                            LoginButton.IsEnabled = false;
                            LoginButton.Opacity = 0.5;

                            SignupButton.IsEnabled = false;
                            SignupButton.Opacity = 0.5;
                        }
                    }
                    else
                    {
                        LoginButton.IsEnabled = true;
                        LoginButton.Opacity = 1.0;

                        SignupButton.IsEnabled = true;
                        SignupButton.Opacity = 1.0;
                    }
                });
                await Task.Delay(500); // Add a delay to allow the UI thread to remain responsive
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            check_credentials();

            try
            {
                using (Py.GIL())
                {
                    bool status = _loginAi.login(UserNameBox.Text, PasswordBox.Password);

                    if (status)
                    {
                        Console.WriteLine($"Login successful for user: {UserNameBox.Text}");
                        UserControlHome.CurrentUser = UserNameBox.Text;

                        if (_rememberMe)
                        {
                            _rememberMePyClass.set_data(UserNameBox.Text, PasswordBox.Password);
                            _rememberMePyClass.save_data();
                        }

                        After_success();
                    }
                    else
                    {
                        Console.WriteLine("Login failed - invalid credentials");
                        MessageBox.Show("Login Failed");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login error: {ex.Message}");
                MessageBox.Show(ex.Message);
            }
        }

        private void check_credentials()
        {
            if (UserNameBox.Text.Length <= 3 || PasswordBox.Password.Length <= 3)
            {
                MessageBox.Show("Username and Password must be at least 4 characters long");
            }
            // here we will check the user credentials using python 
        }

        private void SignupButton_Click(object sender, RoutedEventArgs e)
        {
            check_credentials();

            try
            {
                using (Py.GIL())
                {
                    //MessageBox.Show(python_path);
                    using (dynamic _py = Py.Import("main"))
                    {
                        using (dynamic loginAi = _py.LoginAI())
                        {
                            loginAi.create_table();
                            bool status = loginAi.create_account(UserNameBox.Text, PasswordBox.Password);
                            if (status)
                            {
                                UserControlHome.CurrentUser = UserNameBox.Text;
                                MessageBox.Show("account has been created");
                                After_success();
                            }
                            else
                            {
                                if (loginAi.userpresent)
                                {
                                    MessageBox.Show("account already present");
                                }
                                else
                                    MessageBox.Show("account creation failed");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void After_success()
        {
            _main.MainContent.Content = new UserControlHome();
        }

        private void remember_me_checked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                _rememberMe = checkBox.IsChecked ?? false;

                if (_rememberMe)
                {
                    // if checked, save the username and password
                }

                Console.WriteLine($"Remember Me set to: {_rememberMe}");
            }
        }

        private void Forget_Password_Clicked(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("you must take care of you password", "Forget Password", MessageBoxButton.OK,
                MessageBoxImage.Stop);
        }
    }
}