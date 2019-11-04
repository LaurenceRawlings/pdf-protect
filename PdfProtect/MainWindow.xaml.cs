using System.Windows;
using Microsoft.Win32;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf.Security;

namespace PdfProtect
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Btn_browse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Title = "Browse PDF Files",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "pdf",
                Filter = "pdf files (*.pdf)|*.pdf",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (ofd.ShowDialog() == true)
            {
                txt_filePath.Text = ofd.FileName;
                txt_warning.Content = "Enter a password";
                txt_password.IsEnabled = true;
                txt_confirmPassword.IsEnabled = true;
            } else
            {
                txt_password.IsEnabled = false;
                txt_confirmPassword.IsEnabled = false;
                txt_warning.Content = "Select a file";
                txt_filePath.Clear();
                txt_password.Clear();
                txt_confirmPassword.Clear();
                btn_encrypt.IsEnabled = false;
            }
        }

        private void Encrypt(string filename)
        {
            // Open an existing document. Providing an unrequired password is ignored.
            PdfDocument document = PdfReader.Open(filename, "password");

            PdfSecuritySettings securitySettings = document.SecuritySettings;

            // Setting one of the passwords automatically sets the security level to 
            // PdfDocumentSecurityLevel.Encrypted128Bit.
            securitySettings.UserPassword = txt_password.Password;
            securitySettings.OwnerPassword = txt_password.Password;

            // Don't use 40 bit encryption unless needed for compatibility reasons
            //securitySettings.DocumentSecurityLevel = PdfDocumentSecurityLevel.Encrypted40Bit;

            // Restrict some rights.
            securitySettings.PermitAccessibilityExtractContent = false;
            securitySettings.PermitAnnotations = false;
            securitySettings.PermitAssembleDocument = false;
            securitySettings.PermitExtractContent = false;
            securitySettings.PermitFormsFill = true;
            securitySettings.PermitFullQualityPrint = false;
            securitySettings.PermitModifyDocument = true;
            securitySettings.PermitPrint = false;

            // Save the document...
            document.Save(filename);
        }

        private void Btn_encrypt_Click(object sender, RoutedEventArgs e)
        {
            Encrypt(txt_filePath.Text);
            MessageBox.Show("The PDF was encrpted.", "All Done!");
            txt_filePath.Clear();
            txt_password.Clear();
            txt_confirmPassword.Clear();
            txt_warning.Content = "Select a file";
            btn_encrypt.IsEnabled = false;
            txt_password.IsEnabled = false;
            txt_confirmPassword.IsEnabled = false;
        }

        private void Txt_confirmPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (txt_password.Password == txt_confirmPassword.Password)
            {
                txt_warning.Content = "Ready!";
                btn_encrypt.IsEnabled = true;
            }
            else
            {
                txt_warning.Content = "Passwords don't match!";
                btn_encrypt.IsEnabled = false;
            }
        }
    }
}
