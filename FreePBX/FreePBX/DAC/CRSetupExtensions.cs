using PX.Data;
using PX.Objects.CM;
using PX.Objects.CR;
using PX.Objects.CS;
using PX.Objects.EP;
using PX.Objects;
using System.Collections.Generic;
using System;

namespace PX.Objects.CR
{
  public class CRSetupExt : PXCacheExtension<PX.Objects.CR.CRSetup>
  {
    #region UsrVOIPUrl
    [PXDBString(255)]
    [PXUIField(DisplayName="VOIP URL")]

    public virtual string UsrVOIPUrl { get; set; }
    public abstract class usrVOIPUrl : IBqlField { }
    #endregion
  }
}