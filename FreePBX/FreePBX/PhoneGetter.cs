using System;
using PX.Data;
using PX.Data.EP;
using PX.Objects.AP;
using PX.Objects.CR.MassProcess;
using PX.Objects.CS;
using PX.Objects.CR;
using PX.TM;
using PX.SM;
using PX.Objects.EP;
using System.Collections;
using System.Collections.Generic;
using FreePBXIntegration;
using System.Linq;

namespace Hackathon
{
  public class PhoneGetter : PXGraph<PhoneGetter>
  {

    public PXSave<MasterTable> Save;
    public PXCancel<MasterTable> Cancel;


    public PXSelect<MasterTable> MasterView;
    public PXSelect<PhoneCallerAudit> Audit;
    bool savingRequired = true;
    public IEnumerable masterView()
    {
    MasterTable mt=new MasterTable();

  if(PXView.Filters !=null && PXView.Filters.Length > 0) 
    {
    
    PhoneCallerAudit pca=new PhoneCallerAudit();
    
   for (int i=0;i<PXView.Filters.Length;i++)
    {
      var fr=PXView.Filters[i];
      if(fr.DataField=="PhoneNbr")
      {
         pca.PhoneNubmer =fr.Value.ToString();
    
    mt.PhoneNbr=pca.PhoneNubmer ;
      }
      if(fr.DataField=="CallerID")
      {
        pca.CallerID=fr.Value.ToString();
    mt.CallerID=pca.CallerID;
      }
  }
  PXResultset<BAccount> resultset=PXSelectJoin<BAccount, 
       InnerJoin<Contact, On<Contact.bAccountID, Equal<BAccount.bAccountID>>>,
      Where<Contact.phone1, Equal<Required<Contact.phone1>>>>.Select(this, pca.PhoneNubmer );
  if(resultset.Count>0)
  {
  PXResult<BAccount, Contact> res= (PXResult<BAccount, Contact> )resultset[0];
  Contact contact=res;
  BAccount bacct= res;
  pca.ContactID=contact.ContactID;
  if(bacct!=null)
  {
    mt.AcctName=bacct.AcctName;
  }
}
  if(savingRequired)
  {Audit.Insert(pca);
    //Caches[typeof(PhoneCallerAudit)].Persist(PXDBOperation.Insert); 
  Save.Press();
   savingRequired=false;
  }
  }
    List<MasterTable > result=new List<MasterTable >();
    result.Add(mt);
    return result;
  }
    
public virtual void MasterTable_RowPersisting(PXCache sender, PXRowPersistingEventArgs e)
    {
    e.Cancel=true;
    }
    [Serializable]
    public class MasterTable : IBqlTable
    {
    #region PhoneNbr
       public abstract class phoneNbr
       {}
    [PXString(50, IsUnicode = true, IsKey=true)]
    [PXUIField(DisplayName = "Phone", Visibility = PXUIVisibility.SelectorVisible)]
    //[PhoneValidation]

       public virtual string PhoneNbr
      {get; set;}
        #endregion

   #region CallerID
               public abstract class callerID
       {}

    [PXString(32, IsUnicode = true, IsKey=true)]
    [PXUIField(DisplayName = "Caller ID")]

       public virtual string CallerID
      {get; set;}
        #endregion
        

#region AcctName
    public abstract class acctName : PX.Data.IBqlField
    {
    }
        
    [PXString(60, IsUnicode = true, IsKey=true)]
    [PXUIField(DisplayName = "Account Name", Visibility = PXUIVisibility.SelectorVisible)]
    public virtual String AcctName
    {
      get;set;
    }
    #endregion
    }
  }
}