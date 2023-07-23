using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Data;
using System.Reflection.Emit;

namespace LoginForm.Pages.Users
{
    public class LoginModel : PageModel
    {

        public UserInfo userInfo = new UserInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
		

		}

        public void OnPost()
        {

            userInfo.userName = Request.Form["userName"];
            userInfo.password = Request.Form["password"];

            if (userInfo.userName.Length == 0 || userInfo.password.Length == 0)
            {
                errorMessage = "All the fields are required";
                return;
            }
           

            try
            {

                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=loginform;Integrated Security=True";


				using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "select * from users where userName=@userName and password=@password;";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@userName", userInfo.userName);
                        command.Parameters.AddWithValue("@password", userInfo.password);
                        SqlDataAdapter sda = new SqlDataAdapter(command);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);

                        command.ExecuteNonQuery();

                        if (dt.Rows.Count > 0)
                        {
                            String id1 = "";

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                   id1 = "" + reader.GetInt32(0);


                                }
                            }

                                    Response.Redirect("/Users/Index?id="+id1);
                        }
                        else
                        {

                            errorMessage = "Invalid Login Credentials";
                            return;
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

        }

    }
}
