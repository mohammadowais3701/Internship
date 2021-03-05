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
    public partial class frmRequest : Form
    {
        String _ProccessorID;
        String _HarddiskSerial;
        String _ApplicationPrefix;
        String _filePath;
        XmlDocument _xmlServiceURL;
        public frmRequest(String ProccessorID, String HarddiskSerial, String ApplicationPrefix, XmlDocument xmlServiceURL, String filePath)
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
            Application.Exit();
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
                //SerailWebServiceURL = SerailWebServiceURL.Replace("http://50.31.20.71/", "https://95.211.166.125/");

                System.Net.WebClient webClient = new System.Net.WebClient();
                //String result = webClient.DownloadString(SerailWebServiceURL + "/RequestLicense?Name=" + txtName.Text + "&Email=" + txtEmail.Text + "&ProccessorID=" + _ProccessorID + "&HarddiskSerial=" + _HarddiskSerial + "&ApplicationPrefix=" + _ApplicationPrefix);

                String result = String.Empty;

                try
                {
                    result = webClient.DownloadString(SerailWebServiceURL + "/RequestLicense?Name=" + txtName.Text + "&Email=" + txtEmail.Text + "&ProccessorID=" + _ProccessorID + "&HarddiskSerial=" + _HarddiskSerial + "&ApplicationPrefix=" + _ApplicationPrefix);
                }
                catch (Exception)
                {
                    webClient = new System.Net.WebClient();
                    _xmlServiceURL = null;
                    _xmlServiceURL = LicenseCore.GetServiceURLAlternate();
                    SerailWebServiceURL = _xmlServiceURL.SelectSingleNode("//serviceURL/REQUESTSERIAL").InnerText.Trim();

                    result = webClient.DownloadString(SerailWebServiceURL + "/RequestLicense?Name=" + txtName.Text + "&Email=" + txtEmail.Text + "&ProccessorID=" + _ProccessorID + "&HarddiskSerial=" + _HarddiskSerial + "&ApplicationPrefix=" + _ApplicationPrefix);
                }

                LicenseCore lic = new LicenseCore(_filePath, false);
                lic.WriteLicenseFile(result);
                MessageBox.Show("Your request has been sent");
                Application.Exit();
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
