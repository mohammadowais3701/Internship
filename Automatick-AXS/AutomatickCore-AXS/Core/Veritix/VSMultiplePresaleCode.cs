using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Automatick.Core
{
    [Serializable]
    public class VSMultiplePresaleCode
    {
        #region variables
             String _PresaleCode;
            int _UsedPresaleCodeCount;
            int _TotalPresaleCodeCount;
            bool _IfTicketBought;
            bool _IfUsing;
            static int  _Counter;
            static int _PresaleCurrentCount;
        #endregion

        #region Property
            public bool IfUsing
            {
                get { return _IfUsing; }
                set { _IfUsing = value; }
            }

            public String PresaleCode
            {
                get { return _PresaleCode; }
                set { _PresaleCode = value; }
            }

            public int UsedPresaleCodeCount
            {
                get { return _UsedPresaleCodeCount; }
                set { _UsedPresaleCodeCount = value; }
            }

            public int TotalPresaleCodeCount
            {
                get { return _TotalPresaleCodeCount; }
                set { _TotalPresaleCodeCount = value; }
            }
            public bool IfTicketBought
            {
                get { return _IfTicketBought; }
                set { _IfTicketBought = value; }
            }
            #endregion

        #region constructor
            public VSMultiplePresaleCode()
            { 
            }

            public VSMultiplePresaleCode(string presalecode)
            {
                this._PresaleCode = presalecode;
                this._TotalPresaleCodeCount = 0;  //use code unlimited times
                this._UsedPresaleCodeCount = 0; //
                this._IfTicketBought = false;
                this.IfUsing = false;
            }

            public VSMultiplePresaleCode(string presalecode, int totalpresalecount, int usedpresalecodecount)
            {
                this._PresaleCode = presalecode;
                this._TotalPresaleCodeCount = totalpresalecount;  //use code unlimited times
                this._UsedPresaleCodeCount = usedpresalecodecount; //
                this._IfTicketBought = false;
                this.IfUsing = false;
            }

            public VSMultiplePresaleCode(string presalecode, int totalpresalecount)
            {
                this._PresaleCode = presalecode;
                this._TotalPresaleCodeCount = totalpresalecount;  //use code unlimited times
                this._UsedPresaleCodeCount = 0; //
                this._IfTicketBought = false;
                this.IfUsing = false;
            }
            #endregion

        #region Methods
            //-----assign presale code 
            public static VSMultiplePresaleCode GetNextPresale(BindingList<VSMultiplePresaleCode> mpcList)
            {
                VSMultiplePresaleCode mpc = null;

                Retry:
                try
                {
                    if (_Counter >= mpcList.Count)
                    {
                        if (_PresaleCurrentCount.Equals(mpcList.Count)) _PresaleCurrentCount = 0;
                        do
                        {
                            mpc = mpcList[_PresaleCurrentCount];
                            if ((mpc.UsedPresaleCodeCount < mpc.TotalPresaleCodeCount) && (mpc.IfUsing))
                            {
                                mpc.UsedPresaleCodeCount++;
                                _PresaleCurrentCount++;
                                return mpc;
                            }
                            else if (mpc.TotalPresaleCodeCount.Equals(0))
                            {
                                _PresaleCurrentCount++;
                                return mpc;
                            }
                            else _PresaleCurrentCount++;
                            if (_PresaleCurrentCount.Equals(mpcList.Count)) _PresaleCurrentCount = 0;
                            if (!mpc.IfUsing) break;

                        }
                        while (true);
                    }
                    do
                    {
                        if (mpcList.Count > 0)
                        {
                            mpc = mpcList[_PresaleCurrentCount];
                            if (!mpc.IfUsing)
                            {
                                mpc.IfUsing = true;
                                mpc.UsedPresaleCodeCount++;
                                _PresaleCurrentCount++;
                                _Counter++;
                                return mpc;
                            }
                            else
                            {
                                _PresaleCurrentCount++;
                                _Counter++;
                            }
                        }
                    } while (!mpc.IfUsing);
                }
                catch (Exception)
                {
                    _PresaleCurrentCount = 0;
                    goto Retry;
                }

                return mpc;
            } 
            //----total presale codes
            public int totalPresaleCount(int totalPresaleCodeCount)
            {
                totalPresaleCodeCount += totalPresaleCodeCount;
                return totalPresaleCodeCount;
            }
      
        
        //----if presale count contain zero then it should use unlimited time

            public static Boolean ContainZero(BindingList<VSMultiplePresaleCode> mpcList)
        {
             IEnumerable<bool> b= mpcList.Select(p=>p.TotalPresaleCodeCount.Equals(0));
             if (b.Contains(true))
                 return true;
             else
                 return false;
        }
            //public static Boolean ReleasePresaleCode(BindingList<VSMultiplePresaleCode> mpcList, string presalecode, bool ifbought)
                public static Boolean ReleasePresaleCode(BindingList<VSMultiplePresaleCode> mpcList, string presalecode)
            {
                try
                {
                    VSMultiplePresaleCode mpc = mpcList.Single(p => p.PresaleCode == presalecode);
                    //if (ifbought)
                    //{
                    //    if (mpc.UsedPresaleCodeCount < mpc.TotalPresaleCodeCount)
                    //        mpc.UsedPresaleCodeCount++;
                    //    return true;
                    //}
                    //else
                    {
                        if(mpc.UsedPresaleCodeCount>0)
                        mpc.UsedPresaleCodeCount--;
                        return false;
                    }
                }
                catch
                {
                    return false; 
                }               
           
            }
        //----if searches are more than presale code 
#endregion      
    }
}
