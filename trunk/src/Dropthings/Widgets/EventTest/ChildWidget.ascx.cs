﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dropthings.Widget.Framework;

public partial class Widgets_EventTest_ChildWidget : System.Web.UI.UserControl, IWidget
{
    private IWidgetHost _Host;
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    #region IWidget Members

    public new void Init(IWidgetHost host)
    {
        _Host = host;
        host.EventBroker.AddListener(this);
    }

    public void ShowSettings()
    {
        
    }

    public void HideSettings()
    {
        
    }

    public void Expanded()
    {
        
    }

    public void Collasped()
    {
        
    }

    public void Maximized()
    {
        
    }

    public void Restored()
    {
        
    }

    public void Closed()
    {
        
    }

    #endregion

    #region IEventListener Members

    public void AcceptEvent(object sender, EventArgs e)
    {
        if (sender != this && e is MasterChildEventArgs)
        {
            var arg = e as MasterChildEventArgs;
            this.Received.Text = arg.Who + " says, " + arg.Message;
            _Host.Refresh(this);
        }
    }

    #endregion

    protected void Raise_Clicked(object sender, EventArgs e)
    {
        MasterChildEventArgs args = new MasterChildEventArgs("Child " + _Host.WidgetInstance.Id, this.Message.Text);
        _Host.EventBroker.RaiseEvent(this, args);
    }
}