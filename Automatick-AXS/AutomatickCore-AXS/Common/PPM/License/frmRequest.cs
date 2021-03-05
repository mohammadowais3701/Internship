using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace LicenseAPI
{
    public partial class frmRequestPPPM : Form
    {
        String _ProccessorID;
        String _HarddiskSerial;
        String _ApplicationPrefix;
        String _filePath;
        XmlDocument _xmlServiceURL;
        public frmRequestPPPM(String ProccessorID, String HarddiskSerial, String ApplicationPrefix, XmlDocument xmlServiceURL, String filePath)
        {
            _ProccessorID = ProccessorID;
            _HarddiskSerial = HarddiskSerial;
            _ApplicationPrefix = ApplicationPrefix;
            _filePath = filePath;
            _xmlServiceURL = xmlServiceURL;
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private bool customXertificateValidation(object sender, X509Certificate cert, X509Chain chain, System.Net.Security.SslPolicyErrors error)
        {
            return true;
        }
        private void btnRequest_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtName.Text.Trim()))
            {
                MessageBox.Show("Please provide the Name");
                return;
            }
            if (String.IsNullOrEmpty(txtEmail.Text.Trim()))
            {
                MessageBox.Show("Please provide the Email");
                return;
            }
            try
            {
                ServicePointManager.ServerCertificateValidationCallback += new System.Net.Security.RemoteCertificateValidationCallback(customXertificateValidation);
                String SerailWebServiceURL = _xmlServiceURL.SelectSingleNode("//serviceURL/REQUESTSERIAL").InnerText.Trim();
                

                System.Net.WebClient webClient = new System.Net.WebClient();
                String result = webClient.DownloadString(SerailWebServiceURL + "/RequestLicense?Name=" + txtName.Text + "&Email=" + txtEmail.Text + "&ProccessorID=" + _ProccessorID + "&HarddiskSerial=" + _HarddiskSerial + "&ApplicationPrefix=" + _ApplicationPrefix);

                LicenseCorePPM lic = new LicenseCorePPM(_filePath, false);
                lic.WriteLicenseFile(result);
                MessageBox.Show("Your request has been sent");
                this.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Your request has not been sent. Please try again later.");
            }
            
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }
    }
}
