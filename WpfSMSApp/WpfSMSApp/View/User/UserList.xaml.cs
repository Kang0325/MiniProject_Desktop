using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfSMSApp.Helper;

namespace WpfSMSApp.View.User

{
    /// <summary>
    /// MyAccount.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UserList : Page
    {
        public UserList()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var user = Commons.LOGINED_USER;
                /*TxtUserID.Text = user.UserID.ToString();
                TxtUserIdentityNumber.Text = user.UserIdentityNumber.ToString(); ;
                TxtUserSurName.Text = user.UserSurname.ToString(); ;
                TxtUserName.Text = user.UserName.ToString(); ;
                TxtUserEmail.Text = user.UserEmail.ToString(); ;
                TxtUserAdmin.Text = user.UserAdmin.ToString();
                TxtUserActivated.Text = user.UserActivated.ToString();*/
            }
            catch (Exception ex)
            {
                Commons.LOGGER.Error($"예외 발생 MyAccount Loaded : {ex}");
                throw ex;
            }
        }

        private void BtnMyAccount_Click(object sender, RoutedEventArgs e)
        {
            //NavigationService.Navigate(new EditAccount());
        }

        private void BtnAddUser_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnEditUser_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDeactivateUser_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnExportPdf_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
