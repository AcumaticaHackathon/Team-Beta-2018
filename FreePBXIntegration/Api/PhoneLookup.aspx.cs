using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using PX.Data;
using PX.Objects.CR;
using PX.Objects.AR;
using System.Web.UI.WebControls;

public partial class Api_PhoneLookup : System.Web.UI.Page
{
    public String contactName = "";
    public String accountCD = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        ContactMaint graph = PXGraph.CreateInstance<ContactMaint>();
        var search_phone = Request.QueryString["phone"];
        BusinessAccountMaint baccount_graph = PXGraph.CreateInstance<BusinessAccountMaint>();

        var contacts = PXSelect<Contact,
            Where<Contact.phone1,
                Equal<Required<Contact.phone1>>>>.Select(graph, search_phone);
        // For now, just grab the first result
        var contact = ((Contact)contacts.First());
        contactName = contact.DisplayName;

        var customers = PXSelect<BAccount, Where<BAccount.bAccountID, Equal<Required<Contact.bAccountID>>>>.Select(graph, contact.BAccountID);
        // There should only be one bAccount that matches
        accountCD = ((BAccount)customers.First()).AcctCD;

        var json = string.Format("[\"{0}\", \"{1}\"]", accountCD, contactName);

        Response.Clear();
        Response.ContentType = "application/json; charset=utf-8";
        Response.Write(json);
        Response.End();

    }
}