using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Automatick.Core
{
    [Serializable]
    public class VSTicketType
    {
        public String ID
        {
            get;
            set;
        }
        public String Description
        {
            get;
            set;
        }
        public int MinQuantity
        {
            get;
            set;
        }
        public int MaxQuantity
        {
            get;
            set;
        }
        public int QuantityStep
        {
            get;
            set;
        }
        public String QuantityFormElementKey
        {
            get;
            set;
        }
        public Boolean HasPriceLevel
        {
            get
            {
                if (PriceLevels != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            set
            { }
        }
        public Boolean HasNeighborhoods
        {
            get
            {
                if (Neighborhoods != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            set
            { }
        }
        //public String PasswordFormName
        //{
        //    get;
        //    set;
        //}
        //public String PasswordPrompt
        //{
        //    get;
        //    set;
        //}


        public String DateId
        {
            get;
            set;
        }

        public String DateName
        {
            get;
            set;
        }

        public String Title
        {
            get;
            set;
        }

        public List<VSPriceLevel> PriceLevels
        {
            get;
            set;
        }

        public List<VSNeighborhood> Neighborhoods
        {
            get;
            set;
        }

        public VSTicketType(String id, String description, int minQuantity, int maxQuantity, int quantityStep, String quantityFormElementKey)
        {
            Neighborhoods = new List<VSNeighborhood>();
            PriceLevels = new List<VSPriceLevel>();
            this.ID = id;
            this.Description = description;
            this.MinQuantity = minQuantity;
            this.MaxQuantity = maxQuantity;
            this.QuantityStep = quantityStep;
            this.QuantityFormElementKey = quantityFormElementKey;
            //            this.HasPriceLevel = true;
            //this.PasswordFormName = String.Empty;
            //this.PasswordPrompt = String.Empty;
        }

        public VSTicketType(String id, String description, int minQuantity, int maxQuantity, int quantityStep, String quantityFormElementKey,string dateName,String dateId,string title)
        {
            Neighborhoods = new List<VSNeighborhood>();
            PriceLevels = new List<VSPriceLevel>();
            this.ID = id;
            this.Description = description;
            this.MinQuantity = minQuantity;
            this.MaxQuantity = maxQuantity;
            this.QuantityStep = quantityStep;
            this.QuantityFormElementKey = quantityFormElementKey;
            this.DateName = dateName;
            this.DateId = dateId;
            this.Title = title;
            //            this.HasPriceLevel = true;
            //this.PasswordFormName = String.Empty;
            //this.PasswordPrompt = String.Empty;
        }

        //public Boolean Is_Enabled(String DiscreteID, JToken discrete)
        //{
        //    Boolean result = false;

        //    try
        //    {
        //        if (discrete["public"] != null)
        //        {
        //            this.Discrete_Public = ((JValue)discrete["public"]).Value.ToString();
        //            JToken codes = discrete.SelectToken("codes", false);
        //            if (codes != null)
        //            {
        //                if (codes.Count() > 0)
        //                {
        //                    for (int i = 0; i < codes.Count(); i++)
        //                    {
        //                        if (codes.ElementAt(i)["code"] != null)
        //                        {
        //                            if (((JValue)codes.ElementAt(i)["code"]).Value.ToString() == DiscreteID)
        //                            {
        //                                result = true;
        //                                break;
        //                            }
        //                            else if (this.Discrete_Public == "Y")
        //                            {
        //                                result = true;
        //                                break;
        //                            }
        //                            else if (String.IsNullOrEmpty(DiscreteID))
        //                            {
        //                                result = true;
        //                                break;
        //                            }
        //                        }
        //                        else if (this.Discrete_Public == "Y")
        //                        {
        //                            result = true;
        //                            break;
        //                        }
        //                    }
        //                }
        //                else if (this.Discrete_Public == "Y")
        //                {
        //                    result = true;                            
        //                }                        
        //            }
        //            else
        //            {
        //                if (this.Discrete_Public =="Y")
        //                {
        //                    result = true;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        result = false;
        //    }

        //    return result;
        //}
    }
}
