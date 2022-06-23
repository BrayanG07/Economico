using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Economico.Droid
{
    // ESTA CLASE SIRVE COMO RECEPTOR DE TRANSMISION DE ARRANQUE PARA EL SO
    [BroadcastReceiver(Name = "com.locationservice.app.BootBroadcastReceiver", Enabled = true)]
    [IntentFilter(new[] { Intent.ActionBootCompleted })]
    public class BootBroadcastReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            if (intent.Action.Equals(Intent.ActionBootCompleted))
            {
                // SI EL USUARIO REINICIA SATISFACTORIAMENTE BASICAMENTE LO QUE HACEMOS ES
                // DECIRLE QUE EJECUTRE LA APLICACION
                Intent main = new Intent(context, typeof(MainActivity));
                main.AddFlags(ActivityFlags.NewTask);
                context.StartActivity(main);
            }
        }
    }
}