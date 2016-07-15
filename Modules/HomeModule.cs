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
      Delete["/clients/delete/all"] = _ => {
        Client.DeleteAll();
        return View["removed_clients.cshtml"];
      };
      Delete["/stylists/{id}/clients/delete"] = parameters => {
        int deleteId = Request.Form["clientName"];
        Client SelectedClient = Client.Find(deleteId);
        SelectedClient.Delete();
        Dictionary<string, object> model = new Dictionary<string, object>();
        var searchId = parameters.id;
        var SelectedStylist = Stylist.Find(searchId);
        var StylistClients = SelectedStylist.GetClients();
        model.Add("stylist", SelectedStylist);
        model.Add("clients", StylistClients);
        return View["stylist.cshtml", model];
      };
      Post["/stylists/edit/"] = _ => {
        int searchId = Request.Form["stylistName"];
        var selectedStylist = Stylist.Find(searchId);
        return View["stylist_edit.cshtml", selectedStylist];
      };
      Post["/stylists/{id}/clients/edit"] = parameters => {
        int stylistId = parameters.id;
        var SelectedStylist = Stylist.Find(stylistId);
        int clientId = Request.Form["clientName"];
        Client SelectedClient = Client.Find(clientId);
        Dictionary<string, object> modelEdit = new Dictionary<string, object>();
        modelEdit.Add("stylist", SelectedStylist);
        modelEdit.Add("client", SelectedClient);
        return View["client_edit.cshtml", modelEdit];
      };
      Patch["/stylists/{id}/edit"] = parameters => {
        int stylistId = parameters.id;
        string newName = Request.Form["newName"];
        var SelectedStylist = Stylist.Find(stylistId);
        SelectedStylist.Update(newName);
        Dictionary<string, object> model = new Dictionary<string, object>();
        var StylistClients = SelectedStylist.GetClients();
        model.Add("stylist", SelectedStylist);
        model.Add("clients", StylistClients);
        return View["stylist.cshtml", model];
      };
      Patch["/stylists/{id}/clients/edit"] = parameters => {
        int editId = Request.Form["clientEditId"];
        string editName = Request.Form["newName"];
        var selectedClient = Client.Find(editId);
        selectedClient.Update(editName);
        Dictionary<string, object> model = new Dictionary<string, object>();
        var SelectedStylist = Stylist.Find(parameters.id);
        var StylistClients = SelectedStylist.GetClients();
        model.Add("stylist", SelectedStylist);
        model.Add("clients", StylistClients);
        return View["stylist.cshtml", model];
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
