using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Java.Lang;
using Com.Cloudpos.Printer;
using Com.Cloudpos;
using Android.Util;
using Com.Cloudpos.Sdk.Printer.Impl;

namespace UseAAR
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        TextView tvLog;
        Button btnOpen,btnPrint,btnClose;
        PrinterDeviceImpl printerDevice;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            btnOpen = (Button)FindViewById(Resource.Id.btnOpen);
            btnPrint = (Button)FindViewById(Resource.Id.btnPrint);
            btnClose = (Button)FindViewById(Resource.Id.btnClose);
            tvLog = (TextView)FindViewById(Resource.Id.tv_log);
            btnOpen.Click += BtnOpen_Click;
            btnPrint.Click += BtnPrint_Click;
            btnClose.Click += BtnClose_Click;

            Thread thread1 = new Thread(delegate ()
            {
                printerDevice = (PrinterDeviceImpl)POSTerminal.GetInstance(this).GetDevice(POSTerminal.DeviceNamePrinter);
                Log.Debug("UseAAR", "printerDevice = " + printerDevice);
            });
            thread1.Start();
        }

        private void BtnOpen_Click(object sender, System.EventArgs e)
        {
            try
            {
                printerDevice.Open();
                tvLog.SetText(Resource.String.btn_open);
            }
            catch (Exception ex) 
            {
                tvLog.SetText(Resource.String.btn_already_open);
                ex.PrintStackTrace();
            }
        }

        private void BtnPrint_Click(object sender, System.EventArgs e)
        {
            tvLog.SetText(Resource.String.btn_print);

            string htmlContent = "<!DOCTYPE html>" +
                    "<html>" +
                    "<head>" +
                    "    <style type=\"text/css\">" +
                    "     * {" +
                    "        margin:0;" +
                    "        padding:0;" +
                    "     }" +
                    "    </style>" +
                    "</head>" +
                    "<body>" +
                    "Demo receipts<br />" +
                    "MERCHANT COPY<br />" +
                    "<hr/>" +
                    "MERCHANT NAME<br />" +
                    "SHXXXXXXCo.,LTD.<br />" +
                    "530310041315039<br />" +
                    "TERMINAL NO<br />" +
                    "50000045<br />" +
                    "OPERATOR<br />" +
                    "50000045<br />" +
                    "<hr />" +
                    "CARD NO<br />" +
                    "623020xxxxxx3994 I<br />" +
                    "ISSUER ACQUIRER<br />" +
                    "<br />" +
                    "TRANS TYPE<br />" +
                    "PAY SALE<br />" +
                    "PAY SALE<br />" +
                    "<hr/>" +
                    "DATE/TIME EXP DATE<br />" +
                    "2005/01/21 16:52:32 2099/12<br />" +
                    "REF NO BATCH NO<br />" +
                    "165232857468 000001<br />" +
                    "VOUCHER AUTH NO<br />" +
                    "000042<br />" +
                    "AMOUNT:<br />" +
                    "RMB:0.01<br />" +
                    "<hr/>" +
                    "BEIZHU<br />" +
                    "SCN:01<br />" +
                    "UMPR NUM:4F682D56<br />" +
                    "TC:EF789E918A548668<br />" +
                    "TUR:008004E000<br />" +
                    "AID:A000000333010101<br />" +
                    "TSI:F800<br />" +
                    "ATC:0440<br />" +
                    "APPLAB:PBOC DEBIT<br />" +
                    "APPNAME:PBOC DEBIT<br />" +
                    "AIP:7C00<br />" +
                    "CUMR:020300<br />" +
                    "IAD:07010103602002010A01000000000005DD79CB<br />" +
                    "TermCap:EOE1C8<br />" +
                    "CARD HOLDER SIGNATURE<br />" +
                    "I ACKNOWLEDGE SATISFACTORY RECEIPT OF RELATIVE GOODS/SERVICES<br />" +
                    "I ACKNOWLEDGE SATISFACTORY RECEIPT OF RELATIVE GOODS/SERVICES<br />" +
                    "I ACKNOWLEDGE SATISFACTORY RECEIPT OF RELATIVE GOODS/SERVICES<br />" +
                    "<br />" +
                    "Demo receipts,do not sign!<br />" +
                    "<br />" +
                    "<br />" +
                    "</body>" +
                    "</html>";

            Thread thread = new Thread(delegate ()
            {
                printerDevice.PrintHTML(htmlContent);
            });
            thread.Start();
            
        }

        private void BtnClose_Click(object sender, System.EventArgs e)
        {
            try
            {
                printerDevice.Close();
                tvLog.SetText(Resource.String.btn_close);

            }
            catch (Exception ex)
            {
                tvLog.SetText(Resource.String.btn_already_close);
                ex.PrintStackTrace();
            }
        }



        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}