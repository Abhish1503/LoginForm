using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
namespace LoginForm.Pages.Users
{
    public class IndexModel : PageModel
    {
        public List<UserInfo> listUsers = new List<UserInfo>();
        public void OnGet()
        {
            try
            {

				String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=loginform;Integrated Security=True";

				using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * From users";

					using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                UserInfo userInfo = new UserInfo();
                                userInfo.id = "" + reader.GetInt32(0);
                                userInfo.firstName = reader.GetString(1);
                                userInfo.lastName = reader.GetString(2);
                                userInfo.email = reader.GetString(3);
                                userInfo.userName = reader.GetString(4);
                                userInfo.password = reader.GetString(5);
                                userInfo.address =reader.GetString(6);
                                userInfo.manager=reader.GetString(7);
                                userInfo.created_at=reader.GetDateTime(8).ToString();


                                listUsers.Add(userInfo);

							}
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());

            }
        }
    }

    public class UserInfo
    {

		public String id { get; set; }
		public String firstName { get; set; }
		public String lastName{ get; set; }
		public String userName { get; set; }
        public String email { get; set; }
        public String password { get; set; }

        public String address { get; set; }

        public String manager { get; set; }

        public String created_at { get; set; }

	}
}
