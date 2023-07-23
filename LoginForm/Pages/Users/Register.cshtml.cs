using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace LoginForm.Pages.Users
{
    public class RegisterModel : PageModel
    {


        public UserInfo userInfo = new UserInfo();
        public String errorMessage = "";
        public String successMessage = "";

        public void onGet()
        {

        }


        public void OnPost()
        {

            userInfo.firstName = Request.Form["firstName"];
            userInfo.lastName = Request.Form["lastName"];
            userInfo.email = Request.Form["email"];
            userInfo.userName = Request.Form["userName"];
            userInfo.password = Request.Form["password"];
			userInfo.address = Request.Form["address"];
			userInfo.manager = Request.Form["manager"];



            if (userInfo.firstName.Length == 0 || userInfo.email.Length == 0 || userInfo.lastName.Length == 0 || userInfo.userName.Length == 0 || userInfo.password.Length == 0 || userInfo.address.Length == 0 || userInfo.manager.Length==0)
            {
                errorMessage = "All the fields are required";
                return;
            }

            if (userInfo.firstName.Length < 3)
            {
                errorMessage = "First Name must be atleast 3 characters";
                return;
            }
            if (userInfo.lastName.Length < 3)
            {
                errorMessage = "Last Name must be atleast 3 characters";
                return;
            }

            if(userInfo.userName.Length < 5) {
                errorMessage = "UserName must be atleast 5 characters";
                return;
            }

            string specialCh = @"%!@#$%^&*()?/>.<,:;'\|}]{[_~`+=-" + "\"";
            char[] specialChArray = specialCh.ToCharArray();
            bool flag = false;
            foreach (char ch in specialChArray)
            {
                if (userInfo.userName.Contains(ch))
                {
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                errorMessage = "UserName Must contain a Special Character @\"%!@#$%^&*()?/>.<,:;'\\|}]{[_~`+=-";
                return;
            }

            if (!userInfo.password.Any(char.IsUpper))
            {
                errorMessage = "Password must contain a Uppercase letter";
                return;
            }
            if (!userInfo.password.Any(char.IsLower))
            {
                errorMessage = "Password must contain a Lowercase letter";
                return;
            }
            if (!userInfo.password.Any(char.IsLower))
            {
                errorMessage = "Password must contain a Lowercase letter";
                return;
            }

            flag = false;
            foreach (char ch in specialChArray)
            {
                if (userInfo.password.Contains(ch))
                {
                    flag = true;
                    break;
                }
            }
            if (!flag) {
                errorMessage = "Password Must contain a Special Character @\"%!@#$%^&*()?/>.<,:;'\\|}]{[_~`+=-";
                return;
            }
            flag = false;

            char[] arr = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' } ;

            foreach (char ch in arr)
            {
                if (userInfo.password.Contains(ch))
                {
                    flag = true;
                }
            }
            if (!flag)
            {
                errorMessage = "Password Must contain a Number [0-9]";
                return;
            }




            //save the new client into the db
            try
            {

                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=loginform;Integrated Security=True";


                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO users " + "(firstName,lastName,email,userName,password,address,manager) VALUES " + "(@firstName,@lastName,@email,@userName,@password,@address,@manager);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@firstName", userInfo.firstName);
                        command.Parameters.AddWithValue("@lastName", userInfo.lastName);
                        command.Parameters.AddWithValue("@email", userInfo.email);
                        command.Parameters.AddWithValue("@userName", userInfo.userName);
                        command.Parameters.AddWithValue("@password", userInfo.password);
						command.Parameters.AddWithValue("@address", userInfo.address);
						command.Parameters.AddWithValue("@manager", userInfo.manager);

						command.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }


            userInfo.firstName = "";
            userInfo.lastName = "";
            userInfo.email = "";
            userInfo.userName = "";
            userInfo.password = "";
            userInfo.address = "";
            userInfo.manager = "";
            successMessage = "Registration Succesful - Proceed to Login";
            Response.Redirect("/Users/Login");
        }
    }
}
