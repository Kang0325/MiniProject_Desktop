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
using MahApps.Metro.Controls.Dialogs;
using System.Security.Cryptography;

namespace WpfSMSApp.View.Store
{
    /// <summary>
    /// MyAccount.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class AddStore : Page
    {
        public AddStore()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LblUserIdentityNumber.Visibility = LblUserSurName.Visibility = Visibility.Hidden;

                TxtUserID.Text = TxtUserIdentityNumber.Text = "";
            }
            catch (Exception ex)
            {
                Commons.LOGGER.Error($"예외 발생 MyAccount Loaded : {ex}");
                throw ex;
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        public bool IsValidInput()
        {
            bool isValid = true;

            if (string.IsNullOrEmpty(TxtUserIdentityNumber.Text))
            {
                LblUserSurName.Visibility = Visibility.Visible;
                LblUserSurName.Text = "사번을 입력하세요";
                isValid = false;
            }
            else
            {
                var cnt = Logic.DataAccess.GetUsers().Where(u => u.UserIdentityNumber.Equals(TxtUserIdentityNumber.Text)).Count();
                if (cnt > 0)
                {
                    LblUserIdentityNumber.Visibility = Visibility.Visible;
                    LblUserIdentityNumber.Text = "중복된 사번이 있습니다";
                    isValid = false;
                }
            }


            if (string.IsNullOrEmpty(TxtUserSurName.Text))
            {
                LblUserSurName.Visibility = Visibility.Visible;
                LblUserSurName.Text = "이름(성)을 입력하세요";
                isValid = false;
            }

            return isValid;

        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = true; // 입력된 값이 모두 만족하는지 판별하는 플래그

            LblUserIdentityNumber.Visibility = LblUserSurName.Visibility = Visibility.Hidden;

            var user = new Model.User();

            isValid = IsValidInput();

            if (isValid)
            {
                user.UserIdentityNumber = TxtUserIdentityNumber.Text;
                //MessageBox.Show("DB 수정 처리");
                user.UserSurname = TxtUserSurName.Text;

                try
                {
                    var mdHash = MD5.Create();
                    user.UserPassword = Commons.GetMd5Hash(mdHash, user.UserPassword);

                    var result = Logic.DataAccess.SetUser(user);
                    if (result == 0)
                    {
                        // 수정 안됨
                        LblResult.Text = "계정 수정에 문제가 발생했습니다. 관리자에게 문의바랍니다.";
                        LblResult.Foreground = Brushes.OrangeRed;
                    }
                    else
                    {
                        //NavigationService.Navigate(new UserList());
                    }
                }
                catch (Exception ex)
                {
                    Commons.LOGGER.Error($"예외 발생 : {ex}");
                }
            }
        }
    }
}
