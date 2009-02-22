﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dropthings.DataAccess;
using Dropthings.Business;
using System.Web.UI.HtmlControls;
using Dropthings.Business.Workflows;
using Dropthings.Business.Container;
using Dropthings.Business.Workflows.TabWorkflows;
using System.Workflow.Runtime;
using System.Web.Security;

public partial class TabPage : System.Web.UI.UserControl
{
    private const string WIDGET_CONTAINER = "WidgetContainer.ascx";
    private string[] updatePanelIDs = new string[] { "LeftUpdatePanel", "MiddleUpdatePanel", "RightUpdatePanel" };

    public Dropthings.DataAccess.Page CurrentPage { get; set; }
    public List<Dropthings.DataAccess.Page> Pages { get; set; }
    
    protected override void CreateChildControls()
    {
        base.CreateChildControls();       
    }

    public void LoadTabs(List<Dropthings.DataAccess.Page> pages, Dropthings.DataAccess.Page page)
    {
        this.CurrentPage = page;
        this.Pages = pages;
        this.SetupTabs();
    }

    private void SetupTabs()
    {
        tabList.Controls.Clear();

        foreach (var page in Pages)
        {
            var li = new HtmlGenericControl("li");
            li.ID = "Tab" + page.ID.ToString();
            li.Attributes["class"] = "tab " + (page.ID == CurrentPage.ID ? "activetab" : "inactivetab");

            var liWrapper = new HtmlGenericControl("div");
            li.Controls.Add(liWrapper);
            liWrapper.Attributes["class"] = "tab_wrapper";

            if (page.ID == CurrentPage.ID)
            {
                var tabTextDiv = new HtmlGenericControl("span");
                tabTextDiv.InnerText = page.Title;
                liWrapper.Controls.Add(tabTextDiv);
            }
            else
            {
                var tabLink = new HyperLink { Text = page.Title, NavigateUrl = "/?" + page.TabName() };
                liWrapper.Controls.Add(tabLink);
            }
            tabList.Controls.Add(li);
        }

        var addNewTabLinkButton = new LinkButton();
        addNewTabLinkButton.ID = "AddNewPage";
        addNewTabLinkButton.CssClass = "newtab_add newtab_add_block";
        addNewTabLinkButton.Click += new EventHandler(addNewTabLinkButton_Click);
        var li2 = new HtmlGenericControl("li");
        li2.Attributes["class"] = "newtab";
        li2.Controls.Add(addNewTabLinkButton);
        tabList.Controls.Add(li2);

        addNewTabLinkButton.Click += new EventHandler(addNewTabLinkButton_Click);

        // ILIAS 1/13/2009: Turning it off because new users are not getting the "Add New Page" link
        //if (Roles.Enabled)
        //{
        //    if (!Roles.IsUserInRole("ChangePageSettings"))
        //        addNewTabLinkButton.Visible = false;
        //}
    }

    public void RedirectToTab(Dropthings.DataAccess.Page page)
    {
        Response.Redirect('?' + page.TabName());
    }

    protected void addNewTabLinkButton_Click(object sender, EventArgs e)
    {
        var response = RunWorkflow.Run<AddNewTabWorkflow,AddNewTabWorkflowRequest,AddNewTabWorkflowResponse>(
                            new AddNewTabWorkflowRequest { LayoutType = "1", UserName = Profile.UserName }
                        );

        RedirectToTab(response.NewPage);
    }
    
    protected void Page_Load(object sender, EventArgs e)
    {

    }

}
