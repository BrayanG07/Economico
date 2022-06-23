using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Android.Content;
using Xamarin.Forms;

namespace Economico.Droid
{
    [Activity(Label = "Economico", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        Intent serviceIntent; // INTENCION PARA TRABAJAR EL SERVICIO EN SEGUNDO PLANO YA CONFIGURADO
        private const int REQUEST_CODE = 5469;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Xamarin.FormsMaps.Init(this, savedInstanceState);

            serviceIntent = new Intent(this, typeof(AndroidLocationService));
            SetServiceMethods(); // INVOCAMOS NUESTRO SERVICIO PARA QUE LOS MESSAGES SE SUSCRIBAN A SU CENTRAL DE MESSAGES

            if (Build.VERSION.SdkInt >= BuildVersionCodes.M && !Android.Provider.Settings.CanDrawOverlays(this)) 
            {
                var intent = new Intent(Android.Provider.Settings.ActionManageOverlayPermission);
                intent.SetFlags(ActivityFlags.NewTask);
                this.StartActivity(intent);
            }

            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        //METODOS PARA LA EJECUCION DE LA UBICACION DEL REPARTIDOR EN SEGUNDO PLANO
        void SetServiceMethods() // EJECUTAMOS EL SERVICIO CON ESTE METODO
        {
            MessagingCenter.Subscribe<StartServiceMessage>(this, "ServiceStarted", message =>
            {
                if (!IsServiceRunning(typeof(AndroidLocationService))) // SI EL SERVICIO NO SE ESTA EJECUTANDO
                {
                    //LANZAMOS A EJECUCION EL SERVICIO SEGUN NUESTRO VERSION DE SO
                    if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
                    {
                        StartForegroundService(serviceIntent);
                    }
                    else
                    {
                        StartService(serviceIntent);
                    }
                }
            });

            MessagingCenter.Subscribe<StopServiceMessage>(this, "ServiceStopped", message =>
            {
                if (IsServiceRunning(typeof(AndroidLocationService)))
                    StopService(serviceIntent);
            });
        }

        private bool IsServiceRunning(System.Type cls)
        {
            ActivityManager manager = (ActivityManager)GetSystemService(Context.ActivityService);
            foreach (var service in manager.GetRunningServices(int.MaxValue))
            {
                if (service.Service.ClassName.Equals(Java.Lang.Class.FromType(cls).CanonicalName))
                {
                    return true;
                }
            }
            return false;
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            // SI LOS PERMISOS DE CODIGO COINCIDEN HACEMOS LA SUPERPOSICION DE LA APP
            if (requestCode == REQUEST_CODE) 
            {
                if (Android.Provider.Settings.CanDrawOverlays(this))
                {

                }
            }
            base.OnActivityResult(requestCode, resultCode, data);
        }
    }
}