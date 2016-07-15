using System.Collections.Generic;
using System;
using Nancy;
using Nancy.ViewEngines.Razor;

namespace HairSalon
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
        List<Stylist> allStylists = Stylist.GetAll();
        List<Client> allClients = Client.GetAll();
        Dictionary<string, object> model = new Dictionary<string, object> {};
        model.Add("stylists", allStylists);
        model.Add("clients", allClients);
        return View["index.cshtml", model];
      };
      Get["/stylists/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        var SelectedStylist = Stylist.Find(parameters.id);
        var StylistClients = SelectedStylist.GetClients();
        model.Add("stylist", SelectedStylist);
        model.Add("clients", StylistClients);
        return View["stylist.cshtml", model];
      };
      Post["/stylists/new"] = _ => {
        Stylist newStylist = new Stylist(Request.Form["stylistName"]);
        newStylist.Save();
        List<Stylist> allStylists = Stylist.GetAll();
        List<Client> allClients = Client.GetAll();
        Dictionary<string, object> model = new Dictionary<string, object> {};
        model.Add("stylists", allStylists);
        model.Add("clients", allClients);
        return View["index.cshtml", model];
      };
      Post["/stylists/{id}/clients/new"] = parameters => {
        var SearchId = parameters.id;
        Client newClient = new Client(Request.Form["clientName"], SearchId);
        newClient.Save();
        Dictionary<string, object> model = new Dictionary<string, object>();
        var SelectedStylist = Stylist.Find(SearchId);
        var StylistClients = SelectedStylist.GetClients();
        model.Add("stylist", SelectedStylist);
        model.Add("clients", StylistClients);
        return View["stylist.cshtml", model];
      };
      Delete["/stylists/delete/"] = _ => {
        int searchId = Request.Form["stylistName"];
        Stylist SelectedStylist = Stylist.Find(searchId);
        SelectedStylist.Delete();
        List<Stylist> allStylists = Stylist.GetAll();
        List<Client> allClients = Client.GetAll();
        Dictionary<string, object> model = new Dictionary<string, object> {};
        model.Add("stylists", allStylists);
        model.Add("clients", allClients);
        return View["index.cshtml", model];
      };
      Delete["/clients/delete"] = p_ => {
        int searchId = Request.Form["clientName"];
        Client SelectedClient = Client.Find(searchId);
        SelectedClient.Delete();
        return View["removed_client.cshtml", SelectedClient];
      };
      // Get["/clients/edit/"] = _ => {
      //   return View["client_edit.cshtml"];
      // };
      // Patch["/clients/edit"] = _ => {
      //   int searchId = Request.Form["clientName"];
      //   Client SelectedClient = Client.Find(searchId);
      //   SelectedClient.Update(Request.Form["newName"]);
      //   List<Stylist> allStylists = Stylist.GetAll();
      //   List<Client> allClients = Client.GetAll();
      //   Dictionary<string, object> model = new Dictionary<string, object> {};
      //   model.Add("stylists", allStylists);
      //   model.Add("clients", allClients);
      //   return View["stylist.cshtml", model];
      // }
    }
  }
}
