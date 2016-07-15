using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace HairSalon
{
  public class StylistTest : IDisposable
  {
    public StylistTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=hair_salon_test;Integrated Security=SSPI;";
    }

    public void Dispose()
    {
      Client.DeleteAll();
      Stylist.DeleteAll();
    }
    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      //Arrange, Act
      int result = Stylist.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }
    [Fact]
    public void Test_Equal_ReturnsTrueIfNamesAreTheSame()
    {
      //Arrange, Act
      Stylist firstStylist = new Stylist("Nancy");
      Stylist secondStylist = new Stylist("Nancy");

      //Assert
      Assert.Equal(firstStylist, secondStylist);
    }
    [Fact]
    public void Test_NewStylist_SavesToDatabase()
    {
      //Arrange
      Stylist testStylist = new Stylist("Nancy");

      //Act
      testStylist.Save();
      List<Stylist> result = Stylist.GetAll();
      List<Stylist> testList = new List<Stylist>{testStylist};


      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_FindStylist_FindsStylistInDatabase()
    {
      //Arrange
      Stylist testStylist = new Stylist("Nancy");
      testStylist.Save();

      //Act
      Stylist foundStylist = Stylist.Find(testStylist.GetId());

      //Assert
      Assert.Equal(testStylist, foundStylist);
    }

    [Fact]
    public void Test_EditStylist_UpdatesStylistInDatabase()
    {
      //Arrange
      string name = "Nancy";
      Stylist testStylist = new Stylist(name);
      testStylist.Save();
      string newName = "Nancy Jr";

      //Act
      testStylist.Update(newName);

      string result = testStylist.GetName();

      //Assert
      Assert.Equal(newName, result);
    }

    [Fact]
    public void Test_RemoveStylist_DeletesStylistFromDatabase()
    {
      //Arrange
      string name1 = "Nancy";
      Stylist testStylist1 = new Stylist(name1);
      testStylist1.Save();

      string name2 = "Becky";
      Stylist testStylist2 = new Stylist(name2);
      testStylist2.Save();

      //Act
      testStylist1.Delete();
      List<Stylist> resultStylist = Stylist.GetAll();
      List<Stylist> testStylistList = new List<Stylist> {testStylist2};

      //Assert
      Assert.Equal(testStylistList, resultStylist);
    }

    [Fact]
    public void Test_GetClients_RetrievesAllClientsForStylist()
    {
      Stylist testStylist = new Stylist("Nancy");
      testStylist.Save();

      Client firstClient = new Client("Becky", testStylist.GetId());
      firstClient.Save();
      Client secondClient = new Client("Sam", testStylist.GetId());
      secondClient.Save();


      List<Client> testClientList = new List<Client> {firstClient, secondClient};
      List<Client> resultClientList = testStylist.GetClients();

      Assert.Equal(testClientList, resultClientList);
    }



  }
}
