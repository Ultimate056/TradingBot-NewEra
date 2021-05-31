using NewEra_Bot.Models;
using NewEra_Bot.MVVM.ViewModels;
using System.Windows;

namespace NewEra_Bot
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static User user;
        public static MainWindowViewModel vm;
        public void StartFillInfoAboutUser()
        {
            if (MyBinance.IsAlreadyConnection())
                user = new User(MyBinance.path);
            else
            {
                string name = "", ReferalID = "", API_Key = "", Secret_Key = "";
                long ID_User = 0;
                ModalWindow mw1 = new ModalWindow("Имя: ");
                if (mw1.ShowDialog() == true)
                    name = mw1.GetValue();
                ModalWindow mw2 = new ModalWindow("ReferalID: ");
                if (mw2.ShowDialog() == true)
                    ReferalID = mw2.GetValue();
                ModalWindow mw3 = new ModalWindow("API Key: ");
                if (mw3.ShowDialog() == true)
                    API_Key = mw3.GetValue();
                ModalWindow mw4 = new ModalWindow("Secret Key: ");
                if (mw4.ShowDialog() == true)
                    Secret_Key = mw4.GetValue();
                ModalWindow mw5 = new ModalWindow("ID пользователя: ");
                if (mw5.ShowDialog() == true)
                    ID_User = long.Parse(mw5.GetValue());
                user = new User(name, ReferalID, ID_User, API_Key, Secret_Key);
            }
        }
        public MainWindow()
        { 
            StartFillInfoAboutUser();
            MyBinance.user = user;
            InitializeComponent();
            NameUser.Text = user.GetName();
            ReferalID.Text = user.GetReferalID();
            ID_User.Text = user.GetUserID().ToString();
            Secret_Key.Text = user.GetSecretKey();
            API_Key.Text = user.GetAPIKey();
            user.Balance = MyBinance.GetBalanceUSDT();
            vm = new MainWindowViewModel();
            DataContext = vm;
        }
    }
}
