using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using PX.Data;
using PX.Objects.CR;
using System.Web.UI.WebControls;

public partial class Api_PhoneLookup : System.Web.UI.Page
{
    public String contact_name = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        ContactMaint graph = PXGraph.CreateInstance<ContactMaint>();
        var search_phone = Request.QueryString["phone"];
        var rows = PXSelect<Contact,
            Where<Contact.phone1,
                Equal<Required<Contact.phone1>>>>.Select(graph, search_phone);
        foreach (Contact item in rows)
        {
            contact_name = item.DisplayName;
        }

    }
}