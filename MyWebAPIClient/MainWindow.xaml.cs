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
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Net.Http;

namespace MyWebAPIClient
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

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            string authority = "https://adfs.contoso.com/adfs";
            string resourceURI = "https://win7.contoso.com/MyWebAPIsample/";
            string clientID = "09c9a8a2-6bf1-427d-89ba-45c2c02bb9fc";
            string clientReturnURI = "http://anarbitraryreturnuri/";

            var authContext = new AuthenticationContext(authority, false);
            var authResult = await authContext.AcquireTokenAsync(resourceURI, clientID, new Uri(clientReturnURI), new PlatformParameters(PromptBehavior.Auto));

            string authHeader = authResult.CreateAuthorizationHeader();

            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://win7.contoso.com/MyWebAPIsample/api/values");
            request.Headers.TryAddWithoutValidation("Authorization", authHeader);
            var response = await client.SendAsync(request);
            string responseString = await response.Content.ReadAsStringAsync();
            MessageBox.Show(responseString);
        }
    }
}
