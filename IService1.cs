using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ISaveService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {

        [OperationContract]
        int RegisterUser(String name, String surname, String email, String password, String Phone, String Address, char User_type);

        [OperationContract]
        char Login(String email, String Password);

        [OperationContract]
        int UpdateUser(String name, String surname, String email, String password, String Phone, String Address, char User_type);

        [OperationContract]
        int DeleteUser(int id);

        [OperationContract]
        User GetUser(int id);

        [OperationContract]
        List<User> GetUsers();


        //*******START PRODUCT MANAGEMENT********

        [OperationContract]
        int AddProduct(String name, String description, int price, Char in_stock, decimal energy_saved_watts, decimal carbon_reductionKG, decimal capacity,String image, String link );

        [OperationContract]
        Product GetProduct(int id);

        [OperationContract]
        List<Product> GetProducts();

        //*******END PRODUCT MANAGEMENT********
    }




}
