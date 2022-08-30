using GameIN.Models;
using GameIN.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameIN.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var home = new HomeModel();

            var P_list = new List<string>();
            var R_list = new List<string>();
            var F_list = new List<string>();
            var Fr_list = new List<string>();

            var connStr = ConfigurationManager.ConnectionStrings["GameInDB"].ConnectionString;
            var conn = new SqlConnection(connStr);
            conn.Open();
            
            string query = QueryGenerator.PopularGames();
            var command = new SqlCommand(query, conn);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        P_list.Add(reader.GetString(0));
                    }
                }
            }

            query = QueryGenerator.RecentGames();
            command = new SqlCommand(query, conn);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        R_list.Add(reader.GetString(0));
                    }
                }
            }

            query = QueryGenerator.FutureGames();
            command = new SqlCommand(query, conn);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        F_list.Add(reader.GetString(0));
                    }
                }
            }

            query = QueryGenerator.FreeGames();
            command = new SqlCommand(query, conn);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Fr_list.Add(reader.GetString(0));
                    }
                }
            }

            home.Poupular = P_list;
            home.Recents = R_list;
            home.Future = F_list;
            home.Free = Fr_list;

            return View(home);
        }
        public ActionResult Popular()
        {
            var games = new List<GameModel>();
            var connStr = ConfigurationManager.ConnectionStrings["GameInDB"].ConnectionString;
            var conn = new SqlConnection(connStr);
            conn.Open();
            string query = QueryGenerator.PopularGames();
            var command = new SqlCommand(query, conn);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        games.Add(new GameModel
                        {
                            Name = reader.GetString(0),
                            Description = reader.GetString(1)

                        });
                    }
                }
            }
           
            return View(games);
        }
        public ActionResult Latest()
        {
            var games = new List<GameModel>();
            var connStr = ConfigurationManager.ConnectionStrings["GameInDB"].ConnectionString;
            var conn = new SqlConnection(connStr);
            conn.Open();
            string query = QueryGenerator.RecentGames();
            var command = new SqlCommand(query, conn);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        games.Add(new GameModel
                        {
                            Name = reader.GetString(0),
                            Description = reader.GetString(1)

                        });
                    }
                }
            }

            return View(games);
        }

        public ActionResult Free()
        {
            var games = new List<GameModel>();
            var connStr = ConfigurationManager.ConnectionStrings["GameInDB"].ConnectionString;
            var conn = new SqlConnection(connStr);
            conn.Open();
            string query = QueryGenerator.FreeGames();
            var command = new SqlCommand(query, conn);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        games.Add(new GameModel
                        {
                            Name = reader.GetString(0),
                            Description = reader.GetString(1)

                        });
                    }
                }
            }

            return View(games);
        }


        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(UserModel user) 
        {
            if (ModelState.IsValid)
            {
                Users.CreateUser(user);
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserModel model)
        {
            
            var result = Users.AuthenticateUser(model);
            
            if (result.Item1 != -1)
            {
                if(result.Item1 == 1)
                {
                    Session["Admin"] = "true";                   
                }
                if (result.Item1 == 2)
                {
                    Session["User"] = "true";                
                }
                Session["ActiveUser"] = result.Item2;
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult AllGame()
        {
            List<GameModel> games = new List<GameModel>();
            var connStr = ConfigurationManager.ConnectionStrings["GameInDB"].ConnectionString;
            var conn = new SqlConnection(connStr);
            conn.Open();
            string query = @"select g.Id, g.GameName, c.Category from Game g join Category c on g.GameName = c.GameName";
            var command = new SqlCommand(query, conn);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        games.Add(new GameModel
                        {
                            Id = reader.GetInt32(0),
                            Name = reader["GameName"].ToString(),
                            Category = reader["Category"].ToString(),
                        });
                    }
                }
            }
            return View(games);
        }

        public ActionResult AddGame()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddGame(GameModel game)
        {
            if (ModelState.IsValid)
            {
                Games.AddGame(game);             
            }
            return View();           
        }
        public ActionResult GamePage(string id)
        {
            GameModel game = new GameModel();
            string query =
                @"select g.Id,
                    g.GameName,
		            g.Description,
		            g.Release,
		            g.Price,
		            c.Category,
		            d.Developer,
		            di.Directx,
		            gr.Graphics,
		            p.Platform,
		            pr.Processor,
		            pu.Publisher,
		            r.Ram,
		            ri.Review,
		            s.Storage
	            from Game g 
	            join Category c
		            on g.GameName = c.GameName
	            join Developer d
		            on g.GameName = d.GameName
	            join Directx di
		            on g.GameName = di.GameName
	            join Graphics gr
		            on g.GameName = gr.GameName
	            join Platform p
		            on g.GameName = p.GameName
	            join Processor pr
		            on g.GameName = pr.GameName
	            join Publisher pu
		            on g.GameName = pu.GameName
	            join Ram r
		            on g.GameName = r.GameName
	            join Review ri
		            on g.GameName = ri.GameName
	            join Storage s
		            on g.GameName = s.GameName
                    where g.GameName = @GameName";

            var connStr = ConfigurationManager.ConnectionStrings["GameInDB"].ConnectionString;
            var conn = new SqlConnection(connStr);
            conn.Open();
            var command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@GameName", id);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        game.Id = reader.GetInt32(0);
                        game.Name = reader.GetString(1);
                        game.Release = Convert.ToString(reader["Release"]);
                        game.Description = reader.GetString(2);
                        game.Price = Convert.ToString(reader["Price"]);
                        game.Category = reader.GetString(5);
                        game.Developer = reader.GetString(6);
                        game.Directx = reader.GetString(7);
                        game.Graphics = reader.GetString(8);
                        game.Platform = reader.GetString(9);
                        game.Processor = reader.GetString(10);
                        game.Publisher = reader.GetString(11);
                        game.Ram = reader.GetString(12);
                        game.Review = reader.GetString(13);
                        game.Storage = reader.GetString(14);
                    }                   
                }           
            }
            return View(game);
        }

        public ActionResult Edit(int id)
        {
            GameModel game = new GameModel();
            string query =
                @"select g.Id,
                    g.GameName,
		            g.Description,
		            g.Release,
		            g.Price,
		            c.Category,
		            d.Developer,
		            di.Directx,
		            gr.Graphics,
		            p.Platform,
		            pr.Processor,
		            pu.Publisher,
		            r.Ram,
		            ri.Review,
		            s.Storage
	            from Game g 
	            join Category c
		            on g.GameName = c.GameName
	            join Developer d
		            on g.GameName = d.GameName
	            join Directx di
		            on g.GameName = di.GameName
	            join Graphics gr
		            on g.GameName = gr.GameName
	            join Platform p
		            on g.GameName = p.GameName
	            join Processor pr
		            on g.GameName = pr.GameName
	            join Publisher pu
		            on g.GameName = pu.GameName
	            join Ram r
		            on g.GameName = r.GameName
	            join Review ri
		            on g.GameName = ri.GameName
	            join Storage s
		            on g.GameName = s.GameName
                    where g.Id = @Id";

            var connStr = ConfigurationManager.ConnectionStrings["GameInDB"].ConnectionString;
            var conn = new SqlConnection(connStr);
            conn.Open();
            var command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@Id", id);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        game.Id = reader.GetInt32(0);
                        game.Name = reader.GetString(1);
                        game.Release = Convert.ToString(reader["Release"]);
                        game.Description = reader.GetString(2);
                        game.Price = Convert.ToString(reader["Price"]);
                        game.Category = reader.GetString(5);
                        game.Developer = reader.GetString(6);
                        game.Directx = reader.GetString(7);
                        game.Graphics = reader.GetString(8);
                        game.Platform = reader.GetString(9);
                        game.Processor = reader.GetString(10);
                        game.Publisher = reader.GetString(11);
                        game.Ram = reader.GetString(12);
                        game.Review = reader.GetString(13);
                        game.Storage = reader.GetString(14);
                    }
                }
            }
            return View(game);
        }

        public ActionResult Admin()
        {
           
            
            var admin = new List<UserModel>();
            admin.Add(LoggedUser.Instance.user);
            var users = new List<UserModel>();

            var connStr = ConfigurationManager.ConnectionStrings["GameInDB"].ConnectionString;
            var conn = new SqlConnection(connStr);
            conn.Open();
            string query = QueryGenerator.AllUser();
            var command = new SqlCommand(query, conn);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        users.Add(new UserModel
                        {
                            Name = reader.GetString(0),
                            Email = reader.GetString(1),
                            Password = reader.GetString(2)

                        });
                    }
                }
            }
            var AdminModel = new AdminViewModel
            {
                Admin = admin,
                Users = users
            }; 
            return View(AdminModel);
        }

        public ActionResult User()
        {
            UserModel user = LoggedUser.Instance.user;
            return View(user);
        }

        public ActionResult LogOut()
        {
            Session["Admin"] = null;
            Session["User"] = null;
            LoggedUser.Instance.user = null;
            return RedirectToAction("Index", "Home");
        }

       

        [HttpGet]
        public ActionResult Search(string search)
        {
            var games = new List<GameModel>();
            var connStr = ConfigurationManager.ConnectionStrings["GameInDB"].ConnectionString;
            var conn = new SqlConnection(connStr);
            conn.Open();
            string query = QueryGenerator.SearchGames();
            var command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@search_term", "%" + search + "%");
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        games.Add(new GameModel
                        {
                            Name = reader.GetString(0),
                            Category = reader.GetString(1)
                        });
                    }
                }
            }
            return View(games);
        }

        [ChildActionOnly]
        public ActionResult ManageUser()
        {
            var users = new List<UserModel>();
            var connStr = ConfigurationManager.ConnectionStrings["GameInDB"].ConnectionString;
            var conn = new SqlConnection(connStr);
            conn.Open();
            string query = QueryGenerator.AllUser();
            var command = new SqlCommand(query, conn);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        users.Add(new UserModel
                        {
                            Name = reader.GetString(0),
                            Email = reader.GetString(1),
                            Password = reader.GetString(2)

                        });
                    }
                }
            }

            return PartialView("_ManageUser", users);
        }

        public ActionResult AddToFavorite(string id)
        {
            var connStr = ConfigurationManager.ConnectionStrings["GameInDB"].ConnectionString;
            var conn = new SqlConnection(connStr);
            conn.Open();
            string query = QueryGenerator.AddFavorite();
            var command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@Name", id);
            command.Parameters.AddWithValue("@Email", LoggedUser.Instance.user.Email);
            command.ExecuteNonQuery();
            return JavaScript("<script>alert(\"some message\")</script>");
        }

        private string alert(string v)
        {
            throw new NotImplementedException();
        }

        public FileResult GamePlay(string id)
        {
            var connStr = ConfigurationManager.ConnectionStrings["GameInDB"].ConnectionString;
            var conn = new SqlConnection(connStr);
            conn.Open();
            SqlCommand command = new SqlCommand("select Image from GamePlay where GameName = @GameName", conn);
            command.Parameters.AddWithValue("@GameName", id);
            byte[] bytePic = (byte[])command.ExecuteScalar();
            return File(bytePic, "image/jpg");
        }
        public FileResult Poster(string id)
        {
            var connStr = ConfigurationManager.ConnectionStrings["GameInDB"].ConnectionString;
            var conn = new SqlConnection(connStr);
            conn.Open();
            SqlCommand command = new SqlCommand("select Image from Poster where GameName = @GameName", conn);
            command.Parameters.AddWithValue("@GameName", id);
            byte[] bytePic = (byte[])command.ExecuteScalar();
            return File(bytePic, "image/jpg");
        }


    }
}