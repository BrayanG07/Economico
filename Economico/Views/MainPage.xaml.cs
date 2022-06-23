using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;
using System.Reflection;
using Plugin.CloudFirestore;
using Xamarin.Forms.Maps;
using Economico.Views;

namespace Economico
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        
        public MainPage()
        {
            InitializeComponent();
        }


        private async void BtnFirebase_Clicked(object sender, EventArgs e)
        {
            // OBTENER INFORMACION FILTRADA POR DOCUMENTO
            var document = await CrossCloudFirestore.Current
                                        .Instance
                                        .Collection("users")
                                        .Document("tszGxFhIQQtcor1jIGIj")
                                        .GetAsync();

            await DisplayAlert("Alerta", document.Id, "Ok");

            Dictionary<string, object> city = (Dictionary<string, object>)document.Data;
            foreach (KeyValuePair<string, object> pair in city)
            {
                Console.WriteLine("{0}: {1}", pair.Key, pair.Value);
            }

        }

        private async void BtnViewMap_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MapPage());
        }

        private async void BtnViewLocationRepartidor_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UbicationDeliveryPage());
        }
    }
}