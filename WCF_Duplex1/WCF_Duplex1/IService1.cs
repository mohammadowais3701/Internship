﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WCF_Duplex1
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IHelloWorldServiceCallback))]
    public interface IService1
    {
        [OperationContract]
        void ReceiveFile(String[] Text);
        [OperationContract]
        void Registered();
    }
    public interface IHelloWorldServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void greetings(string msg);
    }

}
