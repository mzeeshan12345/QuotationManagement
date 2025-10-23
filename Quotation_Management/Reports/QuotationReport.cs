using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;

namespace Quotation_Management.Reports
{
    public partial class QuotationReport 
    {
        public QuotationReport()
        {
            InitializeComponent();
        }

        //private void pictureBox1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        //{
        //    XRPictureBox xrBox = sender as XRPictureBox;
        //    string base64String = this.GetCurrentColumnValue("FilePath") as string;
        //    Image img = ByteArrayToImage(Convert.FromBase64String(base64String));
        //    xrBox.Image = img;
        //}

        //public Image ByteArrayToImage(byte[] byteArrayIn)
        //{
        //    MemoryStream ms = new MemoryStream(byteArrayIn);
        //    Image returnImage = Image.FromStream(ms);
        //    return returnImage;
        //}

    }
}
