using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace AccessRights
{
    [Serializable]
    public class ApplicationPermissions
    {
        String URL = String.Empty;
        public List<AccessList> AllAccessList
        {
            get;
            set;
        }
        public String LicenseID
        {
            get;
            set;
        }
        public ApplicationPermissions(String licenseID, String URLPermission)
        {
            this.LicenseID = licenseID;
            //URL = "http://localhost:49625/SerialInfo/ValidateLicense.asmx/getApplicationPermissions?LicenseID=" + LicenseID;
            URL = URLPermission + licenseID;
            LoadAccessList();
        }
        
        void LoadAccessList()
        {
            try
            {
                WebClient wc = new WebClient();
                byte[] data = wc.DownloadData(URL);
                Stream st = new MemoryStream(data);
                st.Position = 0;

                BinaryFormatter formatter = new BinaryFormatter();
                object o = formatter.Deserialize(st);
                AllAccessList = (List<AccessList>)o;
            }
            catch (Exception)
            {
                AllAccessList = new List<AccessList>();
            }
        }
        public void LoadAccessList(String strURL)
        {
            try
            {
                WebClient wc = new WebClient();
                byte[] data = wc.DownloadData(strURL + LicenseID);
                Stream st = new MemoryStream(data);
                st.Position = 0;

                BinaryFormatter formatter = new BinaryFormatter();
                object o = formatter.Deserialize(st);
                AllAccessList = (List<AccessList>)o;
            }
            catch (Exception)
            {
                AllAccessList = new List<AccessList>();
            }
        }
        public void ApplyPemissions(Form targetForm)
        {
            try
            {
                IEnumerable<AccessList> allControlsAccesses = AllAccessList.Where(p => p.form == targetForm.Name);

                foreach (AccessList obj in allControlsAccesses)
                {
                    try
                    {
                        string _controlName = obj.name;
                        string _controlVisibility = obj.access;

                        Control[] ctls = targetForm.Controls.Find(_controlName, true);
                        if (ctls.Length > 0)
                        {
                            Control ctl = ctls.First();

                            Type _type = ctl.GetType();
                            PropertyInfo _typePropertyInfo = _type.GetProperty("Visible");
                            if (_typePropertyInfo!=null)
                            {
                                object abc = _controlVisibility;
                                abc = Convert.ChangeType(abc, Type.GetType(_typePropertyInfo.PropertyType.FullName));
                                _typePropertyInfo.SetValue(ctl, abc, null);
                            }
                            else
                            {
                                ctl.Visible = bool.Parse(_controlVisibility);
                            }
                            
                        }
                        else if (ctls.Length <= 0)
                        {
                            ToolStripItem[] tis = targetForm.MainMenuStrip.Items.Find(_controlName, true);
                            if (tis.Length > 0)
                            {
                                ToolStripItem ctl = tis.First();
                               
                                Type _type = tis.GetType();
                                PropertyInfo _typePropertyInfo = _type.GetProperty("Visible");

                                if (_typePropertyInfo != null)
                                {
                                    object abc = _controlVisibility;
                                    abc = Convert.ChangeType(abc, Type.GetType(_typePropertyInfo.PropertyType.FullName));
                                    _typePropertyInfo.SetValue(ctl, abc, null);
                                }
                                else
                                {
                                    ctl.Visible = bool.Parse(_controlVisibility);
                                }
                            }
                        }

                    }
                    catch (Exception)
                    {
                    }

                }
            }
            catch { }
        }       
    }
}
