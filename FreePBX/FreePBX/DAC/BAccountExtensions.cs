using PX.Data.EP;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Data;
using PX.Objects.AP;
using PX.Objects.AR;
using PX.Objects.CR.MassProcess;
using PX.Objects.CR;
using PX.Objects.CS;
using PX.Objects.EP;
using PX.Objects.GL;
using PX.Objects.TX;
using PX.Objects;
using PX.SM;
using PX.TM;
using System.Collections.Generic;
using System.Diagnostics;
using System;

namespace PX.Objects.CR
{
  public class BAccountExt : PXCacheExtension<PX.Objects.CR.BAccount>
  {
    #region UsrPhoneExtension
    [PXDBString(5)]
    [PXUIField(DisplayName="Phone Extension")]

    public virtual string UsrPhoneExtension { get; set; }
    public abstract class usrPhoneExtension : IBqlField { }
    #endregion

    #region UsrSIPCredential
    [PXDBString(50)]
    [PXUIField(DisplayName="SIP Credential")]

    public virtual string UsrSIPCredential { get; set; }
    public abstract class usrSIPCredential : IBqlField { }
    #endregion
  }
}