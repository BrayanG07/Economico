using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace Economico.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {
        const double destinationLatitude = 14.524974518916082;
        const double destinationLongitude = -87.0829298796564;

        private Pin pinDelivery = new Pin
        {
            Label = "Repartidor",
            Type = PinType.Place,
        };

        private Pin pinDestination = new Pin
        {
            Label = "Destino",
            Type = PinType.Place,
            Position = new Position(destinationLatitude, destinationLongitude)
        };

        public MapPage()
        {
            InitializeComponent();
            GetDataRealTime();
        }

        private void GetDataRealTime()
        {
            string idDocument = "1";
            var index = 0;

            if (index == 0)
            {
                map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(destinationLatitude, destinationLongitude), Distance.FromMeters(100)));
                index = 1;                                                
            }

            map.Pins.Add(pinDelivery);
            map.Pins.Add(pinDestination);

            CrossCloudFirestore.Current
                   .Instance
                   .Collection("Ubication")
                   .AddSnapshotListener((snapshot, error) =>
                   {
                       if (snapshot != null)
                       {
                           foreach (var documentChange in snapshot.DocumentChanges)
                           {
                               if (documentChange.Type == DocumentChangeType.Modified)
                               {

                                   if (documentChange.Document.Id.Equals(idDocument))
                                   {
                                       Dictionary<string, object> city = (Dictionary<string, object>)documentChange.Document.Data;
                                       double latitude = 0, longitude = 0;

                                       foreach (KeyValuePair<string, object> pair in city)
                                       {
                                           if (pair.Key.Equals("ubication"))
                                           {
                                               string[] ubication = pair.Value.ToString().Split(',');
                                               latitude = Convert.ToDouble(ubication[0]);
                                               longitude = Convert.ToDouble(ubication[1]);
                                           }
                                       }
                                       pinDelivery.Position = new Position(latitude, longitude);
                                       map.MoveToRegion(MapSpan.FromCenterAndRadius(pinDelivery.Position, Distance.FromMeters(100)));
                                   }

                               }
                           }
                       }
                   });
        }
    }
}