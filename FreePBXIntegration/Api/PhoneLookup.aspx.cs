using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using PX.Data;
using PX.Objects.CR;
using PX.Objects.AR;
using PX.Objects.EP;
using System.Web.UI.WebControls;
using Hackathon;
using FreePBXIntegration;

public class PhoneAudit : PXGraph<PhoneAudit>
{

    public PXSave<PhoneCallerAudit> Save;
    public PXCancel<PhoneCallerAudit> Cancel;


    public PXSelect<PhoneCallerAudit> Audit;
}
public partial class Api_PhoneLookup : System.Web.UI.Page
{
    public String contactName = "";
    public String accountCD = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        ContactMaint graph = PXGraph.CreateInstance<ContactMaint>();
        var search_phone = Request.QueryString["phone"];
        var default_callID = Request.QueryString["cid"];
        BusinessAccountMaint baccount_graph = PXGraph.CreateInstance<BusinessAccountMaint>();

	Int32? contactId = null;
	Guid? contactGuid = null;

        var contacts = PXSelect<Contact,
            Where<Contact.phone1,
                Equal<Required<Contact.phone1>>,
            Or<Contact.phone2,
                Equal<Required<Contact.phone2>>,
            Or<Contact.phone3,
                Equal<Required<Contact.phone3>>>>>>.Select(graph, search_phone, search_phone, search_phone);
        // For now, just grab the first result
        if (contacts.Count() > 0)
        {
            var contact = ((Contact)contacts.First());
            contactName = contact.DisplayName;
            var customers = PXSelect<BAccount, 
                Where<BAccount.bAccountID, 
                    Equal<Required<Contact.bAccountID>>>>.Select(graph, contact.BAccountID);
            // There should only be one bAccount that matches
            if (customers.Count() > 0)
            { 
                accountCD = ((BAccount)customers.First()).AcctCD;
            }
			
	    contactId = contact.ContactID;
	    contactGuid = contact.NoteID;
			
        } else
        {
            var contact = new Contact();
            contact.LastName = default_callID;
            contact.Phone1 = search_phone;
            contact = graph.Contact.Insert(contact);
            //graph.Contact.Cache.Persist(PXDBOperation.Insert);
            graph.Save.Press();
            contactName = default_callID;
			
	    contactId = graph.Contact.Current.ContactID;
	    contactGuid = graph.Contact.Current.NoteID;
        }

        // Save an audit record of the phone call
        var audit_graph = PXGraph.CreateInstance<PhoneAudit>();
        var audit = new PhoneCallerAudit();
        audit.PhoneNubmer = search_phone;
        audit.CallerID = contactName;
	audit.ContactID = contactId;
        audit_graph.Audit.Insert(audit);
	audit_graph.Save.Press();

            CRActivityMaint activity_graph = PXGraph.CreateInstance<CRActivityMaint>();
            CRActivity activity = new CRActivity();
            activity = activity_graph.Activities.Insert(activity);
            activity.Type = "P";
            activity.Subject = "Inbound Call";
            activity.OwnerID = activity_graph.Accessinfo.UserID;
	    activity.ContactID = contactId;
            activity.RefNoteID = contactGuid ;
            activity_graph.Activities.Update(activity);
            activity_graph.Save.Press();
		
        var json = string.Format("{{\"baccount\": \"{0}\", \"contact\": \"{1}\"}}", accountCD, contactName);

        Response.Clear();
        Response.ContentType = "application/json; charset=utf-8";
        Response.Write(json);
        Response.End();

    }
}