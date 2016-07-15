using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace HairSalon
{
  public class ClientTest : IDisposable
  {
    public ClientTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=hair_salon_test;Integrated Security=SSPI;";
    }

    public void Dispose()
    {
      Client.DeleteAll();
    }
    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      //Arrange, Act
      int result = Client.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }
    [Fact]
    public void Test_Equal_ReturnsTrueIfNamesAreTheSame()
    {
      //Arrange, Act
      Client firstClient = new Client("Becky", 1);
      Client secondClient = new Client("Becky", 1);

      //Assert
      Assert.Equal(firstClient, secondClient);
    }
    [Fact]
    public void Test_NewClient_SavesToDatabase()
    {
      //Arrange
      Client testClient = new Client("Becky", 1);

      //Act
      testClient.Save();
      List<Client> result = Client.GetAll();
      List<Client> testList = new List<Client>{testClient};


      //Assert
      Assert.Equal(testList, result);
    }
    [Fact]
    public void Test_EditClient_UpdatesClientInDatabase()
    {
      //Arrange
      string name = "Beck";
      Client testClient = new Client(name, 1);
      testClient.Save();
      string newName = "Becky";

      //Act
      testClient.Update(newName);

      string result = testClient.GetName();

      //Assert
      Assert.Equal(newName, result);
    }
    [Fact]
    public void Test_RemoveClient_DeletesClientFromDatabase()
    {
      //Arrange
      string name1 = "Becky";
      Client testClient1 = new Client(name1, 1);
      testClient1.Save();

      string name2 = "Sam";
      Client testClient2 = new Client(name2, 1);
      testClient2.Save();

      //Act
      testClient1.Delete();
      List<Client> resultClient = Client.GetAll();
      List<Client> testClientList = new List<Client> {testClient2};

      //Assert
      Assert.Equal(testClientList, resultClient);
    }
  }
}
