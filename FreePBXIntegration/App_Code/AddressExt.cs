using PX.Data;
using PX.Objects.CR.MassProcess;
using PX.Objects.CR;
using PX.Objects.CS;
using PX.Objects;
using System.Collections.Generic;
using System;

namespace PX.Objects.CR
{
      [PXNonInstantiatedExtension]
  public class CR_Address_ExistingColumn : PXCacheExtension<PX.Objects.CR.Address>
  {
      #region CountryID  
        [PXMergeAttributes(Method = MergeMethod.Append)]
[PXDefault("US")]
      public string CountryID { get; set; }
      #endregion
  }
}