using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;
using System.IO;

namespace Automatick.Core
{
    [Serializable]
    public class Parameter
    {
        public AutoResetEvent parameterentered = new AutoResetEvent(false);
        List<AXSSection> _eventDates;
        Boolean _ifeventDatesBind;
        Boolean _ifShown;
  

        public List<AXSSection> EventDates
        {
            get { return _eventDates; }
            set { _eventDates = value; }
        }

        public Boolean IfEventDatesBind
        {
            get { return _ifeventDatesBind; }
            set { _ifeventDatesBind = value; }
        }

        public Boolean IfShown
        {
            get { return _ifShown; }
            set { _ifShown = value; }
        }

        public Parameter()
        {
           
        }

        public Parameter(List<AXSSection> eventDates)
        {
            this._eventDates = eventDates;
      
        }
    }
}
