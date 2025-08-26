using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;


namespace ISaveService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        ISaveLINQDataContext db = new ISaveLINQDataContext();

        public int AddProduct(string name, string description, int price, char in_stock, decimal energy_saved_watts, decimal carbon_reductionKG, decimal capacity, string image, string link)
        {
            
                var tempProduct = (from p in db.Products
                                   where p.Name.Equals(name)
                                   && p.Description.Equals(description)
                                   && p.Price.Equals(price)
                                   select p).FirstOrDefault();

                if (tempProduct != null)
                {
                    var objProduct = new Product();

                    objProduct.Name = name;
                    objProduct.Description = description;
                    objProduct.Price = price;
                    objProduct.InStock = in_stock;
                    objProduct.CarbonReductionKg = carbon_reductionKG;
                    objProduct.EnergySavedWatts = energy_saved_watts;
                    objProduct.Image = image;
                    objProduct.Link = link;
                    objProduct.Capacity = capacity;
                    

                    db.Products.InsertOnSubmit(objProduct);
                    try
                    {
                        db.SubmitChanges();
                        return 0;
                    }
                    catch (Exception ex)
                    {
                        ex.GetBaseException();
                        return -1;
                    }
                }
                else
                {
                    return 1;
                }
        }

        public int DeleteUser(int id)
        {
            var UserToDelete = (from u in db.Users
                                where u.Id.Equals(id)
                                select u).FirstOrDefault();

            if(UserToDelete != null)
            {
                db.Users.DeleteOnSubmit(UserToDelete);
                try
                {
                    db.SubmitChanges();
                    return 0; //user deleted succeffully
                }catch(Exception ex)
                {
                    ex.GetBaseException();
                    return -1; //some internal error 
                }
            }else
            {
                return 1; //user not found
            }
        }

        public Product GetProduct(int id)
        {
            var tempProduct = (from u in db.Products
                               where u.Id.Equals(id)
                               select u).FirstOrDefault();

            if (tempProduct != null)
            {
                var objProduct = new Product();

                objProduct.Name = tempProduct.Name;
                objProduct.Price = tempProduct.Price;
                objProduct.Description = tempProduct.Description;
                objProduct.CarbonReductionKg = tempProduct.CarbonReductionKg;
                objProduct.EnergySavedWatts = tempProduct.EnergySavedWatts;
                objProduct.Image = tempProduct.Image ;
                objProduct.Link = tempProduct.Link;
                objProduct.Capacity = tempProduct.Capacity;

                return objProduct;
            }
            else
            {
                return null;
            }
        }

        public List<Product> GetProducts()
        {
            List<Product> ProductsList = new List<Product>();

            dynamic tempProdsList = (from p in db.Products
                                     where p.InStock.Equals("Y")
                                     select p);

            if (tempProdsList != null)
            {
                foreach (Product product in tempProdsList)
                {
                    var objProduct = new Product();

                    objProduct.Name = product.Name;
                    objProduct.Description = product.Description;
                    objProduct.Price = product.Price;
                    objProduct.CarbonReductionKg = product.CarbonReductionKg;
                    objProduct.EnergySavedWatts = product.EnergySavedWatts;
                    objProduct.Image = product.Image;
                    objProduct.Link = product.Link;
                    objProduct.Capacity = product.Capacity;

                    ProductsList.Add(objProduct);
                }
                return ProductsList;

            }
            else
            {
                return null;
            }
        }

        public User GetUser(int id)
        {
            var tempUser = (from u in db.Users where
                            u.Id.Equals(id)
                            select u).FirstOrDefault();

            if(tempUser != null)
            {
                var objUser = new User();
                objUser.FirstName = tempUser.FirstName;
                objUser.LastName = tempUser.LastName;
                objUser.Email = tempUser.Email;
                objUser.Password = tempUser.Password;
                objUser.Phone = tempUser.Phone;
                objUser.Address = tempUser.Address;

                return objUser;
            }
            else
            {
                return null;
            }
        }

        public List<User> GetUsers()
        {
            List<User> UserList = new List<User>();
            var tempUser = (from u in db.Users
                            where u.UserType.Equals('C')
                            select u);

            if(tempUser != null)
            {
                foreach(User user in tempUser)
                {
                    var objUser = new User();
                    objUser.FirstName = user.FirstName;
                    objUser.LastName = user.LastName;
                    objUser.Email = user.Email;
                    objUser.Password = user.Password;
                    objUser.Phone = user.Phone;
                    objUser.Address = user.Address;

                    UserList.Add(objUser);
                }
                return UserList;

            }
            else
            {
                return null;
            }
        }

        public char Login(string email, string Password)
        {
            var HashedPassword = Secrecy.HashPassword(Password);
            var tempUser = (from u in db.Users where
                            u.Email.Equals(email) &&
                            u.Password.Equals(HashedPassword)
                            select u).FirstOrDefault();

            if(tempUser != null)
            {
                return 'S'; //'S' = success
            }
            else
            {
                return 'F';  //'F' = failure
            }


            
        }

        public int RegisterUser(string name, string surname, string email, string password, string Phone, string Address, char User_type)
        {
            var HashedPassword = Secrecy.HashPassword(password);
            var tempUser = (from u in db.Users where
                             u.Email.Equals(email) &&
                             u.Password.Equals(HashedPassword)
                             select u).FirstOrDefault();

            if(tempUser != null)// checking if uder does mot exist
            {
                var objUser = new User(); // creating a new class object which will be stored in the database

                objUser.FirstName = name;
                objUser.LastName = surname;
                objUser.Email = email;
                objUser.Password = HashedPassword;
                objUser.Phone = Phone;
                objUser.Address = Address;
                objUser.UserType = User_type;
                db.Users.InsertOnSubmit(objUser);

                try
                {
                    db.SubmitChanges();
                    return 0; // everything went well
                }catch(Exception ex)
                {
                    ex.GetBaseException(); //handling some internal errors
                    return -1;
                }

            }
            else
            {
                return 1; //user Exists
            }
        }

        public int UpdateUser(string name, string surname, string email, string password, string Phone, string Address, char User_type)
        {
            var HashedPassword = Secrecy.HashPassword(password);
            var tempUser = (from u in db.Users where
                            u.Email.Equals(email) &&
                            u.Password.Equals(HashedPassword)
                            select u).FirstOrDefault();

            if (tempUser != null)// checking if uder does mot exist
            {
                var objUser = new User(); // creating a new class object which will be stored in the database

                objUser.FirstName = name;
                objUser.LastName = surname;
                objUser.Email = email;
                objUser.Password = HashedPassword;
                objUser.Phone = Phone;
                objUser.Address = Address;
                objUser.UserType = User_type;

                try
                {
                    db.SubmitChanges();
                    return 0; // everything went well
                }
                catch (Exception ex)
                {
                    ex.GetBaseException(); //handling some internal errors
                    return -1;
                }

            }
            else
            {
                return 1; //user Exists
            }
        }
    }
}
